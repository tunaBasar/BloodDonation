using System;

namespace BloodDonationAppV2.Services
{
    public class SessionManager
    {
        private static SessionManager _instance;
        private static readonly object _lock = new object();

        public static SessionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new SessionManager();
                    }
                }
                return _instance;
            }
        }

        private SessionManager() { }

        public UserInfo CurrentUser { get; private set; }
        public bool IsLoggedIn => CurrentUser != null;

        public void SetCurrentUser(UserInfo user)
        {
            CurrentUser = user;
        }

        public void Logout()
        {
            CurrentUser = null;
        }
    }

    public class UserInfo
    {
        public string TcKimlikNo { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Email { get; set; }
        public string KanGrubu { get; set; }
        public int Id { get; set; }
    }
}