using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HealthyCountry.Utilities.DbContext;

/// <summary>
/// Extensions for <see cref="MongoDbContext" />.
/// </summary>
public static class MongoDbContextExtension
{
    /// <summary>
    /// Register new instance of <see cref="MongoDbContext" /> in service collection as a singleton.
    /// </summary>
    public static IServiceCollection AddDbContext<TContext>(
        this IServiceCollection collection,
        Action<MongoDbContextOptions<TContext>> setupAction)
        where TContext : MongoDbContext
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        if (setupAction == null) throw new ArgumentNullException(nameof(setupAction));

        collection.Configure(setupAction);
        return collection.AddSingleton<TContext, TContext>();
    }
    /// <summary>
    /// Register new instance of <see cref="MongoDbContext" /> in service collection as a singleton.
    /// </summary>
    public static IServiceCollection AddDbContext<TContext>(
        this IServiceCollection collection, IConfigurationSection section)
        where TContext : MongoDbContext
    {
        if (collection == null) throw new ArgumentNullException(nameof(collection));
        if (section == null) throw new ArgumentNullException(nameof(section));

        collection.Configure<MongoDbContextOptions<TContext>>(section);
        return collection.AddSingleton<TContext, TContext>();
    }
}