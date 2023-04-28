// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockAsyncEnumerable
//  Author           : RzR
//  Created On       : 2023-04-28 12:41
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-04-28 14:08
// ***********************************************************************
//  <copyright file="AsyncEnumerable.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

#endregion

namespace MockAsyncEnumerable.Helpers
{
    /// <inheritdoc cref="IAsyncEnumerable{T}" />
    public class AsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        /// <inheritdoc />
        public AsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable)
        {
        }

        /// <inheritdoc />
        public AsyncEnumerable(Expression expression) : base(expression)
        {
        }

        /// <inheritdoc />
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
            => new AsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());

        /// <summary>
        ///     Gets current provider.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        IQueryProvider IQueryable.Provider => new AsyncQueryProvider<T>(this);

        /// <summary>
        ///     Get enumerator
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public IAsyncEnumerator<T> GetEnumerator()
            => new AsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
    }
}