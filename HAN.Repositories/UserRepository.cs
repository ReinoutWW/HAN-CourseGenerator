using HAN.Data;
using HAN.Data.Entities;

namespace HAN.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public User CreateUser(User user)
    {
        if(user == null) 
        {
            throw new ArgumentException($"User can not be null. User: {nameof(user)}");
        }

        return _context.Users.Add(user).Entity;
    }

    public IEnumerable<User> GetAllUsers()
    {
        return [.. _context.Users];
    }

    public User GetUserById(int id)
    {
        return _context.Users.FirstOrDefault(p => p.Id == id) ?? throw new KeyNotFoundException();
    }

    public bool SaveChanges()
    {
        return _context.SaveChanges() >= 0;
    }
}