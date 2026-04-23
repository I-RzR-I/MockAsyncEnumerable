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

using RzR.Extensions.EntityMock.Helpers;
using System.Collections.Generic;

#endregion

namespace RzR.Extensions.EntityMock
{
    /// <summary>
    ///     Enumerable invoker
    /// </summary>
    public static class EnumerableInvoker
    {
        /// <summary>
        ///     Info async enumerable
        /// </summary>
        /// <param name="enumerable">Records</param>
        /// <returns></returns>
        /// <typeparam name="T">Entity type</typeparam>
        /// <remarks></remarks>
        public static AsyncEnumerable<T> Invoke<T>(IEnumerable<T> enumerable)
            => new AsyncEnumerable<T>(enumerable);
    }
}