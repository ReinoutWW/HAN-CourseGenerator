using HAN.Domain.Entities;
using HAN.Domain.Entities.CourseComponents;
using HAN.Services.DTOs;

namespace HAN.Services;

public interface ICourseService
{
    // Create
    CourseResponseDto CreateCourse(CreateCourseDto course);
    CourseComponent CreateCourseComponent(CourseComponent component);
    
    // Assign
    void AddEVLToCourse(int courseId, int evlId);
    void AddCourseComponentToCourse(int courseId, int componentId);
}