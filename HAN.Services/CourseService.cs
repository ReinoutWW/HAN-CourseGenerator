using AutoMapper;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using HAN.Services.DTOs;
using HAN.Services.Validation;

namespace HAN.Services;

public interface ICourseService
{
    // Create
    CourseResponseDto CreateCourse(CreateCourseDto course);
    CourseResponseDto GetCourseById(int id);
    IEnumerable<EvlResponseDto> GetEvls(int courseId);
    
    // Assign
    void AddEvlToCourse(int courseId, int evlId);
}

public class CourseService(ICourseRepository courseRepository, IEvlRepository evlRepository, IMapper mapper, IValidationService validationService) : ICourseService
{
    public CourseResponseDto CreateCourse(CreateCourseDto course)
    {
        var courseEntity = mapper.Map<Course>(course);
        validationService.Validate(courseEntity);
        
        courseRepository.Add(courseEntity);
        
        return mapper.Map<CourseResponseDto>(courseEntity);
    }

    public CourseResponseDto GetCourseById(int id)
    {
        var course = courseRepository.GetById(id);
        return mapper.Map<CourseResponseDto>(course);
    }

    public IEnumerable<EvlResponseDto> GetEvls(int courseId)
    {
        var evls = courseRepository.GetEvlsByCourseId(courseId);
        return mapper.Map<IEnumerable<EvlResponseDto>>(evls);
    }

    public void AddEvlToCourse(int courseId, int evlId)
    {
        if (!courseRepository.Exists(courseId))
            throw new KeyNotFoundException($"Course with id {courseId} does not exist");

        if (!evlRepository.Exists(evlId))
            throw new KeyNotFoundException($"Evl with id {evlId} does not exist");

        courseRepository.AddEvlToCourse(courseId, evlId);
    }
}