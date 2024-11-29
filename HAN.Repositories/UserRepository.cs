using HAN.Data;
using HAN.Data.Entities;

namespace HAN.Repositories;

public class UserRepository(AppDbContext context) : RepositoryBase(context), IUserRepository
{
    public User CreateUser(User user)
    {
        if(user == null) 
        {
            throw new ArgumentException($"User can not be null. User: {nameof(user)}");
        }

        return Context.Users.Add(user).Entity;
    }

    public IEnumerable<User> GetAllUsers()
    {
        return [.. Context.Users];
    }

    public User GetUserById(int id)
    {
        return Context.Users.FirstOrDefault(p => p.Id == id) ?? throw new KeyNotFoundException();
    }
}