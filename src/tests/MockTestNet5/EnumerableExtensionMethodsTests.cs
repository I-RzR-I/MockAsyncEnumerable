// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockTestNet5
//  Author           : RzR
//  Created On       : 2026-03-05 17:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-05 17:49
// ***********************************************************************
//  <copyright file="EnumerableExtensionMethodsTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockTestNet5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MockAsyncEnumerable.Extensions;
using MockAsyncEnumerable.Helpers;

namespace MockTestNet5
{
    [TestClass]
    public class EnumerableExtensionMethodsTests
    {
        private IEnumerable<TestEntity> GetTestData()
        {
            return new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "Alice", IsActive = true },
                new TestEntity { Id = 2, Name = "Bob", IsActive = false },
                new TestEntity { Id = 3, Name = "Charlie", IsActive = true }
            };
        }

        [TestMethod]
        public async Task ToMockAsyncEnumerable_WithList_ReturnsAsyncEnumerable()
        {
            // Arrange
            var data = GetTestData();

            // Act
            var result = data.ToMockAsyncEnumerable();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(AsyncEnumerable<TestEntity>));

            var list = await result.ToListAsync();
            Assert.AreEqual(3, list.Count);
        }

        [TestMethod]
        public async Task ToMockAsyncEnumerable_WithIEnumerable_Works()
        {
            // Arrange
            IEnumerable<TestEntity> data = GetTestData();

            // Act
            var result = data.ToMockAsyncEnumerable();

            // Assert
            var list = await result.ToListAsync();
            Assert.AreEqual(3, list.Count);
        }

        [TestMethod]
        public async Task ToMockAsyncEnumerable_WithLinqQuery_Works()
        {
            // Arrange
            var data = GetTestData();
            var filtered = data.Where(x => x.IsActive);

            // Act
            var result = filtered.ToMockAsyncEnumerable();

            // Assert
            var list = await result.ToListAsync();
            Assert.AreEqual(2, list.Count);
            Assert.IsTrue(list.All(x => x.IsActive));
        }

        [TestMethod]
        public void ToMockAsyncEnumerable_WithNull_ThrowsArgumentNullException()
        {
            // Arrange
            IEnumerable<TestEntity> data = null;

            // Act
            Assert.ThrowsException<ArgumentNullException>(() => data.ToMockAsyncEnumerable());
        }

        [TestMethod]
        public async Task ToMockAsyncEnumerable_WithEmptyList_ReturnsEmpty()
        {
            // Arrange
            var data = new List<TestEntity>();

            // Act
            var result = data.ToMockAsyncEnumerable();

            // Assert
            var list = await result.ToListAsync();
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public async Task ToMockAsyncEnumerable_SupportsChaining()
        {
            // Arrange
            var data = GetTestData();

            // Act
            var result = await data.ToMockAsyncEnumerable()
                .Where(x => x.IsActive)
                .OrderBy(x => x.Name)
                .Select(x => x.Name)
                .ToListAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Alice", result[0]);
            Assert.AreEqual("Charlie", result[1]);
        }

        [TestMethod]
        public async Task ToMockAsyncEnumerable_WithQueryable_ReturnsAsyncEnumerable()
        {
            // Arrange
            var data = GetTestData().AsQueryable();

            // Act
            var result = data.ToMockAsyncEnumerable();

            // Assert
            Assert.IsNotNull(result);
            var list = await result.ToListAsync();
            Assert.AreEqual(3, list.Count);
        }

        [TestMethod]
        public async Task ToMockAsyncEnumerable_WithQueryableFilter_Works()
        {
            // Arrange
            var data = GetTestData().AsQueryable().Where(x => x.Id > 1);

            // Act
            var result = data.ToMockAsyncEnumerable();

            // Assert
            var list = await result.ToListAsync();
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public void ToMockAsyncEnumerable_WithNullQueryable_ThrowsArgumentNullException()
        {
            // Arrange
            IQueryable<TestEntity> data = null;

            // Act
            Assert.ThrowsException<ArgumentNullException>(() => data.ToMockAsyncEnumerable());
        }

        [TestMethod]
        public async Task AsAsyncEnumerable_WithList_ReturnsAsyncEnumerable()
        {
            // Arrange
            var data = GetTestData();

            // Act
            var result = data.ToMockAsyncEnumerable();

            // Assert
            Assert.IsNotNull(result);
            var list = await result.ToListAsync();
            Assert.AreEqual(3, list.Count);
        }

        [TestMethod]
        public void AsAsyncEnumerable_WithNullList_ThrowsArgumentNullException()
        {
            // Arrange
            List<TestEntity> data = null;

            // Act
            Assert.ThrowsException<ArgumentNullException>(() => data.ToMockAsyncEnumerable());
        }

        [TestMethod]
        public async Task AsAsyncEnumerable_WithEmptyList_ReturnsEmpty()
        {
            // Arrange
            var data = new List<TestEntity>();

            // Act
            var result = data.ToMockAsyncEnumerable();

            // Assert
            var list = await result.ToListAsync();
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public async Task AsAsyncEnumerable_WithArray_ReturnsAsyncEnumerable()
        {
            // Arrange
            var data = new[]
            {
                new TestEntity { Id = 1, Name = "Alice" },
                new TestEntity { Id = 2, Name = "Bob" }
            };

            // Act
            var result = data.ToMockAsyncEnumerable();

            // Assert
            Assert.IsNotNull(result);
            var list = await result.ToListAsync();
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public void AsAsyncEnumerable_WithNullArray_ThrowsArgumentNullException()
        {
            // Arrange
            TestEntity[] data = null;

            // Act
            Assert.ThrowsException<ArgumentNullException>(() => data.ToMockAsyncEnumerable());
        }

        [TestMethod]
        public async Task AsAsyncEnumerable_WithEmptyArray_ReturnsEmpty()
        {
            // Arrange
            var data = new TestEntity[0];

            // Act
            var result = data.ToMockAsyncEnumerable();

            // Assert
            var list = await result.ToListAsync();
            Assert.AreEqual(0, list.Count);
        }
    }
}