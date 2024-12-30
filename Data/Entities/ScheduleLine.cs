using System.ComponentModel.DataAnnotations;

namespace HAN.Data.Entities;

public class ScheduleLine : BaseEntity
{
    public int ScheduleId { get; set; }
    public Schedule Schedule { get; set; }
    
    [Required]
    public int WeekSequenceNumber { get; set; }
    public int CourseComponentId { get; set; }
}