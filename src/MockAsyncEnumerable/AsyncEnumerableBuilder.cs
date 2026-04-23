// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockAsyncEnumerable
//  Author           : RzR
//  Created On       : 2026-03-05 17:03
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-05 17:43
// ***********************************************************************
//  <copyright file="AsyncEnumerableBuilder.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using RzR.Extensions.EntityMock.Helpers;
using RzR.Extensions.EntityMock.Helpers.Internal;
using System.Collections.Generic;

#endregion

namespace RzR.Extensions.EntityMock
{
    /// <summary>
    ///     Builder class for creating mock async enumerable with a fluent API
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection</typeparam>
    public class AsyncEnumerableBuilder<T>
    {
        private readonly List<T> _items = new List<T>();

        /// <summary>
        ///     Adds an item to the builder
        /// </summary>
        /// <param name="item">The item to add</param>
        /// <returns>The builder for method chaining</returns>
        public AsyncEnumerableBuilder<T> Add(T item)
        {
            GuardEnsure.NotNullable(item, nameof(item));
            _items.Add(item);

            return this;
        }

        /// <summary>
        ///     Adds multiple items to the builder
        /// </summary>
        /// <param name="items">The items to add</param>
        /// <returns>The builder for method chaining</returns>
        public AsyncEnumerableBuilder<T> AddRange(IEnumerable<T> items)
        {
            GuardEnsure.NotNullable(items, nameof(items));

            _items.AddRange(items);

            return this;
        }

        /// <summary>
        ///     Builds the mock async enumerable
        /// </summary>
        /// <returns>A mock async enumerable containing all added items</returns>
        public AsyncEnumerable<T> Build() => new AsyncEnumerable<T>(_items);

        /// <summary>
        ///     Clears this object to its blank/initial state.
        /// </summary>
        public void Clear() => _items.Clear();
    }
}