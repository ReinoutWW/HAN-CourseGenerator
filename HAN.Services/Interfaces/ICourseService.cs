using HAN.Services.DTOs;

namespace HAN.Services.Interfaces;

public interface ICourseService
{
    CourseDto CreateCourse(CourseDto course);
    CourseDto GetCourseById(int id);
    IEnumerable<EvlDto> GetEvls(int courseId);
    public ScheduleDto AddSchedule(ScheduleDto scheduleDto, int courseId);
    public ScheduleDto GetScheduleById(int id);
    public ScheduleDto UpdateSchedule(int courseId, ScheduleDto scheduleDto);
    void AddEvlToCourse(int courseId, int evlId);
    public List<CourseDto> GetAllCourses();
}