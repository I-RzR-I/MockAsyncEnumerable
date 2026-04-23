// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockAsyncEnumerable
//  Author           : RzR
//  Created On       : 2026-02-26 21:02
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-02-27 09:57
// ***********************************************************************
//  <copyright file="GuardEnsure.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using RzR.Extensions.EntityMock.Extensions.Internal;
using System;
#if NET6_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

#endregion

namespace RzR.Extensions.EntityMock.Helpers.Internal
{
    /// <summary>
    ///     A guard ensure.
    /// </summary>
    internal static class GuardEnsure
    {
        /// <summary>
        ///     Not null.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="source">Source for the null guard.</param>
        /// <param name="paramName">(Optional) Name of the parameter.</param>
        /// <param name="message">(Optional) The message.</param>
        public static void NotNullable(
            object source,
#if NET6_0_OR_GREATER
            [CallerArgumentExpression(nameof(source))] string paramName = null,
#else
            string paramName = null,
#endif
        string message = null
        )
        {
            if (source != null)
                return;

            var effectiveParamName = paramName ?? nameof(source);
            var errorMessage = effectiveParamName.GetGuardNotNullMessage(message);

            throw new ArgumentNullException(effectiveParamName, errorMessage);
        }

        /// <summary>
        ///     Not null.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="source">Source for the null guard.</param>
        /// <param name="paramName">(Optional) Name of the parameter.</param>
        /// <param name="message">(Optional) The message.</param>
        /// <returns>
        ///     A T.
        /// </returns>
        public static T NotNull<T>(
            T source,
#if NET6_0_OR_GREATER
            [CallerArgumentExpression(nameof(source))] string paramName = null,
#else
            string paramName = null,
#endif
        string message = null
        ) where T : class
        {
            if (source != null)
                return source;

            var effectiveParamName = paramName ?? nameof(source);
            var errorMessage = effectiveParamName.GetGuardNotNullMessage(message);

            throw new ArgumentNullException(effectiveParamName, errorMessage);
        }
    }
}