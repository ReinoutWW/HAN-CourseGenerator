using AutoMapper;
using HAN.Data.Entities;
using HAN.Repositories.Interfaces;
using HAN.Services.DTOs;
using HAN.Services.Interfaces;
using HAN.Services.Validation;
using HAN.Services.VolatilityDecomposition;
using HAN.Services.VolatilityDecomposition.Notifications;
using HAN.Utilities;

namespace HAN.Services;

public class CourseService(ICourseRepository courseRepository, 
    IEvlRepository evlRepository, 
    IMapper mapper, 
    IValidationService validationService,
    IScheduleRepository scheduleRepository,
    CourseComponentService courseComponentService,
    IMessageBroker messageBroker
) : ICourseService
{
    public CourseDto CreateCourse(CourseDto course)
    {
        validationService.Validate(course);
        
        var courseEntity = mapper.Map<Course>(course);
        
        courseRepository.Add(courseEntity);
        
        var courseDto = mapper.Map<CourseDto>(courseEntity);
        OnCourseCreated(courseDto);

        return courseDto;
    }
    
    private void OnCourseCreated(CourseDto course)
    {
        var notification = new EntityPersistedNotification
        {
            Title = "Course with name " + course.Name + " has been created successfully!",
            Message = "An entity has been successfully created.",
            Type = NotificationType.EntityPersisted,
            PersistData = new EntityPersistedData
            {
                EntityName = course.Name,
                Entity = course
            }
        };
        
        messageBroker.Publish(new NotificationEvent
        {
            Type = NotificationType.EntityPersisted,
            Notification = notification
        });
    }

    public CourseDto UpdateCourse(CourseDto course)
    {
        validationService.Validate(course);
        
        var courseEntity = mapper.Map<Course>(course);
        
        courseRepository.Update(courseEntity);

        return mapper.Map<CourseDto>(courseEntity);
    }

    public CourseDto GetCourseById(int id)
    {
        var course = courseRepository.GetById(id);
        var evls = courseRepository.GetEvlsByCourseId(id).ToList();
        
        if(course == null)
            throw new KeyNotFoundException($"Course with id {id} not found");

        var courseDto = mapper.Map<CourseDto>(course);
        courseDto.Evls = mapper.Map<List<EvlDto>>(evls);
        
        return courseDto;
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

    public List<CourseDto> GetAllCourses()
    {
        var courses = courseRepository.GetAll();

        List<CourseDto> courseDtos = [];
        foreach (var course in courses)
        {
            var evls = courseRepository.GetEvlsByCourseId(course.Id);
            var courseDto = mapper.Map<CourseDto>(course);
            courseDto.Evls = mapper.Map<List<EvlDto>>(evls);
            courseDtos.Add(courseDto);
        }
        
        return courseDtos;
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

    public ScheduleDto UpdateSchedule(ScheduleDto scheduleDto)
    {
        if (!scheduleRepository.Exists(scheduleDto.Id))
            throw new KeyNotFoundException($"Schedule with id {scheduleDto.Id} not found");

        var existingSchedule = scheduleRepository.GetById(scheduleDto.Id);

        if (existingSchedule == null)
            throw new KeyNotFoundException($"Schedule with id {scheduleDto.Id} not found");

        mapper.Map(scheduleDto, existingSchedule);
        scheduleRepository.Update(existingSchedule);

        return mapper.Map<ScheduleDto>(existingSchedule);
    }
}