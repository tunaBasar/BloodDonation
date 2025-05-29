using System.Collections.ObjectModel;
using BloodDonationApp.Helpers;
using BloodDonationApp.Models;
using BloodDonationApp.Services;


namespace BloodDonationApp
{
    public partial class UserHomePage : ContentPage
    {
        private readonly IApiService _apiServices;

        public UserHomePage(IApiService apiServices)
        {
            InitializeComponent();
            _apiServices = apiServices;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var user = SessionManager.GetUser();
            if (user != null)
            {
                FullNameLabel.Text = $"{user.Name} {user.SurName}";
                EmailLabel.Text = user.Mail;
                PhoneLabel.Text = user.PhoneNumber ?? "Belirtilmemiş";
                BloodTypeLabel.Text = $"Kan Grubu: {user.BloodType}";
            }

            LoadDonationRequests();
        }

        private async void LoadDonationRequests()
        {
            try
            {
                var user = SessionManager.GetUser();

                var response = await _apiServices.GetDonationRequestsAsync(user.Id);

                if (response.Success && response.Data != null)
                {
                    var donationRequests = new ObservableCollection<DonationRequest>(response.Data);
                    DonationRequestsCollectionView.ItemsSource = donationRequests;
                }
                else
                {
                    DonationRequestsCollectionView.ItemsSource = new ObservableCollection<DonationRequest>();

                    if (!string.IsNullOrEmpty(response.Message))
                    {
                        await DisplayAlert("Bilgi", $"Bağış istekleri yüklenemedi: {response.Message}", "Tamam");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Bağış istekleri yüklenirken hata oluştu: {ex.Message}", "Tamam");

                DonationRequestsCollectionView.ItemsSource = new ObservableCollection<DonationRequest>();
            }
        }

        private void OnSearchButtonPressed(object sender, EventArgs e)
        {
            var searchBar = sender as SearchBar;
            var searchText = searchBar?.Text?.ToLower();

            if (DonationRequestsCollectionView.ItemsSource is ObservableCollection<DonationRequest> requests)
            {
                var allRequests = requests.ToList(); 
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    DonationRequestsCollectionView.ItemsSource = new ObservableCollection<DonationRequest>(allRequests);
                }
                else
                {
                    var filtered = allRequests.Where(r =>
                        (!string.IsNullOrEmpty(r.PatientName) && r.PatientName.ToLower().Contains(searchText)) ||
                        (!string.IsNullOrEmpty(r.HospitalName) && r.HospitalName.ToLower().Contains(searchText)) ||
                        (!string.IsNullOrEmpty(r.BloodType) && r.BloodType.ToLower().Contains(searchText)) ||
                        (!string.IsNullOrEmpty(r.Description) && r.Description.ToLower().Contains(searchText))
                    ).ToList();

                    DonationRequestsCollectionView.ItemsSource = new ObservableCollection<DonationRequest>(filtered);
                }
            }
        }

        private async void OnFilterButtonClicked(object sender, EventArgs e)
        {
            var result = await DisplayActionSheet("Filtrele", "İptal", null,
                "Tümü", "Acil", "Normal", "A Rh+", "A Rh-", "B Rh+", "B Rh-", "AB Rh+", "AB Rh-", "0 Rh+", "0 Rh-");
        }

        private async void OnDonateButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is int donationId)
            {
                var confirm = await DisplayAlert("Bağış Onayı",
                    "Bu bağış isteği için bağışta bulunmak istediğinize emin misiniz?",
                    "Evet", "Hayır");

                if (confirm)
                {
                    try
                    {
                        var user = SessionManager.GetUser();
                        var result = await _apiServices.DoDonation(donationId,user.Id);

                        await DisplayAlert("Başarılı", "Bağış isteğiniz alındı. Sizinle iletişime geçeceğiz.", "Tamam");

                        LoadDonationRequests();
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Hata", $"Bağış işlemi sırasında hata oluştu: {ex.Message}", "Tamam");
                    }
                }
            }
        }
        private async void OnCreateRequestClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(RequestPage));
        }
        private async void OnDonationClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(UserDonationPage));
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert("Çıkış", "Çıkış yapmak istediğinizden emin misiniz?", "Evet", "Hayır");
            if (result)
            {
                SessionManager.ClearSession();
                SecureStorage.RemoveAll();
                await Shell.Current.GoToAsync("//MainPage");
            }
        }
    }
}
