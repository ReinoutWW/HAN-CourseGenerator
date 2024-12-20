﻿using AutoMapper;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using HAN.Services.DTOs.CourseComponents;
using HAN.Services.Validation;

namespace HAN.Services;

public class CourseComponentService(ICourseComponentRepository<CourseComponent> repository, IValidationService validationService, IMapper mapper) : AbstractCourseComponentService<CourseComponentDto, CourseComponent>(repository, validationService, mapper)
{
    
}