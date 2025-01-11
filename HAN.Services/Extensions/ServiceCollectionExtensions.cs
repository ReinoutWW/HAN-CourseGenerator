using System.ComponentModel;
using System.Reflection;
using HAN.Repositories.Extensions;
using HAN.Services.Interfaces;
using HAN.Services.Validation;
using HAN.Services.VolatilityDecomposition;
using HAN.Services.VolatilityDecomposition.Notifications.Engines;
using HAN.Utilities;
using HAN.Utilities.Messaging.Abstractions;
using HAN.Utilities.Messaging.RabbitMQ;
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
            .AddScoped<IExporterService, FileExporterService>()
            .AddScoped<IExporterService, FileExporterService>()
            .AddScoped<IExporterService, FileExporterService>()
            .AddScoped<IExporterService, FileExporterService>()
            .AddScoped<LessonService>()
            .AddScoped<ExamService>()
            .AddScoped<ICourseCompletenessValidator, CourseCompletenessValidator>()
            .AddScoped<ICourseOrderValidator, CourseOrderValidator>()
            .AddScoped<ICourseValidationService, CourseValidationService>()
            .AddScoped<CourseComponentService>()
            .AddRepositories()
            .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
            .AddScoped<InternalNotificationStateService>()
            .AddNotificationServices()
            .AddEventDrivenMessaging();

        return services;
    }

    private static IServiceCollection AddEventDrivenMessaging(this IServiceCollection services)
    {
        var nodeId = Guid.NewGuid().ToString("N");
        return services.AddSingleton(new RabbitMqSubscriber("localhost", nodeId))
            .AddSingleton<IMessagePublisher>(sp => new RabbitMqPublisher("localhost", nodeId))
            .AddSingleton<IResponseListener, ResponseListenerService>()
            .AddScoped<IGradeService, GradeService>()
            .AddScoped<IBlockchainService, BlockchainService>()
            .AddScoped<IMonitorService, MonitorService>();
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
    
    public static TAttribute? GetAttribute<TAttribute>(this Enum enumValue) 
        where TAttribute : Attribute
    {
        return enumValue.GetType()
            .GetMember(enumValue.ToString()).First()
            .GetCustomAttribute<TAttribute>();
    }
    
    public static string? GetDescription(this Enum enumValue)
    {
        var attribute = enumValue.GetAttribute<DescriptionAttribute>();
        return attribute?.Description ?? null;
    }
}