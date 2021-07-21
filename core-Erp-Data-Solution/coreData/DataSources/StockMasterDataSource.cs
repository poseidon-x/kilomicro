using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;
using System.Data.Entity;
using coreLogic.Models;
using coreLogic.Models.Inventory;

namespace coreData.DataSources
{
    [DataObject]
    public class StockMasterDataSource
    {

        private readonly ICommerceEntities le;
        private readonly Icore_dbEntities ctx;

        //call a constructor to instialize a the  context 
        public StockMasterDataSource()
        {
            var db2 = new CommerceEntities();
            ctx = new core_dbEntities();

            ctx.Configuration.LazyLoadingEnabled = false;
            ctx.Configuration.ProxyCreationEnabled = false;

            le= db2;

            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }


        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<StockMasterViewModel> GetInventoryStatement()
        {
            var data = le.inventoryItemDetails
                        .Include(p => p.inventoryItem)
                        .Include(p => p.inventoryItem.product)
                        .Include(p => p.inventoryItem.product.productSubCategory)
                        .Include(p => p.inventoryItem.product.productSubCategory.productCategory)
                        .Select(p=> new StockMasterViewModel
                        {
                            inventoryItemId = p.inventoryItem.inventoryItemId,
                            inventoryItemName = p.inventoryItem.inventoryItemName,
                            unitPrice = p.inventoryItem.unitPrice
                        }).OrderBy(p => p.inventoryItemName)
                        .ToList();

            foreach (StockMasterViewModel record in data)
            {
                record.detailItems = GetInventoryItemDetails(record.inventoryItemId);
                record.openingQuantityOnHand = le.openningBalances.FirstOrDefault(p => p.inventoryItemId == record.inventoryItemId).quantityOnHand;
                record.unitCost = le.inventoryItemDetails.FirstOrDefault(p => p.inventoryItemId == record.inventoryItemId).unitCost;
                var openBal = le.openningBalances.Where(p => p.inventoryItemId == record.inventoryItemId);
                record.openingQuantityOnHand = 0;
                foreach (var rec in openBal)
                {
                    record.openingQuantityOnHand += rec.quantityOnHand;
                }
                var invtTrans = le.inventoryTransferDetails.Where(p => p.inventoryItemId == record.inventoryItemId);
                record.totalQuantityTransferred = 0;
                if (invtTrans.Any())
                {
                    foreach (var rec in invtTrans)
                    {
                        record.totalQuantityTransferred += rec.quantityTransferred;
                    }
                }
                var invtShrink = le.shrinkages.Where(p => p.inventoryItemId == record.inventoryItemId);
                record.totalQuantityShrunk = 0;
                if (invtShrink.Any())
                {
                    foreach (var rec in invtShrink)
                    {
                        record.totalQuantityShrunk += rec.quantityShrunk;
                    }
                }
                record.currentQuantityOnHand = le.inventoryItemDetails.FirstOrDefault(p => p.inventoryItemId == record.inventoryItemId).quantityOnHand;                
                record.currentInventoryValue = record.currentQuantityOnHand * record.unitCost;
            }
            return data;
        }

        //[DataObjectMethod(DataObjectMethodType.Select)]
        //public List<InventoryItemsViewModel> GetInventoryItems()
        //{

        //    var data = le.inventoryItems
        //                .Select(p => new InventoryItemsViewModel
        //                {
        //                    inventoryItemId = p.inventoryItemId,
        //                    itemNumber = p.itemNumber,
        //                    inventoryItemName = p.inventoryItemName,
        //                    productId = p.productId,
        //                    brandId = p.brandId
        //                }).OrderBy(p => p.inventoryItemName)
        //                .ToList();

        //    foreach (InventoryItemsViewModel record in data)
        //    {
        //        record.productName = le.products.FirstOrDefault(p => p.productId == record.productId).productName;
        //        record.productSubCategoryId = le.products.FirstOrDefault(p => p.productId == record.productId).productSubCategoryId;
        //        record.productSubCategoryName = le.productSubCategories.FirstOrDefault(p => p.productSubCategoryId == record.productSubCategoryId).productSubCategoryName;
        //        record.productCategoryId = le.productSubCategories.FirstOrDefault(p => p.productSubCategoryId == record.productSubCategoryId).productCategoryId;
        //        record.productCategoryName = le.productCategories.FirstOrDefault(p => p.productCategoryId == record.productCategoryId).productCategoryName;
        //        record.brandName = le.brands.FirstOrDefault(p => p.brandId == record.brandId).brandName;
        //    }
        //    return data;
        //}

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<InventoryItemDetailsViewModel> GetInventoryItemDetails(long itemId)
        {

            var data = le.inventoryItemDetails
                .Where(p => p.inventoryItemId == itemId)
                        .Select(p => new InventoryItemDetailsViewModel
                        {
                            inventoryItemDetailId = p.inventoryItemDetailId,
                            inventoryItemId = p.inventoryItemId,
                            batchNumber = p.batchNumber,
                            mfgDate = p.mfgDate,
                            expiryDate = p.expiryDate,
                            unitCost = p.unitCost,
                            quantityOnHand = p.quantityOnHand,
                            reservedQuantity = p.reservedQuantity,
                            startSerialNumber = p.startSerialNumber,
                            endSerialNumber = p.endSerialNumber
                        }).OrderBy(p => p.inventoryItemId)
                        .ToList();

            return data;
        }


    }
}
