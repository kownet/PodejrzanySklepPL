using MongoDB.Bson;
using MongoDB.Driver;
using Pspl.Shared.Db;
using Pspl.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pspl.Shared.Repositories
{
    public interface IAdRepository
    {
        Task<IEnumerable<Ad>> FindByAsync(Expression<Func<Ad, bool>> predicate);
        Task<IEnumerable<Ad>> GetAllAsync();
        Task CreateAsync(Ad ad);
        Task<bool> DeleteAsync(string name);
        Task<bool> DeleteAllAsync();
    }

    public class AdRepository : IAdRepository
    {
        private readonly IAdContext _context;

        public AdRepository(IAdContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Ad ad)
        {
            await _context
                    .Ads
                    .InsertOneAsync(ad);
        }

        public async Task<bool> DeleteAllAsync()
        {
            var filter = new BsonDocument();

            var docs = _context
                .Ads
                .Find(filter)
                .ToList();

            var idsFilter = Builders<Ad>.Filter.In(d => d.InternalId, docs.Select(d => d.InternalId));

            var deleteResult = await _context
                .Ads
                .DeleteManyAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        public async Task<bool> DeleteAsync(string name)
        {
            FilterDefinition<Ad> filter = Builders<Ad>.Filter.Eq(m => m.Name, name);
            DeleteResult deleteResult = await _context
                                                .Ads
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<Ad>> FindByAsync(Expression<Func<Ad, bool>> predicate)
        {
            return await _context
                            .Ads
                            .Find(predicate)
                            .ToListAsync();
        }

        public async Task<IEnumerable<Ad>> GetAllAsync()
        {
            return await _context
                            .Ads
                            .Find(_ => true)
                            .ToListAsync();
        }
    }
}