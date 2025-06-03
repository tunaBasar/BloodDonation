using System.Collections.ObjectModel;
using BloodDonationApp.Helpers;
using BloodDonationApp.Models;
using BloodDonationApp.Services;

namespace BloodDonationApp
{
    public partial class UserHomePage : ContentPage
    {
        private readonly IApiService _apiServices;
        private ObservableCollection<DonationRequest> _allRequests;

        public UserHomePage(IApiService apiServices)
        {
            InitializeComponent();
            _apiServices = apiServices;
            _allRequests = new ObservableCollection<DonationRequest>();
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
                BloodTypeLabel.Text = $"Kan Grubu: {GetBloodTypeDisplayName(user.BloodType)}";
            }

            _ = LoadDonationRequestsAsync();
        }

        private async Task LoadDonationRequestsAsync()
        {
            try
            {
                var user = SessionManager.GetUser();
                if (user == null)
                {
                    await DisplayAlert("Hata", "Kullanıcı oturumu bulunamadı.", "Tamam");
                    return;
                }

                // Loading göstergesi ekleyebilirsiniz
                System.Diagnostics.Debug.WriteLine($"Loading requests for user ID: {user.Id}");

                var response = await _apiServices.GetDonationRequestsAsync(user.Id);

                System.Diagnostics.Debug.WriteLine($"API Response - Success: {response.Success}, Message: {response.Message}");
                
                if (response.Success && response.Data != null)
                {
                    _allRequests.Clear();
                    foreach (var request in response.Data)
                    {
                        _allRequests.Add(request);
                    }
                    
                    DonationRequestsCollectionView.ItemsSource = _allRequests;
                    
                    System.Diagnostics.Debug.WriteLine($"Loaded {_allRequests.Count} donation requests");
                }
                else
                {
                    _allRequests.Clear();
                    DonationRequestsCollectionView.ItemsSource = _allRequests;

                    if (!string.IsNullOrEmpty(response.Message))
                    {
                        System.Diagnostics.Debug.WriteLine($"API Error: {response.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception in LoadDonationRequestsAsync: {ex.Message}");
                await DisplayAlert("Hata", $"Bağış istekleri yüklenirken hata oluştu: {ex.Message}", "Tamam");

                _allRequests.Clear();
                DonationRequestsCollectionView.ItemsSource = _allRequests;
            }
        }

        private string GetBloodTypeDisplayName(BloodType bloodType)
        {
            return bloodType switch
            {
                BloodType.APositive => "A Rh+",
                BloodType.ANegative => "A Rh-",
                BloodType.BPositive => "B Rh+",
                BloodType.BNegative => "B Rh-",
                BloodType.ABPositive => "AB Rh+",
                BloodType.ABNegative => "AB Rh-",
                BloodType.OPositive => "O Rh+",
                BloodType.ONegative => "O Rh-",
                _ => bloodType.ToString()
            };
        }

        private void OnSearchButtonPressed(object sender, EventArgs e)
        {
            var searchBar = sender as SearchBar;
            var searchText = searchBar?.Text?.ToLower();

            if (string.IsNullOrWhiteSpace(searchText))
            {
                DonationRequestsCollectionView.ItemsSource = _allRequests;
            }
            else
            {
                var filtered = _allRequests.Where(r =>
                    (!string.IsNullOrEmpty(r.Description) && r.Description.ToLower().Contains(searchText)) ||
                    (!string.IsNullOrEmpty(r.PatientName) && r.PatientName.ToLower().Contains(searchText)) ||
                    r.BloodType.ToString().ToLower().Contains(searchText)
                ).ToList();

                DonationRequestsCollectionView.ItemsSource = new ObservableCollection<DonationRequest>(filtered);
            }
        }

        private async void OnFilterButtonClicked(object sender, EventArgs e)
        {
            var result = await DisplayActionSheet("Filtrele", "İptal", null,
                "Tümü", "Acil", "Kritik", "Yüksek", "Orta", "Düşük");

            if (result != null && result != "İptal")
            {
                if (result == "Tümü")
                {
                    DonationRequestsCollectionView.ItemsSource = _allRequests;
                }
                else
                {
                    var urgencyFilter = result switch
                    {
                        "Düşük" => UrgencyLevel.Low,
                        "Orta" => UrgencyLevel.Medium,
                        "Yüksek" => UrgencyLevel.High,
                        "Kritik" => UrgencyLevel.Critical,
                        "Acil" => UrgencyLevel.Critical,
                        _ => (UrgencyLevel?)null
                    };

                    if (urgencyFilter.HasValue)
                    {
                        var filtered = _allRequests.Where(r => r.UrgencyLevel == urgencyFilter.Value).ToList();
                        DonationRequestsCollectionView.ItemsSource = new ObservableCollection<DonationRequest>(filtered);
                    }
                }
            }
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
                        var result = await _apiServices.DoDonation(donationId, user.Id);

                        if (result.Success)
                        {
                            await DisplayAlert("Başarılı", "Bağış isteğiniz alındı. Sizinle iletişime geçeceğiz.", "Tamam");
                            await LoadDonationRequestsAsync(); // Listeyi yenile
                        }
                        else
                        {
                            await DisplayAlert("Hata", $"Bağış işlemi başarısız: {result.Message}", "Tamam");
                        }
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