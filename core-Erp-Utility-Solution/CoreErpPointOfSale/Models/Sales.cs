using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;

namespace coreErpPointOfSale.Models
{
    public class Sales
    {
     
        public string description { get; set; }
        public string unitPrice { get; set; }
        public string quantity { get; set; }
        public double totalAmount { get; set; }

        public Sales(arInvoiceLine item)
        {
            if (item != null)
            {
                description = item.description;
                unitPrice = item.unitPrice.ToString();
                quantity = item.quantity.ToString();
                totalAmount = item.totalAmount;
            }
        }

        public Sales(string descrip, string unitPric, string quan, double totalAm)
        {
            description = descrip;
            unitPrice = unitPric;
            quantity = quan;
            totalAmount = totalAm;
        }



    }

}
