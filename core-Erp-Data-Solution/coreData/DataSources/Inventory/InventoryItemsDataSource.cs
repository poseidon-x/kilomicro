using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using coreLogic;
using coreLogic.Models.Inventory;

namespace coreData.DataSources.Inventory
{
    [DataObject]
    public class InventoryItemsDataSource
    {
            private readonly ICommerceEntities le;
            private readonly Icore_dbEntities ctx;

        //call a constructor to instialize a the  context 
            public InventoryItemsDataSource()
        {
            var db2 = new CommerceEntities();
            var db3 = new core_dbEntities();

            le = db2;
            ctx = db3;

            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }


        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<InventoryProductsViewModel> GetInventoryProducts()
        {

            var data = le.products
                        .Select(p => new InventoryProductsViewModel
                        {
                            productId = p.productId,
                            productName = p.productName,
                            productCode = p.productCode,
                            subCategoryId = p.productSubCategoryId
                        }).OrderBy(p => p.productName)
                        .ToList();

            foreach (InventoryProductsViewModel record in data)
            {
                record.subCategoryName = le.productSubCategories.FirstOrDefault(p => p.productSubCategoryId == record.subCategoryId).productSubCategoryName;
                record.categoryId = le.productSubCategories.FirstOrDefault(p => p.productSubCategoryId == record.subCategoryId).productCategoryId;
                record.categoryName = le.productCategories.FirstOrDefault(p => p.productCategoryId == record.categoryId).productCategoryName;
                record.items = GetInventoryItems(record.productId);
            }
            return data;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<InventoryItemsViewModel> GetInventoryItems(long prodId)
        {
            var data = le.inventoryItems
                                .Where(p => p.productId == prodId)
                        .Select(p=> new InventoryItemsViewModel
                        {
                            inventoryItemId = p.inventoryItemId,
                            itemNumber = p.itemNumber,
                            inventoryItemName = p.inventoryItemName,
                            unitPrice = p.unitPrice,
                            brandId = p.brandId,
                            safetyStockLevel = p.safetyStockLevel,
                            reorderPoint = p.reorderPoint
                        }).OrderBy(p => p.inventoryItemName)
                        .ToList();

            foreach (InventoryItemsViewModel record in data)
            {
                record.brandName = le.brands.FirstOrDefault(p => p.brandId == record.brandId).brandName;
                record.detailItems = GetInventoryItemDetails(record.inventoryItemId);
            }
            return data;
        }


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

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<InventoryLocationsViewModel> GetInventoryLocations()
        {

            var data = le.locations
                        .Select(p => new InventoryLocationsViewModel
                        {
                            locationId = p.locationId,
                            locationName = p.locationName,
                            locationTypeId = p.locationTypeId,
                            locationCode = p.locationCode,
                            physicalAddress = p.physicalAddress,
                            cityId = p.cityId
                        }).OrderBy(p => p.locationName)
                        .ToList();

            foreach (InventoryLocationsViewModel record in data)
            {
                record.locationTypeName =
                    le.locationTypes.FirstOrDefault(p => p.locationTypeId == record.locationTypeId).locationTypeName;
                record.cityName = ctx.cities.FirstOrDefault(p => p.city_id == record.cityId).city_name;
                record.items = GetInventoryItemsByLocation(record.locationId);
            }
            return data;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<InventoryItemsViewModel> GetInventoryItemsByLocation(long locId)
        {

            var data = le.inventoryItems
                                .Where(p => p.productId == locId)
                        .Select(p => new InventoryItemsViewModel
                        {
                            inventoryItemId = p.inventoryItemId,
                            itemNumber = p.itemNumber,
                            inventoryItemName = p.inventoryItemName,
                            unitPrice = p.unitPrice,
                            brandId = p.brandId,
                            safetyStockLevel = p.safetyStockLevel,
                            reorderPoint = p.reorderPoint
                        }).OrderBy(p => p.inventoryItemName)
                        .ToList();

            foreach (InventoryItemsViewModel record in data)
            {
                record.brandName = le.brands.FirstOrDefault(p => p.brandId == record.brandId).brandName;
                record.detailItems = GetInventoryItemDetails(record.inventoryItemId);
            }
            return data;
        }

    }
}

