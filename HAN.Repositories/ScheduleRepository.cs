using HAN.Data;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;

namespace HAN.Repositories;

public class ScheduleRepository(AppDbContext context) : GenericRepository<Schedule>(context), IScheduleRepository
{
    
}