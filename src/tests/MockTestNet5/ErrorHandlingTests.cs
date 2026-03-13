// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockTestNet5
//  Author           : RzR
//  Created On       : 2026-02-27 11:02
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-02-27 11:37
// ***********************************************************************
//  <copyright file="ErrorHandlingTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MockAsyncEnumerable;
using MockAsyncEnumerable.Helpers;
using MockTestNet5.Models;

namespace MockTestNet5
{
    [TestClass]
    public class ErrorHandlingTests
    {
        [TestMethod]
        public void Invoke_WithNullEnumerable_ThrowsArgumentNullException()
        {
            // Arrange
            IEnumerable<TestEntity> data = null;
            
            // Assert - Exception expected
            Assert.ThrowsException<ArgumentNullException>(
                () => EnumerableInvoker.Invoke(data));
        }

        [TestMethod]
        public void AsyncEnumerable_WithNullEnumerable_ThrowsArgumentNullException()
        {
            // Arrange
            IEnumerable<TestEntity> data = null;

            // Assert - Exception expected
            Assert.ThrowsException<ArgumentNullException>(
                () => new AsyncEnumerable<TestEntity>(data));
        }

        [TestMethod]
        public async Task FirstAsync_OnEmptyCollection_ThrowsInvalidOperationException()
        {
            // Arrange
            var data = new List<TestEntity>();
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                async () => await asyncEnumerable.FirstAsync());
        }

        [TestMethod]
        public async Task SingleAsync_WithMultipleElements_ThrowsInvalidOperationException()
        {
            // Arrange
            var data = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "Alice" },
                new TestEntity { Id = 2, Name = "Bob" }
            };
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act & Assert
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(
                async () => await asyncEnumerable.SingleAsync());
        }
    }
}