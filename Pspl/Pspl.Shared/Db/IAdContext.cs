using MongoDB.Driver;
using Pspl.Shared.Models;
using Pspl.Shared.Providers;

namespace Pspl.Shared.Db
{
    public interface IAdContext
    {
        IMongoCollection<Ad> Ads { get; }
    }

    public class AdContext : IAdContext
    {
        private readonly IMongoDatabase _db;
        private readonly IMongoDbProvider _mongoDbProvider;

        public AdContext(IMongoDbProvider mongoDbProvider)
        {
            _mongoDbProvider = mongoDbProvider;

            var client = new MongoClient(_mongoDbProvider.ConnectionString());

            _db = client.GetDatabase(_mongoDbProvider.Database());
        }

        public IMongoCollection<Ad> Ads => _db.GetCollection<Ad>("Ads");
    }
}