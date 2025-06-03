using Microsoft.Maui.Controls;

namespace BloodDontaionAppV2
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnUserLoginClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//UserLoginPage");
        }

        private async void OnUserRegisterClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//UserRegisterPage");
        }

        private async void OnAdminLoginClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Bilgi", "Admin girişi henüz aktif değil", "Tamam");
        }
    }
}