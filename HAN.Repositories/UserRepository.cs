using HAN.Data;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;

namespace HAN.Repositories;

public class UserRepository(AppDbContext context) : GenericRepository<User>(context), IUserRepository
{  }