using AutoMapper;
using HAN.Services.DTOs;
using DbEntityEvl = HAN.Data.Entities.Evl;
using DbCourse = HAN.Data.Entities.Course;
using DbCourseComponent = HAN.Data.Entities.CourseComponent;
using DbUser = HAN.Data.Entities.User;
using Course = HAN.Domain.Entities.Course;
using CourseComponent = HAN.Domain.Entities.CourseComponents.CourseComponent;

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
        
        CreateMap<CreateCourseComponentDto, DbCourseComponent>();
        CreateMap<CreateCourseComponentDto, CourseComponent>();
        CreateMap<DbCourseComponent, CourseComponent>();
        CreateMap<DbCourseComponent, CourseComponentResponseDto>();
        
        CreateMap<CreateEvlDto, DbEntityEvl>();
        CreateMap<DbEntityEvl, EvlResponseDto>();
        
        CreateMap<CreateUserDto, DbUser>();
        CreateMap<DbUser, UserResponseDto>();
    }
}