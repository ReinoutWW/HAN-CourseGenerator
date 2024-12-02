using AutoMapper;
using HAN.Domain.Entities.CourseComponents;
using HAN.Repositories.Interfaces;
using HAN.Services.Attributes;
using HAN.Services.DTOs;

namespace HAN.Services;

public interface ICourseService
{
    // Create
    CourseResponseDto CreateCourse(CreateCourseDto course);
    CourseComponent CreateCourseComponent(CourseComponent component);
    CourseResponseDto GetCourseById(int id);
    IEnumerable<EvlResponseDto> GetEvls(int courseId);
    
    // Assign
    void AddEvlToCourse(int courseId, int evlId);
}


public class CourseService(ICourseRepository courseRepository, IEvlRepository evlRepository, IMapper mapper) : ICourseService
{
    [ValidateEntities]
    public CourseResponseDto CreateCourse(CreateCourseDto course)
    {
        var courseResult = courseRepository.CreateCourse(
                mapper.Map<Data.Entities.Course>(course)
            );
        
        return mapper.Map<CourseResponseDto>(courseResult);
    }

    public CourseResponseDto GetCourseById(int id)
    {
        var course = courseRepository.GetCourseById(id);
        return mapper.Map<CourseResponseDto>(course);
    }

    public IEnumerable<EvlResponseDto> GetEvls(int courseId)
    {
        var evls = courseRepository.GetEvlsByCourseId(courseId);
        
        return mapper.Map<IEnumerable<EvlResponseDto>>(evls);
    }

    public void AddEvlToCourse(int courseId, int evlId)
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
}