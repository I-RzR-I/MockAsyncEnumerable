// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockTestNet5
//  Author           : RzR
//  Created On       : 2026-02-27 11:02
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-02-27 11:41
// ***********************************************************************
//  <copyright file="EFCoreIntegrationTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MockTestNet5.Models;
using RzR.Extensions.EntityMock;

namespace MockTestNet5
{
    [TestClass]
    public class EFCoreIntegrationTests
    {
        [TestMethod]
        public async Task AsQueryable_WorksWithLinqExpressions()
        {
            // Arrange
            var data = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "Alice", CreatedDate = DateTime.Now.AddDays(-5) },
                new TestEntity { Id = 2, Name = "Bob", CreatedDate = DateTime.Now.AddDays(-3) },
                new TestEntity { Id = 3, Name = "Charlie", CreatedDate = DateTime.Now.AddDays(-1) }
            };
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act
            var recentDate = DateTime.Now.AddDays(-4);
            var result = await asyncEnumerable
                .Where(x => x.CreatedDate > recentDate)
                .ToListAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task ComplexProjection_WorksCorrectly()
        {
            // Arrange
            var data = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "Alice", CreatedDate = DateTime.Now.AddDays(-5) },
                new TestEntity { Id = 2, Name = "Bob", CreatedDate = DateTime.Now.AddDays(-3) }
            };
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act
            var result = await asyncEnumerable
                .Select(x => new
                {
                    x.Id,
                    UpperName = x.Name.ToUpper(),
                    DaysOld = (DateTime.Now - x.CreatedDate).Days
                })
                .ToListAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("ALICE", result[0].UpperName);
            Assert.IsTrue(result[0].DaysOld >= 5);
        }
    }
}