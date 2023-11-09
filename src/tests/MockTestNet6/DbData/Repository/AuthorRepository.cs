// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockTestNet6
//  Author           : RzR
//  Created On       : 2023-11-09 15:44
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-09 17:10
// ***********************************************************************
//  <copyright file="AuthorRepository.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MockTestNet6.DbData.Models;

#endregion

namespace MockTestNet6.DbData.Repository
{
    public class AuthorRepository
    {
        private readonly AppDbContext _context;

        public AuthorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(AuthorEntity authorEntity)
        {
            if (authorEntity.Id != 0) throw new InvalidOperationException();

            await _context.AddAsync(authorEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AuthorEntity>> GetAuthorsAsync(bool asNoTracking = false)
        {
            var queryable = _context.Authors.AsQueryable();
            if (asNoTracking) return await queryable.AsNoTracking().ToListAsync();

            return await _context.Authors.ToListAsync();
        }

        public async Task<AuthorEntity> GetAuthorByIdAsync(int id, bool asNoTracking = false)
        {
            var queryable = _context.Authors.Include(x => x.Posts).AsQueryable();
            if (asNoTracking) return await queryable.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return await queryable.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task DeleteAsync(int id)
        {
            var author = await _context.Authors.FirstOrDefaultAsync(x => x.Id == id);
            if (author == null) throw new InvalidOperationException();

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
        }
    }
}