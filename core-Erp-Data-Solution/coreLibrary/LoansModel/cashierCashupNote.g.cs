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
    
    public partial class cashierCashupNote
    {
        public int cashierCashupNoteId { get; set; }
        public int cashierCashupId { get; set; }
        public int currencyNoteId { get; set; }
        public int quantity { get; set; }
        public double total { get; set; }
    
        public virtual cashierCashup cashierCashup { get; set; }
        public virtual currencyNote currencyNote { get; set; }
    }
}
