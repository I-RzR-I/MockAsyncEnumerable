// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockTestNet5
//  Author           : RzR
//  Created On       : 2026-03-05 17:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-05 17:54
// ***********************************************************************
//  <copyright file="AsyncEnumerableBuilderTests.cs" company="RzR SOFT & TECH">
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
using MockTestNet5.Models;
using RzR.Extensions.EntityMock;

namespace MockTestNet5
{
    [TestClass]
    public class AsyncEnumerableBuilderTests
    {
        [TestMethod]
        public void Builder_CanBeCreated()
        {
            // Act
            var builder = new AsyncEnumerableBuilder<TestEntity>();

            // Assert
            Assert.IsNotNull(builder);
        }

        [TestMethod]
        public async Task Builder_Add_AddsSingleItem()
        {
            // Arrange
            var builder = new AsyncEnumerableBuilder<TestEntity>();
            var entity = new TestEntity { Id = 1, Name = "Alice" };

            // Act
            builder.Add(entity);
            var result = builder.Build();

            // Assert
            var list = await result.ToListAsync();
            Assert.AreEqual(1, list.Count);
            Assert.AreEqual("Alice", list[0].Name);
        }

        [TestMethod]
        public async Task Builder_Add_SupportsChaining()
        {
            // Arrange
            var builder = new AsyncEnumerableBuilder<TestEntity>();

            // Act
            var result = builder
                .Add(new TestEntity { Id = 1, Name = "Alice" })
                .Add(new TestEntity { Id = 2, Name = "Bob" })
                .Add(new TestEntity { Id = 3, Name = "Charlie" })
                .Build();

            // Assert
            var list = await result.ToListAsync();
            Assert.AreEqual(3, list.Count);
        }

        [TestMethod]
        public async Task Builder_AddRange_AddsMultipleItems()
        {
            // Arrange
            var builder = new AsyncEnumerableBuilder<TestEntity>();
            var entities = new[]
            {
                new TestEntity { Id = 1, Name = "Alice" },
                new TestEntity { Id = 2, Name = "Bob" }
            };

            // Act
            builder.AddRange(entities);
            var result = builder.Build();

            // Assert
            var list = await result.ToListAsync();
            Assert.AreEqual(2, list.Count);
        }

        [TestMethod]
        public void Builder_AddRange_WithNull_ThrowsArgumentNullException()
        {
            // Arrange
            var builder = new AsyncEnumerableBuilder<TestEntity>();

            // Act
            Assert.ThrowsException<ArgumentNullException>(() => builder.AddRange(null));
        }

        [TestMethod]
        public async Task Builder_CombineAddAndAddRange_Works()
        {
            // Arrange
            var builder = new AsyncEnumerableBuilder<TestEntity>();
            var moreEntities = new[]
            {
                new TestEntity { Id = 3, Name = "Charlie" },
                new TestEntity { Id = 4, Name = "Diana" }
            };

            // Act
            var result = builder
                .Add(new TestEntity { Id = 1, Name = "Alice" })
                .Add(new TestEntity { Id = 2, Name = "Bob" })
                .AddRange(moreEntities)
                .Build();

            // Assert
            var list = await result.ToListAsync();
            Assert.AreEqual(4, list.Count);
        }

        [TestMethod]
        public async Task Builder_Build_WithNoItems_ReturnsEmpty()
        {
            // Arrange
            var builder = new AsyncEnumerableBuilder<TestEntity>();

            // Act
            var result = builder.Build();

            // Assert
            var list = await result.ToListAsync();
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public async Task Builder_CanBuildMultipleTimes()
        {
            // Arrange
            var builder = new AsyncEnumerableBuilder<TestEntity>();
            builder.Add(new TestEntity { Id = 1, Name = "Alice" });

            // Act
            var result1 = builder.Build();
            var result2 = builder.Build();

            // Assert - Both should work
            var list1 = await result1.ToListAsync();
            var list2 = await result2.ToListAsync();
            Assert.AreEqual(1, list1.Count);
            Assert.AreEqual(1, list2.Count);
        }

        [TestMethod]
        public async Task Builder_FluentStyle_ReadsNaturally()
        {
            // Arrange & Act
            var result = new AsyncEnumerableBuilder<TestEntity>()
                .Add(new TestEntity { Id = 1, Name = "Alice" })
                .Add(new TestEntity { Id = 2, Name = "Bob" })
                .Build();

            // Assert
            var list = await result.ToListAsync();
            Assert.AreEqual(2, list.Count);
        }
    }
}