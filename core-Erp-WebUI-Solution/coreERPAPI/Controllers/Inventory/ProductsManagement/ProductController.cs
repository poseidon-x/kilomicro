using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;

namespace coreErpApi.Controllers.Controllers.Inventory.ProductsManagement
{
    [AuthorizationFilter()]
    public class ProductController : ApiController
    {
        //Declaration of constant variables for error messages
        private const string emptyFieldMessage = "Please make sure all fields are provided and valid <br />";
        private const string uniqueNameMessage = "Product Category Name Already Exist <br />";
        private const string nameLengthMessage = "Maximum length of Product Category Name is 250 <br />";

        private string errorMessage = "";


        //Declaration of variables
        private ICommerceEntities le;

        //Construct without parameter
        public ProductController()
        {
            le = new CommerceEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }//ProductController

        //Constructor with parameter
        public ProductController(ICommerceEntities lent)
        {
            le = lent;
        }//ProductController

        // GET: api/Product 
        [HttpGet]
        public IEnumerable<product> Get()
        {
            var productData = le.products
                    .OrderBy(p => p.productName)
                    .ToList();

            return productData;
        }// Get

        // GET: api/product/- fetch a record with the specified Id
        [HttpGet]
        public product Get(int id)
        {
            product value = le.products
                .FirstOrDefault(p => p.productId == id);

            //Creates a new product if id is null
            if (value == null)
            {
                value = new product();
            }
            return value;
        }//Get


        [HttpPost]
        public KendoResponse Get([FromBody]KendoRequest req)
        {
            string order = "productName";

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<product>(req, parameters);

            var query = le.products.AsQueryable();
            if (whereClause != null && whereClause.Trim().Length > 0)
            {
                query = query.Where(whereClause, parameters.ToArray());
            }

            var data = query
                .OrderBy(order.ToString())
                .Skip(req.skip)
                .Take(req.take)
                .ToArray();

            return new KendoResponse(data, query.Count());
        }


        // POST&PUT: api/product/ 
        [HttpPost]
        public product Post(product value)
        {
            product toBeSaved = null;

            if (value.productId > 0)
                {
                    toBeSaved = le.products
                    .First(p => p.productId == value.productId);
                    populateFields(toBeSaved, value);
                                }
            else
            {
                toBeSaved = new product();
                populateFields(toBeSaved, value);
                le.products.Add(toBeSaved);
            }

            le.SaveChanges();
            return toBeSaved;
        }//Post

        //ASSIGNS THE VALUES TO BE SAVED
        private void populateFields(product toBeSaved, product value)
        {
            toBeSaved.productSubCategoryId = value.productSubCategoryId;
            toBeSaved.productCode = value.productCode;
            toBeSaved.productName = value.productName;
            toBeSaved.productDescription = value.productDescription;
            toBeSaved.inventoryMethodId = value.inventoryMethodId;
            toBeSaved.unitOfMeasurementId = value.unitOfMeasurementId;
            toBeSaved.productMakeId = value.productMakeId;
            toBeSaved.productStatusId = value.productStatusId;
            toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities()); 
            toBeSaved.created = DateTime.Now;
            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            toBeSaved.modified = DateTime.Now;


        }//populateFields



        [HttpDelete]
        // DELETE: api/product/
        public void Delete([FromBody]product value)
        {
            var forDelete = le.products
                .FirstOrDefault(p => p.productId == value.productId);

            if (forDelete != null)
            {
                le.products.Remove(forDelete);
                le.SaveChanges();
            }
        }//Delete



        //Validate to ensure Input fields are not empty.
        private void ValidateProductFieldsEmpty(product prod)
        {
            if ((String.IsNullOrEmpty(prod.productSubCategoryId.ToString()))
                || (String.IsNullOrEmpty(prod.productCode.Trim())) ||
                (String.IsNullOrEmpty(prod.productName.Trim())) || (String.IsNullOrEmpty(prod.productDescription.Trim())) ||
                (String.IsNullOrEmpty(prod.inventoryMethodId.ToString())) || (String.IsNullOrEmpty(prod.unitOfMeasurementId.ToString())) ||
                (String.IsNullOrEmpty(prod.productMakeId.ToString())) || (String.IsNullOrEmpty(prod.productStatusId.ToString()))
                )
            {
                errorMessage += emptyFieldMessage;
            }
        }


        //Validate To Ensure Product Name is Unique.
        private void ValidateProductNameIsUnique(product prod)
        {
            if (le.products
                .Any(p => p.productName == prod.productName.Trim())
               )
            {
                errorMessage += uniqueNameMessage;
            }

        }

        //Validate To Ensure Product Name is Unique.
        private void ValidateProductCodeIsUnique(product prod)
        {
            if (le.products
                .Any(p => p.productCode == prod.productCode.Trim())
               )
            {
                errorMessage += uniqueNameMessage;
            }

        }

        //Validate To Ensure Product Name does not exceed maximum length.
        private void ValidateProductNameLength(product prod)
        {
            if (prod.productName.Length > 250)
            {
                errorMessage += nameLengthMessage;
            }
        }

        //Validate To Ensure Product Name does not exceed maximum length.
        private void ValidateProductCodeLength(product prod)
        {
            if (prod.productCode.Length > 25)
            {
                errorMessage += nameLengthMessage;
            }
        }

        //Validate To Ensure Product Name does not exceed maximum length.
        private void ValidateProductDescriptionLength(product prod)
        {
            if (prod.productDescription.Length > 250)
            {
                errorMessage += nameLengthMessage;
            }
        }


    }//ProductController 
}
