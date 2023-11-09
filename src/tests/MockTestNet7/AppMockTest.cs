﻿// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockTestNet7
//  Author           : RzR
//  Created On       : 2023-11-09 15:45
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-09 17:10
// ***********************************************************************
//  <copyright file="AppMockTest.cs" company="">
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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockAsyncEnumerable;
using MockTestNet7.DbData;
using MockTestNet7.Extensions;

#endregion

namespace MockTestNet7
{
    [TestClass]
    public class AppMockTest
    {
        private DbContextOptions<AppDbContext> _dbContextOptions;

        [TestInitialize]
        public void Init()
        {
            var dbName = $"AuthorPostsDb_{DateTime.Now.ToFileTimeUtc()}";

            _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
        }

        [TestMethod]
        public async Task GetAuthorsAsync_Success_Test()
        {
            var repository = await InitDataHelper.CreateRepositoryAsync(_dbContextOptions);

            // Act
            var authorList = await repository.GetAuthorsAsync();

            // Assert
            Assert.AreEqual(3, authorList.Count);
        }


        [TestMethod]
        public async Task GetAuthorsAsync_Success_Test1()
        {
            _ = await InitDataHelper.CreateRepositoryAsync(_dbContextOptions);
            var ctx = new AppDbContext(_dbContextOptions);

            // Act
            var authors = ctx.Authors.GetRecordsInTop(new List<int> { 1, 2 });
            var authorList = await EnumerableInvoker.Invoke(authors).ToListAsync();

            // Assert
            Assert.AreEqual(2, authorList.Count);
        }
    }
}