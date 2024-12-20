using AutoMapper;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using HAN.Services.DTOs;
using HAN.Services.DTOs.CourseComponents;
using HAN.Services.Interfaces;
using HAN.Services.Validation;

namespace HAN.Services;

public abstract class CourseComponentService<TDto, TEntity>(
    ICourseComponentRepository<TEntity> repository,
    IValidationService validationService,
    IMapper mapper) : 
            ICourseComponentService<TDto> 
                where TDto : CourseComponentDto 
                where TEntity : CourseComponent
{
    public TDto CreateCourseComponent(TDto courseComponentDto)
    {
        validationService.Validate(courseComponentDto);

        // Map DTO to entity
        var entity = mapper.Map<TEntity>(courseComponentDto);

        // Persist entity
        repository.Add(entity);

        // Map entity back to DTO
        return mapper.Map<TDto>(entity);
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
    
    public List<TDto> GetAllCourseComponentsByCourseId(int courseId)
    {
        var entities = repository.GetAll();

        return mapper.Map<List<TDto>>(entities);
    }

    public TDto GetCourseComponentById(int id)
    {
        var entity = repository.GetById(id);

        if (entity == null)
            throw new KeyNotFoundException($"CourseComponent with ID {id} not found.");

        return mapper.Map<TDto>(entity);
    }
}
