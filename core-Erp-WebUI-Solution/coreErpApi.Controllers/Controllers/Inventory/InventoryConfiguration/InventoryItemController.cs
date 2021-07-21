//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Web.Http;
//using coreERP;
//using coreLogic;
//using coreERP.Providers;
//using System.Linq.Dynamic;

//namespace coreErpApi.Controllers.Controllers.Inventory.InventoryConfiguration
//{
//    [AuthorizationFilter()]
//    public class InventoryItemController : ApiController
//    {
//        ICommerceEntities le;
//        Icore_dbEntities ent;


//        public InventoryItemController()
//        {
//            le = new CommerceEntities();
//            ent = new core_dbEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//            ent.Configuration.LazyLoadingEnabled = false;
//            ent.Configuration.ProxyCreationEnabled = false;
//        }

//        public InventoryItemController(ICommerceEntities lent, Icore_dbEntities cent)
//        {
//            le = lent;
//            ent = cent;
//        }

//        [HttpGet]
//        // GET: api/Product
//        public IEnumerable<inventoryItem> Get()
//        {
//            return le.inventoryItems
//                .OrderBy(p => p.inventoryItemName)
//                .ToList();
//        }

//        [HttpGet]
//        // GET: api/Product
//        public IEnumerable<inventoryItemDetail> GetDetails(long id)
//        {
//            return le.inventoryItemDetails
//                .Where(p => p.inventoryItemId == id)
//                .OrderBy(p => p.batchNumber)
//                .ToList();
//        }

//        [HttpGet]
//        // GET: api/inventoryItem
//        public IEnumerable<inventoryItem> GetItemByLocation(long id)
//        {
//            return le.inventoryItems
//                .Where(p => p.locationId == id)
//                .OrderBy(p => p.inventoryItemName)
//                .ToList();
//        }

//        [HttpGet]
//        // GET: api/inventoryItem
//        public IEnumerable<inventoryItem> GetItemByProduct(long id)
//        {
//            return le.inventoryItems
//                .Include(p => p.inventoryItemDetails)
//                .Where(p => p.productId == id)
//                .OrderBy(p => p.inventoryItemName)
//                .ToList();
//        }

//        [HttpGet]
//        // GET: api/inventoryItem
//        public IEnumerable<inventoryItem> GetItemByProductSubCat(long id)
//        {
//            List<int> productIds = le.products
//                .Where(p => p.productSubCategoryId == id)
//                .Select(p => p.productId)
//                .ToList();


//            return le.inventoryItems
//                .Include(p => p.inventoryItemDetails)
//                .Where(p => productIds.Contains(p.productId))
//                .OrderBy(p => p.inventoryItemName)
//                .ToList();
//        }


//        [HttpGet]
//        // GET: api/inventoryItem
//        public IEnumerable<inventoryItem> GetItemByCategory(long id)
//        {
//            List<int> productSubCategoryIds = le.productSubCategories
//                .Where(p => p.productCategoryId == id)
//                .Select(p => p.productSubCategoryId)
//                .ToList();

//            List<int> productIds = le.products
//                .Where(p => productSubCategoryIds.Contains(p.productSubCategoryId))
//                .Select(p => p.productId)
//                .ToList();


//            return le.inventoryItems
//                .Include(p => p.inventoryItemDetails)
//                .Where(p => productIds.Contains(p.productId))
//                .OrderBy(p => p.inventoryItemName)
//                .ToList();
//        }

                        


//        [HttpPost]
//        public KendoResponse Get([FromBody]KendoRequest req)
//        {
//            string order = "inventoryItemName";

//            KendoHelper.getSortOrder(req, ref order);
//            var parameters = new List<object>();
//            var whereClause = KendoHelper.getWhereClause<brand>(req, parameters);

//            var query = le.inventoryItems.AsQueryable();
//            if (whereClause != null && whereClause.Trim().Length > 0)
//            {
//                query = query.Where(whereClause, parameters.ToArray());
//            }

//            var data = query
//                .OrderBy(order.ToString())
//                .Skip(req.skip)
//                .Take(req.take)
//                .ToArray();

//            return new KendoResponse(data, query.Count());
//        }

//        [HttpPost]
//        public KendoResponse Get([FromBody]KendoRequest req, long id)
//        {
//            string order = "inventoryItemName";

//            KendoHelper.getSortOrder(req, ref order);
//            var parameters = new List<object>();
//            var whereClause = KendoHelper.getWhereClause<brand>(req, parameters);

//            var query = le.inventoryItems
//                .Where(p => p.productId == id)
//                .AsQueryable();
//            if (whereClause != null && whereClause.Trim().Length > 0)
//            {
//                query = query.Where(whereClause, parameters.ToArray());
//            }

//            var data = query
//                .OrderBy(order.ToString())
//                .Skip(req.skip)
//                .Take(req.take)
//                .ToArray();

//            return new KendoResponse(data, query.Count());
//        }

//        [HttpPost]
//        public KendoResponse Post(inventoryItem input)
//        {
//            var prod = le.products
//                .Include(p=> p.productSubCategory)
//                .Include(p=> p.productSubCategory.productCategory)
//                .First(p => p.productId == input.productId); 
//            var productPrefix = prod.productSubCategory.productCategory.productCategoryName.Substring(0, 1)
//                    .ToUpper()
//                                + prod.productSubCategory.productCategoryId.ToString();
//            input.itemNumber = (new IDGenerator()).NewInventoryItemNumber(input.inventoryItemId, le, ent, productPrefix);
//            input.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            input.created = DateTime.Now;
//            input.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            input.modified = DateTime.Now;
//            le.inventoryItems
//                .Add(input);
//            le.SaveChanges();

//            return new KendoResponse { Data = new inventoryItem[] { input } };
//        }

//        [HttpPut]
//        // PUT: api/brands/5
//        public KendoResponse Put([FromBody]inventoryItem value)
//        {
//            var toBeUpdated = le.inventoryItems.First(p => p.inventoryItemId == value.inventoryItemId);

//            toBeUpdated.productId = value.productId;
//            toBeUpdated.locationId = value.locationId;
//            toBeUpdated.brandId = value.brandId;
//            toBeUpdated.itemNumber = value.itemNumber;
//            toBeUpdated.inventoryItemName = value.inventoryItemName;
//            toBeUpdated.unitPrice = value.unitPrice;
//            toBeUpdated.safetyStockLevel = value.safetyStockLevel;
//            toBeUpdated.reorderPoint = value.reorderPoint;
//            toBeUpdated.accountId = value.accountId;
//            toBeUpdated.shrinkageAccountId = value.shrinkageAccountId;
//            toBeUpdated.inventoryMethodId = value.inventoryMethodId;
//            toBeUpdated.currencyId = value.currencyId;
//            toBeUpdated.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            toBeUpdated.modified = DateTime.Now;

//            le.SaveChanges();

//            return new KendoResponse { Data = new inventoryItem[] { toBeUpdated } };
//        }

//        [HttpDelete]
//        // DELETE: api/brands/5
//        public void Delete([FromBody]inventoryItem value)
//        {
//            var forDelete = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == value.inventoryItemId);
//            if (forDelete != null)
//            {
//                le.inventoryItems.Remove(forDelete);
//                le.SaveChanges();
//            }
//        }


//    }
//}




  




