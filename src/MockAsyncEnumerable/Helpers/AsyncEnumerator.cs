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

using MockAsyncEnumerable.Helpers.Internal;
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
        /// <summary>
        ///     Inner enumerator
        /// </summary>
        private readonly IEnumerator<T> _innerEnumerator;

        /// <summary>
        ///     Disposed
        /// </summary>
        private bool _disposed;

        /// <inheritdoc />
        public T Current => _innerEnumerator.Current;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncEnumerator{T}" /> class.
        /// </summary>
        /// <param name="innerEnumerator">Inner enumerator</param>
        /// <remarks></remarks>
        public AsyncEnumerator(IEnumerator<T> innerEnumerator)
        {
            GuardEnsure.NotNull(innerEnumerator);

            _innerEnumerator = innerEnumerator;
        }

        /// <inheritdoc />
        public async ValueTask<bool> MoveNextAsync()
            => await Task.FromResult(_innerEnumerator.MoveNext()).ConfigureAwait(false);

        /// <summary>
        ///     Move next value
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public Task<bool> MoveNext(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(_innerEnumerator.MoveNext());
        }

        /// <inheritdoc />
        public async ValueTask DisposeAsync()
        {
            if (_disposed)
                return;

            await DisposeAsyncCore().ConfigureAwait(false);

            Dispose(disposing: false);
            _disposed = true;

            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Dispose asynchronous core.
        /// </summary>
        /// <returns>
        ///     A ValueTask.
        /// </returns>
        protected virtual ValueTask DisposeAsyncCore()
        {
            _innerEnumerator?.Dispose();

            return default;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_disposed)
                return;

            Dispose(disposing: true);
            _disposed = true;

            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Releases the unmanaged resources used by the
        ///     <see cref="AsyncEnumerator{T}" /> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">If set to <see langword="true" />, then enumerator will be disposed; otherwise, not.</param>
        /// <remarks></remarks>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _innerEnumerator?.Dispose();
        }
    }
}