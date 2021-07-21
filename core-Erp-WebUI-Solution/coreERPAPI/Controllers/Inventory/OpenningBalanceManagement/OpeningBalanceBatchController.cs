using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using System.Security.Policy;

namespace coreErpApi.Controllers.Controllers.Inventory.OpenningBalanceManagement
{
    [AuthorizationFilter()]
    public class OpeningBalanceBatchController : ApiController
    {
        private const string OPENNING_BALANCE_BATCH_DROP_DOWN_ERROR_MESSAGE = "Be sure you have selected from the provided Drop Down list in the form<br />";
        private const string OPENNING_BALANCE_BATCH_BALANCE_DATE_ERROR_MESSAGE = "Opening Balance Batch Date cannot be a future date<br />";
        private const string OPENNING_BALANCE_BATCH_EMPTY_FIELDS_ERROR_MESSAGE = "Please make sure all the form fields are not Empty<br />";
        private const string OPENNING_BALANCE_DROP_DOWN_ERROR_MESSAGE = "Be sure you selected from the provided Drop Down list in the Details Grid<br />";
        private const string OPENNING_BALANCE_EMPTY_FIELDS_ERROR_MESSAGE = "Please make sure all the Grid fields are not Empty<br />";
        private const string OPENNING_BALANCE_QUANTITY_ON_HAND_SIZE_ERROR_MESSAGE = "Please make sure all quantity shrunk values are a real numbers<br />";
        private const string OPENNING_BALANCE_GRID_WITHOUT_DATA_ERROR_MESSAGE = "Opening Balance Details Grid cannot be empty<br />";

        private string errorMessage = "";

        // db entities
        private ICommerceEntities le;
        private Icore_dbEntities ctx;


        public OpeningBalanceBatchController()
        {
            le = new CommerceEntities();
            ctx = new core_dbEntities();

            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
            ctx.Configuration.LazyLoadingEnabled = false;
            ctx.Configuration.ProxyCreationEnabled = false;
        }

        public OpeningBalanceBatchController(ICommerceEntities lent, Icore_dbEntities ent)
        {
            le = lent;
            ctx = ent;
        }


        public IEnumerable<openningBalanceBatch> Get()
        {
            return le.openningBalanceBatches
                .Include(p => p.openningBalances)
                .OrderBy(p => p.openningBalanceBatchId)
                .ToList();
        }



        [HttpGet]
        public openningBalanceBatch Get(int Id)
        {
            openningBalanceBatch value = le.openningBalanceBatches
                .Include(p => p.openningBalances)
                .FirstOrDefault(p => p.openningBalanceBatchId == Id);

            if (value == null)
            {
                value = new openningBalanceBatch();
            }
            return value;
        }

        [HttpPost]
        public KendoResponse Get([FromBody]KendoRequest req)
        {
            string order = "balanceDate";

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<brand>(req, parameters);

            var query = le.openningBalanceBatches.AsQueryable();
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

        [HttpPost]
        public openningBalanceBatch Post(openningBalanceBatch value)
        {
            openningBalanceBatch toBeSaved = null;

            if (ValidateOpenningBalanceBatch(value) == false)
            {
                throw new ApplicationException(errorMessage);
            }
            if (ValidateOpenningBalanceBatch(value))
            {
                if (value.openningBalanceBatchId > 0)
                {
                    if (value.approved == false)
                    {
                        toBeSaved = le.openningBalanceBatches
                            .Include(p => p.openningBalances)
                            .First(p => p.openningBalanceBatchId == value.openningBalanceBatchId);
                        populateFields(toBeSaved, value);
                    }
                    else throw new ApplicationException("This record has already been approve & you cann't update the details.");//

                }
                else
                {
                    toBeSaved = new openningBalanceBatch();
                    populateFields(toBeSaved, value);
                    le.openningBalanceBatches.Add(toBeSaved);
                }



                foreach (var openning in value.openningBalances)
                {
                    if (ValidateOpenningBalance(openning) == false)
                    {
                        throw new ApplicationException(errorMessage);
                    }
                    if (ValidateOpenningBalance(openning))
                    {
                        openningBalance openningToBeSaved = null;
                        if (openning.openningBalanceId > 0)
                        {
                            openningToBeSaved = toBeSaved.openningBalances
                                .First(p => p.openningBalanceId == openning.openningBalanceId);
                            populateOpenningFields(openningToBeSaved, openning);
                        }
                        else
                        {
                            openningToBeSaved = new openningBalance();
                            populateOpenningFields(openningToBeSaved, openning);
                            toBeSaved.openningBalances.Add(openningToBeSaved);
                        }
                    }
                    le.SaveChanges();
                }

            }

            return toBeSaved;
        }

        [HttpPost]
        public openningBalanceBatch PostApproval(openningBalanceBatch value)
        {
            openningBalanceBatch toBeSaved = null;

            if (value.openningBalanceBatchId > 0)
            {
                toBeSaved = le.openningBalanceBatches
                    .First(p => p.openningBalanceBatchId == value.openningBalanceBatchId);
                toBeSaved.approved = true;
                toBeSaved.approvedBy = LoginHelper.getCurrentUser(new coreSecurityEntities()); ;
                toBeSaved.approvalDate = DateTime.Now;
                toBeSaved.approvalComments = value.approvalComments;
            }

            le.SaveChanges();
            return toBeSaved;
        }

        [HttpPost]
        public openningBalanceBatch BalancePost(openningBalanceBatch value)
        {
            openningBalanceBatch toBeSaved = null;


            if (value.openningBalanceBatchId > 0)
            {
                if (value.approved)
                {

                    foreach (var opening in value.openningBalances)
                    {
                        jnl_batch toBePosted;                        
                        inventoryItemDetail invtItmToBeSaved = null;
                        var openRecord = le.openningBalances.FirstOrDefault(p => p.openningBalanceId == opening.openningBalanceId);

                        var openingInvtId = openRecord.inventoryItemId;
                        var openingAcctId = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == openingInvtId).shrinkageAccountId;
                        var acctId = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == openingInvtId).accountId;
                        //var unitCost = le.inventoryItemDetails.FirstOrDefault(p => p.inventoryItemId == openingInvtId).unitCost;
                        var unitCost = 0.00;
                        var amount = unitCost * opening.quantityOnHand;
                        var descript = value.postingComments;
                        var curryId = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == openingInvtId).currencyId;
                        var openingDate = value.balanceDate;
                        var refNum = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == openingInvtId).itemNumber;
                        var user = LoginHelper.getCurrentUser(new coreSecurityEntities());

                        toBePosted = (new JournalExtensions()).Post("IV", openingAcctId, acctId, amount, descript, curryId, openingDate,
                            refNum, ctx, user, null);
                        ctx.jnl_batch.Add(toBePosted);

                        //Save inventoryItemDetail 
                        invtItmToBeSaved = new inventoryItemDetail();
                        invtItmToBeSaved.inventoryItemId = openingInvtId;
                        invtItmToBeSaved.batchNumber = openRecord.batchNumber;
                        invtItmToBeSaved.unitCost = 0;
                        invtItmToBeSaved.quantityOnHand = openRecord.quantityOnHand;
                        invtItmToBeSaved.reservedQuantity = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == openingInvtId).safetyStockLevel;
                        invtItmToBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
                        invtItmToBeSaved.created = DateTime.Now;
                        invtItmToBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
                        invtItmToBeSaved.modified = DateTime.Now;
                        le.inventoryItemDetails.Add(invtItmToBeSaved);
                    }


                    //Mark batch as posted
                    toBeSaved = le.openningBalanceBatches
                        .First(p => p.openningBalanceBatchId == value.openningBalanceBatchId);
                    toBeSaved.posted = true;
                    toBeSaved.postedDate = DateTime.Now;
                    toBeSaved.postedBy = LoginHelper.getCurrentUser(new coreSecurityEntities());
                    toBeSaved.postingComments = value.postingComments;
                }
                else// IF SHRINKAGE IS NOT APPROVED, THROW AN EXCEPTION
                {
                    throw new ApplicationException("Sorry, unapproved Balance cann't be posted");
                }
            }
            ctx.SaveChanges();
            le.SaveChanges();
            return toBeSaved;
        }


        private void populateFields(openningBalanceBatch toBeSaved, openningBalanceBatch value)
        {
            toBeSaved.balanceDate = value.balanceDate;
            toBeSaved.locationId = value.locationId;
            if (value.openningBalanceBatchId < 1)
            {
                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
                toBeSaved.created = DateTime.Now;
            }
            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            toBeSaved.modified = DateTime.Now;
            toBeSaved.enteredBy = LoginHelper.getCurrentUser(new coreSecurityEntities());
            toBeSaved.posted = false;
            toBeSaved.postedBy = "";
            toBeSaved.approved = false;
            toBeSaved.approvedBy = "";
        }

        //populate shrinkage the fields to be saved
        private void populateOpenningFields(openningBalance toBeSaved, openningBalance value)
        {
            toBeSaved.openningBalanceBatchId = value.openningBalanceBatchId;
            toBeSaved.inventoryItemId = value.inventoryItemId;
            toBeSaved.quantityOnHand = value.quantityOnHand;
            toBeSaved.productId = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == value.inventoryItemId).productId;
            toBeSaved.brandId = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == value.inventoryItemId).brandId;
            toBeSaved.unitOfMeasurementId = value.unitOfMeasurementId;
            toBeSaved.drAccountId = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == value.inventoryItemId).shrinkageAccountId;
            toBeSaved.crAccountId = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == value.inventoryItemId).accountId;
            toBeSaved.batchNumber = value.batchNumber;
            if (value.openningBalanceId < 1)
            {
                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
                toBeSaved.created = DateTime.Now;
            }
            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities()); ;
            toBeSaved.modified = DateTime.Now;
        }


        [HttpDelete]
        // DELETE: api/productCategory/5
        public void Delete([FromBody]openningBalanceBatch value)
        {
            var forDelete = le.openningBalanceBatches
                .Include(p => p.openningBalances)
                .FirstOrDefault(p => p.openningBalanceBatchId == value.openningBalanceBatchId);
            if (forDelete != null)
            {
                foreach (var openning in value.openningBalances)
                {
                    if (value.openningBalances.Any(p => p.openningBalanceId == openning.openningBalanceId))
                    {
                        forDelete.openningBalances.Remove(openning);
                    }
                }

                le.openningBalanceBatches.Remove(forDelete);
                le.SaveChanges();
            }
        }



        //valiadtions

        private bool ValidateOpenningBalanceBatch(openningBalanceBatch openningBatch)
        {
            //If Shrinkage Grid is empty, Catch the error and return false
            if (openningBatch.openningBalances.Any())
            {
                ValidateOpenningBalanceBatchLocationDropDown(openningBatch);
                ValidateOpenningBalanceBatchEmptyFields(openningBatch);
                ValidateOpenningBalanceBatchDate(openningBatch);

                //If errorMessage is empty test Pass
                if (errorMessage == "")
                {
                    return true;
                }
                return false;                
            }
            
            errorMessage += OPENNING_BALANCE_GRID_WITHOUT_DATA_ERROR_MESSAGE;
            return false;
        }


        private bool ValidateOpenningBalance(openningBalance openningBal)
        {
            ValidateOpenningBalanceDropDown(openningBal);
            ValidateOpenningBalanceEmptyFields(openningBal);
            ValidateOpenningBalanceQuantityOnHandSize(openningBal);
            if (errorMessage == "")
            {
                return true;
            }
            return false;
        }


        private void ValidateOpenningBalanceBatchLocationDropDown(openningBalanceBatch openningBatch)
        {
            if (locationExists(openningBatch.locationId) == false)
            {
                errorMessage += OPENNING_BALANCE_BATCH_DROP_DOWN_ERROR_MESSAGE;
            }
        }

        private void ValidateOpenningBalanceBatchEmptyFields(openningBalanceBatch openningBatch)
        {
            if ((openningBatch.balanceDate == DateTime.MinValue))
            {
                errorMessage += OPENNING_BALANCE_BATCH_EMPTY_FIELDS_ERROR_MESSAGE;
            }
        }

        private void ValidateOpenningBalanceBatchDate(openningBalanceBatch openningBatch)
        {
            if (openningBatch.balanceDate > DateTime.Today)
            {
                errorMessage += OPENNING_BALANCE_BATCH_BALANCE_DATE_ERROR_MESSAGE;
            }
        }

        private bool locationExists(int? locId)
        {
            if (le.locations.Any(p => p.locationId == locId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ValidateOpenningBalanceDropDown(openningBalance openningBal)
        {
            if ((inventoryItemExists(openningBal.inventoryItemId) == false)
                || (unitOfMeasurementExists(openningBal.unitOfMeasurementId) == false)
                )
            {
                errorMessage += OPENNING_BALANCE_DROP_DOWN_ERROR_MESSAGE;
            }
        }

        private bool inventoryItemExists(long? inventId)
        {
            if (le.inventoryItems.Any(p => p.inventoryItemId == inventId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool productExists(int? proId)
        {
            if (le.products.Any(p => p.productId == proId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool brandExists(int? branId)
        {
            if (le.brands.Any(p => p.brandId == branId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool unitOfMeasurementExists(int? unitId)
        {
            if (le.unitOfMeasurements.Any(p => p.unitOfMeasurementId == unitId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private bool drAccountExists(int? acctId)
        {
            if (ctx.accts.Any(p => p.acct_id == acctId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool crAccountExists(int? acctId)
        {
            if (ctx.accts.Any(p => p.acct_id == acctId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ValidateOpenningBalanceEmptyFields(openningBalance openningBal)
        {
            if ((openningBal.inventoryItemId <= 0) || (openningBal.unitOfMeasurementId <= 0))
            {
                errorMessage += OPENNING_BALANCE_EMPTY_FIELDS_ERROR_MESSAGE;
            }
        }

        private void ValidateOpenningBalanceQuantityOnHandSize(openningBalance openningBal)
        {
            if (openningBal.quantityOnHand <= 0)
            {
                errorMessage += OPENNING_BALANCE_QUANTITY_ON_HAND_SIZE_ERROR_MESSAGE;
            }
        }

        public string generateInvtTransBatchNumber()
        {
            string nextBatchNumber = !le.inventoryItemDetails.Any()
                ? "BHN-000001"
                : // if it's empty, start with BHN-000001
                "BHN-" +
                (int.Parse(
                    le.inventoryItemDetails.OrderByDescending(i => i.batchNumber) // order by code descending
                    .First() // get first one (last code)
                    .batchNumber.Split('-')[1]) // get only the number part
                + 1).ToString("000000"); // add 1 and format with 6 digits

            return nextBatchNumber;
        }


    }
}


