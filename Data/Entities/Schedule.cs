namespace HAN.Data.Entities;

public class Schedule : BaseEntity
{
    public List<ScheduleLine> ScheduleLines { get; set; } = [];
}