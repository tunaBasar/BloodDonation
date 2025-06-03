using Microsoft.Maui.Controls;
using BloodDonationAppV2.Models;
using BloodDonationAppV2.Services;

namespace BloodDonationAppV2.Views
{
    public partial class UserRegisterPage : ContentPage
    {
        private readonly IApiService _apiService;

        public UserRegisterPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void OnRegisterClicked(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;

            RegisterBtn.IsEnabled = false;
            RegisterBtn.Text = "Kayıt yapılıyor...";

            try
            {
                var registerRequest = new RegisterRequest
                {
                    TcKimlikNo = TcEntry.Text.Trim(),
                    Ad = FirstNameEntry.Text.Trim(),
                    Soyad = LastNameEntry.Text.Trim(),
                    Email = EmailEntry.Text.Trim(),
                    KanGrubu = BloodGroupPicker.SelectedItem?.ToString(),
                    Sifre = PasswordEntry.Text
                };

                var response = await _apiService.RegisterAsync(registerRequest);

                if (response.Success)
                {
                    await DisplayAlert("Başarılı", "Kayıt işlemi başarıyla tamamlandı. Giriş yapabilirsiniz.", "Tamam");
                    await Shell.Current.GoToAsync("//UserLoginPage");
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
                RegisterBtn.IsEnabled = true;
                RegisterBtn.Text = "Kayıt Ol";
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(TcEntry.Text) || TcEntry.Text.Length != 11)
            {
                DisplayAlert("Hata", "TC Kimlik Numarası 11 haneli olmalıdır", "Tamam");
                return false;
            }

            if (string.IsNullOrWhiteSpace(FirstNameEntry.Text))
            {
                DisplayAlert("Hata", "Ad alanı boş bırakılamaz", "Tamam");
                return false;
            }

            if (string.IsNullOrWhiteSpace(LastNameEntry.Text))
            {
                DisplayAlert("Hata", "Soyad alanı boş bırakılamaz", "Tamam");
                return false;
            }

            if (string.IsNullOrWhiteSpace(EmailEntry.Text) || !EmailEntry.Text.Contains("@"))
            {
                DisplayAlert("Hata", "Geçerli bir e-mail adresi giriniz", "Tamam");
                return false;
            }

            if (BloodGroupPicker.SelectedItem == null)
            {
                DisplayAlert("Hata", "Kan grubu seçiniz", "Tamam");
                return false;
            }

            if (string.IsNullOrWhiteSpace(PasswordEntry.Text) || PasswordEntry.Text.Length < 6)
            {
                DisplayAlert("Hata", "Şifre en az 6 karakter olmalıdır", "Tamam");
                return false;
            }

            if (PasswordEntry.Text != ConfirmPasswordEntry.Text)
            {
                DisplayAlert("Hata", "Şifreler eşleşmiyor", "Tamam");
                return false;
            }

            return true;
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//UserLoginPage");
        }
    }
}