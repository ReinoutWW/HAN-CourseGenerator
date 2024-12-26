using HAN.Services.DTOs;

namespace HAN.Services.VolatilityDecomposition.Notifications;

public class EntityPersistedData
{
    public string EntityName { get; set; }
    public BaseDto Entity { get; set; }
}