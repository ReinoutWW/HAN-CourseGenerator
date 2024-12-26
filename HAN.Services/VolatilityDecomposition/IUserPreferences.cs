using HAN.Services.VolatilityDecomposition.Notifications;

namespace HAN.Services.VolatilityDecomposition;

public interface IUserPreferences
{
    public Dictionary<NotificationType, List<NotificationMethod>> Preferences { get; }
}