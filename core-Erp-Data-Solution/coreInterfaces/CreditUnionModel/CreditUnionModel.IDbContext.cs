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
    
    public interface ICreditUnionModels : IDisposable
    {
    	int SaveChanges();
    	System.Data.Entity.Infrastructure.DbContextConfiguration Configuration {get;}
    	System.Data.Entity.Database Database {get;}
        IDbSet<creditUnionChapter> creditUnionChapters { get; set; }
        IDbSet<creditUnionMember> creditUnionMembers { get; set; }
        IDbSet<creditUnionShareTransaction> creditUnionShareTransactions { get; set; }
    }
}
