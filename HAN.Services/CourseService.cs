using AutoMapper;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using HAN.Services.DTOs;
using HAN.Services.Interfaces;
using HAN.Services.Validation;

namespace HAN.Services;

public class CourseService(ICourseRepository courseRepository, IEvlRepository evlRepository, IMapper mapper, IValidationService validationService) : ICourseService
{
    public CourseDto CreateCourse(CourseDto course)
    {
        validationService.Validate(course);
        
        var courseEntity = mapper.Map<Course>(course);
        
        courseRepository.Add(courseEntity);

        return mapper.Map<CourseDto>(courseEntity);
    }

    public CourseDto GetCourseById(int id)
    {
        var course = courseRepository.GetById(id);
        return mapper.Map<CourseDto>(course);
    }

    public IEnumerable<EvlDto> GetEvls(int courseId)
    {
        var evls = courseRepository.GetEvlsByCourseId(courseId);
        return mapper.Map<IEnumerable<EvlDto>>(evls);
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