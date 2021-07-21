using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;

namespace coreErpPointOfSale.Models
{
    public class SalesItems
    {
     
        public string description { get; set; }
        public double unitPrice { get; set; }
        public double quantity { get; set; }
        public double totalAmount { get; set; }

        public SalesItems(inventoryItem item)
        {
            if (item != null)
            {
                description = item.inventoryItemName;
                unitPrice = item.unitPrice;
                quantity = 1;
                totalAmount = quantity * unitPrice;
            }
        }

        public SalesItems(salesOrderline item)
        {
            if (item != null)
            {
                description = item.description;
                unitPrice = item.unitPrice;
                quantity = item.quantity;
                totalAmount = item.totalAmount;
            }
        }

        public SalesItems(arInvoiceLine item)
        {
            if (item != null)
            {
                description = item.description;
                unitPrice = item.unitPrice;
                quantity = item.quantity;
                totalAmount = item.totalAmount;
            }
        }

        public SalesItems(string descrip, double unitPric, double quan, double totalAm)
        {
            description = descrip;
            unitPrice = unitPric;
            quantity = quan;
            totalAmount = totalAm;
        }

        public SalesItems(SalesItems item)
        {
            description = item.description;
            unitPrice = item.unitPrice;
            quantity = item.quantity;
            totalAmount = item.totalAmount;
        }


    }

}
