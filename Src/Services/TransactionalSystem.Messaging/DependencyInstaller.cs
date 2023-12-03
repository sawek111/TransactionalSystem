using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace TransactionalSystem.Messaging;

public static class Installer
{
    /// <summary>
    /// Requires additional configuration declared in RabbitSettings
    /// </summary>
    public static void AddMessagingInfrastructure(this IServiceCollection services, ConfigurationManager configuration, Assembly? assembly = null, 
        Action<IBusRegistrationConfigurator> configureMassTransit = null)
    {
        
        services.AddOptions<EventBusSettings>()
            .BindConfiguration(EventBusSettings.SectionName)
            .ValidateDataAnnotations();
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<EventBusSettings>>().Value);
        var settings = configuration.GetSection(EventBusSettings.SectionName).Get<EventBusSettings>();

        services.AddMassTransit(config => {
            if (assembly is not null)
            {
                config.AddConsumers(assembly);
            }
            
            config.SetKebabCaseEndpointNameFormatter();
            configureMassTransit?.Invoke(config);
            
            var rabbitMqUri = new Uri($"rabbitmq://{settings!.UserName}:{settings.Password}@{settings!.Host}");
            config.UsingRabbitMq((ctx, cfg) => {
                cfg.Host(rabbitMqUri);
                cfg.ConfigureEndpoints(ctx);
            });
        });
    }
}