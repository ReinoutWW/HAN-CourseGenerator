﻿using AutoMapper;
using HAN.Data.Entities;
using HAN.Data.Entities.CourseComponents;
using HAN.Services.DTOs;

namespace HAN.Services.Mappers;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        // From - To (Easy to f* up)
        
        // Persistence
        CreateMap<CourseDto, Course>();
        CreateMap<Course, CourseDto>();
        
        CreateMap<CourseComponentDto, CourseComponent>();
        CreateMap<CourseComponent, CourseComponentDto>();
        
        CreateMap<EvlDto, Evl>();
        CreateMap<Evl, EvlDto>();
        
        CreateMap<UserDto, User>();
        CreateMap<User, UserDto>();
    }
}