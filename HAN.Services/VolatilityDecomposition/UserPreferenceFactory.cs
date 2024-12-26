using HAN.Services.VolatilityDecomposition.Notifications;

namespace HAN.Services.VolatilityDecomposition;

public class UserPreferenceFactory
{
    public IUserPreferences CreatePreferences()
    {
        return new HardCodedUserPreferences();
    }
}

// For the experiment, we will use a hard-coded user preference class
public class HardCodedUserPreferences : IUserPreferences
{
    public Dictionary<NotificationType, List<NotificationMethod>> Preferences { get; }

    public HardCodedUserPreferences()
    {
        Preferences = new Dictionary<NotificationType, List<NotificationMethod>>
        {
            {
                NotificationType.EntityPersisted, new List<NotificationMethod>
                {
                    NotificationMethod.Internal,
                    NotificationMethod.Email
                }
            }
        };
    }
}