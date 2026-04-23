// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockAsyncEnumerable
//  Author           : RzR
//  Created On       : 2026-03-05 17:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-05 17:42
// ***********************************************************************
//  <copyright file="AsyncEnumerableFactory.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using RzR.Extensions.EntityMock.Helpers;
using RzR.Extensions.EntityMock.Helpers.Internal;
using System.Linq;

#endregion

namespace RzR.Extensions.EntityMock
{
    /// <summary>
    ///     Factory methods for creating mock async enumerable
    /// </summary>
    public static class AsyncEnumerableFactory
    {
        /// <summary>
        ///     Creates an empty mock async enumerable
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <returns>An empty mock async enumerable</returns>
        public static AsyncEnumerable<T> Empty<T>() 
            => new AsyncEnumerable<T>(Enumerable.Empty<T>());

        /// <summary>
        ///     Creates a mock async enumerable from a single item
        /// </summary>
        /// <typeparam name="T">The type of the element</typeparam>
        /// <param name="item">The single item</param>
        /// <returns>A mock async enumerable containing the single item</returns>
        public static AsyncEnumerable<T> Single<T>(T item) 
            => new AsyncEnumerable<T>(new[] { item });

        /// <summary>
        ///     Creates a mock async enumerable from multiple items
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <param name="items">The items to include</param>
        /// <returns>A mock async enumerable containing all items</returns>
        public static AsyncEnumerable<T> Create<T>(params T[] items)
        {
            GuardEnsure.NotNullable(items, nameof(items));

            return new AsyncEnumerable<T>(items);
        }

        /// <summary>
        ///     Creates a builder for constructing mock async enumerable fluently
        /// </summary>
        /// <typeparam name="T">The type of elements</typeparam>
        /// <returns>A new builder instance</returns>
        public static AsyncEnumerableBuilder<T> Builder<T>() 
            => new AsyncEnumerableBuilder<T>();
    }
}