// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockTestNet5
//  Author           : RzR
//  Created On       : 2026-02-27 11:02
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-02-27 11:35
// ***********************************************************************
//  <copyright file="LinqOperationsTests.cs" company="RzR SOFT & TECH">
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
    public class LinqOperationsTests
    {
        private List<TestEntity> GetTestData()
        {
            return new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "Alice", Age = 30, Department = "IT" },
                new TestEntity { Id = 2, Name = "Bob", Age = 25, Department = "HR" },
                new TestEntity { Id = 3, Name = "Charlie", Age = 35, Department = "IT" },
                new TestEntity { Id = 4, Name = "Diana", Age = 28, Department = "HR" },
                new TestEntity { Id = 5, Name = "Eve", Age = 32, Department = "IT" }
            };
        }

        [TestMethod]
        public async Task Where_FiltersCorrectly()
        {
            // Arrange
            var data = GetTestData();
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act
            var result = await asyncEnumerable
                .Where(x => x.Age > 30)
                .ToListAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task Select_ProjectsCorrectly()
        {
            // Arrange
            var data = GetTestData();
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act
            var result = await asyncEnumerable
                .Select(x => x.Name)
                .ToListAsync();

            // Assert
            Assert.AreEqual(5, result.Count);
            CollectionAssert.Contains(result, "Alice");
            CollectionAssert.Contains(result, "Bob");
            CollectionAssert.Contains(result, "Charlie");
        }

        [TestMethod]
        public async Task SelectAnonymousType_Works()
        {
            // Arrange
            var data = GetTestData();
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act
            var result = await asyncEnumerable
                .Select(x => new { x.Name, x.Age })
                .ToListAsync();

            // Assert
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual("Alice", result[0].Name);
            Assert.AreEqual(30, result[0].Age);
        }

        [TestMethod]
        public async Task OrderBy_SortsAscending()
        {
            // Arrange
            var data = GetTestData();
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act
            var result = await asyncEnumerable
                .OrderBy(x => x.Age)
                .ToListAsync();

            // Assert
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual(25, result[0].Age);
            Assert.AreEqual(28, result[1].Age);
            Assert.AreEqual(30, result[2].Age);
            Assert.AreEqual(32, result[3].Age);
            Assert.AreEqual(35, result[4].Age);
        }

        [TestMethod]
        public async Task OrderByDescending_SortsDescending()
        {
            // Arrange
            var data = GetTestData();
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act
            var result = await asyncEnumerable
                .OrderByDescending(x => x.Age)
                .ToListAsync();

            // Assert
            Assert.AreEqual(35, result[0].Age);
            Assert.AreEqual(25, result[4].Age);
        }

        [TestMethod]
        public async Task Skip_SkipsElements()
        {
            // Arrange
            var data = GetTestData();
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act
            var result = await asyncEnumerable
                .Skip(2)
                .ToListAsync();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Charlie", result[0].Name);
        }

        [TestMethod]
        public async Task Take_TakesElements()
        {
            // Arrange
            var data = GetTestData();
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act
            var result = await asyncEnumerable
                .Take(3)
                .ToListAsync();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual("Alice", result[0].Name);
            Assert.AreEqual("Bob", result[1].Name);
            Assert.AreEqual("Charlie", result[2].Name);
        }

        [TestMethod]
        public async Task SkipTake_PaginatesCorrectly()
        {
            // Arrange
            var data = GetTestData();
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act
            var result = await asyncEnumerable
                .Skip(2)
                .Take(2)
                .ToListAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Charlie", result[0].Name);
            Assert.AreEqual("Diana", result[1].Name);
        }

        [TestMethod]
        public async Task GroupBy_GroupsCorrectly()
        {
            // Arrange
            var data = GetTestData();
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act
            var result = await asyncEnumerable
                .GroupBy(x => x.Department)
                .ToListAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
            var itGroup = result.First(g => g.Key == "IT");
            var hrGroup = result.First(g => g.Key == "HR");

            Assert.AreEqual(3, itGroup.Count());
            Assert.AreEqual(2, hrGroup.Count());
        }

        [TestMethod]
        public async Task ComplexQuery_CombinesMultipleOperations()
        {
            // Arrange
            var data = GetTestData();
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act
            var result = await asyncEnumerable
                .Where(x => x.Age >= 28)
                .OrderByDescending(x => x.Age)
                .Select(x => new { x.Name, x.Age })
                .Skip(1)
                .Take(2)
                .ToListAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Eve", result[0].Name);
            Assert.AreEqual("Alice", result[1].Name);
        }
    }
}