// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockAsyncEnumerable
//  Author           : RzR
//  Created On       : 2026-04-24 08:04
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-04-24 08:26
// ***********************************************************************
//  <copyright file="IMockAsyncEnumerable.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.Linq;

#endregion

namespace RzR.Extensions.EntityMock.Abstractions
{
    /// <summary>
    ///     Public, stable surface returned by the factory, builder and extension methods.
    ///     Combines <see cref="IAsyncEnumerable{T}" /> and <see cref="IQueryable{T}" /> so that
    ///     user can use both EF Core async LINQ (e.g. <c>ToListAsync()</c>) and the standard
    ///     <see cref="IQueryable{T}" /> LINQ surface against a single returned value.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    public interface IMockAsyncEnumerable<out T> : IAsyncEnumerable<T>, IQueryable<T>
    {
    }
}