using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using coreLogic;
using System.Threading.Tasks;
using coreErpApi.Controllers.Controllers.Inventory.ShrinkageManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;




namespace coreErpApi.Tests.ShrinkageManagement
{
    [TestClass]
    public class ShrinkageBatchControllerTests
    {
        //Declare variable to be used for testing
        private shrinkageBatch shrinkageBatchTobeSaved;
        private shrinkageBatch shrinkageBatchUpdate;
        private shrinkageBatch shrinkageBatchUpdateTwo;
        private shrinkageBatch noShrinkageBatchDateToBeSaved;
        private shrinkageBatch noShrinkageBatchLocationIdToBeSaved;
        private shrinkage shrinkageTobeSaved;
        private shrinkage shrinkageTobeSavedTwo;
        private shrinkage noShrinkageInventoryItemIdToBeSaved;
        private shrinkage noShrinkageLocationIdToBeSaved;
        private shrinkage noShrinkageQuantityShrunkToBeSaved;


        private inventoryItem inventoryItemOne;
        private inventoryItem inventoryItemTwo;
        private location locationOne;
        private location locationTwo;
        private unitOfMeasurement unitOfMeasurementOne;
        private unitOfMeasurement unitOfMeasurementTwo;

        private ICommerceEntities _context;
        private Icore_dbEntities _le;        
       
        [TestInitialize]
        public void TestInit()
        {
            //Mock CommerceEntities
            _context = new MockCommerceEntities();
            _le = new Mockcore_dbEntities();

            DateTime batchDate = DateTime.ParseExact("2015-02-12", "yyyy-mm-dd", null);
            DateTime batchDateUpdate = DateTime.ParseExact("2015-02-23", "yyyy-mm-dd", null);


           //Assign content to the declared variables
            shrinkageBatchTobeSaved = new shrinkageBatch
            {
                locationId = 1,
                shrinkageDate = batchDate
            };

            shrinkageBatchUpdate = new shrinkageBatch
            {
                shrinkageBatchId = 1,
                locationId = 2,
                shrinkageDate = batchDateUpdate
            };

            shrinkageBatchUpdateTwo = new shrinkageBatch
            {
                shrinkageBatchId = 1,
                locationId = 1,
                shrinkageDate = batchDate
            };

            noShrinkageBatchDateToBeSaved = new shrinkageBatch
            {
                locationId = 2
            };

            noShrinkageBatchLocationIdToBeSaved = new shrinkageBatch
            {
                shrinkageDate = batchDate
            };

            shrinkageTobeSaved = new shrinkage
            {
                inventoryItemId = 1,
                quantityShrunk = 200,
                unitOfMeasurementId = 1
            };

            shrinkageTobeSavedTwo = new shrinkage
            {
                inventoryItemId = 2,
                quantityShrunk = 300,
                unitOfMeasurementId = 2
            };

            noShrinkageInventoryItemIdToBeSaved = new shrinkage
            {
                quantityShrunk = 300,
                unitOfMeasurementId = 2
            };

            noShrinkageLocationIdToBeSaved = new shrinkage
            {
                inventoryItemId = 2,
                quantityShrunk = 300
            };

            noShrinkageQuantityShrunkToBeSaved = new shrinkage
            {
                inventoryItemId = 2,
                unitOfMeasurementId = 2
            };


            inventoryItemOne = new inventoryItem
            {
                inventoryItemId = 1,
                inventoryItemName = "Close-Up"
            };

            inventoryItemTwo = new inventoryItem
            {
                inventoryItemId = 2,
                inventoryItemName = "Pepsodent"
            };

            locationOne = new location
            {
                locationId = 1,
                locationName = "Apenkwa"
            };

            locationTwo = new location
            {
                locationId = 2,
                locationName = "Tema"
            };

            unitOfMeasurementOne = new unitOfMeasurement
            {
                unitOfMeasurementId = 1,
                unitOfMeasurementName = "Unit(s)"
            };

            unitOfMeasurementTwo = new unitOfMeasurement
            {
                unitOfMeasurementId = 2,
                unitOfMeasurementName = "Dozen"
            };


            _context.inventoryItems.Add(inventoryItemOne);
            _context.inventoryItems.Add(inventoryItemTwo);
            _context.locations.Add(locationOne);
            _context.locations.Add(locationTwo);
            _context.unitOfMeasurements.Add(unitOfMeasurementOne);
        }

        [TestMethod]
        public void Test_Shrinkage_Batch_Returns_Non_Empty_List_After_Post()
        {
            ShrinkageBatchController controller = new ShrinkageBatchController(_context, _le);
            shrinkageBatchTobeSaved.shrinkages.Add(shrinkageTobeSaved);
            controller.Post(shrinkageBatchTobeSaved);
            var returnedShrinkageBatch = controller.Get();
            Assert.IsTrue(returnedShrinkageBatch.LongCount() > 0);
        }

        [TestMethod]
        public void Test_Shrinkage_Batch_Update_Post()
        {
            ShrinkageBatchController controller = new ShrinkageBatchController(_context, _le);
            //Add Shrinkage to Shrinkage batch 
            shrinkageBatchUpdate.shrinkages.Add(shrinkageTobeSaved);
            //Add Shrinkage Batch to _context
            _context.shrinkageBatches.Add(shrinkageBatchUpdate);
            //Add Shrinkage to Shrinkage batch 
            shrinkageBatchUpdateTwo.shrinkages.Add(shrinkageTobeSaved);
            //Post Shrinkage Batch with same Id as the Batch that was added to Context
            controller.Post(shrinkageBatchUpdateTwo);
            var returnedShrinkageBatch = controller.Get();
            Assert.IsTrue(returnedShrinkageBatch.LongCount() < 2);
        }

        [TestMethod]
        public void Test_Shrinkage_Batch_Returns_Not_Null_After_Post()
        {
            ShrinkageBatchController controller = new ShrinkageBatchController(_context, _le);
            shrinkageBatchTobeSaved.shrinkages.Add(shrinkageTobeSaved);
            controller.Post(shrinkageBatchTobeSaved);
            Assert.IsTrue(_context.shrinkageBatches.Any());
        }

        [TestMethod]
        public void Test_Get_Shrinkage_Batch_Returns_Non_Empty_List()
        {
            ShrinkageBatchController controller = new ShrinkageBatchController(_context, _le);
            shrinkageBatchTobeSaved.shrinkages.Add(shrinkageTobeSaved);
            _context.shrinkageBatches.Add(shrinkageBatchTobeSaved);
            var returnedShrinkageBatch = controller.Get();
            Assert.IsTrue(returnedShrinkageBatch.LongCount() > 0);
        }

        [TestMethod]
        public void Test_Delete_Shrinkage_Batch_Returns_Empty_List()
        {
            ShrinkageBatchController controller = new ShrinkageBatchController(_context, _le);
            shrinkageBatchTobeSaved.shrinkages.Add(shrinkageTobeSaved);
            _context.shrinkageBatches.Add(shrinkageBatchTobeSaved);
            controller.Delete(shrinkageBatchTobeSaved);
            Assert.IsFalse(_context.shrinkageBatches.Any());
        }


        //Check
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Empty_Or_Null_Shrinkage_Batch_Date_Should_Not_Save()
        {
            ShrinkageBatchController controller = new ShrinkageBatchController(_context, _le);
            noShrinkageBatchDateToBeSaved.shrinkages.Add(shrinkageTobeSaved);
            controller.Post(noShrinkageBatchDateToBeSaved);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Empty_Or_Null_Shrinkage_Batch_LocationId_Should_Not_Save()
        {
            ShrinkageBatchController controller = new ShrinkageBatchController(_context, _le);
            noShrinkageBatchLocationIdToBeSaved.shrinkages.Add(shrinkageTobeSavedTwo);
            controller.Post(noShrinkageBatchLocationIdToBeSaved);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Empty_Or_Null_InventoryItemId_Should_Not_Save()
        {
            ShrinkageBatchController controller = new ShrinkageBatchController(_context, _le);
            shrinkageBatchTobeSaved.shrinkages.Add(noShrinkageInventoryItemIdToBeSaved);
            controller.Post(shrinkageBatchTobeSaved);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Empty_Or_Null_LocationId_Should_Not_Save()
        {
            ShrinkageBatchController controller = new ShrinkageBatchController(_context, _le);
            shrinkageBatchTobeSaved.shrinkages.Add(noShrinkageLocationIdToBeSaved);
            controller.Post(shrinkageBatchTobeSaved);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Empty_Or_Null_QuantityShrunk_Should_Not_Save()
        {
            ShrinkageBatchController controller = new ShrinkageBatchController(_context, _le);
            shrinkageBatchTobeSaved.shrinkages.Add(noShrinkageQuantityShrunkToBeSaved);
            controller.Post(shrinkageBatchTobeSaved);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Empty_Shrinkage__Should_Not_Save()
        {
            ShrinkageBatchController controller = new ShrinkageBatchController(_context, _le);
            controller.Post(shrinkageBatchTobeSaved);
        }
       

    }
}
