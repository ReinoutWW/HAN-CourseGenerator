using AutoMapper;
using HAN.Data.Entities;
using HAN.Repositories;
using HAN.Repositories.Interfaces;
using HAN.Services.DTOs;
using HAN.Services.Interfaces;
using HAN.Services.Validation;

namespace HAN.Services;

public class ScheduleService(
        IValidationService validationService, 
        IScheduleRepository scheduleRepository,
        ICourseRepository courseRepository,
        IMapper mapper
    ) : IScheduleService
{
    public ScheduleDto CreateSchedule(ScheduleDto scheduleDto, int courseId)
    {
        validationService.Validate(scheduleDto);
        
        if(!courseRepository.Exists(courseId))
            throw new KeyNotFoundException($"Course with id {courseId} not found");
        
        var scheduleEntity = mapper.Map<Schedule>(scheduleDto);
        var course = courseRepository.GetById(courseId);
        
        if(course == null)
            throw new KeyNotFoundException($"Course with id {courseId} not found");
        
        course.Schedule = scheduleEntity;
        
        courseRepository.Update(course);
        
        return mapper.Map<ScheduleDto>(scheduleEntity);
    }

    public ScheduleDto GetScheduleById(int id)
    {
        var schedule = scheduleRepository.GetById(id);
        
        if(schedule == null)
            throw new KeyNotFoundException($"Schedule with id {id} not found");
        
        return mapper.Map<ScheduleDto>(schedule);
    }
}