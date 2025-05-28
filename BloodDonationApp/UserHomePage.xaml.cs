using System.Collections.ObjectModel;
using System.Text.Json;
using BloodDonationApp.Models;
using BloodDonationApp.Services;
using Microsoft.Maui.Controls;

namespace BloodDonationApp
{
    public partial class UserHomePage : ContentPage
    {
        private UserResponseDto _userData;
        private readonly IApiService _apiServices;

        public UserHomePage(IApiService apiServices)
        {
            InitializeComponent();
            _apiServices = apiServices;
            LoadDonationRequests();
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            if (Shell.Current.CurrentState.Location.OriginalString.Contains(nameof(UserHomePage)))
            {
                if (Shell.Current.CurrentState.Location.OriginalString.Split('?') is string[] routeParts && routeParts.Length > 1)
                {
                    var query = System.Web.HttpUtility.ParseQueryString(routeParts[1]);
                    if (query["UserData"] is string userDataJson)
                    {
                        _userData = JsonSerializer.Deserialize<UserResponseDto>(userDataJson);
                        LoadUserData();
                    }
                }
            }
        }

        private void LoadUserData()
        {
            if (_userData != null)
            {
                FullNameLabel.Text = $"{_userData.Name} {_userData.SurName}";
                EmailLabel.Text = _userData.Mail;
                PhoneLabel.Text = _userData.PhoneNumber ?? "Belirtilmemiş";
                BloodTypeLabel.Text = _userData.BloodType.ToString()??"Belirtilmemiş"; 
            }
        }

        private async void LoadDonationRequests()
        {
            try
            {
                // API'den gerçek bağış isteklerini çekme
                // Örnek: var requests = await _apiServices.GetDonationRequestsAsync();
                
                // Örnek veri (API entegrasyonu yapılınca bu kısım değişecek)
                var sampleRequests = new ObservableCollection<object>
                {
                    new {
                        Id = 1,
                        BloodType = "A+",
                        PatientName = "Mehmet Yılmaz",
                        HospitalName = "Atatürk Hastanesi",
                        UrgencyLevel = "Acil",
                        Description = "Ameliyat için acil kan ihtiyacı vardır.",
                        CreatedDate = DateTime.Now.AddDays(-1)
                    },
                    new {
                        Id = 2,
                        BloodType = "O-",
                        PatientName = "Ayşe Kara",
                        HospitalName = "Şehir Hastanesi",
                        UrgencyLevel = "Orta",
                        Description = "Kanser tedavisi için kan desteği gerekli.",
                        CreatedDate = DateTime.Now.AddDays(-2)
                    }
                };

                DonationRequestsCollectionView.ItemsSource = sampleRequests;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Bağış istekleri yüklenirken hata oluştu: {ex.Message}", "Tamam");
            }
        }

        private void OnSearchButtonPressed(object sender, EventArgs e)
        {
            var searchBar = sender as SearchBar;
            var searchText = searchBar.Text?.ToLower();
            
            if (DonationRequestsCollectionView.ItemsSource is ObservableCollection<object> requests)
            {
                var filtered = requests.Where(r => 
                    (r.GetType().GetProperty("PatientName")?.GetValue(r)?.ToString()?.ToLower()?.Contains(searchText) ?? false) ||
                    (r.GetType().GetProperty("HospitalName")?.GetValue(r)?.ToString()?.ToLower()?.Contains(searchText) ?? false) ||
                    (r.GetType().GetProperty("BloodType")?.GetValue(r)?.ToString()?.ToLower()?.Contains(searchText) ?? false)
                ).ToList();
                
                DonationRequestsCollectionView.ItemsSource = new ObservableCollection<object>(filtered);
            }
        }

        private async void OnFilterButtonClicked(object sender, EventArgs e)
        {
            var result = await DisplayActionSheet("Filtrele", "İptal", null, 
                "Tümü", "Acil", "Normal", "A Rh+", "A Rh-", "B Rh+", "B Rh-", "AB Rh+", "AB Rh-", "0 Rh+", "0 Rh-");
            
            // Filtreleme işlemleri burada yapılacak
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
                    // API'ye bağış isteği gönder
                    try
                    {
                        // Örnek: await _apiServices.CreateDonationAsync(donationId, _userData.Id);
                        await DisplayAlert("Başarılı", "Bağış isteğiniz alındı.", "Tamam");
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Hata", $"Bağış işlemi sırasında hata oluştu: {ex.Message}", "Tamam");
                    }
                }
            }
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            bool result = await DisplayAlert("Çıkış", "Çıkış yapmak istediğinizden emin misiniz?", "Evet", "Hayır");
            if (result)
            {
                
                SecureStorage.RemoveAll();
                
                await Shell.Current.GoToAsync($"//{nameof(UserLoginPage)}");
            }
        }
    }
}