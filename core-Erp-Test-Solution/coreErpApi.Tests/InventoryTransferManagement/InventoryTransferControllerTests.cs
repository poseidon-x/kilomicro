using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using coreLogic;
using System.Threading.Tasks;
using coreErpApi.Controllers.Controllers.Inventory.InventoryTransferManagement;
using Microsoft.VisualStudio.TestTools.UnitTesting;




namespace coreErpApi.Tests.InventoryTransferManagement
{
    [TestClass]
    public class InventoryTransferControllerTests
    {
        //Declare variable to be used for testing
        private inventoryTransfer transferBatchTobeSaved;
        private inventoryTransfer transferBatchSameTransitLocationTobeSaved;
        private inventoryTransfer transferBatchTobeSavedTwo;
        private inventoryTransfer transferBatchTobeSavedUpdate;
        private inventoryTransfer noTransferBatchRequisitionDateToBeSaved;
        private inventoryTransfer noTransferBatchFromLocationIdToBeSaved;
        private inventoryTransfer noTransferBatchToLocationIdToBeSaved;
        private inventoryTransferDetail inventTransDetTobeSaved;
        private inventoryTransferDetail inventTransDetTobeSavedTwo;
        private inventoryTransferDetail inventTransDetTobeSavedUpdate;
        private inventoryTransferDetail noInventTransDetInventoryItemIdToBeSaved;
        private inventoryTransferDetail noInventTransDetQuantityTransferedToBeSaved;
        private inventoryTransferDetail inValidInventTransDetQuantityTransferedToBeSaved;
        private inventoryTransferDetail noInventTransDetFromAccountIdToBeSaved;
        private inventoryTransferDetail noInventTransDetToAccountIdToBeSaved;
        private inventoryTransferDetailLine inventTransDetLinTobeSaved;
        private inventoryTransferDetailLine inventTransDetLinTobeSavedTwo;
        private inventoryTransferDetailLine inventTransDetLinTobeSavedUpdate;
        private inventoryTransferDetailLine noInventTransDetLinQuantityTransferedToBeSaved;
        private inventoryTransferDetailLine inValidInventTransDetLinQuantityTransferedToBeSaved;
        private inventoryTransferDetailLine noInventTransDetLinMfgDateToBeSaved;
        private inventoryTransferDetailLine noInventTransDetLinExpiryDateToBeSaved;
        private inventoryTransferDetailLine noInventTransDetLinUnitCostToBeSaved;


        private inventoryItem inventoryItemOne;
        private inventoryItem inventoryItemTwo;
        private location locationOne;
        private location locationTwo;
        private accts accountLookUpOne;
        private accts accountLookUpTwo;
        private accts accountLookUpThree;
        private ICommerceEntities _le;
        private Icore_dbEntities _ctx; 
     
        [TestInitialize]
        public void TestInit()
        {
            //Mock CommerceEntities
            _le = new MockCommerceEntities();
            _ctx = new Mockcore_dbEntities();

            DateTime batchDate = DateTime.ParseExact("2015-02-12", "yyyy-mm-dd", null);
            DateTime batchDateUpdate = DateTime.ParseExact("2015-02-23", "yyyy-mm-dd", null);

            DateTime lineMfgDate = DateTime.ParseExact("2015-02-23", "yyyy-mm-dd", null);
            DateTime lineExpiryDate = DateTime.ParseExact("2016-02-23", "yyyy-mm-dd", null);


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


           //Assign content to the declared variables
            transferBatchTobeSaved = new inventoryTransfer
            {
                fromLocationId = 1,
                toLocationId = 2,
                requisitionDate = batchDate
            };

            transferBatchSameTransitLocationTobeSaved = new inventoryTransfer
            {
                fromLocationId = 2,
                toLocationId = 2,
                requisitionDate = batchDate
            };

            transferBatchTobeSavedTwo = new inventoryTransfer
            {
                inventoryTransferId = 1,
                fromLocationId = 1,
                toLocationId = 2,
                requisitionDate = batchDate,
                approved = false
            };

            transferBatchTobeSavedUpdate = new inventoryTransfer
            {
                inventoryTransferId = 1,
                fromLocationId = 2,
                toLocationId = 1,
                requisitionDate = batchDateUpdate,
                approved = false
            };


            noTransferBatchRequisitionDateToBeSaved = new inventoryTransfer
            {
                fromLocationId = 3,
                toLocationId = 2
            };

            noTransferBatchFromLocationIdToBeSaved = new inventoryTransfer
            {
                toLocationId = 2,
                requisitionDate = batchDate
            };

            noTransferBatchToLocationIdToBeSaved = new inventoryTransfer
            {
                fromLocationId = 3,
                requisitionDate = batchDate
            };

            inventTransDetTobeSaved = new inventoryTransferDetail
            {
                inventoryItemId = 2,
                quantityTransferred = 150,
                fromAccountId = 3,
                toAccountId = 2
            };

            inventTransDetTobeSavedTwo = new inventoryTransferDetail
            {
                inventoryTransferId = 1,
                inventoryTransferDetailId = 1,
                inventoryItemId = 1,
                quantityTransferred = 50,
                fromAccountId = 3,
                toAccountId = 2
            };

            inventTransDetTobeSavedUpdate = new inventoryTransferDetail
            {
                inventoryTransferId = 1,
                inventoryTransferDetailId = 1,
                inventoryItemId = 1,
                quantityTransferred = 200,
                fromAccountId = 1,
                toAccountId = 2
            };

            noInventTransDetInventoryItemIdToBeSaved = new inventoryTransferDetail
            {
                quantityTransferred = 200,
                fromAccountId = 1,
                toAccountId = 2
            };

            noInventTransDetQuantityTransferedToBeSaved = new inventoryTransferDetail
            {
                inventoryItemId = 1,
                fromAccountId = 1,
                toAccountId = 2
            };

            inValidInventTransDetQuantityTransferedToBeSaved = new inventoryTransferDetail
            {
                inventoryItemId = 1,
                quantityTransferred = -200,
                fromAccountId = 1,
                toAccountId = 2
            };

            noInventTransDetFromAccountIdToBeSaved = new inventoryTransferDetail
            {
                inventoryItemId = 1,
                quantityTransferred = 200,
                toAccountId = 2
            };

            noInventTransDetToAccountIdToBeSaved = new inventoryTransferDetail
            {
                inventoryItemId = 2,
                quantityTransferred = 200,
                fromAccountId = 3
            };

            inventTransDetLinTobeSaved = new inventoryTransferDetailLine
            {
                quantityTransferred = 500,
                mfgDate = lineMfgDate,
                expiryDate = lineExpiryDate,
                unitCost = 150
            
            };

            inventTransDetLinTobeSavedTwo = new inventoryTransferDetailLine
            {
                inventoryTransferDetailId = 1,
                inventoryTransferDetailLineId = 1,
                quantityTransferred = 30,
                mfgDate = lineMfgDate,
                expiryDate = lineExpiryDate,
                unitCost = 340
            };

            inventTransDetLinTobeSavedUpdate = new inventoryTransferDetailLine
            {
                inventoryTransferDetailId = 1,
                inventoryTransferDetailLineId = 1,
                quantityTransferred = 140,
                mfgDate = lineMfgDate,
                expiryDate = lineExpiryDate,
                unitCost = 700
            };


            noInventTransDetLinQuantityTransferedToBeSaved = new inventoryTransferDetailLine
            {
                mfgDate = lineMfgDate,
                expiryDate = lineExpiryDate,
                unitCost = 150
            };

            inValidInventTransDetLinQuantityTransferedToBeSaved = new inventoryTransferDetailLine
            {
                quantityTransferred = -140,
                mfgDate = lineMfgDate,
                expiryDate = lineExpiryDate,
                unitCost = 150
            };

            noInventTransDetLinMfgDateToBeSaved = new inventoryTransferDetailLine
            {
                quantityTransferred = 500,
                expiryDate = lineExpiryDate,
                unitCost = 150
            };

            noInventTransDetLinExpiryDateToBeSaved = new inventoryTransferDetailLine
            {
                quantityTransferred = 500,
                mfgDate = lineMfgDate,
                unitCost = 150
            };

            noInventTransDetLinUnitCostToBeSaved = new inventoryTransferDetailLine
            {
                quantityTransferred = 500,
                mfgDate = lineMfgDate,
                expiryDate = lineExpiryDate,
            };


            _le.inventoryItems.Add(inventoryItemOne);
            _le.inventoryItems.Add(inventoryItemTwo);
            _le.locations.Add(locationOne);
            _le.locations.Add(locationTwo);
            _ctx.accts.Add(accountLookUpOne);
            _ctx.accts.Add(accountLookUpTwo);
            _ctx.accts.Add(accountLookUpThree);
            
        }

        [TestMethod]
        public void Test_Transfer_Batch_Returns_Non_Empty_List_After_Post()
        {
            InventoryTransferController controller = new InventoryTransferController(_le, _ctx);
            inventTransDetTobeSaved.inventoryTransferDetailLines.Add(inventTransDetLinTobeSaved);
            transferBatchTobeSaved.inventoryTransferDetails.Add(inventTransDetTobeSaved);
            controller.Post(transferBatchTobeSaved);
            var returnedTransferBatch = controller.Get();
            Assert.IsTrue(returnedTransferBatch.LongCount() > 0);
        }

        [TestMethod]
        public void Test_Transfer_Batch_Update_Post()
        {
            InventoryTransferController controller = new InventoryTransferController(_le, _ctx);
            //Add content to TransferDetailLines  
            inventTransDetTobeSavedTwo.inventoryTransferDetailLines.Add(inventTransDetLinTobeSavedTwo);

            //Add content to TransferDetails 
            transferBatchTobeSavedTwo.inventoryTransferDetails.Add(inventTransDetTobeSavedTwo);
            transferBatchTobeSavedUpdate.inventoryTransferDetails.Add(inventTransDetTobeSavedTwo);

            //Add context to Transfer Batch
            _le.inventoryTransfers.Add(transferBatchTobeSavedUpdate);

    

                //Update Transfer Batch just Added
                controller.Post(transferBatchTobeSavedUpdate);
                var returnedTransferBatch = controller.Get();
            


            //Assert.IsTrue(returnedTransferBatch.LongCount() > 0);
            //Assert.IsTrue(_le.inventoryTransfers.Any());

        }

        [TestMethod]
        public void Test_Transfer_Batch_Returns_Not_Null_After_Post()
        {
            InventoryTransferController controller = new InventoryTransferController(_le, _ctx);
            inventTransDetTobeSaved.inventoryTransferDetailLines.Add(inventTransDetLinTobeSaved);
            transferBatchTobeSaved.inventoryTransferDetails.Add(inventTransDetTobeSaved);
            controller.Post(transferBatchTobeSaved);
            Assert.IsTrue(_le.inventoryTransfers.Any());
        }

        [TestMethod]
        public void Test_Get_Transfer_Batch_Returns_Non_Empty_List()
        {
            InventoryTransferController controller = new InventoryTransferController(_le, _ctx);
            inventTransDetTobeSaved.inventoryTransferDetailLines.Add(inventTransDetLinTobeSaved);
            transferBatchTobeSaved.inventoryTransferDetails.Add(inventTransDetTobeSaved);
            _le.inventoryTransfers.Add(transferBatchTobeSaved);
            var returnedTransferBatch = controller.Get();
            Assert.IsTrue(returnedTransferBatch.LongCount() > 0);
        }

        [TestMethod]
        public void Test_Delete_Transfer_Batch_Returns_Empty_List()
        {
            InventoryTransferController controller = new InventoryTransferController(_le, _ctx);
            inventTransDetTobeSaved.inventoryTransferDetailLines.Add(inventTransDetLinTobeSaved);
            transferBatchTobeSavedTwo.inventoryTransferDetails.Add(inventTransDetTobeSaved);
            _le.inventoryTransfers.Add(transferBatchTobeSavedTwo);
            controller.Delete(transferBatchTobeSavedTwo);
            Assert.IsFalse(_le.inventoryTransfers.Any());
        }


        //Check
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Empty_Or_Null_Transfer_Batch_Requisition_Date_Should_Not_Save()
        {
            InventoryTransferController controller = new InventoryTransferController(_le, _ctx);
            inventTransDetTobeSaved.inventoryTransferDetailLines.Add(inventTransDetLinTobeSaved);
            noTransferBatchRequisitionDateToBeSaved.inventoryTransferDetails.Add(inventTransDetTobeSaved);
            controller.Post(noTransferBatchRequisitionDateToBeSaved);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Empty_Or_Null_Transfer_Batch_From_LocationId_Should_Not_Save()
        {
            InventoryTransferController controller = new InventoryTransferController(_le, _ctx);
            inventTransDetTobeSaved.inventoryTransferDetailLines.Add(inventTransDetLinTobeSaved);
            noTransferBatchFromLocationIdToBeSaved.inventoryTransferDetails.Add(inventTransDetTobeSaved);
            controller.Post(noTransferBatchFromLocationIdToBeSaved);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Empty_Or_Null_Transfer_Batch_To_LocationId_Should_Not_Save()
        {
            InventoryTransferController controller = new InventoryTransferController(_le, _ctx);
            inventTransDetTobeSaved.inventoryTransferDetailLines.Add(inventTransDetLinTobeSaved);
            noTransferBatchToLocationIdToBeSaved.inventoryTransferDetails.Add(inventTransDetTobeSaved);
            controller.Post(noTransferBatchToLocationIdToBeSaved);
        }


        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Empty_Or_Null_InventoryTransferDetail_InventoryItemId_Should_Not_Save()
        {
            InventoryTransferController controller = new InventoryTransferController(_le, _ctx);
            noInventTransDetInventoryItemIdToBeSaved.inventoryTransferDetailLines.Add(inventTransDetLinTobeSaved);
            transferBatchTobeSaved.inventoryTransferDetails.Add(noInventTransDetInventoryItemIdToBeSaved);
            controller.Post(transferBatchTobeSaved); 
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Empty_Or_Null_InventoryTransferDetail_QuantityTransfered_Should_Not_Save()
        {
            InventoryTransferController controller = new InventoryTransferController(_le, _ctx);
            noInventTransDetQuantityTransferedToBeSaved.inventoryTransferDetailLines.Add(inventTransDetLinTobeSaved);
            transferBatchTobeSaved.inventoryTransferDetails.Add(noInventTransDetQuantityTransferedToBeSaved);
            controller.Post(transferBatchTobeSaved);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Negetive_InventoryTransferDetail_QuantityTransfered_Should_Not_Save()
        {
            InventoryTransferController controller = new InventoryTransferController(_le, _ctx);
            inValidInventTransDetQuantityTransferedToBeSaved.inventoryTransferDetailLines.Add(inventTransDetLinTobeSaved);
            transferBatchTobeSaved.inventoryTransferDetails.Add(inValidInventTransDetQuantityTransferedToBeSaved);
            controller.Post(transferBatchTobeSaved);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Empty_Or_Null_InventoryTransferDetail_FromAccountId_Should_Not_Save()
        {
            InventoryTransferController controller = new InventoryTransferController(_le, _ctx);
            noInventTransDetFromAccountIdToBeSaved.inventoryTransferDetailLines.Add(inventTransDetLinTobeSaved);
            transferBatchTobeSaved.inventoryTransferDetails.Add(noInventTransDetFromAccountIdToBeSaved);
            controller.Post(transferBatchTobeSaved);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Empty_Or_Null_InventoryTransferDetail_ToAccountId_Should_Not_Save()
        {
            InventoryTransferController controller = new InventoryTransferController(_le, _ctx);
            noInventTransDetToAccountIdToBeSaved.inventoryTransferDetailLines.Add(inventTransDetLinTobeSaved);
            transferBatchTobeSaved.inventoryTransferDetails.Add(noInventTransDetToAccountIdToBeSaved);
            controller.Post(transferBatchTobeSaved);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Empty_Or_Null_InventoryTransferDetailLine_QuantityTransfered_Should_Not_Save()
        {
            InventoryTransferController controller = new InventoryTransferController(_le, _ctx);
            inventTransDetTobeSaved.inventoryTransferDetailLines.Add(noInventTransDetLinQuantityTransferedToBeSaved);
            transferBatchTobeSaved.inventoryTransferDetails.Add(inventTransDetTobeSaved);
            controller.Post(transferBatchTobeSaved);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Negetive_InventoryTransferDetailLine_QuantityTransfered_Should_Not_Save()
        {
            InventoryTransferController controller = new InventoryTransferController(_le, _ctx);
            inventTransDetTobeSaved.inventoryTransferDetailLines.Add(inValidInventTransDetLinQuantityTransferedToBeSaved);
            transferBatchTobeSaved.inventoryTransferDetails.Add(inventTransDetTobeSaved);
            controller.Post(transferBatchTobeSaved);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Empty_Or_Null_InventoryTransferDetailLine_MfgDate_Should_Not_Save()
        {
            InventoryTransferController controller = new InventoryTransferController(_le, _ctx);
            inventTransDetTobeSaved.inventoryTransferDetailLines.Add(noInventTransDetLinMfgDateToBeSaved);
            transferBatchTobeSaved.inventoryTransferDetails.Add(inventTransDetTobeSaved);
            controller.Post(transferBatchTobeSaved);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Empty_Or_Null_InventoryTransferDetailLine_ExpiryDate_Should_Not_Save()
        {
            InventoryTransferController controller = new InventoryTransferController(_le, _ctx);
            inventTransDetTobeSaved.inventoryTransferDetailLines.Add(noInventTransDetLinExpiryDateToBeSaved);
            transferBatchTobeSaved.inventoryTransferDetails.Add(inventTransDetTobeSaved);
            controller.Post(transferBatchTobeSaved);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Empty_Or_Null_InventoryTransferDetailLine_UnitCost_Should_Not_Save()
        {
            InventoryTransferController controller = new InventoryTransferController(_le, _ctx);
            inventTransDetTobeSaved.inventoryTransferDetailLines.Add(noInventTransDetLinUnitCostToBeSaved);
            transferBatchTobeSaved.inventoryTransferDetails.Add(inventTransDetTobeSaved);
            controller.Post(transferBatchTobeSaved);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Empty_TransferDetail__Should_Not_Save()
        {
            InventoryTransferController controller = new InventoryTransferController(_le, _ctx);
            controller.Post(transferBatchTobeSaved);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Empty_TransferDetailLine_Should_Not_Save()
        {
            InventoryTransferController controller = new InventoryTransferController(_le, _ctx);
            transferBatchTobeSaved.inventoryTransferDetails.Add(inventTransDetTobeSaved);
            controller.Post(transferBatchTobeSaved);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void Test_Transfer_Batch_Same_Transit_Locations_Should_Not_Save()
        {
            InventoryTransferController controller = new InventoryTransferController(_le, _ctx);
            inventTransDetTobeSaved.inventoryTransferDetailLines.Add(inventTransDetLinTobeSaved);
            transferBatchSameTransitLocationTobeSaved.inventoryTransferDetails.Add(inventTransDetTobeSaved);
            controller.Post(transferBatchSameTransitLocationTobeSaved);
        }
       

    }
}
