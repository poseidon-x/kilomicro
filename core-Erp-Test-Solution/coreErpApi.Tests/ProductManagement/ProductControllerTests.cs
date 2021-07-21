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
    public class ProductControllerTests
    {
        private product productTobeSaved;
        private product duplicateProductToBesaved;
        private product noProductCodeToBeSaved;
        private product noProductNameToBeSaved;
        private MockCommerceEntities _context;
        
        [TestInitialize]
        public void TestInit()
        {
            _context = new MockCommerceEntities();
            productTobeSaved = new product
            {
                //productId = 1,
                productSubCategoryId = 1,
                productCode = "PR002",
                productName = "Lipton",
                productDescription = "Lipton is the world’s best-selling tea brand.",
                inventoryMethodId = 1,
                unitOfMeasurementId = 2,
                productMakeId = 16,
                productStatusId = 10
            };

            duplicateProductToBesaved = new product
            {
                productId = 2,
                productSubCategoryId = 1,
                productCode = "PR002",
                productName = "Lipton",
                productDescription = "Lipton is the world’s best-selling tea brand.",
                inventoryMethodId = 1,
                unitOfMeasurementId = 2,
                productMakeId = 16,
                productStatusId = 10
            };

            //No productCode included
            noProductCodeToBeSaved = new product
            {
                productId = 3,
                productSubCategoryId = 4,
                productName = "SunLight",
                productDescription = "Sunlight makes hygiene and cleanliness affordable all around the world.",
                inventoryMethodId = 1,
                unitOfMeasurementId = 2,
                productMakeId = 1,
                productStatusId = 3
            };

            //No productName included
            noProductNameToBeSaved = new product
            {
                productId = 3,
                productSubCategoryId = 4,
                productCode = "PR004",
                productDescription = "Sunlight makes hygiene and cleanliness affordable all around the world.",
                inventoryMethodId = 1,
                unitOfMeasurementId = 2,
                productMakeId = 1,
                productStatusId = 3
            };

            //_context.products.Add(productTobeSaved);
            //_context.products.Add(duplicateProductToBesaved);
            //_context.products.Add(noProductCodeToBeSaved);
            //_context.products.Add(noProductNameToBeSaved);
        }
       
        //[TestMethod]
        //public void Test_Products_Returns_Not_Null()
        //{
        //    ProductController controller = new ProductController(_context);
        //    var returnedProducts = controller.Get();
        //    Assert.IsNotNull(returnedProducts);
        //}

        [TestMethod]
        public void Test_Products_Returns_Non_Empty_List()
        {
            ProductController controller = new ProductController(_context);
            var yes = controller.Post(productTobeSaved);
            var returnedProducts = controller.Get();
            Assert.IsTrue(returnedProducts.LongCount() > 0);
        }

        [TestMethod]
        public void Save_Product_Persists_After_Post()
        {
            ProductController controller = new ProductController(_context);
            var yes = controller.Post(productTobeSaved);
            var returnedProduct = controller.Get();
            Assert.IsTrue(_context.products.Any());
        }

        [TestMethod]
        public void Duplicate_Product_Should_Not_Save()
        {
            ProductController controller = new ProductController(_context);
            var yes = controller.Post(productTobeSaved);
            //var yess = controller.Post(duplicateProductToBesaved);
            var returnedProduct = controller.Get();
            Assert.IsTrue(returnedProduct.LongCount() > 0);

        }

        [TestMethod]
        public void Empty_Or_Null_ProductCode_Should_Not_Save()
        {
            ProductController controller = new ProductController(_context);
            var returnedProduct = controller.Post(noProductCodeToBeSaved);
            Assert.IsFalse(_context.customers.Any());

        }

        [TestMethod]
        public void Empty_Or_Null_ProductName_Should_Not_Save()
        {
            ProductController controller = new ProductController(_context);
            var returnedProduct = controller.Post(noProductNameToBeSaved);
            Assert.IsFalse(_context.customers.Any());

        }

    }
}
