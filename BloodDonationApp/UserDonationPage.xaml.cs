using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;

namespace BloodDonationApp
{
    public partial class UserDonationPage : ContentPage
    {
        private ObservableCollection<DonationDto> _allDonations;

        public UserDonationPage()
        {
            InitializeComponent();
            LoadUserDonations();
        }

        private void LoadUserDonations()
        {
            _allDonations = new ObservableCollection<DonationDto>
            {
                new DonationDto
                {
                    BloodType = "A+",
                    PatientName = "Ahmet Yılmaz",
                    HospitalName = "Acıbadem Hastanesi",
                    Description = "Acil ihtiyaç.",
                    DonationDate = DateTime.Now.AddDays(-5)
                },
                new DonationDto
                {
                    BloodType = "B-",
                    PatientName = "Mehmet Can",
                    HospitalName = "Memorial Hastanesi",
                    Description = "Düzenli bağış.",
                    DonationDate = DateTime.Now.AddDays(-10)
                }
            };

            UserDonationsCollectionView.ItemsSource = _allDonations;
        }

        private void OnSearchButtonPressed(object sender, EventArgs e)
        {
            var keyword = UserDonationSearchBar.Text?.ToLower() ?? "";

            var filtered = _allDonations.Where(d =>
                d.BloodType.ToLower().Contains(keyword) ||
                d.PatientName.ToLower().Contains(keyword) ||
                d.HospitalName.ToLower().Contains(keyword) ||
                d.Description.ToLower().Contains(keyword)
            );

            UserDonationsCollectionView.ItemsSource = new ObservableCollection<DonationDto>(filtered);
        }
    }

    // DTO modelin burada yer alabilir
    public class DonationDto
    {
        public string BloodType { get; set; }
        public string PatientName { get; set; }
        public string HospitalName { get; set; }
        public string Description { get; set; }
        public DateTime DonationDate { get; set; }
    }
}
