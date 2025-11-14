namespace interfaces;
using models;

public interface IUserRepository
{
    Usuario GetUser(string username, string password);
}