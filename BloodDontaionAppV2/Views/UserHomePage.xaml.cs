using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;
using BloodDonationAppV2.Models;
using BloodDonationAppV2.Services;

namespace BloodDonationAppV2.Views
{
    public partial class UserHomePage : ContentPage
    {
        private readonly IApiService _apiService;
        public ObservableCollection<DonationRequest> DonationRequests { get; set; }

        public UserHomePage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            DonationRequests = new ObservableCollection<DonationRequest>();
            BindingContext = this;
            
            LoadUserInfo();
            LoadDonationRequests();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadDonationRequests();
        }

        private void LoadUserInfo()
        {
            var currentUser = SessionManager.Instance.CurrentUser;
            if (currentUser != null)
            {
                UserNameLabel.Text = $"{currentUser.Ad} {currentUser.Soyad}";
                UserTcLabel.Text = $"TC: {currentUser.TcKimlikNo}";
                UserBloodGroupLabel.Text = $"Kan Grubu: {currentUser.KanGrubu}";
                UserEmailLabel.Text = currentUser.Email;
            }
        }

        private async void LoadDonationRequests()
        {
            try
            {
                var requests = await _apiService.GetDonationRequestsAsync();
                DonationRequests.Clear();
                
                foreach (var request in requests)
                {
                    DonationRequests.Add(request);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Bağış istekleri yüklenirken hata oluştu: {ex.Message}", "Tamam");
            }
        }

        private async void OnCreateDonationRequestClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//CreateDonationRequestPage");
        }

        private async void OnMyRequestsClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MyRequestsPage");
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Çıkış", "Çıkış yapmak istediğinizden emin misiniz?", "Evet", "Hayır");
            if (answer)
            {
                SessionManager.Instance.Logout();
                await Shell.Current.GoToAsync("//MainPage");
            }
        }

        private async void OnSearchClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchEntry.Text))
            {
                LoadDonationRequests();
                return;
            }

            try
            {
                var allRequests = await _apiService.GetDonationRequestsAsync();
                var filteredRequests = allRequests.Where(r => 
                    r.Konum.ToLower().Contains(SearchEntry.Text.ToLower()) ||
                    r.KanGrubu.ToLower().Contains(SearchEntry.Text.ToLower()) ||
                    r.Ad.ToLower().Contains(SearchEntry.Text.ToLower()) ||
                    r.Soyad.ToLower().Contains(SearchEntry.Text.ToLower())
                ).ToList();

                DonationRequests.Clear();
                foreach (var request in filteredRequests)
                {
                    DonationRequests.Add(request);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Arama sırasında hata oluştu: {ex.Message}", "Tamam");
            }
        }

        private async void OnFilterClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//FilterPage");
        }
    }
}