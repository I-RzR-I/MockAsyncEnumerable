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

using RzR.Extensions.EntityMock.Abstractions;
using RzR.Extensions.EntityMock.Faults;
using RzR.Extensions.EntityMock.Helpers;
using RzR.Extensions.EntityMock.Helpers.Internal;
using System;
using System.Collections.Generic;

#endregion

namespace RzR.Extensions.EntityMock
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Builder class for creating mock async enumerable with a fluent API.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// =================================================================================================
    public class AsyncEnumerableBuilder<T>
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the items.
        /// </summary>
        /// =================================================================================================
        private readonly List<T> _items = new List<T>();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The faults.
        /// </summary>
        /// =================================================================================================
        private FaultInjectionOptions _faults;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the faults.
        /// </summary>
        /// <value>
        ///     The faults.
        /// </value>
        /// =================================================================================================
        private FaultInjectionOptions Faults
            => _faults ?? (_faults = new FaultInjectionOptions());

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds an item to the builder.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>
        ///     The builder for method chaining.
        /// </returns>
        /// =================================================================================================
        public AsyncEnumerableBuilder<T> Add(T item)
        {
            GuardEnsure.NotNullable(item, nameof(item));
            _items.Add(item);

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Adds multiple items to the builder.
        /// </summary>
        /// <param name="items">The items to add.</param>
        /// <returns>
        ///     The builder for method chaining.
        /// </returns>
        /// =================================================================================================
        public AsyncEnumerableBuilder<T> AddRange(IEnumerable<T> items)
        {
            GuardEnsure.NotNullable(items, nameof(items));

            _items.AddRange(items);

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Configures an artificial delay applied before every <c>MoveNextAsync</c>
        ///     call on the produced sequence. Useful for exercising timeout, cancellation and back-
        ///     pressure behaviour in the code under test.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="delay" /> is negative.
        /// </exception>
        /// <param name="delay">Per-item delay; must be non-negative.</param>
        /// <returns>
        ///     The builder for method chaining.
        /// </returns>
        /// =================================================================================================
        public AsyncEnumerableBuilder<T> WithDelay(TimeSpan delay)
        {
            if (delay < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(delay), "Delay must be non-negative.");

            Faults.PerItemDelay = delay;

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Configures the produced sequence to throw the supplied exception
        ///     <i>instead of</i> yielding the item at the given zero-based index.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="afterIndex" /> is negative.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="exception" /> is <see langword="null" />.
        /// </exception>
        /// <param name="afterIndex">
        ///     Zero-based position at which the exception should be raised. <c>0</c> throws before any
        ///     element is produced; <c>N</c> throws after <c>N</c> successful items.
        /// </param>
        /// <param name="exception">The exception instance to throw.</param>
        /// <returns>
        ///     The builder for method chaining.
        /// </returns>
        /// =================================================================================================
        public AsyncEnumerableBuilder<T> ThrowAfter(int afterIndex, Exception exception)
        {
            GuardEnsure.NotNull(exception, nameof(exception));

            return ThrowAfter(afterIndex, _ => exception);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Configures the produced sequence to throw an exception (created by the supplied factory)
        ///     at the given zero-based index.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="afterIndex" /> is negative.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="exceptionFactory" /> is <see langword="null" />.
        /// </exception>
        /// <param name="afterIndex">Zero-based position at which to throw.</param>
        /// <param name="exceptionFactory">
        ///     Factory invoked with the index that triggered the throw. Must not return
        ///     <see langword="null" />.
        /// </param>
        /// <returns>
        ///     The builder for method chaining.
        /// </returns>
        /// =================================================================================================
        public AsyncEnumerableBuilder<T> ThrowAfter(int afterIndex, Func<int, Exception> exceptionFactory)
        {
            if (afterIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(afterIndex), "Index must be non-negative.");
            GuardEnsure.NotNull(exceptionFactory, nameof(exceptionFactory));

            Faults.ThrowAfterIndex = afterIndex;
            Faults.ExceptionFactory = exceptionFactory;

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Cancels the enumeration after the supplied duration, regardless of whether the caller
        ///     passed a <see cref="System.Threading.CancellationToken" />. The internal token is linked
        ///     with the caller token so either source aborts iteration with <see cref="OperationCanceledException" />
        ///     .
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="duration" /> is negative.
        /// </exception>
        /// <param name="duration">Time after which to cancel; must be non-negative.</param>
        /// <returns>
        ///     The builder for method chaining.
        /// </returns>
        /// =================================================================================================
        public AsyncEnumerableBuilder<T> CancelAfter(TimeSpan duration)
        {
            if (duration < TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(duration), "Duration must be non-negative.");

            Faults.CancelAfter = duration;

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Builds the mock async enumerable.
        /// </summary>
        /// <returns>
        ///     A mock async enumerable containing all added items.
        /// </returns>
        /// =================================================================================================
        public IMockAsyncEnumerable<T> Build()
        {
            var snapshot = _items.ToArray();

            return _faults == null || _faults.IsEmpty
                ? (IMockAsyncEnumerable<T>)new AsyncEnumerable<T>(snapshot)
                : new FaultyAsyncEnumerable<T>(snapshot, _faults);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Clears this object to its blank/initial state.
        /// </summary>
        /// =================================================================================================
        public void Clear()
        {
            _items.Clear();
            _faults = null;
        }
    }
}