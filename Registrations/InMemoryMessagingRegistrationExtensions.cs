using Fundation.Abstractions.Messaging;
using Fundation.Abstractions.Messaging.PersistMessage;
using Fundation.Abstractions.Scheduling;
using Fundation.Core.Extensions.ServiceCollection;
using Fundation.Core.Messaging.BackgroundServices;
using Fundation.Core.Messaging.Broker.InMemory;
using Fundation.Core.Messaging.MessagePersistence.InMemory;
using Microsoft.Extensions.Configuration;

namespace Fundation.Core.Registrations;

public static partial class InMemoryMessagingRegistrationExtensions
{
    public static IServiceCollection AddInMemoryMessagePersistence(
        this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
    {
        services.Add<IMessagePersistenceRepository, InMemoryMessagePersistenceRepository>(serviceLifetime);

        services.Replace<IMessagePersistenceService, InMemoryMessagePersistenceService>(serviceLifetime);

        return services;
    }

    public static IServiceCollection AddInMemoryCommandScheduler(
        this IServiceCollection services,
        ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        services.Replace<ICommandScheduler, InMemoryCommandScheduler>(serviceLifetime);

        return services;
    }

    public static IServiceCollection AddInMemoryBroker(this IServiceCollection services, IConfiguration configuration)
    {
        services.ReplaceSingleton<IBus, InMemoryBus>();

        return services;
    }
}
