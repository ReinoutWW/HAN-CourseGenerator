using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace HAN.Utilities.Messaging.RabbitMQ;

public static class RabbitMqConnectionExtensions
{
    public static ConnectionFactory CreateConnectionFactory(this IConfiguration configuration)
    {
        ConnectionFactory factory = new() { 
            HostName = configuration.GetValue<string>("RabbitMQ:Hostname") ?? string.Empty,
            VirtualHost = "/"
        };

        if (!configuration.HasRabbitMqCredentials())
            return factory;

        factory.UserName = configuration.GetRabbitMqUsername();
        factory.Password = configuration.GetRabbitMqPassword();

        return factory;
    }

    private static bool HasRabbitMqCredentials(this IConfiguration configuration)
    {
        var username = configuration["RabbitMQ:Username"];
        var password = configuration["RabbitMQ:Password"];
        return !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);
    }
    
    private static string GetRabbitMqUsername(this IConfiguration configuration) =>
        configuration.GetValue<string>("RabbitMQ:Username");
    
    private static string GetRabbitMqPassword(this IConfiguration configuration) =>
        configuration.GetValue<string>("RabbitMQ:Password");
}