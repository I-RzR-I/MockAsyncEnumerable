// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockAsyncEnumerable
//  Author           : RzR
//  Created On       : 2026-04-24 09:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-24 09:47
// ***********************************************************************
//  <copyright file="FaultInjectionOptions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;

#endregion

namespace RzR.Extensions.EntityMock.Faults
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Configuration captured by <see cref="AsyncEnumerableBuilder{T}" /> describing the
    ///     artificial faults that should be injected into the produced async sequence.
    /// </summary>
    /// <remarks>
    ///     Instances are immutable after the owning enumerable is built. They are used by
    ///     <see cref="FaultyAsyncEnumerable{T}" /> to drive the per-iteration behavior.
    /// </remarks>
    /// =================================================================================================
    internal sealed class FaultInjectionOptions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Optional artificial delay applied before every <c>MoveNextAsync</c> call.
        ///     <see langword="null" /> means no delay.
        /// </summary>
        /// <value>
        ///     The per item delay.
        /// </value>
        /// =================================================================================================
        public TimeSpan? PerItemDelay { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Zero-based index at which to throw an exception. The exception is thrown
        ///     <i>instead</i> of yielding the item at that position. <see langword="null" />
        ///     means never throw.
        /// </summary>
        /// <value>
        ///     The throw after index.
        /// </value>
        /// =================================================================================================
        public int? ThrowAfterIndex { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Factory invoked to produce the exception thrown by <see cref="ThrowAfterIndex" />.
        ///     Receives the zero-based index that triggered the throw.
        /// </summary>
        /// <value>
        ///     A function delegate that yields an Exception.
        /// </value>
        /// =================================================================================================
        public Func<int, Exception> ExceptionFactory { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     If set, an internal cancellation source is started together with the enumeration and
        ///     cancels after this duration. The resulting cancellation is linked with the caller-
        ///     provided token so that either source aborts the iteration with <see cref="OperationCanceledException" />
        ///     .
        /// </summary>
        /// <value>
        ///     The cancel after.
        /// </value>
        /// =================================================================================================
        public TimeSpan? CancelAfter { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns <see langword="true" /> when no fault is configured. Used by the builder to short-
        ///     circuit and produce a plain <c>AsyncEnumerable</c>.
        /// </summary>
        /// <value>
        ///     True if this object is empty, false if not.
        /// </value>
        /// =================================================================================================
        public bool IsEmpty
            => !PerItemDelay.HasValue
               && !ThrowAfterIndex.HasValue
               && !CancelAfter.HasValue;
    }
}