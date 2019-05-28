using Pspl.Shared.Db;
using Pspl.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Pspl.Shared.Repositories
{
    public interface IAdRepository
    {
        Task<IEnumerable<Ad>> FindByAsync(Expression<Func<Ad, bool>> predicate);
        Task<IEnumerable<Ad>> GetAllAsync();
        Task<int> CreateAsync(Ad ad);
    }

    public class AdRepository : IAdRepository
    {
        private readonly IAdContext _context;

        public AdRepository(IAdContext context)
        {
            _context = context;
        }

        public Task<int> CreateAsync(Ad ad)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Ad>> FindByAsync(Expression<Func<Ad, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Ad>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}