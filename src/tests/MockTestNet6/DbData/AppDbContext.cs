﻿// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockTestNet6
//  Author           : RzR
//  Created On       : 2023-11-09 15:44
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-09 17:10
// ***********************************************************************
//  <copyright file="AppDbContext.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.EntityFrameworkCore;
using MockTestNet6.DbData.Models;

#endregion

namespace MockTestNet6.DbData
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<AuthorEntity> Authors { get; set; }

        public DbSet<PostEntity> Posts { get; set; }
    }
}