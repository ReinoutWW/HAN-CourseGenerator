using HAN.Data.Entities;

namespace HAN.Repositories.Interfaces;

public interface ICourseRepository
{
    bool SaveChanges();
    
    Course CreateCourse(Course course);
    Course GetCourseById(int id);
    IEnumerable<Course> GetAllCourses();
    IEnumerable<Evl> GetEvlsByCourseId(int id);
    bool CourseExists(int courseId);
    void AddEvlToCourse(int courseId, int evlId);
}