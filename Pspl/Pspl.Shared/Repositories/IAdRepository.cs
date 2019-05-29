﻿using MongoDB.Driver;
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
        Task CreateAsync(Ad ad);
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