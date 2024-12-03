using AutoMapper;
using HAN.Services.DTOs;
using DbEntityEvl = HAN.Data.Entities.Evl;
using DbCourseEvl = HAN.Data.Entities.Course;
using Evl = HAN.Domain.Entities.Evl;
using Course = HAN.Domain.Entities.Course;

namespace HAN.Services.Mappers;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        // From - To (Easy to f* up)
        
        // Persistence
        CreateMap<CreateCourseDto, DbCourseEvl>();
        CreateMap<CreateCourseDto, Course>();
        CreateMap<DbCourseEvl, Course>();
        CreateMap<DbCourseEvl, CourseResponseDto>();
        
        CreateMap<CreateEvlDto, DbEntityEvl>();
        CreateMap<CreateEvlDto, Evl>();
        CreateMap<DbEntityEvl, Evl>();
        CreateMap<DbEntityEvl, EvlResponseDto>();
    }
}