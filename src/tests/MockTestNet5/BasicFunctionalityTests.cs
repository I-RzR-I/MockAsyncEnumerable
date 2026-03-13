// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockTestNet5
//  Author           : RzR
//  Created On       : 2026-02-27 11:02
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-02-27 11:33
// ***********************************************************************
//  <copyright file="BasicFunctionalityTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MockAsyncEnumerable;
using MockTestNet5.Models;

namespace MockTestNet5
{
    [TestClass]
    public class BasicFunctionalityTests
    {
        private List<TestEntity> GetTestData()
        {
            return new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "Alice", Age = 30 },
                new TestEntity { Id = 2, Name = "Bob", Age = 25 },
                new TestEntity { Id = 3, Name = "Charlie", Age = 35 },
                new TestEntity { Id = 4, Name = "Diana", Age = 28 },
                new TestEntity { Id = 5, Name = "Eve", Age = 32 }
            };
        }

        [TestMethod]
        public async Task ToListAsync_ReturnsAllElements()
        {
            // Arrange
            var data = GetTestData();
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act
            var result = await asyncEnumerable.ToListAsync();

            // Assert
            Assert.AreEqual(5, result.Count);
            Assert.AreEqual("Alice", result[0].Name);
            Assert.AreEqual("Eve", result[4].Name);
        }

        [TestMethod]
        public async Task CountAsync_ReturnsCorrectCount()
        {
            // Arrange
            var data = GetTestData();
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act
            var count = await asyncEnumerable.CountAsync();

            // Assert
            Assert.AreEqual(5, count);
        }

        [TestMethod]
        public async Task FirstOrDefaultAsync_ReturnsFirstElement()
        {
            // Arrange
            var data = GetTestData();
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act
            var result = await asyncEnumerable.FirstOrDefaultAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Alice", result.Name);
        }

        [TestMethod]
        public async Task FirstOrDefaultAsync_WithPredicate_ReturnsMatchingElement()
        {
            // Arrange
            var data = GetTestData();
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act
            var result = await asyncEnumerable.FirstOrDefaultAsync(x => x.Name == "Bob");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual("Bob", result.Name);
        }

        [TestMethod]
        public async Task FirstOrDefaultAsync_NoMatch_ReturnsNull()
        {
            // Arrange
            var data = GetTestData();
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act
            var result = await asyncEnumerable.FirstOrDefaultAsync(x => x.Name == "NonExistent");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task AnyAsync_WithMatchingElements_ReturnsTrue()
        {
            // Arrange
            var data = GetTestData();
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act
            var result = await asyncEnumerable.AnyAsync(x => x.Age > 30);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task AnyAsync_WithNoMatch_ReturnsFalse()
        {
            // Arrange
            var data = GetTestData();
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act
            var result = await asyncEnumerable.AnyAsync(x => x.Age > 100);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task EmptyCollection_ReturnsEmptyResult()
        {
            // Arrange
            var data = new List<TestEntity>();
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act
            var result = await asyncEnumerable.ToListAsync();

            // Assert
            Assert.AreEqual(0, result.Count);
        }
    }
}