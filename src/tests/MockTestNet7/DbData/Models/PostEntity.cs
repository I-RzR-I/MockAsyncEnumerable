// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.MockTestNet7
//  Author           : RzR
//  Created On       : 2023-11-09 15:45
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-11-09 17:10
// ***********************************************************************
//  <copyright file="PostEntity.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace MockTestNet7.DbData.Models
{
    public class PostEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Contents { get; set; }

        public DateTime? CreatedOn { get; set; }

        [ForeignKey(nameof(Author))] public int AuthorId { get; set; }

        public AuthorEntity Author { get; set; }
    }
}