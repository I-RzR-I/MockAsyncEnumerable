// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockAsyncEnumerable
//  Author           : RzR
//  Created On       : 2023-04-28 13:24
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-04-28 14:08
// ***********************************************************************
//  <copyright file="EnumerableInvoker.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using RzR.Extensions.EntityMock.Abstractions;
using RzR.Extensions.EntityMock.Helpers;
using System.Collections.Generic;

#endregion

namespace RzR.Extensions.EntityMock
{
    /// <summary>
    ///     Legacy entry point for wrapping an <see cref="IEnumerable{T}" /> as a mock
    ///     async enumerable.
    /// </summary>
    /// <remarks>
    ///     This type is kept for backwards compatibility and will be removed in a future
    ///     major release. Prefer <see cref="AsyncEnumerableFactory" />, the
    ///     <c>ToMockAsyncEnumerable()</c> extension methods, or
    ///     <see cref="AsyncEnumerableBuilder{T}" /> for new code.
    /// </remarks>
    public static class EnumerableInvoker
    {
        /// <summary>
        ///     Wraps the supplied sequence as a mock async enumerable.
        /// </summary>
        /// <param name="enumerable">Records</param>
        /// <typeparam name="T">Entity type</typeparam>
        /// <returns>A mock async enumerable wrapping <paramref name="enumerable" />.</returns>
        public static IMockAsyncEnumerable<T> Invoke<T>(IEnumerable<T> enumerable)
            => new AsyncEnumerable<T>(enumerable);
    }
}