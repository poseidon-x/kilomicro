//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public interface IcreditUnionMember
    {
        long creditUnionMemberID { get; set; }
        int clientID { get; set; }
        System.DateTime joinedDate { get; set; }
        double sharesBalance { get; set; }
        int creditUnionChapterID { get; set; }
    
        IcreditUnionChapter creditUnionChapter { get; set; }
        ICollection<IcreditUnionShareTransaction> creditUnionShareTransactions { get; set; }
     
    } 
    
}
