namespace HAN.Services.DTOs;

public class ScheduleDto
{
    public int Id { get; set; }
    public List<ScheduleLineDto> ScheduleLines { get; set; } = [];
}