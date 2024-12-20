using System.ComponentModel.DataAnnotations;

namespace HAN.Data.Entities;

public class ScheduleLine : BaseEntity
{
    [Required]
    public int WeekSequenceNumber { get; set; }
    public int CourseComponentId { get; set; }
}