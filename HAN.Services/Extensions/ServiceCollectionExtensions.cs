using HAN.Repositories.Extensions;
using HAN.Services.Interfaces;
using HAN.Services.Validation;
using HAN.Services.VolatilityDecomposition;
using HAN.Services.VolatilityDecomposition.Notifications.Engines;
using HAN.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace HAN.Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCourseServices(this IServiceCollection services)
    {
        services.AddScoped<IValidationService, ValidationService>()
            .AddScoped<ICourseService, CourseService>()
            .AddScoped<IEvlService, EvlService>()
            .AddScoped<IUserService, UserService>()
            .AddScoped<IFileService, FileService>()
            .AddScoped<LessonService>()
            .AddScoped<ExamService>()
            .AddScoped<ICourseValidationService, CourseValidationService>()
            .AddScoped<CourseComponentService>()
            .AddRepositories()
            .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
            .AddScoped<InternalNotificationStateService>()
            .AddNotificationServices();

        return services;
    }

    private static IServiceCollection AddNotificationServices(this IServiceCollection services)
    {
        services.AddScoped<IMessageBroker, SimpleMessageBroker>();

        services.AddScoped<INotificationEngine, InternalNotificationEngine>();
        services.AddScoped<INotificationEngine, EmailNotificationEngine>();

        services.AddScoped<NotificationMethodRegistry>(sp =>
        {
            var engines = sp.GetServices<INotificationEngine>();
            return new NotificationMethodRegistry(engines);
        });

        services.AddScoped<NotificationManager>(sp =>
        {
            var userPreferencesFactory = new UserPreferenceFactory().CreatePreferences;
            var methodRegistry = sp.GetRequiredService<NotificationMethodRegistry>();
            var messageBroker = sp.GetRequiredService<IMessageBroker>();
            return new NotificationManager(userPreferencesFactory, methodRegistry, messageBroker);
        });

        return services;
    }
}