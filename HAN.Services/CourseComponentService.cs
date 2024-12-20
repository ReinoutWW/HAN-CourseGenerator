using AutoMapper;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using HAN.Services.DTOs;
using HAN.Services.DTOs.CourseComponents;
using HAN.Services.Interfaces;
using HAN.Services.Mappers;
using HAN.Services.Validation;

namespace HAN.Services;

public class CourseComponentService(
    ICourseComponentRepository repository,
    IValidationService validationService,
    IMapper mapper) : ICourseComponentService
{

    public T CreateCourseComponent<T>(T courseComponentDto) where T : CourseComponentDto
    {
        validationService.Validate(courseComponentDto);

        var entityType = CourseComponentTypeMap.GetEntityType(typeof(T));

        var entity = (CourseComponent)mapper.Map(courseComponentDto, typeof(T), entityType);

        repository.Add(entity);

        return (T)mapper.Map(entity, entityType, typeof(T));
    }

    public void AddEvlToCourseComponent(int courseComponentId, int evlId)
    {
        repository.AddEvlToCourseComponent(courseComponentId, evlId);
    }

    public void AddFileToCourseComponent(int courseComponentId, int fileId)
    {
        repository.AddFileToCourseComponent(courseComponentId, fileId);
    }

    public List<FileDto> GetFilesForCourseComponent(int courseComponentId)
    {
        return repository.GetFilesForCourseComponent(courseComponentId)
            .Select(mapper.Map<FileDto>)
            .ToList();
    }

    public List<EvlDto> GetEvlsForCourseComponent(int courseComponentId)
    {
        return repository.GetEvlsForCourseComponent(courseComponentId)
            .Select(mapper.Map<EvlDto>)
            .ToList();
    }

    public T GetCourseComponentById<T>(int id) where T : CourseComponentDto
    {
        var entityType = CourseComponentTypeMap.GetEntityType(typeof(T));

        var entity = repository.GetById(id);

        return (T)mapper.Map(entity, entityType, typeof(T));
    }
}