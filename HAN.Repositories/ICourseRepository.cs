using HAN.Data.Entities;

namespace HAN.Repositories;

public interface ICourseRepository
{
    bool SaveChanged();
    
    Course CreateCourse(Course course);
}