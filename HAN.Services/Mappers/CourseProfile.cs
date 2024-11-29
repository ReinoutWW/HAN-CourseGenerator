using AutoMapper;
using HAN.Services.DTOs;

namespace HAN.Services.Mappers;

public class CourseProfile : Profile
{
    public CourseProfile()
    {
        // From - To (Easy to f* up)
        
        // Persistence
        CreateMap<CreateCourseDto, Data.Entities.Course>();
        CreateMap<Data.Entities.Course, Domain.Entities.Course>();
        CreateMap<Data.Entities.Course, CourseResponseDto>();

        // Domain
        CreateMap<Domain.Entities.Course, CourseResponseDto>();
    }
}