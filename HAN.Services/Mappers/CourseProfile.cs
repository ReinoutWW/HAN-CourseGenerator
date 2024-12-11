using AutoMapper;
using HAN.Data.Entities;
using HAN.Services.DTOs;

namespace HAN.Services.Mappers;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        // From - To (Easy to f* up)
        
        // Persistence
        CreateMap<CreateCourseDto, Course>();
        CreateMap<Course, CourseResponseDto>();
        
        CreateMap<CreateCourseComponentDto, CourseComponent>();
        CreateMap<CourseComponent, CourseComponentResponseDto>();
        
        CreateMap<CreateEvlDto, Evl>();
        CreateMap<Evl, EvlResponseDto>();
        
        CreateMap<CreateUserDto, User>();
        CreateMap<User, UserResponseDto>();
    }
}