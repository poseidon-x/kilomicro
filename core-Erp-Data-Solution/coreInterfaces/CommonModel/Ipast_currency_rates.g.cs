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
    
    
    public interface Ipast_currency_rates
    {
        int currency_rate_id { get; set; }
        double buy_rate { get; set; }
        double sell_rate { get; set; }
        System.DateTime tran_datetime { get; set; }
        System.DateTime creation_date { get; set; }
        string creator { get; set; }
        Nullable<System.DateTime> modification_date { get; set; }
        string last_modifier { get; set; }
    
        Icurrencies currencies { get; set; }
    }
    
}
