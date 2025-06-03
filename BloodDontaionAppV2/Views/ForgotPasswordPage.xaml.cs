using Microsoft.Maui.Controls;

namespace BloodDonationAppV2.Views
{
    public partial class ForgotPasswordPage : ContentPage
    {
        public ForgotPasswordPage()
        {
            InitializeComponent();
        }

        private async void OnResetClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TcEntry.Text) || string.IsNullOrWhiteSpace(EmailEntry.Text))
            {
                await DisplayAlert("Hata", "Lütfen tüm alanları doldurunuz", "Tamam");
                return;
            }

            if (TcEntry.Text.Length != 11)
            {
                await DisplayAlert("Hata", "TC Kimlik Numarası 11 haneli olmalıdır", "Tamam");
                return;
            }

            if (!EmailEntry.Text.Contains("@"))
            {
                await DisplayAlert("Hata", "Geçerli bir e-mail adresi giriniz", "Tamam");
                return;
            }

            ResetBtn.IsEnabled = false;
            ResetBtn.Text = "Gönderiliyor...";

            try
            {
                await Task.Delay(2000);

                await DisplayAlert("Başarılı", 
                    "Şifre sıfırlama linki e-mail adresinize gönderildi. Lütfen e-mail kutunuzu kontrol ediniz.", 
                    "Tamam");
                
                await Shell.Current.GoToAsync("//UserLoginPage");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hata", $"Bir hata oluştu: {ex.Message}", "Tamam");
            }
            finally
            {
                ResetBtn.IsEnabled = true;
                ResetBtn.Text = "Şifre Sıfırlama Linki Gönder";
            }
        }

        private async void OnBackToLoginClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//UserLoginPage");
        }
    }
}