using HAN.Services.DTOs;
using HAN.Services.DTOs.CourseComponents;

namespace HAN.Services;

public interface ICourseComponentService
{
    LessonDto CreateLesson(LessonDto lesson);
    
    
    void AddEvlToCourseComponent(int courseComponentId, int evlId);
}