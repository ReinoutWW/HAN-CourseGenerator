using HAN.Data;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HAN.Repositories;

public class ScheduleRepository(AppDbContext context) : GenericRepository<Schedule>(context), IScheduleRepository
{
    
}