// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockTestNet5
//  Author           : RzR
//  Created On       : 2023-11-09 15:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-09 17:10
// ***********************************************************************
//  <copyright file="InitDataHelper.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MockTestNet5.DbData;
using MockTestNet5.DbData.Models;
using MockTestNet5.DbData.Repository;

#endregion

namespace MockTestNet5
{
    public static class InitDataHelper
    {
        public static async Task<AuthorRepository> CreateRepositoryAsync(DbContextOptions<AppDbContext> contextOptions)
        {
            var context = new AppDbContext(contextOptions);
            await PopulateDataAsync(context);

            return new AuthorRepository(context);
        }

        private static async Task PopulateDataAsync(AppDbContext context)
        {
            var index = 1;

            while (index <= 3)
            {
                var author = new AuthorEntity
                {
                    Name = $"Author_{index}",
                    Posts = new List<PostEntity>
                    {
                        new PostEntity
                        {
                            Title = $"First_{index}", Contents = $"Some contents_{index}",
                            CreatedOn = DateTime.Now
                        },
                        new PostEntity
                        {
                            Title = $"Second_{index}", Contents = $"Some contents_{index}",
                            CreatedOn = DateTime.Now
                        }
                    }
                };

                index++;
                await context.Authors.AddAsync(author);
            }

            await context.SaveChangesAsync();
        }
    }
}