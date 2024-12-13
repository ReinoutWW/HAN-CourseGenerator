using HAN.Data;
using HAN.Data.Entities;
using HAN.Repositories;

namespace HAN.Tests.Mocks;

public class ExampleGenericRepository(AppDbContext context) : GenericRepository<ExampleEntity>(context)
{
    
}