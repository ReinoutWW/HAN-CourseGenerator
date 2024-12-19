using AutoMapper;
using HAN.Repositories.Interfaces;
using HAN.Services.DTOs;
using HAN.Services.Validation;

namespace HAN.Services;

public class CourseComponentService(
    ICourseComponentRepository courseComponentRepository,
    IEvlRepository evlRepository,
    IMapper mapper,
    IValidationService validationService
) : ICourseComponentService
{
    public CourseComponentDto CreateCourseComponent(CourseComponentDto courseComponent)
    {
        throw new NotImplementedException();
    }

    public CourseComponentDto GetCourseComponentById(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<EvlDto> GetEvls(int courseComponentId)
    {
        throw new NotImplementedException();
    }

    public void AddEvlToCourseComponent(int courseComponentId, int evlId)
    {
        throw new NotImplementedException();
    }
}