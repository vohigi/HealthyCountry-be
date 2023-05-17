using System.Threading.Tasks;
using MongoDB.Driver;

namespace HealthyCountry.Utilities.DbContext;

public interface IMongoDbContext
{
    IMongoDatabase Database { get; }
    Task MigrateAsync();
    void Migrate();
}