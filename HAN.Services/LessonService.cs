using AutoMapper;
using HAN.Data.Entities.CourseComponents;
using HAN.Repositories.Interfaces;
using HAN.Services.DTOs.CourseComponents;
using HAN.Services.Validation;

namespace HAN.Services;

public class LessonService(ICourseComponentRepository<Lesson> repository, 
    IValidationService validationService, 
    IMapper mapper,
    IEvlRepository evlRepository) : AbstractCourseComponentService<LessonDto, Lesson>(
        repository, 
        validationService, 
        mapper,
        evlRepository)
{
    
}