using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using coreLogic;
using System.Threading.Tasks;
using coreErpApi.Controllers.Controllers.Inventory.ProductsManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;




namespace coreErpApi.Tests.ProductManagement
{
    [TestClass]
    public class ProductCategoryControllerTests
    {
        private productCategory productCategoryTobeSaved;
        private productCategory productCategoryTobeSavedTwo;
        private productCategory duplicateProductCategoryToBeSaved;
        private productCategory noProductCategoryNameToBeSaved;
        private productCategory noCogsAccountIdToBeSaved;
        private productCategory noInventoryAccountIdToBeSaved;
        private productCategory noApAccountIdToBeSaved;
        private productCategory noArAccountIdToBeSaved;
        private productCategory noIncomeAccountIdToBeSaved;
        private productCategory noExpenseAccountIdToBeSaved;                      

        private accts accountLookUpOne;
        private accts accountLookUpTwo;
        private accts accountLookUpThree;
        private ICommerceEntities _context;
        private Icore_dbEntities ctx;
        
        [TestInitialize]
        public void TestInit()
        {
            _context = new MockCommerceEntities();

            ctx = new Mockcore_dbEntities();
           
            //A full Product Category To Save
            productCategoryTobeSaved = new productCategory
            {
                //productCategoryId = 1,
                productCategoryName = "Key Soap",
                cogsAccountId = 1,
                inventoryAccountId = 2,
                apAccountId = 1,
                arAccountId = 2,
                incomeAccountId = 3,
                expenseAccountId = 1
            };            

            //A full Product Category To Save
            productCategoryTobeSavedTwo = new productCategory
            {
                //productCategoryId = 7,
                productCategoryName = "Pepsodent",
                cogsAccountId = 1,
                inventoryAccountId = 2,
                apAccountId = 1,
                arAccountId = 2,
                incomeAccountId = 3,
                expenseAccountId = 1
            };

            //Duplicate Product category or duplicate product category name
            duplicateProductCategoryToBeSaved = new productCategory
            {
                //productCategoryId = 2,
                productCategoryName = "Key Soap",
                cogsAccountId = 1,
                inventoryAccountId = 2,
                apAccountId = 1,
                arAccountId = 2,
                incomeAccountId = 3,
                expenseAccountId = 1,
            };

            //Product Category Without A Name
            noProductCategoryNameToBeSaved = new productCategory
            {
                //productCategoryId = 3,
                cogsAccountId = 1,
                inventoryAccountId = 2,
                apAccountId = 3,
                arAccountId = 2,
                incomeAccountId = 3,
                expenseAccountId = 1,
            };

            //Product Category Without CogsAccountId
            noCogsAccountIdToBeSaved = new productCategory
            {
                //productCategoryId = 4,
                productCategoryName = "Dark Bar Soap",
                inventoryAccountId = 2,
                apAccountId = 1,
                arAccountId = 2,
                incomeAccountId = 3,
                expenseAccountId = 1,
            };

            //Product Category Without InventoryAccountId
            noInventoryAccountIdToBeSaved = new productCategory
            {
                //productCategoryId = 5,
                productCategoryName = "Geisha Soap",
                cogsAccountId = 1,
                apAccountId = 1,
                arAccountId = 2,
                incomeAccountId = 3,
                expenseAccountId = 1,
            };

            noApAccountIdToBeSaved = new productCategory
            {
                //productCategoryId = 1,
                productCategoryName = "Key Soap",
                cogsAccountId = 1,
                inventoryAccountId = 2,
                arAccountId = 2,
                incomeAccountId = 3,
                expenseAccountId = 1
            };

            noArAccountIdToBeSaved = new productCategory
            {
                //productCategoryId = 1,
                productCategoryName = "Key Soap",
                cogsAccountId = 1,
                inventoryAccountId = 2,
                apAccountId = 1,
                incomeAccountId = 3,
                expenseAccountId = 1
            };

            noIncomeAccountIdToBeSaved = new productCategory
            {
                //productCategoryId = 1,
                productCategoryName = "Key Soap",
                cogsAccountId = 1,
                inventoryAccountId = 2,
                apAccountId = 1,
                arAccountId = 2,
                expenseAccountId = 1
            };

            noExpenseAccountIdToBeSaved = new productCategory
            {
                //productCategoryId = 1,
                productCategoryName = "Key Soap",
                cogsAccountId = 1,
                inventoryAccountId = 2,
                apAccountId = 1,
                arAccountId = 2,
                incomeAccountId = 3
            };


            accountLookUpOne = new accts
            {
                acct_id = 1,
                acc_name = "Accumlated Depreciation"
            };

            accountLookUpTwo = new accts
            {
                acct_id = 2,
                acc_name = "Charges Income"
            };

            accountLookUpThree = new accts
            {
                acct_id = 3,
                acc_name = "Fee Income"
            };


            ctx.accts.Add(accountLookUpOne);
            ctx.accts.Add(accountLookUpTwo);
            ctx.accts.Add(accountLookUpTwo);
        }

        [TestMethod]
        public void Test_Product_Category_Returns_Non_Empty_List_After_Post()
        {
            ProductCategoryController controller = new ProductCategoryController(_context, ctx);
            controller.Post(productCategoryTobeSaved);
            controller.Post(productCategoryTobeSavedTwo);
            var returnedProductCategory = controller.Get();
            Assert.IsTrue(returnedProductCategory.LongCount() > 1);
        }
       
        [TestMethod]
        public void Test_Product_Category_Returns_Not_Null_After_Post()
        {
            ProductCategoryController controller = new ProductCategoryController(_context,ctx);
            controller.Post(productCategoryTobeSaved);
            Assert.IsTrue(_context.productCategories.Any());
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Duplicate_Product_Category_Name_Should_Not_Save()
        {
            ProductCategoryController controller = new ProductCategoryController(_context, ctx);
            controller.Post(productCategoryTobeSaved);
            controller.Post(duplicateProductCategoryToBeSaved);
            var returned = controller.Get();
            Assert.IsTrue(returned.LongCount() > 1);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void No_Product_Category_Name_Should_Not_Save()
        {
            ProductCategoryController controller = new ProductCategoryController(_context, ctx);
            controller.Post(noProductCategoryNameToBeSaved);
            Assert.IsTrue(_context.productCategories.Any());

        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Empty_Or_Null_CogsAccountId_Should_Not_Save()
        {
            ProductCategoryController controller = new ProductCategoryController(_context, ctx);
            controller.Post(noCogsAccountIdToBeSaved);
            Assert.IsTrue(_context.productCategories.Any());

        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Empty_Or_Null_InventoryAccountId_Should_Not_Save()
        {
            ProductCategoryController controller = new ProductCategoryController(_context, ctx);
            controller.Post(noInventoryAccountIdToBeSaved);
            Assert.IsTrue(_context.productCategories.Any());
       }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Empty_Or_Null_Account_Payable_AccountId_Should_Not_Save()
        {
            ProductCategoryController controller = new ProductCategoryController(_context, ctx);
            controller.Post(noApAccountIdToBeSaved);
            Assert.IsTrue(_context.productCategories.Any());
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Empty_Or_Null_Account_Recievable_AccountId_Should_Not_Save()
        {
            ProductCategoryController controller = new ProductCategoryController(_context, ctx);
            controller.Post(noArAccountIdToBeSaved);
            Assert.IsTrue(_context.productCategories.Any());
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Empty_Or_Null_Income_AccountId_Should_Not_Save()
        {
            ProductCategoryController controller = new ProductCategoryController(_context, ctx);
            controller.Post(noIncomeAccountIdToBeSaved);
            Assert.IsTrue(_context.productCategories.Any());
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Empty_Or_Null_Expense_AccountId_Should_Not_Save()
        {
            ProductCategoryController controller = new ProductCategoryController(_context, ctx);
            controller.Post(noExpenseAccountIdToBeSaved);
            Assert.IsTrue(_context.productCategories.Any());
        }

        [TestMethod]
        public void Test_Get_Is_Able_To_Retrieve_Records_After_Post()
        {
            ProductCategoryController controller = new ProductCategoryController(_context, ctx);
            controller.Post(productCategoryTobeSaved);
            var returnedProductCategories = controller.Get();
            Assert.IsTrue(returnedProductCategories.LongCount() > 0);
        }

        

        //[TestMethod]
        //public void Empty_Or_Null_ProductName_Should_Not_Save()
        //{
        //    ProductController controller = new ProductController(_context);
        //    var returnedProduct = controller.Post(noProductNameToBeSaved);
        //    Assert.IsFalse(_context.customers.Any());

        //}

    }
}
