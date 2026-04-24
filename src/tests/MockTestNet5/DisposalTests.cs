// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockTestNet5
//  Author           : RzR
//  Created On       : 2026-02-27 11:02
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-02-27 11:39
// ***********************************************************************
//  <copyright file="DisposalTests.cs" company="RzR SOFT & TECH">
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
using MockTestNet5.Models;
using RzR.Extensions.EntityMock;

#pragma warning disable CS0618

namespace MockTestNet5
{
    [TestClass]
    public class DisposalTests
    {
        private class DisposableTestEntity : IDisposable
        {
            public int Id { get; set; }

            public bool IsDisposed { get; private set; }

            public void Dispose()
            {
                IsDisposed = true;
            }
        }

        [TestMethod]
        public async Task GetAsyncEnumerator_CanBeDisposed()
        {
            // Arrange
            var data = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "Alice" }
            };
            var asyncEnumerable = EnumerableInvoker.Invoke(data);

            // Act & Assert - Should not throw
            await using (var enumerator = asyncEnumerable.GetAsyncEnumerator())
            {
                await enumerator.MoveNextAsync();
            }
        }

        [TestMethod]
        public async Task MultipleDisposals_DoNotThrow()
        {
            // Arrange
            var data = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "Alice" }
            };
            var asyncEnumerable = EnumerableInvoker.Invoke(data);
            var enumerator = asyncEnumerable.GetAsyncEnumerator();

            // Act - Dispose multiple times
            await enumerator.DisposeAsync();
            await enumerator.DisposeAsync();
            await enumerator.DisposeAsync();

            // Assert - Should not throw
            Assert.IsTrue(true);
        }

        [TestMethod]
        public async Task AwaitForeach_AutomaticallyDisposesEnumerator()
        {
            // Arrange
            var data = new List<TestEntity>
            {
                new TestEntity { Id = 1, Name = "Alice" },
                new TestEntity { Id = 2, Name = "Bob" }
            };
            var asyncEnumerable = EnumerableInvoker.Invoke(data);
            var count = 0;

            // Act
            await foreach (var entity in asyncEnumerable)
            {
                count++;
            }

            // Assert
            Assert.AreEqual(2, count);
        }
    }
}