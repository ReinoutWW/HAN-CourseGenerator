using HAN.Services.DTOs;

namespace HAN.Services.Interfaces;

public interface IScheduleService
{
        public ScheduleDto AddSchedule(ScheduleDto schedule, int courseId);
        public ScheduleDto GetScheduleById(int id);
        public ScheduleDto UpdateSchedule(int courseId, ScheduleDto scheduleDto);
}