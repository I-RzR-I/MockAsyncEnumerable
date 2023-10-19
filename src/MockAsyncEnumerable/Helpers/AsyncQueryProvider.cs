// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockAsyncEnumerable
//  Author           : RzR
//  Created On       : 2023-04-28 12:41
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-04-28 14:08
// ***********************************************************************
//  <copyright file="AsyncQueryProvider.cs" company="">
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
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

#endregion

namespace MockAsyncEnumerable.Helpers
{
    /// <inheritdoc cref="IAsyncQueryProvider" />
    internal class AsyncQueryProvider<TEntity> : IAsyncQueryProvider
    {
        /// <summary>
        ///     Current query provider
        /// </summary>
        /// <remarks></remarks>
        private readonly IQueryProvider _queryProvider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncQueryProvider{TEntity}" />
        ///     class.
        /// </summary>
        /// <param name="queryProvider">Inner query provider</param>
        /// <remarks></remarks>
        internal AsyncQueryProvider(IQueryProvider queryProvider) => _queryProvider = queryProvider;

        /// <inheritdoc />
        public IQueryable CreateQuery(Expression expression)
            => new AsyncEnumerable<TEntity>(expression);

        /// <inheritdoc />
        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
            => new AsyncEnumerable<TElement>(expression);

        /// <inheritdoc />
        public object Execute(Expression expression)
            => _queryProvider.Execute(expression);

        /// <inheritdoc />
        public TResult Execute<TResult>(Expression expression)
            => _queryProvider.Execute<TResult>(expression);

        /// <inheritdoc />
        TResult IAsyncQueryProvider.ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
        {
            var expectedResultType = typeof(TResult).GetGenericArguments()[0];
            var executionResult = typeof(IQueryProvider)
                .GetMethods()
                .First(method => method.Name == nameof(IQueryProvider.Execute) && method.IsGenericMethod)
                .MakeGenericMethod(expectedResultType)
                .Invoke(this, new object[] { expression });

            return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))!
                .MakeGenericMethod(expectedResultType)
                .Invoke(null, new[] { executionResult });
        }

        /// <summary>
        ///     Execute expression async
        /// </summary>
        /// <param name="expression">Expression</param>
        /// <returns></returns>
        /// <typeparam name="TResult">Type of entity</typeparam>
        /// <remarks></remarks>
        public IAsyncEnumerable<TResult> ExecuteAsync<TResult>(Expression expression)
            => new AsyncEnumerable<TResult>(expression);

        /// <summary>
        ///     Execute expression async
        /// </summary>
        /// <param name="expression">Expression</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        /// <typeparam name="TResult">Type of entity</typeparam>
        /// <remarks></remarks>
        public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
            => Task.FromResult(Execute<TResult>(expression));
    }
}