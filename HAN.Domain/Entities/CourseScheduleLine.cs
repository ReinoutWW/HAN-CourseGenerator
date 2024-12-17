namespace HAN.Domain.Entities;

public class CourseScheduleLine
{
    public int SequenceWeekNumber { get; set; }
    public ICourseComponent CourseComponent { get; set; }
}