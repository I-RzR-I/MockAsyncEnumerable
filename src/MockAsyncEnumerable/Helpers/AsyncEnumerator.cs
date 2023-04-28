// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockAsyncEnumerable
//  Author           : RzR
//  Created On       : 2023-04-28 12:41
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-04-28 14:08
// ***********************************************************************
//  <copyright file="AsyncEnumerator.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace MockAsyncEnumerable.Helpers
{
    /// <inheritdoc cref="IAsyncEnumerator{T}" />
    internal class AsyncEnumerator<T> : IAsyncEnumerator<T>, IDisposable
    {
        private readonly IEnumerator<T> _inner;
        private bool _disposed;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncEnumerator{T}" /> class.
        /// </summary>
        /// <param name="inner">Inner enumerator</param>
        /// <remarks></remarks>
        public AsyncEnumerator(IEnumerator<T> inner)
            => _inner = inner;

        /// <inheritdoc />
        public async ValueTask<bool> MoveNextAsync()
            => await Task.FromResult(_inner.MoveNext());

        /// <inheritdoc />
        public T Current => _inner.Current;

        /// <inheritdoc />
        public async ValueTask DisposeAsync()
        {
            if (_disposed)
                return;

            Dispose(true);
            _disposed = true;

            await Task.CompletedTask;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_disposed)
                return;

            Dispose(true);
            GC.SuppressFinalize(this);

            _disposed = true;
        }

        /// <summary>
        ///     Releases the unmanaged resources used by the
        ///     <see cref="AsyncEnumerator{T}" /> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">If set to <see langword="true" />, then enumerator will be disposed; otherwise, not.</param>
        /// <remarks></remarks>
        private void Dispose(bool disposing)
        {
            if (disposing.Equals(true))
                _inner.Dispose();
        }

        /// <summary>
        ///     Move next value
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Task<bool> MoveNext(CancellationToken cancellationToken)
            => Task.FromResult(_inner.MoveNext());
    }
}