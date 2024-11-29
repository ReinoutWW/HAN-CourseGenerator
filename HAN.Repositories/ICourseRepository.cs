using HAN.Data.Entities;

namespace HAN.Repositories;

public interface ICourseRepository
{
    bool SaveChanges();
    
    Course CreateCourse(Course course);
    Course GetCourseById(int id);
    IEnumerable<Course> GetAllCourses();
}