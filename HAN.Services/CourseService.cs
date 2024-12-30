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
        courseEntity.Schedule ??= new Schedule
        {
            Course = courseEntity,
            CourseId = courseEntity.Id
        };
        
        courseRepository.Add(courseEntity);
        
        var courseDto = mapper.Map<CourseDto>(courseEntity);
        courseDto.Evls = course.Evls;
        
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

    public CourseDto UpdateCourse(CourseDto courseDto)
    {
        validationService.Validate(courseDto);

        var existingCourse = courseRepository.GetById(courseDto.Id);
        if (existingCourse == null)
            throw new KeyNotFoundException($"Course with id {courseDto.Id} not found");

        existingCourse.Name = courseDto.Name;
        existingCourse.Description = courseDto.Description;
        existingCourse.EvlIds = courseDto.Evls.Select(e => e.Id).ToList();

        UpdateSchedule(existingCourse.Schedule, courseDto.Schedule);

        existingCourse.UpdatedAt = DateTime.UtcNow;

        courseRepository.Update(existingCourse);
        
        OnCourseUpdated(courseDto);

        return mapper.Map<CourseDto>(existingCourse);
    }
    
    private void OnCourseUpdated(CourseDto course)
    {
        var notification = new EntityPersistedNotification
        {
            Title = "Course with name " + course.Name + " has been updated successfully!",
            Message = "An entity has been successfully updated.",
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

    public CourseDto GetCourseById(int id)
    {
        var course = courseRepository.GetById(id);
        var evls = courseRepository.GetEvlsByCourseId(id).ToList();
        
        if(course == null)
            throw new KeyNotFoundException($"Course with id {id} not found");

        var courseDto = mapper.Map<CourseDto>(course);
        courseDto.Evls = mapper.Map<List<EvlDto>>(evls);
        courseDto.Schedule.ScheduleLines = [];
        
        foreach (var slEntity in course.Schedule.ScheduleLines)
        {
            var slDto = mapper.Map<ScheduleLineDto>(slEntity);   
            slDto.CourseComponent = courseComponentService.GetCourseComponentById(slEntity.CourseComponentId);
            courseDto.Schedule.ScheduleLines.Add(slDto);
        }
        
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

    public ScheduleDto AddSchedule(ScheduleDto schedule)
    {
        var scheduleEntity = mapper.Map<Schedule>(schedule);
        scheduleRepository.Add(scheduleEntity);
        
        return mapper.Map<ScheduleDto>(scheduleEntity);
    }
    
    private void UpdateSchedule(Schedule? existingSchedule, ScheduleDto scheduleDto)
    {
        if (existingSchedule == null)
        {
            throw new InvalidOperationException("The course does not have an associated schedule.");
        }

        // Clear existing lines and overwrite with new ones
        existingSchedule.ScheduleLines.Clear();
        foreach (var lineDto in scheduleDto.ScheduleLines)
        {
            var newLine = new ScheduleLine
            {
                WeekSequenceNumber = lineDto.WeekSequenceNumber,
                CourseComponentId = lineDto.CourseComponentId,
                Schedule = existingSchedule
            };
            existingSchedule.ScheduleLines.Add(newLine);
        }
    }
}