using System.ComponentModel.DataAnnotations;
using HAN.Services.DTOs.CourseComponents;

namespace HAN.Services.DTOs;

public class ScheduleLineDto
{
    public int Id { get; set; }
    [Required]
    public int WeekSequenceNumber { get; set; }
    public CourseComponentDto CourseComponent { get; set; }
}