﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace coreLogic
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CreditUnionModels : DbContext, ICreditUnionModels
    {
        public CreditUnionModels()
            : base("name=CreditUnionModels")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual IDbSet<creditUnionChapter> creditUnionChapters { get; set; }
        public virtual IDbSet<creditUnionMember> creditUnionMembers { get; set; }
        public virtual IDbSet<creditUnionShareTransaction> creditUnionShareTransactions { get; set; }
    }
}