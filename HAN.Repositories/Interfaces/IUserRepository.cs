using HAN.Data.Entities;

namespace HAN.Repositories.Interfaces;

public interface IUserRepository
{
    bool SaveChanges();
    
    IEnumerable<User> GetAllUsers();
    User GetUserById(int id);
    User CreateUser(User user);
}