using AutoMapper;
using HAN.Data.Entities;
using HAN.Data.Entities.CourseComponents;
using HAN.Services.DTOs;
using File = HAN.Data.Entities.File;

namespace HAN.Services.Mappers;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        // From - To (Easy to f* up)
        
        // Persistence
        CreateMap<CourseDto, Course>();
        CreateMap<Course, CourseDto>();
        
        CreateMap<EvlDto, Evl>();
        CreateMap<Evl, EvlDto>();
        
        CreateMap<UserDto, User>();
        CreateMap<User, UserDto>();
        
        CreateMap<FileDto, File>();
        CreateMap<File, FileDto>();
    }
}