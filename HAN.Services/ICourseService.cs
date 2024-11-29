using HAN.Domain.Entities;
using HAN.Domain.Entities.CourseComponents;

namespace HAN.Services;

public interface ICourseService
{
    // Create
    Course CreateCourse(Course course);
    EVL CreateEVL(EVL evl);
    CourseComponent CreateCourseComponent(CourseComponent component);
    
    // Assign
    void AddEVLToCourse(int courseId, int evlId);
    void AddCourseComponentToCourse(int courseId, int componentId);
}