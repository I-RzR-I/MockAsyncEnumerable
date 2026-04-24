// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockAsyncEnumerable
//  Author           : RzR
//  Created On       : 2026-04-24 09:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-24 09:47
// ***********************************************************************
//  <copyright file="FaultyAsyncEnumerable.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using RzR.Extensions.EntityMock.Abstractions;
using RzR.Extensions.EntityMock.Helpers;
using RzR.Extensions.EntityMock.Helpers.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

#endregion

namespace RzR.Extensions.EntityMock.Faults
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Mock async enumerable that yields the wrapped sequence through a
    ///     <see cref="FaultyAsyncEnumerator{T}" />, allowing tests to simulate latency,
    ///     exceptions and cancellation without changing the production code under test.
    /// </summary>
    /// <remarks>
    ///     Synchronous <see cref="System.Linq.IQueryable{T}" /> usage is unaffected — the fault
    ///     settings are only applied to <c>GetAsyncEnumerator</c>.
    /// </remarks>
    /// <typeparam name="T">The element type of the sequence.</typeparam>
    /// <seealso cref="T:RzR.Extensions.EntityMock.Helpers.AsyncEnumerable{T}"/>
    /// <seealso cref="T:RzR.Extensions.EntityMock.Abstractions.IMockAsyncEnumerable{T}"/>
    /// =================================================================================================
    internal sealed class FaultyAsyncEnumerable<T> : AsyncEnumerable<T>, IMockAsyncEnumerable<T>
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) options for controlling the operation.
        /// </summary>
        /// =================================================================================================
        private readonly FaultInjectionOptions _options;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) source data.
        /// </summary>
        /// =================================================================================================
        private readonly IEnumerable<T> _source;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="FaultyAsyncEnumerable{T}"/> class.
        /// </summary>
        /// <param name="source">Source data.</param>
        /// <param name="options">Options for controlling the operation.</param>
        /// =================================================================================================
        public FaultyAsyncEnumerable(IEnumerable<T> source, FaultInjectionOptions options)
            : base(source ?? Enumerable.Empty<T>())
        {
            _source = GuardEnsure.NotNull(source);
            _options = GuardEnsure.NotNull(options);
        }

        /// <inheritdoc/>
        public override IAsyncEnumerator<T> GetAsyncEnumerator(
            CancellationToken cancellationToken = default)
            => new FaultyAsyncEnumerator<T>(
                _source.GetEnumerator(), _options, cancellationToken);
    }
}