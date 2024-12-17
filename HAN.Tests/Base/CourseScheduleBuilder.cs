using HAN.Domain.Entities;
using CourseSchedule = HAN.Domain.Entities.CourseSchedule;

namespace HAN.Tests.Base;

public class CourseScheduleBuilder
{
    private CourseSchedule _courseSchedule;
    
    public CourseScheduleBuilder()
    {
        
    }

    private CourseScheduleBuilder AddLessonInSchedule(Lesson lesson, int sequence)
    {
        _courseSchedule.AddOrUpdateCourseScheduleLine(lesson, sequence);
        return this;
    }
}