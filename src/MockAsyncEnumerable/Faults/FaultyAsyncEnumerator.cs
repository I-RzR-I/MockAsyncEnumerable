// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockAsyncEnumerable
//  Author           : RzR
//  Created On       : 2026-04-24 09:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-24 09:47
// ***********************************************************************
//  <copyright file="FaultyAsyncEnumerator.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using RzR.Extensions.EntityMock.Helpers.Internal;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace RzR.Extensions.EntityMock.Faults
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     <see cref="IAsyncEnumerator{T}" /> decorator that injects configurable faults (delay,
    ///     exceptions, cancellation) on top of a synchronous source enumerator.
    /// </summary>
    /// <typeparam name="T">The element type of the sequence.</typeparam>
    /// <seealso cref="T:System.Collections.Generic.IAsyncEnumerator{T}"/>
    /// <seealso cref="T:IDisposable"/>
    /// =================================================================================================
    internal sealed class FaultyAsyncEnumerator<T> : IAsyncEnumerator<T>, IDisposable
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) A token that allows processing to be cancelled.
        /// </summary>
        /// =================================================================================================
        private readonly CancellationToken _effectiveToken;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the inner.
        /// </summary>
        /// =================================================================================================
        private readonly IEnumerator<T> _inner;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the internal cts.
        /// </summary>
        /// =================================================================================================
        private readonly CancellationTokenSource _internalCts;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the linked cts.
        /// </summary>
        /// =================================================================================================
        private readonly CancellationTokenSource _linkedCts;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) options for controlling the operation.
        /// </summary>
        /// =================================================================================================
        private readonly FaultInjectionOptions _options;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     True if disposed.
        /// </summary>
        /// =================================================================================================
        private bool _disposed;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Number of yielded.
        /// </summary>
        /// =================================================================================================
        private int _yieldedCount;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="FaultyAsyncEnumerator{T}"/> class.
        /// </summary>
        /// <param name="inner">The inner.</param>
        /// <param name="options">Options for controlling the operation.</param>
        /// <param name="cancellationToken">A token that allows processing to be cancelled.</param>
        /// =================================================================================================
        public FaultyAsyncEnumerator(
            IEnumerator<T> inner,
            FaultInjectionOptions options,
            CancellationToken cancellationToken)
        {
            _inner = GuardEnsure.NotNull(inner);
            _options = GuardEnsure.NotNull(options);

            if (_options.CancelAfter.HasValue)
            {
                _internalCts = new CancellationTokenSource(_options.CancelAfter.Value);
                _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
                    cancellationToken, _internalCts.Token);
                _effectiveToken = _linkedCts.Token;
            }
            else
                _effectiveToken = cancellationToken;
        }

        /// <inheritdoc/>
        public T Current => _inner.Current;

        /// <inheritdoc/>
        public async ValueTask<bool> MoveNextAsync()
        {
            _effectiveToken.ThrowIfCancellationRequested();

            if (_options.PerItemDelay.HasValue)
            {
                await Task.Delay(_options.PerItemDelay.Value, _effectiveToken)
                    .ConfigureAwait(false);
            }

            if (_options.ThrowAfterIndex.HasValue
                && _yieldedCount == _options.ThrowAfterIndex.Value)
            {
                var factory = _options.ExceptionFactory
                              ?? (i => new InvalidOperationException(
                                  $"Fault injected at index {i}."));
                throw factory(_yieldedCount);
            }

            var hasNext = _inner.MoveNext();
            if (hasNext)
                _yieldedCount++;

            return hasNext;
        }

        /// <inheritdoc/>
        public ValueTask DisposeAsync()
        {
            Dispose();

            return default;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;

            try
            { _inner.Dispose(); }
            catch
            {
                /* swallow */
            }

            _linkedCts?.Dispose();
            _internalCts?.Dispose();
        }
    }
}