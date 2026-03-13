// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockAsyncEnumerable
//  Author           : RzR
//  Created On       : 2026-02-27 11:02
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-02-27 11:14
// ***********************************************************************
//  <copyright file="StringExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace MockAsyncEnumerable.Extensions.Internal
{
    /// <summary>
    ///     A string extensions.
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        ///     A string extension method that gets guard not null message.
        /// </summary>
        /// <param name="parameterName">The parameterName to act on.</param>
        /// <param name="customMessage">Message describing the custom.</param>
        /// <returns>
        ///     The guard not null message.
        /// </returns>
        public static string GetGuardNotNullMessage(this string parameterName, string customMessage)
        {
            var errorMessage = string.IsNullOrWhiteSpace(customMessage)
                ? $"Parameter '{parameterName}' cannot be null"
                : customMessage;

            return errorMessage;
        }
    }
}

