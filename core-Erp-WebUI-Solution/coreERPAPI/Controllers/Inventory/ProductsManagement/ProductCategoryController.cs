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
    public class ProductCategoryController : ApiController
    {
        //Declaration of constant variables for error messages
        private const string lookupMessage = "Please make sure you select from the provided Drop Down list<br />";
        private const string emptyFieldMessage = "Please make sure all fields are provided and valid<br />";
        private const string uniqueNameMessage = "Product Category Name Already Exist<br />";
        private const string nameLengthMessage = "Maximum length of Product Category Name is 250<br />";

        //Declaration of variables
        private ICommerceEntities le;
        private Icore_dbEntities ctx;
        private string errorMessage = "";

        //Constructor without a parameter
        public ProductCategoryController()
        {
            le = new CommerceEntities();
            ctx = new core_dbEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        //Constructor with a parameter
        public ProductCategoryController(ICommerceEntities lent, Icore_dbEntities ent)
        {
            le = lent; 
        }

        // GET: api/productCategory
        public IEnumerable<productCategory> Get()
        {
            return le.productCategories
                .Include(p => p.productSubCategories)
                .OrderBy(p => p.productCategoryName)
                .ToList();
        }



        // GET: api/productCategory/- fetch a record with the specified Id
        [HttpGet]
        public productCategory Get(int id)
        {
            productCategory value = le.productCategories
                .Include(p => p.productSubCategories)
                .FirstOrDefault(p => p.productCategoryId == id);
                
            //Creates a new Product Category if id is null
            if (value == null)
            {
                value = new productCategory
                {
                    productSubCategories = new List<productSubCategory>()
                };
            }
            return value;
        }


        // POST&PUT : api/productCategory/ 
        [HttpPost]
        public productCategory Post(productCategory value)
        {
            productCategory toBeSaved = null;

            if (ValidateProductCategory(value) == true)
            {
                if (value.productCategoryId > 0)
                {
                toBeSaved = le.productCategories
                    .Include(p => p.productSubCategories)
                    .First(p => p.productCategoryId == value.productCategoryId);
                populateFields(toBeSaved, value);
                    toBeSaved.modifiedDate = DateTime.Now;
                    toBeSaved.modifiedBy = "";
            }
            else
            {
                toBeSaved = new productCategory();
                populateFields(toBeSaved, value);
                    toBeSaved.creationDate = DateTime.Now;
                    toBeSaved.createdBy = "";
                le.productCategories.Add(toBeSaved);
            }

            foreach (var sub in value.productSubCategories)
            {
                    productSubCategory subToBeSaved = null;

                    if (sub.productSubCategoryId > 0)
                {
                    subToBeSaved = toBeSaved.productSubCategories
                    .First(p => p.productSubCategoryId == sub.productSubCategoryId);
                    populateSubFields(subToBeSaved, sub);
                        subToBeSaved.modifiedDate = DateTime.Now;
                        subToBeSaved.modifiedBy = "";
                }
                else
                {
                    subToBeSaved = new productSubCategory();
                    populateSubFields(subToBeSaved, sub);
                        subToBeSaved.creationDate = DateTime.Now;
                        subToBeSaved.createdBy = "";
                    toBeSaved.productSubCategories.Add(subToBeSaved);
                }
            }
            le.SaveChanges();
            return toBeSaved;
        }
            
                return toBeSaved;
           
        }

        //Validate to ensure User is Selecting from the drop-down and not typing thier own values.
        private bool ValidateProductCategory(productCategory cat)
        {
            ValidateProductCategoryDropDown(cat);
            ValidateProductCategoryFieldsEmpty(cat);
            ValidateProductCategorNameIsUnique(cat);
            ValidateProductCategorNameLength(cat);
            if (errorMessage == "")
            {
                return true;
            }
    
            throw new ApplicationException(errorMessage);
        }

        //Retrieve values to Populate Product Category fields
        private void populateFields(productCategory toBeSaved, productCategory value)
        {
            toBeSaved.productCategoryName = value.productCategoryName.Trim();
            toBeSaved.cogsAccountId = value.cogsAccountId;
            toBeSaved.inventoryAccountId = value.inventoryAccountId;
            toBeSaved.apAccountId = value.apAccountId;
            toBeSaved.arAccountId = value.arAccountId;
            toBeSaved.incomeAccountId = value.incomeAccountId;
            toBeSaved.expenseAccountId = value.expenseAccountId;
            toBeSaved.createdBy = LoginHelper.getCurrentUser(new coreSecurityEntities());
            toBeSaved.creationDate = DateTime.Now;
            toBeSaved.modifiedBy = LoginHelper.getCurrentUser(new coreSecurityEntities());
            toBeSaved.modifiedDate = value.modifiedDate;
        }

        //Retrieve values to Populate Product Sub-Category fields
        private void populateSubFields(productSubCategory toBeSaved, productSubCategory value)
        {
            toBeSaved.productSubCategoryId = value.productSubCategoryId;
            toBeSaved.productCategoryId = value.productCategoryId;
            toBeSaved.productSubCategoryName = value.productSubCategoryName;
            toBeSaved.createdBy = LoginHelper.getCurrentUser(new coreSecurityEntities()); ;
            toBeSaved.creationDate = DateTime.Now;
            toBeSaved.modifiedBy = LoginHelper.getCurrentUser(new coreSecurityEntities());
            toBeSaved.modifiedDate = DateTime.Now; 
        }

        [HttpDelete]
        // DELETE: api/productCategory/
        public void Delete([FromBody]productCategory value)
        {
            var forDelete = le.productCategories.FirstOrDefault(p => p.productCategoryId == value.productCategoryId);
            if (forDelete != null)
            {
                le.productCategories.Remove(forDelete);
                le.SaveChanges();
            }
        }

        //Validate to ensure User is Selecting from the drop-down and not typing thier own values.
        private void ValidateProductCategoryDropDown(productCategory cat)
        {
            if ((accountExists(cat.cogsAccountId) == false) || (accountExists(cat.apAccountId) == false)
                || (accountExists(cat.arAccountId) == false) || (accountExists(cat.expenseAccountId) == false)
                || (accountExists(cat.incomeAccountId) == false) || (accountExists(cat.inventoryAccountId) == false))
            {
                errorMessage += lookupMessage;
            }
        }

        //Check user input to see, if what he/she slected has an Id   
        private bool accountExists(int? accountId)
        {
            if (ctx.accts.Any(p => p.acct_id == accountId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //Validate to ensure Input fields are not empty.
        private void ValidateProductCategoryFieldsEmpty(productCategory cat)
        {
            if ((String.IsNullOrEmpty(cat.productCategoryName.Trim()))
                || ((cat.cogsAccountId == null) || (cat.cogsAccountId == 0)) ||
                (String.IsNullOrEmpty(cat.inventoryAccountId.ToString().Trim())) || (String.IsNullOrEmpty(cat.arAccountId.ToString().Trim())) ||
                (String.IsNullOrEmpty(cat.apAccountId.ToString().Trim())) || (String.IsNullOrEmpty(cat.incomeAccountId.ToString().Trim())) ||
                (String.IsNullOrEmpty(cat.expenseAccountId.ToString().Trim())) || (String.IsNullOrEmpty(cat.incomeAccountId.ToString().Trim()))
                )
            {
                errorMessage += emptyFieldMessage;
            }
        }


        //Validate To Ensure Product Name is Unique.
        private void ValidateProductCategorNameIsUnique(productCategory cat)
        {
            if (le.productCategories
                .Any(p => p.productCategoryName == cat.productCategoryName.Trim()
                && p.productCategoryId != cat.productCategoryId)
               )
            {
                errorMessage += uniqueNameMessage;
            }

        }

        //Validate To Ensure Product Name does not exceed maximum length.
        private void ValidateProductCategorNameLength(productCategory cat)
        {
            if (cat.productCategoryName.Length > 250)
            {
                errorMessage += nameLengthMessage;
            }
        }


    }
}
