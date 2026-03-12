// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockTestNet5
//  Author           : RzR
//  Created On       : 2026-02-27 11:02
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-02-27 11:40
// ***********************************************************************
//  <copyright file="PerformanceTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MockAsyncEnumerable;
using MockTestNet5.Models;

namespace MockTestNet5
{
    [TestClass]
    public class PerformanceTests
    {
        [TestMethod]
        public async Task LargeDataSet_PerformsWell()
        {
            // Arrange
            var data = Enumerable.Range(1, 10000)
                .Select(i => new TestEntity { Id = i, Name = $"Entity{i}" })
                .ToList();
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var result = await asyncEnumerable
                .Where(x => x.Id % 2 == 0)
                .Take(100)
                .ToListAsync();
            sw.Stop();

            // Assert
            Assert.AreEqual(100, result.Count);
            Assert.IsTrue(sw.ElapsedMilliseconds < 1000,
                $"Query took {sw.ElapsedMilliseconds}ms, expected < 1000ms");
        }

        [TestMethod]
        public async Task MultipleEnumerations_AreIndependent()
        {
            // Arrange
            var data = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "Alice" },
                new TestEntity { Id = 2, Name = "Bob" }
            };
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act
            var result1 = await asyncEnumerable.ToListAsync();
            var result2 = await asyncEnumerable.ToListAsync();

            // Assert
            Assert.AreEqual(result1.Count, result2.Count);
            Assert.AreNotSame(result1, result2); // Different list instances
        }

        [TestMethod]
        public async Task VeryLongQuery_HandlesCorrectly()
        {
            // Arrange
            var data = Enumerable.Range(1, 1000)
                .Select(i => new TestEntity { Id = i, Name = $"Entity{i}" })
                .ToList();
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act
            var result = await asyncEnumerable
                .Where(x => x.Id > 10)
                .Where(x => x.Id < 990)
                .OrderBy(x => x.Name)
                .Skip(100)
                .Take(50)
                .Select(x => x.Id)
                .ToListAsync();

            // Assert
            Assert.AreEqual(50, result.Count);
        }
    }
}