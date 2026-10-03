using System.Reflection;
using Fundation.Abstractions.Core;
using Fundation.Abstractions.CQRS.Event;
using Fundation.Abstractions.Serialization;
using Fundation.Abstractions.Types;
using Fundation.Core.CQRS.Event;
using Fundation.Core.Extensions.ServiceCollection;
using Fundation.Core.IdsGenerator;
using Fundation.Core.Serialization;
using Fundation.Core.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Fundation.Core.Registrations;

public static class CoreRegistrationExtensions
{
    public static IServiceCollection AddCore(
        this IServiceCollection services,
        IConfiguration configuration,
        string? rootSectionName = null,
        params Assembly[] assembliesToScan)
    {
        var systemInfo = MachineInstanceInfo.New();

        services.AddSingleton<IMachineInstanceInfo>(systemInfo);

        services.AddSingleton(systemInfo);

        services.AddSingleton<IExclusiveLock>(serviceProvider =>
            new ExclusiveLock(serviceProvider.GetRequiredService<ILogger<ExclusiveLock>>()));

        services.AddTransient<IAggregatesDomainEventsRequestStore, AggregatesDomainEventsStore>();

        services.AddHttpContextAccessor();

        AddDefaultSerializer(services);

        services.AddMessagingCore(configuration, rootSectionName: rootSectionName);

        switch (configuration["IdGenerator:Type"])
        {
            case "Guid":
                services.AddSingleton<IIdGenerator<Guid>, GuidIdGenerator>();
                break;
            default:
                services.AddSingleton<IIdGenerator<long>, SnowFlakIdGenerator>();
                break;
        }

        return services;
    }


    private static void AddDefaultSerializer(
        IServiceCollection services,
        ServiceLifetime lifetime = ServiceLifetime.Transient)
    {
        services.Add<ISerializer, DefaultSerializer>(lifetime);
        services.Add<IMessageSerializer, DefaultMessageSerializer>(lifetime);
    }
}
