using HAN.Services.DTOs;

namespace HAN.Services.Interfaces;

public interface ICourseService
{
    CourseDto CreateCourse(CourseDto course);
    CourseDto UpdateCourse(CourseDto course);
    CourseDto GetCourseById(int id);
    IEnumerable<EvlDto> GetEvls(int courseId);
    void AddEvlToCourse(int courseId, int evlId);
    public List<CourseDto> GetAllCourses();
}