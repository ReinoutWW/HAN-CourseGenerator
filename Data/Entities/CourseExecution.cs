namespace HAN.Data.Entities;

public class CourseExecution
{
    public Course Course { get; set; }
    public CourseSchedule Schedule { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}