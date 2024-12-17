using AutoMapper;
using HAN.Domain.Entities;

namespace HAN.Services;

public class CourseValidationService(ICourseService courseService, IMapper mapper) : ICourseValidationService
{
    public bool ValidateCourse(int courseId)
    {
        var course = courseService.GetCourseById(courseId);

        if(course == null)
            throw new KeyNotFoundException("Course with id {courseId} not found.");
        
        var courseDomainEntity = mapper.Map<Course>(course);

        return courseDomainEntity.Validate().IsValid;
    }
}