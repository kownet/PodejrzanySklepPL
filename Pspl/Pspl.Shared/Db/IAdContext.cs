using MongoDB.Driver;
using Pspl.Shared.Models;

namespace Pspl.Shared.Db
{
    public interface IAdContext
    {
        IMongoCollection<Ad> Ads { get; }
    }

    public class AdContext : IAdContext
    {
        private readonly IMongoDatabase _db;

        public AdContext(MongoDBConfig config)
        {
            var client = new MongoClient(config.ConnectionString);

            _db = client.GetDatabase(config.Database);
        }
        public IMongoCollection<Ad> Ads => _db.GetCollection<Ad>("Ads");
    }
}