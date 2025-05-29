
using BloodDonationApp.Models;

namespace BloodDonationApp.Helpers;

public static class SessionManager
{
    private static UserResponseDto _currentUser;

    public static void SetUser(UserResponseDto user)
    {
        _currentUser = user;
    }

    public static UserResponseDto GetUser()
    {
        return _currentUser;
    }

    public static bool IsUserLoggedIn => _currentUser != null;

    public static void ClearSession()
    {
        _currentUser = null;
    }
}
