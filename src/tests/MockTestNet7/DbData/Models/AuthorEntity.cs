// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockTestNet7
//  Author           : RzR
//  Created On       : 2023-11-09 15:45
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-09 17:10
// ***********************************************************************
//  <copyright file="AuthorEntity.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Collections.Generic;

#endregion

namespace MockTestNet7.DbData.Models
{
    public class AuthorEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<PostEntity> Posts { get; set; }
    }
}