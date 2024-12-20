using HAN.Services.DTOs;

namespace HAN.Services.Interfaces;

public interface IScheduleService
{
    public ScheduleDto CreateSchedule(ScheduleDto schedule, int courseId);
    public ScheduleDto GetScheduleById(int id);
}