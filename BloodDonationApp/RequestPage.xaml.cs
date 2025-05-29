using BloodDonationApp.Helpers;
using BloodDonationApp.Models;
using BloodDonationApp.Services;

namespace BloodDonationApp;

public partial class RequestPage : ContentPage
{
    private readonly IApiService _apiService;

    public RequestPage(IApiService apiService)
    {
        InitializeComponent();
        _apiService = apiService;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadUserInfo();
    }

    private void LoadUserInfo()
    {
        var user = SessionManager.GetUser();
        if (user != null)
        {
            UserInfoLabel.Text = $"{user.Name} {user.SurName} - {user.Mail}";
        }
        else
        {
            UserInfoLabel.Text = "Kullanıcı bilgisi bulunamadı";
        }
    }

    private async void OnCreateRequestClicked(object sender, EventArgs e)
    {
        try
        {
            if (!ValidateForm())
                return;

            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            CreateRequestButton.IsEnabled = false;

            var user = SessionManager.GetUser();
            if (user == null)
            {
                await DisplayAlert("Hata", "Kullanıcı oturumu bulunamadı. Lütfen tekrar giriş yapın.", "Tamam");
                return;
            }

            // Kan grubu mapping
            var bloodTypeMapping = new Dictionary<string, string>
            {
                { "A Rh+", "APositive" },
                { "A Rh-", "ANegative" },
                { "B Rh+", "BPositive" },
                { "B Rh-", "BNegative" },
                { "AB Rh+", "ABPositive" },
                { "AB Rh-", "ABNegative" },
                { "O Rh+", "OPositive" },
                { "O Rh-", "ONegative" }
            };

            var selectedBloodType = BloodTypePicker.SelectedItem?.ToString();
            var apiBloodType = bloodTypeMapping.ContainsKey(selectedBloodType) 
                ? bloodTypeMapping[selectedBloodType] 
                : selectedBloodType;

            var requestData = new
            {
                bloodType = apiBloodType,
                userId = user.Id,
                urgencyLevel = UrgencyLevelPicker.SelectedItem?.ToString(),
                description = DescriptionEditor.Text?.Trim()
            };

            var result = await _apiService.CreateRequestAsync(requestData);

            if (result.IsSuccess)
            {
                await DisplayAlert("Başarılı", "Kan isteğiniz başarıyla oluşturuldu.", "Tamam");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await DisplayAlert("Hata", $"İstek oluşturulamadı: {result.ErrorMessage}", "Tamam");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hata", $"Beklenmeyen bir hata oluştu: {ex.Message}", "Tamam");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            CreateRequestButton.IsEnabled = true;
        }
    }

    private bool ValidateForm()
    {
        if (BloodTypePicker.SelectedItem == null)
        {
            DisplayAlert("Hata", "Lütfen kan grubu seçiniz.", "Tamam");
            return false;
        }

        if (UrgencyLevelPicker.SelectedItem == null)
        {
            DisplayAlert("Hata", "Lütfen aciliyet seviyesi seçiniz.", "Tamam");
            return false;
        }

        if (string.IsNullOrWhiteSpace(DescriptionEditor.Text))
        {
            DisplayAlert("Hata", "Lütfen açıklama yazınız.", "Tamam");
            return false;
        }

        if (DescriptionEditor.Text.Trim().Length < 10)
        {
            DisplayAlert("Hata", "Açıklama en az 10 karakter olmalıdır.", "Tamam");
            return false;
        }

        return true;
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        bool result = await DisplayAlert("İptal", "İsteği iptal etmek istediğinizden emin misiniz?", "Evet", "Hayır");
        if (result)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}