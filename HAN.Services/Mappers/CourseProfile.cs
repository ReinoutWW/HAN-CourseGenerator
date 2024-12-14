using AutoMapper;
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
        CreateMap<CreateCourseDto, Course>();
        CreateMap<Data.Entities.Course, CourseResponseDto>();
        CreateMap<CourseResponseDto, Domain.Entities.Course>();
        
        CreateMap<CreateCourseComponentDto, CourseComponent>();
        CreateMap<CourseComponent, CourseComponentResponseDto>();
        
        CreateMap<CreateEvlDto, Evl>();
        CreateMap<Evl, Domain.Entities.Evl>();
        CreateMap<Evl, EvlResponseDto>();
        
        CreateMap<CreateUserDto, User>();
        CreateMap<User, UserResponseDto>();

        CreateMap<Lesson, Domain.Entities.Lesson>();
        CreateMap<Exam, Domain.Entities.Exam>();
    }
}