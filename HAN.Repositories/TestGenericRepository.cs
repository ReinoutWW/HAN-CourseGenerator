using HAN.Data;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;

namespace HAN.Repositories;

public class TestGenericRepository(AppDbContext context) : GenericRepository<GenericTest>(context)
{
    
}