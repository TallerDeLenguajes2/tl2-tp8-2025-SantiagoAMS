public interface IAuthenticationService
{
    bool Login(string user, string pass);
    void Logout();
    bool IsAuthenticated();
    bool HasAccessLevel(string requiredAccessLevel);
}