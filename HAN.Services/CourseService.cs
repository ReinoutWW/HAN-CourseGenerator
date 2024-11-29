using AutoMapper;
using HAN.Domain.Entities;
using HAN.Domain.Entities.CourseComponents;
using HAN.Repositories;
using HAN.Services.DTOs;

namespace HAN.Services;

public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public CourseService(ICourseRepository courseRepository, IMapper mapper)
    {
        _courseRepository = courseRepository;
        _mapper = mapper;
    }

    public CourseResponseDto CreateCourse(CreateCourseDto course)
    {
        var courseEntity = _mapper.Map<HAN.Data.Entities.Course>(course);
        var courseResult = _courseRepository.CreateCourse(courseEntity);
        
        return _mapper.Map<CourseResponseDto>(courseResult);
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