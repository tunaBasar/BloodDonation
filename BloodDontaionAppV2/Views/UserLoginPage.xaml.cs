using Microsoft.Maui.Controls;
using BloodDonationAppV2.Services;

namespace BloodDonationAppV2.Views
{
    public partial class UserLoginPage : ContentPage
    {
        private readonly IApiService _apiService;

        public UserLoginPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TcEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await DisplayAlert("Hata", "Lütfen tüm alanları doldurunuz", "Tamam");
                return;
            }

            if (TcEntry.Text.Length != 11)
            {
                await DisplayAlert("Hata", "TC Kimlik Numarası 11 haneli olmalıdır", "Tamam");
                return;
            }

            LoginBtn.IsEnabled = false;
            LoginBtn.Text = "Giriş yapılıyor...";

            try
            {
                var response = await _apiService.UserLoginAsync(TcEntry.Text, PasswordEntry.Text);

                if (response.Success)
                {
                    SessionManager.Instance.SetCurrentUser(response.User);
                    await Shell.Current.GoToAsync("//UserHomePage");
                }
                else
                {
                    await DisplayAlert("Hata", response.Message, "Tamam");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Bir hata oluştu: {ex.Message}", "Tamam");
            }
            finally
            {
                LoginBtn.IsEnabled = true;
                LoginBtn.Text = "Giriş Yap";
            }
        }

        private async void OnForgotPasswordClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//ForgotPasswordPage");
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//UserRegisterPage");
        }
    }
}