using AutoMapper;
using HAN.Domain.Entities;
using HAN.Domain.Entities.CourseComponents;
using HAN.Repositories;
using HAN.Repositories.Interfaces;
using HAN.Services.Attributes;
using HAN.Services.DTOs;

namespace HAN.Services;

public class CourseService(ICourseRepository courseRepository, IEvlRepository evlRepository, IMapper mapper) : ICourseService
{
    [ValidateEntities]
    public CourseResponseDto CreateCourse(CreateCourseDto course)
    {
        var courseResult = courseRepository.CreateCourse(
                mapper.Map<HAN.Data.Entities.Course>(course)
            );
        
        return mapper.Map<CourseResponseDto>(courseResult);
    }

    public CourseResponseDto GetCourseById(int id)
    {
        var course = courseRepository.GetCourseById(id);
        return mapper.Map<CourseResponseDto>(course);
    }

    public IEnumerable<CourseResponseDto> GetEvls(int courseId)
    {
        var evls = courseRepository.GetEvlsByCourseId(courseId);
        
        return mapper.Map<IEnumerable<CourseResponseDto>>(evls);
    }

    public void AddEVLToCourse(int courseId, int evlId)
    {
        if (!courseRepository.CourseExists(courseId))
            throw new KeyNotFoundException($"Course with id {courseId} does not exist");

        if (!evlRepository.EvlExists(evlId))
            throw new KeyNotFoundException($"Evl with id {evlId} does not exist");

        courseRepository.AddEvlToCourse(courseId, evlId);
    }

    public CourseComponent CreateCourseComponent(CourseComponent component)
    {
        throw new NotImplementedException();
    }

    public void AddCourseComponentToCourse(int courseId, int componentId)
    {
        throw new NotImplementedException();
    }
}