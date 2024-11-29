using HAN.Data.Entities;

namespace HAN.Repositories;

public interface IUserRepository
{
    bool SaveChanges();
    
    IEnumerable<User> GetAllUsers();
    User GetUserById(int id);
    User CreateUser(User user);
}