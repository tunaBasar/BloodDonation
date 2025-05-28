using BloodDonationApp.Models;
using BloodDonationApp.Services;
namespace BloodDonationApp;

public partial class UserRegisterPage : ContentPage
{
    private readonly IApiService _apiService;

    public UserRegisterPage()
    {
        InitializeComponent();
        _apiService = new ApiService();
    }


    private async void OnLoginButtonClicked(object sender, EventArgs e)
    {
        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            var loginRequest = new LoginRequestDto
            {
                Tc = EmailEntry.Text?.Trim(),
                Password = PasswordEntry.Text
            };

            if (string.IsNullOrWhiteSpace(loginRequest.Tc) || string.IsNullOrWhiteSpace(loginRequest.Password))
            {
                await DisplayAlert("Hata", "Tc ve şifre alanları boş olamaz.", "Tamam");
                return;
            }
            var userResponse = await _apiService.LoginAsync(loginRequest);

            if (userResponse != null)
            {
                await Shell.Current.GoToAsync($"//UserHomePage", true, new Dictionary<string, object>
                {
                    ["UserData"] = userResponse
                });
            }
        }
        catch (HttpRequestException httpEx)
        {
            await DisplayAlert("Giriş Hatası", "Email veya şifre hatalı.", "Tamam");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", "Bir hata oluştu. Lütfen tekrar deneyin.", "Tamam");
        }
        finally
        {

            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
    }

    private async void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        try
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            RegisterButton.IsEnabled = false;

            if (!ValidateForm())
                return;

            var registerRequest = new RegisterDto
            {
                name = FirstNameEntry.Text?.Trim(),
                surName = LastNameEntry.Text?.Trim(),
                mail = EmailEntry.Text?.Trim(),
                password = PasswordEntry.Text,
                phoneNumber = PhoneEntry.Text?.Trim(),
                tc = TcEntry.Text?.Trim(),
                BloodType = Enum.Parse<BloodType>(BloodTypePicker.SelectedItem?.ToString())
            };

            var (isSuccess, errorMessage) = await _apiService.RegisterAsync(registerRequest);

            if (isSuccess)
            {
                await DisplayAlert("Başarılı", "Kayıt işlemi başarıyla tamamlandı.", "Tamam");
                await Shell.Current.GoToAsync("//UserLoginPage");
            }
            else
            {
                await DisplayAlert("Hata", $"Kayıt başarısız. {errorMessage}", "Tamam");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Bilinmeyen bir hata oluştu: {ex.Message}", "Tamam");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            RegisterButton.IsEnabled = true;
        }
    }

    private bool ValidateForm()
    {
        // Zorunlu alanlar kontrolü
        if (string.IsNullOrWhiteSpace(FirstNameEntry.Text))
        {
            DisplayAlert("Hata", "Ad alanı boş olamaz.", "Tamam");
            return false;
        }

        if (string.IsNullOrWhiteSpace(LastNameEntry.Text))
        {
            DisplayAlert("Hata", "Soyad alanı boş olamaz.", "Tamam");
            return false;
        }

        if (string.IsNullOrWhiteSpace(PhoneEntry.Text))
        {
            DisplayAlert("Hata", "Telefon numarası boş olamaz.", "Tamam");
            return false;
        }

        if (BloodTypePicker.SelectedItem == null)
        {
            DisplayAlert("Hata", "Kan grubu seçiniz.", "Tamam");
            return false;
        }

        if (string.IsNullOrWhiteSpace(PasswordEntry.Text) || PasswordEntry.Text.Length < 6)
        {
            DisplayAlert("Hata", "Şifre en az 6 karakter olmalıdır.", "Tamam");
            return false;
        }

        if (PasswordEntry.Text != ConfirmPasswordEntry.Text)
        {
            DisplayAlert("Hata", "Şifreler eşleşmiyor.", "Tamam");
            return false;
        }

        if (!IsValidPhoneNumber(PhoneEntry.Text))
        {
            DisplayAlert("Hata", "Geçerli bir telefon numarası girin (05XX XXX XX XX).", "Tamam");
            return false;
        }

        return true;
    }

    private bool IsValidPhoneNumber(string phone)
    {
        var cleanPhone = phone.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
        return cleanPhone.Length == 11 && cleanPhone.StartsWith("05");
    }

    private async void OnPrivacyPolicyTapped(object sender, EventArgs e)
    {
        await DisplayAlert("Gizlilik Politikası", "Gizlilik politikası detayları burada gösterilecek.", "Tamam");
    }

    private async void OnLoginTapped(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//UserLoginPage");
    }
}


