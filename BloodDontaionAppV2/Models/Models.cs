using System;
using BloodDonationAppV2.Services;

namespace BloodDonationAppV2.Models
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public UserInfo User { get; set; }
    }

    public class RegisterRequest
    {
        public string TcKimlikNo { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public string Sifre { get; set; }
        public string KanGrubu { get; set; }
    }

    public class RegisterResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class DonationRequest
    {
        public int Id { get; set; }
        public string TcKimlikNo { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string KanGrubu { get; set; }
        public string Aciklama { get; set; }
        public DateTime TarihSaat { get; set; }
        public string Konum { get; set; }
        public string Telefon { get; set; }
        public bool Acil { get; set; }
        public string Durum { get; set; }
    }

    public class CreateDonationRequest
    {
        public string TcKimlikNo { get; set; }
        public string KanGrubu { get; set; }
        public string Aciklama { get; set; }
        public DateTime TarihSaat { get; set; }
        public string Konum { get; set; }
        public string Telefon { get; set; }
        public bool Acil { get; set; }
    }
}