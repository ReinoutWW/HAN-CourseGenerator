using HAN.Services.DTOs;
using HAN.Services.DTOs.CourseComponents;

namespace HAN.Tests.Base;

public class CourseDtoBuilder
{
    private readonly CourseDto _courseDto;
    
    public CourseDtoBuilder(CourseDto? course = null)
    {
        _courseDto = course ?? DbEntityCreator<CourseDto>.CreateEntity();
    }
    
    public CourseDtoBuilder WithId(int id)
    {
        _courseDto.Id = id;
        return this;
    }
    
    public CourseDtoBuilder WithName(string name)
    {
        _courseDto.Name = name;
        return this;
    }
    
    public CourseDtoBuilder WithDescription(string description)
    {
        _courseDto.Description = description;
        return this;
    }
    
    public CourseDtoBuilder WithEvls(List<EvlDto> evls)
    {
        _courseDto.Evls = evls;
        return this;
    }
    
    public CourseDtoBuilder WithValidSchedule(List<CourseComponentDto> courseComponents)
    {
        if (_courseDto.Evls == null)
            throw new InvalidOperationException("Evls must be created before Schedule.");
        
        var schedule = new ScheduleDto();

        var orderedComponents = courseComponents
            .OrderBy(cc => cc is AssessmentDto) // Assessments come after lessons.
            .ThenBy(cc => cc is LessonDto)     // Lessons are prioritized first.
            .ToList();

        int weekSequenceNumber = 0;

        foreach (var component in orderedComponents)
        {
            var scheduleLine = new ScheduleLineDto
            {
                CourseComponentId = component.Id,
                WeekSequenceNumber = weekSequenceNumber++
            };
            schedule.ScheduleLines.Add(scheduleLine);
        }

        _courseDto.Schedule = schedule;
        return this;
    }

    public CourseDtoBuilder WithInvalidSchedule(List<CourseComponentDto> courseComponents)
    {
        if (_courseDto.Evls == null)
            throw new InvalidOperationException("Evls must be created before Schedule.");
        
        var schedule = new ScheduleDto();

        var orderedComponents = courseComponents
            .OrderBy(cc => cc is LessonDto)   // Lessons come after assessments.
            .ThenBy(cc => cc is AssessmentDto) // Assessments are prioritized first.
            .ToList();

        int weekSequenceNumber = 0;

        foreach (var component in orderedComponents)
        {
            var scheduleLine = new ScheduleLineDto
            {
                CourseComponentId = component.Id,
                WeekSequenceNumber = weekSequenceNumber++
            };
            schedule.ScheduleLines.Add(scheduleLine);
        }

        _courseDto.Schedule = schedule;
        return this;
    }

    
    public CourseDto Build()
    {
        return _courseDto;
    }
}