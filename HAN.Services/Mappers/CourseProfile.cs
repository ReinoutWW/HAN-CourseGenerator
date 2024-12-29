using AutoMapper;
using HAN.Data.Entities;
using HAN.Data.Entities.CourseComponents;
using HAN.Services.DTOs;
using HAN.Services.DTOs.CourseComponents;
using File = HAN.Data.Entities.File;

namespace HAN.Services.Mappers;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        // From - To (Easy to f* up)
        
        // Persistence
        CreateMap<CourseDto, Course>()
            .ForMember(dest => dest.EvlIds, opt => opt.MapFrom(src => src.Evls.Select(e => e.Id).ToList()));
        CreateMap<Course, CourseDto>();
        
        CreateMap<EvlDto, Evl>();
        CreateMap<Evl, EvlDto>();
        
        CreateMap<UserDto, User>();
        CreateMap<User, UserDto>();
        
        CreateMap<FileDto, File>();
        CreateMap<File, FileDto>();
        
        CreateMap<ScheduleDto, Schedule>();
        CreateMap<Schedule, ScheduleDto>();
        
        CreateMap<ScheduleLineDto, ScheduleLine>();
        CreateMap<ScheduleLine, ScheduleLineDto>();
        
        CreateMap<CourseComponentDto, CourseComponent>()
            .ForMember(dest => dest.EvlIds, opt => opt.MapFrom(src => src.Evls.Select(e => e.Id).ToList()));

        CreateMap<CourseComponent, CourseComponentDto>();

        CreateMap<LessonDto, Lesson>()
            .ForMember(dest => dest.EvlIds, opt => opt.MapFrom(src => src.Evls.Select(e => e.Id).ToList()));
        
        CreateMap<ExamDto, Exam>()
            .ForMember(dest => dest.EvlIds, opt => opt.MapFrom(src => src.Evls.Select(e => e.Id).ToList()));

        CreateMap<Exam, ExamDto>();
        CreateMap<Lesson, LessonDto>();
    }
}