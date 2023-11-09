// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockTestNet6
//  Author           : RzR
//  Created On       : 2023-11-09 15:44
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-09 17:10
// ***********************************************************************
//  <copyright file="FilterExtensions.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;
using System.Linq;
using MockTestNet6.DbData.Models;

#endregion

namespace MockTestNet6.Extensions
{
    public static class FilterExtensions
    {
        public static IQueryable<TSource> GetRecordsInTop<TSource>(this IQueryable<TSource> source,
            ICollection<int> ids = null) where TSource : AuthorEntity
        {
            if (ids != null && ids.Any()) return source.Where(x => ids.Contains(x.Id));

            return Enumerable.Empty<TSource>().AsQueryable();
        }
    }
}