// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockAsyncEnumerable
//  Author           : RzR
//  Created On       : 2026-02-27 11:02
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-03-05 17:43
// ***********************************************************************
//  <copyright file="AsyncEnumerableExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using RzR.Extensions.EntityMock.Helpers;
using RzR.Extensions.EntityMock.Helpers.Internal;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace RzR.Extensions.EntityMock.Extensions
{
    /// <summary>
    ///     Extension methods for creating mock async enumerable
    /// </summary>
    public static class AsyncEnumerableExtensions
    {
        /// <summary>
        ///     Converts an <see cref="IEnumerable{T}" /> to a mock <see cref="IAsyncEnumerable{T}" />
        ///     for testing purposes.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        /// <param name="source">The source enumerable</param>
        /// <returns>A mock async enumerable that can be used for testing EF Core queries</returns>
        /// <exception cref="ArgumentNullException">Thrown when source is null</exception>
        /// <example>
        ///     <code>
        ///         var data = new List&lt;User&gt; { new User { Id = 1 }, new User { Id = 2 } };
        ///         var mockDbSet = data.ToMockAsyncEnumerable();
        ///         var result = await mockDbSet.ToListAsync();
        ///     </code>
        /// </example>
        public static AsyncEnumerable<T> ToMockAsyncEnumerable<T>(this IEnumerable<T> source)
        {
            GuardEnsure.NotNullable(source, nameof(source));

            return new AsyncEnumerable<T>(source);
        }

        /// <summary>
        ///     Converts an <see cref="IQueryable{T}" /> to a mock <see cref="IAsyncEnumerable{T}" />
        ///     for testing purposes.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        /// <param name="source">The source queryable</param>
        /// <returns>A mock async enumerable that can be used for testing EF Core queries</returns>
        /// <exception cref="ArgumentNullException">Thrown when source is null</exception>
        /// <example>
        ///     <code>
        ///         var query = users.Where(u => u.IsActive).AsQueryable();
        ///         var mockDbSet = query.ToMockAsyncEnumerable();
        ///         var result = await mockDbSet.ToListAsync();
        ///     </code>
        /// </example>
        public static AsyncEnumerable<T> ToMockAsyncEnumerable<T>(this IQueryable<T> source)
        {
            GuardEnsure.NotNullable(source, nameof(source));

            return new AsyncEnumerable<T>(source);
        }
        
        /// <summary>
        ///     Converts an array to a mock async enumerable.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection</typeparam>
        /// <param name="source">The source array</param>
        /// <returns>A mock async enumerable that can be used for testing EF Core queries</returns>
        /// <exception cref="ArgumentNullException">Thrown when source is null</exception>
        public static AsyncEnumerable<T> ToMockAsyncEnumerable<T>(this T[] source)
        {
            GuardEnsure.NotNullable(source, nameof(source));

            return new AsyncEnumerable<T>(source);
        }
    }
}