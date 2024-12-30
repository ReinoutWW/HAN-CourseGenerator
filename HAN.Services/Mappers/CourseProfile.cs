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
        

        // DTO -> Entity
        CreateMap<EvlDto, Evl>();
        CreateMap<UserDto, User>();
        CreateMap<FileDto, File>();
        CreateMap<ScheduleDto, Schedule>();
        CreateMap<ScheduleLineDto, ScheduleLine>();
        CreateMap<Course, CourseDto>();

        CreateMap<CourseComponentDto, CourseComponent>()
            .ForMember(dest => dest.EvlIds, opt => opt.MapFrom(src => src.Evls.Select(e => e.Id).ToList()));
        CreateMap<LessonDto, Lesson>()
            .ForMember(dest => dest.EvlIds, opt => opt.MapFrom(src => src.Evls.Select(e => e.Id).ToList()));
        CreateMap<ExamDto, Exam>()
            .ForMember(dest => dest.EvlIds, opt => opt.MapFrom(src => src.Evls.Select(e => e.Id).ToList()));

        // Entity -> DTO
        CreateMap<Evl, EvlDto>();
        CreateMap<User, UserDto>();
        CreateMap<File, FileDto>();
        CreateMap<Schedule, ScheduleDto>();
        CreateMap<ScheduleLine, ScheduleLineDto>()
            .ForMember(dest => dest.CourseComponentId, opt => opt.Ignore());
        CreateMap<CourseDto, Course>()
            .ForMember(dest => dest.EvlIds, opt => opt.MapFrom(src => src.Evls.Select(e => e.Id).ToList()));

        CreateMap<CourseComponent, CourseComponentDto>();
        CreateMap<Lesson, LessonDto>().IncludeBase<CourseComponent, CourseComponentDto>();
        CreateMap<Exam, ExamDto>().IncludeBase<CourseComponent, CourseComponentDto>();
    }
}