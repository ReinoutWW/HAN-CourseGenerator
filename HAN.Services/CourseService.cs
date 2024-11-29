using AutoMapper;
using HAN.Domain.Entities;
using HAN.Domain.Entities.CourseComponents;
using HAN.Repositories;
using HAN.Services.DTOs;
using HAN.Services.Validators;
using ValidationException = HAN.Services.Exceptions.ValidationException;

namespace HAN.Services;

public class CourseService(ICourseRepository courseRepository, IMapper mapper) : ICourseService
{
    public CourseResponseDto CreateCourse(CreateCourseDto course)
    {
        ValidateCreateCourseDto(course);
        
        var courseResult = courseRepository.CreateCourse(
                mapper.Map<HAN.Data.Entities.Course>(course)
            );
        
        return mapper.Map<CourseResponseDto>(courseResult);
    }

    private static void ValidateCreateCourseDto(CreateCourseDto domainCourse)
    {
        var validationResult = new CourseValidator().Validate(domainCourse);

        if (!validationResult.IsValid)
            throw new ValidationException("The provided course is invalid.", validationResult.Errors);
    }

    public EVL CreateEVL(EVL evl)
    {
        throw new NotImplementedException();
    }

    public CourseComponent CreateCourseComponent(CourseComponent component)
    {
        throw new NotImplementedException();
    }

    public void AddEVLToCourse(int courseId, int evlId)
    {
        throw new NotImplementedException();
    }

    public void AddCourseComponentToCourse(int courseId, int componentId)
    {
        throw new NotImplementedException();
    }
}