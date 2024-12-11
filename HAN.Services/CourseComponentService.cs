using AutoMapper;
using HAN.Repositories.Interfaces;
using HAN.Services.DTOs;
using HAN.Services.Validation;

namespace HAN.Services;

public interface ICourseComponentService
{
    // Create
    CourseComponentResponseDto CreateCourseComponent(CreateCourseComponentDto courseComponent);
    CourseComponentResponseDto GetCourseComponentById(int id);
    IEnumerable<EvlResponseDto> GetEvls(int courseComponentId);
    
    // Assign
    void AddEvlToCourseComponent(int courseComponentId, int evlId);
}

public class CourseComponentService(
    ICourseComponentRepository courseComponentRepository,
    IEvlRepository evlRepository,
    IMapper mapper,
    IValidationService validationService
) : ICourseComponentService
{
    public CourseComponentResponseDto CreateCourseComponent(CreateCourseComponentDto courseComponent)
    {
        throw new NotImplementedException();
    }

    public CourseComponentResponseDto GetCourseComponentById(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<EvlResponseDto> GetEvls(int courseComponentId)
    {
        throw new NotImplementedException();
    }

    public void AddEvlToCourseComponent(int courseComponentId, int evlId)
    {
        throw new NotImplementedException();
    }
}