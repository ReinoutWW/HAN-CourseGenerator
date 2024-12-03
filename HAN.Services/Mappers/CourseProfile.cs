﻿using AutoMapper;
using HAN.Domain.Entities.User;
using HAN.Services.DTOs;
using DbEntityEvl = HAN.Data.Entities.Evl;
using DbCourse = HAN.Data.Entities.Course;
using DbUser = HAN.Data.Entities.User;
using Evl = HAN.Domain.Entities.Evl;
using Course = HAN.Domain.Entities.Course;

namespace HAN.Services.Mappers;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        // From - To (Easy to f* up)
        
        // Persistence
        CreateMap<CreateCourseDto, DbCourse>();
        CreateMap<CreateCourseDto, Course>();
        CreateMap<DbCourse, Course>();
        CreateMap<DbCourse, CourseResponseDto>();
        
        CreateMap<CreateEvlDto, DbEntityEvl>();
        CreateMap<CreateEvlDto, Evl>();
        CreateMap<DbEntityEvl, Evl>();
        CreateMap<DbEntityEvl, EvlResponseDto>();
        
        CreateMap<CreateUserDto, DbUser>();
        CreateMap<CreateUserDto, User>();
        CreateMap<DbUser, User>();
        CreateMap<DbUser, UserResponseDto>();
    }
}