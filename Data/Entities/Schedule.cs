namespace HAN.Data.Entities;

public class Schedule : BaseEntity
{
    public int CourseId { get; set; }
    public Course Course { get; set; }
    public List<ScheduleLine> ScheduleLines { get; set; } = [];
}