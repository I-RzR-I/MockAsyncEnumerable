// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockTestNet5
//  Author           : RzR
//  Created On       : 2026-03-05 17:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-05 17:55
// ***********************************************************************
//  <copyright file="AsyncEnumerableFactoryTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MockAsyncEnumerable;
using MockTestNet5.Models;

namespace MockTestNet5
{
    [TestClass]
    public class AsyncEnumerableFactoryTests
    {
        #region Empty()

        [TestMethod]
        public async Task Factory_Empty_ReturnsEmptyAsyncEnumerable()
        {
            // Act
            var result = AsyncEnumerableFactory.Empty<TestEntity>();

            // Assert
            Assert.IsNotNull(result);
            var list = await result.ToListAsync();
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public async Task Factory_Empty_SupportsLinqOperations()
        {
            // Act
            var result = AsyncEnumerableFactory.Empty<TestEntity>();
            var count = await result.CountAsync();

            // Assert
            Assert.AreEqual(0, count);
        }

        #endregion

        #region Single()

        [TestMethod]
        public async Task Factory_Single_ReturnsSingleItemEnumerable()
        {
            // Arrange
            var entity = new TestEntity { Id = 1, Name = "Alice" };

            // Act
            var result = AsyncEnumerableFactory.Single(entity);

            // Assert
            var list = await result.ToListAsync();
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("Alice", list[0].Name);
        }

        [TestMethod]
        public async Task Factory_Single_WithNull_ReturnsEnumerableWithNull()
        {
            // Act
            var result = AsyncEnumerableFactory.Single<TestEntity>(null);

            // Assert
            var list = await result.ToListAsync();
            Assert.AreEqual(1, list.Count);
            Assert.IsNull(list[0]);
        }

        #endregion

        #region Create()

        [TestMethod]
        public async Task Factory_Create_WithMultipleItems_ReturnsAsyncEnumerable()
        {
            // Arrange
            var entity1 = new TestEntity { Id = 1, Name = "Alice" };
            var entity2 = new TestEntity { Id = 2, Name = "Bob" };
            var entity3 = new TestEntity { Id = 3, Name = "Charlie" };

            // Act
            var result = AsyncEnumerableFactory.Create(entity1, entity2, entity3);

            // Assert
            var list = await result.ToListAsync();
            Assert.AreEqual(3, list.Count);
            Assert.AreEqual("Alice", list[0].Name);
            Assert.AreEqual("Bob", list[1].Name);
            Assert.AreEqual("Charlie", list[2].Name);
        }

        [TestMethod]
        public async Task Factory_Create_WithNoItems_ReturnsEmpty()
        {
            // Act
            var result = AsyncEnumerableFactory.Create<TestEntity>();

            // Assert
            var list = await result.ToListAsync();
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Factory_Create_WithNullArray_ThrowsArgumentNullException()
        {
            // Act
            var result = AsyncEnumerableFactory.Create<TestEntity>(null);

            // Assert - Exception expected
        }

        #endregion

        #region Builder()

        [TestMethod]
        public void Factory_Builder_ReturnsNewBuilder()
        {
            // Act
            var builder = AsyncEnumerableFactory.Builder<TestEntity>();

            // Assert
            Assert.IsNotNull(builder);
            Assert.IsInstanceOfType(builder, typeof(AsyncEnumerableBuilder<TestEntity>));
        }

        [TestMethod]
        public async Task Factory_Builder_CanBeUsedFluently()
        {
            // Act
            var result = AsyncEnumerableFactory.Builder<TestEntity>()
                .Add(new TestEntity { Id = 1, Name = "Alice" })
                .Add(new TestEntity { Id = 2, Name = "Bob" })
                .Build();

            // Assert
            var list = await result.ToListAsync();
            Assert.AreEqual(2, list.Count);
        }

        #endregion
    }
}