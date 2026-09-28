using System.Reflection;
using Fundation.Abstractions.Persistence.EventStore;
using Fundation.Abstractions.Persistence.EventStore.Projections;
using Fundation.Core.Extensions.ServiceCollection;
using Fundation.Core.Persistence.EventStore;
using Fundation.Core.Persistence.EventStore.InMemory;

namespace Fundation.Core.Registrations;

public static class EventStoreRegistrationExtentions
{
    public static IServiceCollection AddInMemoryEventStore(this IServiceCollection services)
    {
        return AddEventStore<InMemoryEventStore>(services, ServiceLifetime.Singleton);
    }

    public static IServiceCollection AddEventStore<TEventStore>(
        this IServiceCollection services,
        ServiceLifetime withLifetime = ServiceLifetime.Scoped)
        where TEventStore : class, IEventStore
    {
        services.Add<IAggregateStore, AggregateStore>(withLifetime);

        return services.Add<TEventStore, TEventStore>(withLifetime)
            .Add<IEventStore>(sp => sp.GetRequiredService<TEventStore>(), withLifetime);
    }

    public static IServiceCollection AddReadProjections(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddSingleton<IReadProjectionPublisher, ReadProjectionPublisher>();
        var assembliesToScan = assemblies.Any() ? assemblies : new[] { Assembly.GetEntryAssembly() };

        RegisterProjections(services, assembliesToScan!);

        return services;
    }

    private static void RegisterProjections(IServiceCollection services, Assembly[] assembliesToScan)
    {
        services.Scan(scan => scan
            .FromAssemblies(assembliesToScan)
            .AddClasses(classes => classes.AssignableTo<IHaveReadProjection>()) // Filter classes
            .AsImplementedInterfaces()
            .WithTransientLifetime());
    }
}
