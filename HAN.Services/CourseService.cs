using AutoMapper;
using HAN.Domain.Entities;
using HAN.Domain.Entities.CourseComponents;
using HAN.Repositories;
using HAN.Services.Attributes;
using HAN.Services.DTOs;

namespace HAN.Services;

public class CourseService(ICourseRepository courseRepository, IMapper mapper) : ICourseService
{
    [ValidateEntities]
    public CourseResponseDto CreateCourse(CreateCourseDto course)
    {
        var courseResult = courseRepository.CreateCourse(
                mapper.Map<HAN.Data.Entities.Course>(course)
            );
        
        return mapper.Map<CourseResponseDto>(courseResult);
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