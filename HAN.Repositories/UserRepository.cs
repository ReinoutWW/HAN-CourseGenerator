using HAN.Data;
using HAN.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HAN.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public void CreateUser(User user)
    {
        if(user == null) 
        {
            throw new ArgumentException(nameof(user));
        }

        _context.Users.Add(user);
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