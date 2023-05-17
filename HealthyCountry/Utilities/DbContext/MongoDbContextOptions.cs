using System;
using System.Collections.Generic;

namespace HealthyCountry.Utilities.DbContext;

/// <summary>
/// The options to be used by a <see cref="MongoDbContext" />.
/// </summary>
public class MongoDbContextOptions
{
    /// <summary>
    /// Mongodb connection string.
    /// </summary>
    public string MongoDbConnectionString { get; set; }

    private IDictionary<string, string> _mapping;

    /// <summary>
    /// Uses for default collection names overriding
    /// </summary>
    public IDictionary<string, string> Mapping
    {
        get => _mapping;
        set => _mapping = new Dictionary<string, string>(value ?? new Dictionary<string, string>(), StringComparer.InvariantCultureIgnoreCase);
    }
}

/// <summary>
/// <see cref="MongoDbContextOptions" />.
/// </summary>
/// <typeparam name="TContext"> The type of the context these options apply to. </typeparam>
// ReSharper disable once UnusedTypeParameter
public class MongoDbContextOptions<TContext> : MongoDbContextOptions where TContext : MongoDbContext
{
        
}