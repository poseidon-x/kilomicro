//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Web.Http;
//using coreERP;
//using coreLogic;
//using coreERP.Providers;
//using System.Linq.Dynamic;

//namespace coreErpApi.Controllers.Controllers.Inventory.InventoryTransferManagement
//{
//    [AuthorizationFilter()]
//    public class InventoryTransferController : ApiController
//    {
//        //Declaration of constant variables for error messages
//        private const string TRANSFER_BATCH_SAME_TRANSIT_LOCATIONS_ERROR_MESSAGE = "Request & Destination Lacation cannot be same<br />";
//        private const string TRANSFER_BATCH_DROP_DOWN_ERROR_MESSAGE = "Invalid Drop Down value from the form<br />";
//        private const string TRANSFER_BATCH_REQUISITION_DATE_ERROR_MESSAGE = "Requisition Date cannot be later than today<br />";
//        private const string TRANSFER_BATCH_INVALID_REQUISITION_DATE_ERROR_MESSAGE = "Invalid Requisition Date cannot be later than today<br />";        
//        private const string TRANSFER_BATCH_EMPTY_FIELDS_ERROR_MESSAGE = "Please make sure all the form fields have value<br />";
//        private const string TRANSFER_DETAILS_DROP_DOWN_ERROR_MESSAGE = "Invalid Drop Down value from the Details Grid<br />";
//        private const string TRANSFER_DETAILS_FIELDS_ERROR_MESSAGE = "One or more Detail Grid fields is/are Empty<br />";
//        private const string TRANSFER_DETAILS_QUANTITY_TRANSFERED_SIZE_ERROR_MESSAGE = "Details Grid Quantity Transfered cannot be less than 1<br />";
//        private const string TRANSFER_DETAILS_GRID_WITHOUT_DATA_ERROR_MESSAGE = "Transfer Details Grid cannot be empty<br />";
//        private const string TRANSFER_DETAIL_LINE_EMPTY_FIELDS_ERROR_MESSAGE = "One or more Detail Line Grid fields is/are Empty<br />";
//        private const string TRANSFER_DETAIL_LINE_QUANTITY_TRANSFERED_SIZE_ERROR_MESSAGE = "Details Line Grid's Quantity Transfered cannot be less than 1<br />";
//        private const string TRANSFER_DETAIL_LINE_UNIT_COST_SIZE_ERROR_MESSAGE = "Details Line Grid's Unit cost cannot be less than 1<br />";
//        private const string TRANSFER_DETAIL_LINE_MFG_DATE_ERROR_MESSAGE = "Manufacturing Date cannot be later than today<br />";
//        private const string TRANSFER_DETAIL_LINE_EXPIRY_DATE_ERROR_MESSAGE = "Expiry Date must be a future date<br />";
//        private const string TRANSFER_DETAILS_LINE_GRID_WITHOUT_DATA_ERROR_MESSAGE = "Transfer Detail Lines Grid cannot be empty<br />";
//        private const string TRANSFER_DETAILS_LINE_GRID_INVALID_DATE_ERROR_MESSAGE = "Transfer Detail Lines Grid has Invalid Date<br />";

//        private string nextBatchNumber;
//        private string errorMessage = "";
//        private bool save = true;
//        //Declare a Database(Db) context variable 
//        private ICommerceEntities le;
//        private Icore_dbEntities ctx;


//        //call a constructor to instialize a the Dv context 
//        public InventoryTransferController()
//        {
//            le = new CommerceEntities();
//            ctx = new core_dbEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        //A constructor wiith a parameter
//        public InventoryTransferController(ICommerceEntities lent, Icore_dbEntities ent)
//        {
//            le = lent;
//            ctx = ent;
//        }

//        // GET: api/shrinkageBatch
//        public IEnumerable<inventoryTransfer> Get()
//        {
//            return le.inventoryTransfers
//                .Include(p => p.inventoryTransferDetails)
//                .Include(p =>p.inventoryTransferDetails.Select(q => q.inventoryTransferDetailLines))
//                .OrderBy(p => p.inventoryTransferId)
//                .ToList();
//        }

//        [HttpPost]
//        public KendoResponse Get([FromBody]KendoRequest req)
//        {
//            string order = "inventoryTransferId";

//            KendoHelper.getSortOrder(req, ref order);
//            var parameters = new List<object>();
//            var whereClause = KendoHelper.getWhereClause<brand>(req, parameters);

//            var query = le.inventoryTransfers.AsQueryable();
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

//        // GET: api/shrinkageBatches/
//        [HttpGet]
//        public inventoryTransfer Get(int id)
//        {
//            inventoryTransfer value = le.inventoryTransfers
//                .Include(p => p.inventoryTransferDetails)
//                .Include(p => p.inventoryTransferDetails.Select(q => q.inventoryTransferDetailLines))
//                .FirstOrDefault(p => p.inventoryTransferId == id);


//            if (value == null)
//            {
//                value = new inventoryTransfer();
//            }
//            return value;
//        }

//        [HttpGet]
//        public IEnumerable<inventoryTransferDetail> GetDetail(int id)
//        {
//            return le.inventoryTransferDetails
//                .Where(p => p.inventoryTransferId == id)
//                .OrderBy(p => p.inventoryTransferId)
//                .ToList();
//        }

//        [HttpGet]
//        public IEnumerable<inventoryTransferDetailLine> GetDetailLine(int id)
//        {
//            return le.inventoryTransferDetailLines
//                .Where(p => p.inventoryTransferDetailId == id)
//                .OrderBy(p => p.inventoryTransferDetailId)
//                .ToList();
//        }





//        [HttpPost]
//        public inventoryTransfer Post(inventoryTransfer value)
//        {
//            inventoryTransfer toBeSaved = null;
//            inventoryTransferDetail invtTransDetToBeSaved = null;
//            inventoryTransferDetailLine invtTransDetLineToBeSaved = null;

//            if (ValidateTransferBatch(value) == false)
//            {
//                throw new ApplicationException(errorMessage);
//            }

//            if (ValidateTransferBatch(value))
//            {
//            //If shrinkageBatchId is > 0 Its an update, and perform a PUT operation
//            if (value.inventoryTransferId > 0)
//            {
//                //If the record has not been approved, then update
//                if (value.approved == false)
//                {
//                    toBeSaved = le.inventoryTransfers
//                        .Include(p => p.inventoryTransferDetails)
//                        .Include(p => p.inventoryTransferDetails.Select(q => q.inventoryTransferDetailLines))
//                        .First(p => p.inventoryTransferId == value.inventoryTransferId);
//                    populateFields(toBeSaved, value);
//                } //If record has been approved already then, send an error message.
//                else {

          
//                        throw new ApplicationException(
//                        "This record has already been approve & you cann't update the details.");
                    
                
//                }
//            }
//            else //Else its a new record, and perform a POST
//            {
//                toBeSaved = new inventoryTransfer();
//                populateFields(toBeSaved, value);
//                le.inventoryTransfers.Add(toBeSaved);
//            }

//            //For the child table inventoryTransferDetail
//                foreach (var invtTransDet in value.inventoryTransferDetails)
//                {

//                    if (ValidateInventoryTransferDetail(invtTransDet) == false)
//                    {
//                        throw new ApplicationException(errorMessage);
//                    }

//                    if (ValidateInventoryTransferDetail(invtTransDet))
//                    {

//                    //If inventoryTransferDetailsId is > 0 Its an update, and perform a PUT operation
//                    if (invtTransDet.inventoryTransferDetailId > 0)
//                    {
//                        invtTransDetToBeSaved = toBeSaved.inventoryTransferDetails
//                            .First(p => p.inventoryTransferDetailId == invtTransDet.inventoryTransferDetailId);
//                        populateInventoryTransferDetailFields(invtTransDetToBeSaved, invtTransDet);
//                    }
//                    //Else its a new record, and perform a POST
//                    else
//                    {
//                        invtTransDetToBeSaved = new inventoryTransferDetail();
//                        populateInventoryTransferDetailFields(invtTransDetToBeSaved, invtTransDet);
//                        toBeSaved.inventoryTransferDetails.Add(invtTransDetToBeSaved);
//                    }

//                        foreach (var invtTransDetLine in invtTransDet.inventoryTransferDetailLines)
//                        {
//                            if (ValidateinventoryTransferDetailLine(invtTransDetLine) == false)
//                            {
//                                throw new ApplicationException(errorMessage);
//                            }

//                            if (ValidateinventoryTransferDetailLine(invtTransDetLine))
//                            {
//                            //If inventoryTransferDetailsId is > 0 Its an update, and perform a PUT operation
//                            if (invtTransDetLine.inventoryTransferDetailId > 0)
//                            {
//                                invtTransDetLineToBeSaved = invtTransDetToBeSaved.inventoryTransferDetailLines
//                                    .First(
//                                        p =>
//                                            p.inventoryTransferDetailLineId ==
//                                            invtTransDetLine.inventoryTransferDetailLineId);
//                                populateInventoryTransferDetailLineFields(invtTransDetLineToBeSaved, invtTransDetLine);

//                            }
//                            //Else its a new record, and perform a POST
//                            else
//                            {
//                                invtTransDetLineToBeSaved = new inventoryTransferDetailLine();
//                                populateInventoryTransferDetailLineFields(invtTransDetLineToBeSaved, invtTransDetLine);
//                                invtTransDetToBeSaved.inventoryTransferDetailLines.Add(invtTransDetLineToBeSaved);
//                            }

//                        }
//                    }

//                }

//            }

//            for (var i = toBeSaved.inventoryTransferDetails.Count - 1; i >= 0; i--)
//            {
//                var inDb = toBeSaved.inventoryTransferDetails.ToList()[i];
//                if (
//                    !value.inventoryTransferDetails.Any(
//                        p => p.inventoryTransferDetailId == inDb.inventoryTransferDetailId))
//                {
//                    le.inventoryTransferDetails.Remove(inDb);
//                }
//            }

//            //If save is set to false, we clear the ShrinkageBatch records that had been saved earlier
//            if (save == false)
//            {
//                return toBeSaved = null;
//            }
//            //Else we save the changes
//            else
//            {
//                le.SaveChanges();
//            }
//        }
//            return toBeSaved;

//    }

//        //UPDATE SHRINKAGE TO APPROVED 
//        [HttpPost]
//        public inventoryTransfer PostApproval(inventoryTransfer value)
//        {
//            inventoryTransfer toBeSaved = null;

//            if (value.inventoryTransferId > 0)
//            {
//                toBeSaved = le.inventoryTransfers
//                    .First(p => p.inventoryTransferId == value.inventoryTransferId);
//                toBeSaved.approved = true;
//                toBeSaved.approver = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                toBeSaved.approvedDate = DateTime.Now;
//                toBeSaved.approvalComments = value.approvalComments;
//            }

//            le.SaveChanges();
//            return toBeSaved;
//        }

//        //UPDATE SHRINKAGE TO POSTED 
//        [HttpPost]
//        public inventoryTransfer TransferPost(inventoryTransfer value)
//        {
//            jnl_batch toBePosted;
//            inventoryTransfer toBeSaved = null;
//            inventoryItemDetail invtItmDetToBeSaved = null;


//            if (value.inventoryTransferId > 0)
//            {
//                //IF SHRINKAGE IS APPROVED, THEN POST
//                if (value.approved)
//                {
//                    foreach (var invtTrans in value.inventoryTransferDetails)
//                    {
//                        var quantity = 0;
//                        double totalAmount = 0;
//                        foreach (var invtTransLin in invtTrans.inventoryTransferDetailLines)
//                        {
//                            quantity += (int)invtTransLin.quantityTransferred;
//                            double amnt = invtTransLin.quantityTransferred * invtTransLin.unitCost;
//                            totalAmount += amnt;
//                        }
//                        var invtItemId = le.inventoryTransferDetails.FirstOrDefault(p => p.inventoryTransferDetailId == invtTrans.inventoryTransferId).inventoryItemId;
//                        var invtAcctId = (int)le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == invtItemId).shrinkageAccountId;
//                        var acctId = (int)le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == invtItemId).accountId;
//                        var descript = value.postingComments;
//                        var curryId = (int)le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == invtItemId).currencyId;
//                        var invtTransDate = value.requisitionDate.Value;
//                        var refNum = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == invtItemId).itemNumber;
//                        var user = LoginHelper.getCurrentUser(new coreSecurityEntities()); 

//                        toBePosted = (new JournalExtensions()).Post("IV", invtAcctId, acctId, totalAmount, descript, curryId, invtTransDate,
//                            refNum, ctx, user, null);
//                        ctx.jnl_batch.Add(toBePosted);
//                    }


//                    toBeSaved = le.inventoryTransfers
//                        .First(p => p.inventoryTransferId == value.inventoryTransferId);
//                    toBeSaved.posted = true;
//                    toBeSaved.postedDate = DateTime.Now;
//                    toBeSaved.postedBy = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                    toBeSaved.postingComments = value.postingComments;
//                }
//                else// IF SHRINKAGE IS NOT APPROVED, THROW AN EXCEPTION
//                {
//                    throw new ApplicationException("Sorry, unapproved transfer cann't be posted");                    
//                }
//            }

//            //SAVE CHANGES
//            ctx.SaveChanges();
//            le.SaveChanges();
//            return toBeSaved;
//        }

//        [HttpPost]
//        public inventoryTransfer PostDelivery(inventoryTransfer value)
//        {
//            inventoryTransfer toBeSaved = null;

//            if (value.inventoryTransferId > 0)
//            {
//                //IF SHRINKAGE IS APPROVED, THEN POST
//                if (value.approved)
//                {
//                    if (value.posted)
//                    {
//                        toBeSaved = le.inventoryTransfers
//                            .First(p => p.inventoryTransferId == value.inventoryTransferId);
//                        toBeSaved.delivered = true;
//                        toBeSaved.deliveredDate = DateTime.Now;
//                        toBeSaved.deliveredBy = value.deliveredBy;
//                        toBeSaved.receivedBy = value.receivedBy;
//                        if (!(String.IsNullOrEmpty(value.deliveryComments.ToString())))
//                        {
//                            toBeSaved.deliveryComments = value.deliveryComments;
//                        }
//                        if (!(String.IsNullOrEmpty(value.deliveryComments.ToString())))
//                        {
//                            toBeSaved.deliveryComments = value.deliveryComments;
//                        }
//                    }
//                    else// IF SHRINKAGE IS NOT APPROVED, THROW AN EXCEPTION
//                    {
//                        throw new ApplicationException("Sorry, unposted transfer cann't be marked for delivery");
//                    }
//                }
//                else// IF SHRINKAGE IS NOT APPROVED, THROW AN EXCEPTION
//                {
//                    throw new ApplicationException("Sorry, unapproved transfer cann't be posted");
//                }
//            }

//            //SAVE CHANGES
//            le.SaveChanges();
//            return toBeSaved;
//        }


//        //populate shrinkageBatch the fields to be saved
//        private void populateFields(inventoryTransfer toBeSaved, inventoryTransfer value)
//        {
//            toBeSaved.fromLocationId = value.fromLocationId;
//            toBeSaved.toLocationId = value.toLocationId;
//            toBeSaved.requisitionDate = value.requisitionDate;
//            toBeSaved.approved = false;
//            toBeSaved.enteredBy = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            toBeSaved.delivered = false;
//            toBeSaved.posted = false;
//            if (value.inventoryTransferId < 1)
//            {
//                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                toBeSaved.created = DateTime.Now;
//            }
//            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            toBeSaved.modified = DateTime.Now;
//        }

//        //populate shrinkage the fields to be saved
//        private void populateInventoryTransferDetailFields(inventoryTransferDetail toBeSaved, inventoryTransferDetail value)
//        {
//            var invtId = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == value.inventoryItemId).inventoryItemId;
//            var invtAcctId = (int)le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == invtId).shrinkageAccountId;
//            var acctId = (int)le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == invtId).accountId;

//            toBeSaved.inventoryTransferId = value.inventoryTransferId;
//            toBeSaved.inventoryItemId = value.inventoryItemId;
//            toBeSaved.quantityTransferred = value.quantityTransferred;
//            toBeSaved.fromAccountId = acctId;
//            toBeSaved.toAccountId = invtAcctId;
//            if (value.inventoryTransferDetailId < 1)
//            {
//                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                toBeSaved.created = DateTime.Now;
//            }
//            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities()); ;
//            toBeSaved.modified = DateTime.Now;
//        }

//        //populate shrinkage the fields to be saved
//        private void populateInventoryTransferDetailLineFields(inventoryTransferDetailLine toBeSaved, inventoryTransferDetailLine value)
//        {
//            toBeSaved.inventoryTransferDetailId = value.inventoryTransferDetailId;
//            toBeSaved.quantityTransferred = value.quantityTransferred;
//            toBeSaved.batchNumber = generateInvtTransBatchNumber();
//            toBeSaved.mfgDate = value.mfgDate;
//            toBeSaved.expiryDate = value.expiryDate;
//            toBeSaved.unitCost = value.unitCost;
//            if (value.inventoryTransferDetailLineId < 1)
//            {
//                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                toBeSaved.created = DateTime.Now;
//            }
//            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            toBeSaved.modified = DateTime.Now;
//        }


//        public string generateInvtTransBatchNumber()
//        {
//            nextBatchNumber = !le.inventoryTransferDetailLines.Any()
//                ? "BHN-000001"
//                : // if it's empty, start with BHN-000001
//                "BHN-" +
//                (int.Parse(
//                    le.inventoryTransferDetailLines.OrderByDescending(i => i.batchNumber) // order by code descending
//                    .First() // get first one (last code)
//                    .batchNumber.Split('-')[1]) // get only the number part
//                + 1).ToString("000000"); // add 1 and format with 6 digits

//            return nextBatchNumber;
//        }

//        [HttpDelete]
//        // DELETE: api/productCategory/5
//        public void Delete([FromBody]inventoryTransfer value)
//        {
//            var forDelete = le.inventoryTransfers
//                .Include(p => p.inventoryTransferDetails)
//                .Include(p => p.inventoryTransferDetails.Select(q => q.inventoryTransferDetailLines))
//                .FirstOrDefault(p => p.inventoryTransferId == value.inventoryTransferId);
//            if (forDelete != null)
//            {
//                le.inventoryTransfers.Remove(forDelete);
//                le.SaveChanges();
//            }
//        }







//        //Validate Fields in Shrinkage batch Form
//        private bool ValidateTransferBatch(inventoryTransfer inventTrans)
//        {
//            //If Shrinkage Grid is empty, Catch the error and return false
//            if (inventTrans.inventoryTransferDetails.Any())
//            {
//                //Execute the List validation tests
//                ValidateTransferBatchLocationDropDown(inventTrans);
//                ValidateTransferBatchEmptyFields(inventTrans);
//                ValidateRequisitionDate(inventTrans);
//                ValidateTransferBatchTransitLocations(inventTrans);

//                //If errorMessage is empty test Pass
//                if (errorMessage == "")
//                {
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//            else
//            {
//                errorMessage += TRANSFER_DETAILS_GRID_WITHOUT_DATA_ERROR_MESSAGE;
//                return false;
//            }
//        }

//        private bool ValidateInventoryTransferDetail(inventoryTransferDetail inventTransDet)
//        {
//            if (inventTransDet.inventoryTransferDetailLines.Any())
//            {
//                ValidateInventoryTransferDetailDropDown(inventTransDet);
//                ValidateInventoryTransferDetailEmptyFields(inventTransDet);
//                ValidateInventoryTransferDetailSize(inventTransDet);
//                if (errorMessage == "")
//                {
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//        }
//            else
//            {
//                errorMessage += TRANSFER_DETAILS_LINE_GRID_WITHOUT_DATA_ERROR_MESSAGE;
//                return false;
//            }
//    }

//        private bool ValidateinventoryTransferDetailLine(inventoryTransferDetailLine inventTransDetLin)
//        {
//            ValidateInventoryTransferDetailLineEmptyFields(inventTransDetLin);
//            ValidateInventoryTransferDetailLineQuanTransSize(inventTransDetLin);
//            ValidateInventoryTransferDetailLineUnitCostSize(inventTransDetLin);
//            ValidateInventoryTransferDetailLineMfgDate(inventTransDetLin);
//            ValidateInventoryTransferDetailLineExpiryDate(inventTransDetLin);
//            ValidateInventoryTransferInvalidDateFields(inventTransDetLin);

//                if (errorMessage == "")
//                {
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//        }

//        //validate shrinkagebatch Location Drop Down to ensure User selected from the Drop down
//        private void ValidateTransferBatchLocationDropDown(inventoryTransfer inventTrans)
//        {
//            if (locationExists(inventTrans.fromLocationId) == false || locationExists(inventTrans.toLocationId) == false)
//            {
//                errorMessage += TRANSFER_BATCH_DROP_DOWN_ERROR_MESSAGE;
//            }
//        }



//        //validate to ensure shrinkageBatch fields are not empty
//        private void ValidateTransferBatchEmptyFields(inventoryTransfer inventTrans)
//        {
//            if ((inventTrans.requisitionDate == DateTime.MinValue)
//                || (inventTrans.fromLocationId < 1) || (inventTrans.toLocationId < 1))
//            {
//                errorMessage += TRANSFER_BATCH_EMPTY_FIELDS_ERROR_MESSAGE;
//            }
//        }

//        private void ValidateTransferBatchTransitLocations(inventoryTransfer inventTrans)
//        {
//            if (inventTrans.fromLocationId == inventTrans.toLocationId)
//            {
//                errorMessage += TRANSFER_BATCH_SAME_TRANSIT_LOCATIONS_ERROR_MESSAGE;
//            }
//        }

////        //validate to ensure shrinkageBatch date is not a future date
//        private void ValidateRequisitionDate(inventoryTransfer inventTrans)
//        {
//            if (inventTrans.requisitionDate > DateTime.Today)
//            {
//                errorMessage += TRANSFER_BATCH_REQUISITION_DATE_ERROR_MESSAGE;
//            }
//        }

//        //validate shrinkage inventoryItem Drop Down to ensure User selected from the Drop down
//        private void ValidateInventoryTransferDetailDropDown(inventoryTransferDetail inventTransDet)
//        {
//            if ((inventoryItemExists(inventTransDet.inventoryItemId) == false))
//            {
//                errorMessage += TRANSFER_DETAILS_DROP_DOWN_ERROR_MESSAGE;
//            }
//        }



//        //validate to ensure shrinkage fields are not empty
//        private void ValidateInventoryTransferDetailEmptyFields(inventoryTransferDetail inventTransDet)
//        {
//            if ((inventTransDet.inventoryItemId <= 0) || (inventTransDet.quantityTransferred <= 0))
//            {
//                errorMessage += TRANSFER_DETAILS_FIELDS_ERROR_MESSAGE;
//            }
//        }

//        //validate to ensure shrinkage shrinkageQuantityShrunk size is reasonable
//        private void ValidateInventoryTransferDetailSize(inventoryTransferDetail inventTransDet)
//        {
//            if (inventTransDet.quantityTransferred < 1)
//            {
//                errorMessage += TRANSFER_DETAILS_QUANTITY_TRANSFERED_SIZE_ERROR_MESSAGE;
//            }
//        }

//        private void ValidateInventoryTransferDetailLineEmptyFields(inventoryTransferDetailLine inventTransDetLin)
//        {
//            if ((inventTransDetLin.quantityTransferred <= 0) || (inventTransDetLin.mfgDate == DateTime.MinValue)
//                || (inventTransDetLin.expiryDate == DateTime.MinValue) || (inventTransDetLin.unitCost <= 0)||
//                (String.IsNullOrEmpty(inventTransDetLin.mfgDate.ToString())) ||
//                (String.IsNullOrEmpty(inventTransDetLin.expiryDate.ToString())))
//            {
//                errorMessage += TRANSFER_DETAIL_LINE_EMPTY_FIELDS_ERROR_MESSAGE;
//            }

//        }

//        private void ValidateInventoryTransferInvalidDateFields(inventoryTransferDetailLine inventTransDetLin)
//        {
//            DateTime value;
//            if (!DateTime.TryParse(inventTransDetLin.mfgDate.ToString(), out value) ||
//                !DateTime.TryParse(inventTransDetLin.expiryDate.ToString(), out value))
//            {
//                errorMessage += TRANSFER_DETAILS_LINE_GRID_INVALID_DATE_ERROR_MESSAGE;
//            }
//        }





//        private void ValidateInventoryTransferDetailLineQuanTransSize(inventoryTransferDetailLine inventTransDetLin)
//        {
//            if (inventTransDetLin.quantityTransferred < 1)
//            {
//                errorMessage += TRANSFER_DETAIL_LINE_QUANTITY_TRANSFERED_SIZE_ERROR_MESSAGE;
//            }
//        }

//        private void ValidateInventoryTransferDetailLineUnitCostSize(inventoryTransferDetailLine inventTransDetLin)
//        {
//            if (inventTransDetLin.unitCost < 1)
//            {
//                errorMessage += TRANSFER_DETAIL_LINE_UNIT_COST_SIZE_ERROR_MESSAGE;
//            }
//        }

//        private void ValidateInventoryTransferDetailLineMfgDate(inventoryTransferDetailLine inventTransDetLin)
//        {
//            if (inventTransDetLin.mfgDate > DateTime.Today)
//            {
//                errorMessage += TRANSFER_DETAIL_LINE_MFG_DATE_ERROR_MESSAGE;
//            }
//        }

//        private void ValidateInventoryTransferDetailLineExpiryDate(inventoryTransferDetailLine inventTransDetLin)
//        {
//            if (inventTransDetLin.expiryDate <= DateTime.Today)
//            {
//                errorMessage += TRANSFER_DETAIL_LINE_EXPIRY_DATE_ERROR_MESSAGE;
//            }
//        }
        

//        //Validate to ensure location selected has an Id
//        private bool locationExists(int? locId)
//        {
//            if (le.locations.Any(p => p.locationId == locId))
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        //Validate to ensure inventoryItem selected has an Id
//        private bool inventoryItemExists(long? invtId)
//        {
//            if (le.inventoryItems.Any(p => p.inventoryItemId == invtId))
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        private bool accountExists(int? accountId)
//        {
//            if (ctx.accts.Any(p => p.acct_id == accountId))
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }


//        private bool batchNumExists(string batchNum)
//        {
//            if (le.inventoryTransferDetailLines.Any(p => p.batchNumber == batchNum))
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }
        

//    }
//}


