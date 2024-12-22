using AutoMapper;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using HAN.Services.DTOs;
using HAN.Services.Interfaces;
using HAN.Services.Validation;

namespace HAN.Services;

public class CourseService(ICourseRepository courseRepository, 
    IEvlRepository evlRepository, 
    IMapper mapper, 
    IValidationService validationService,
    IScheduleRepository scheduleRepository,
    CourseComponentService courseComponentService
) : ICourseService
{
    public CourseDto CreateCourse(CourseDto course)
    {
        validationService.Validate(course);
        
        var courseEntity = mapper.Map<Course>(course);
        
        courseRepository.Add(courseEntity);

        return mapper.Map<CourseDto>(courseEntity);
    }

    public CourseDto GetCourseById(int id)
    {
        var course = courseRepository.GetById(id);
        return mapper.Map<CourseDto>(course);
    }

    public IEnumerable<EvlDto> GetEvls(int courseId)
    {
        var evls = courseRepository.GetEvlsByCourseId(courseId);
        return mapper.Map<IEnumerable<EvlDto>>(evls);
    }

    public void AddEvlToCourse(int courseId, int evlId)
    {
        if (!courseRepository.Exists(courseId))
            throw new KeyNotFoundException($"Course with id {courseId} does not exist");

        if (!evlRepository.Exists(evlId))
            throw new KeyNotFoundException($"Evl with id {evlId} does not exist");

        courseRepository.AddEvlToCourse(courseId, evlId);
    }
    
    public ScheduleDto AddSchedule(ScheduleDto scheduleDto, int courseId)
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
        
        var scheduleDto = mapper.Map<ScheduleDto>(schedule);

        foreach (var scheduleLine in scheduleDto.ScheduleLines)
        {
            scheduleLine.CourseComponent =
                courseComponentService.GetCourseComponentById(scheduleLine.CourseComponentId);
        }
        
        return scheduleDto;
    }

    public ScheduleDto UpdateSchedule(int courseId, ScheduleDto scheduleDto)
    {
        var course = courseRepository.GetById(courseId);
            
        if(course == null)
            throw new KeyNotFoundException($"Course with id {courseId} not found");
        
        var schedule = mapper.Map<Schedule>(scheduleDto);
        
        course.Schedule = schedule;
        courseRepository.Update(course);
        
        return mapper.Map<ScheduleDto>(schedule);
    }
    
    
}