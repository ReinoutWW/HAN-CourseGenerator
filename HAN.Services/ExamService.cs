using AutoMapper;
using HAN.Data.Entities.CourseComponents;
using HAN.Repositories.Interfaces;
using HAN.Services.DTOs.CourseComponents;
using HAN.Services.Validation;

namespace HAN.Services;

public class ExamService(ICourseComponentRepository<Exam> repository, 
    IValidationService validationService, 
    IMapper mapper) : AbstractCourseComponentService<ExamDto, Exam>(
        repository, 
        validationService, 
        mapper)
{
    
}