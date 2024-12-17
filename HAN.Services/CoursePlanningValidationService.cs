using AutoMapper;
using HAN.Domain.Entities;
using HAN.Repositories.Interfaces;

namespace HAN.Services;

public class CoursePlanningValidationService : ICoursePlanningValidationService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IMapper _mapper;

    public CoursePlanningValidationService(ICourseRepository courseRepository, IMapper mapper)
    {
        _courseRepository = courseRepository;
        _mapper = mapper;
    }
    
    public bool ValidateCoursePlanning(int id)
    {
        var course = _courseRepository.GetById(id);
        var courseEntity = _mapper.Map<Course>(course);
        var evls = courseEntity.Evls.ToList();
        
        return courseEntity.Schedule.IsValid(evls);
    }
}