// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockTestNet5
//  Author           : RzR
//  Created On       : 2026-02-27 11:02
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-02-27 11:38
// ***********************************************************************
//  <copyright file="CancellationTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MockTestNet5.Models;
using RzR.Extensions.EntityMock;

namespace MockTestNet5
{
    [TestClass]
    public class CancellationTests
    {
        [TestMethod]
        public async Task ToListAsync_WithValidToken_Succeeds()
        {
            // Arrange
            var data = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "Alice" },
                new TestEntity { Id = 2, Name = "Bob" }
            };
            var asyncEnumerable = EnumerableInvoker.Invoke(data);
            using var cts = new CancellationTokenSource();

            // Act
            var result = await asyncEnumerable.ToListAsync(cts.Token);

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task ToListAsync_WithCancelledToken_ThrowsOperationCanceledException()
        {
            // Arrange
            var data = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "Alice" }
            };
            var asyncEnumerable = EnumerableInvoker.Invoke(data);
            using var cts = new CancellationTokenSource();
            cts.Cancel(); // Cancel immediately

            // Act & Assert
            await Assert.ThrowsExceptionAsync<OperationCanceledException>(
                async () => await asyncEnumerable.ToListAsync(cts.Token));
        }

        [TestMethod]
        public async Task CountAsync_WithCancelledToken_ThrowsOperationCanceledException()
        {
            // Arrange
            var data = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "Alice" }
            };
            var asyncEnumerable = EnumerableInvoker.Invoke(data);
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            // Act & Assert
            await Assert.ThrowsExceptionAsync<OperationCanceledException>(
                async () => await asyncEnumerable.CountAsync(cts.Token));
        }
    }
}