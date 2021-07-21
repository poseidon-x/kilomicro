using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;

namespace coreErpApi.Controllers.Controllers.Inventory.ShrinkageManagement
{
    [AuthorizationFilter()]
    public class ShrinkageBatchController : ApiController
    {
        //Declaration of constant variables for error messages
        private const string SHRINKAGE_BATCH_DROP_DOWN_ERROR_MESSAGE = "Be sure you have selected from the provided Drop Down list in the form<br />";
        private const string SHRINKAGE_BATCH_SHRINKKAGE_DATE_ERROR_MESSAGE = "Shrinkage Date cannot be later than today<br />";
        private const string SHRINKAGE_BATCH_EMPTY_FIELDS_ERROR_MESSAGE = "Please make sure all the form fields are not Empty<br />";
        private const string SHRINKAGE_DROP_DOWN_ERROR_MESSAGE = "Be sure you have selected from the provided Drop Down list in the Details Grid<br />";
        private const string SHRINKAGE_EMPTY_FIELDS_ERROR_MESSAGE = "Please make sure all the Grid fields are not Empty<br />";
        private const string SHRINKAGE_QUANTITY_SHRUNK_SIZE_ERROR_MESSAGE = "Please make sure all quantity shrunk is a real number<br />";
        private const string SHRINKAGE_GRID_WITHOUT_DATA_ERROR_MESSAGE = "Shrinkage Details Grid cannot be empty<br />";

        private string errorMessage = "";
        //Declare a Database(Db) context variable 
        ICommerceEntities le;
        Icore_dbEntities ctx;


        //call a constructor to instialize a the Dv context 
        public ShrinkageBatchController()
        {
            le = new CommerceEntities();
            ctx = new core_dbEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
            ctx.Configuration.LazyLoadingEnabled = false;
            ctx.Configuration.ProxyCreationEnabled = false;
        }

        //A constructor wiith a parameter
        public ShrinkageBatchController(ICommerceEntities lent, Icore_dbEntities ent)
        {
            le = lent;
            ctx = ent;
        }

        // GET: api/shrinkageBatch
        public IEnumerable<shrinkageBatch> Get()
        {
            return le.shrinkageBatches
                .Include(p => p.shrinkages)
                .OrderBy(p => p.shrinkageBatchId)
                .ToList();
        }

        [HttpPost]
        public KendoResponse Get([FromBody]KendoRequest req)
        {
            string order = "shrinkageDate";

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<brand>(req, parameters);

            var query = le.shrinkageBatches.AsQueryable();
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

        // GET: api/shrinkageBatches/
        [HttpGet]
        public shrinkageBatch Get(int id)
        {
            shrinkageBatch value = le.shrinkageBatches
                .Include(p => p.shrinkages)
                .FirstOrDefault(p => p.shrinkageBatchId == id);

            if (value == null)
            {
                value = new shrinkageBatch();
            }
            return value;
        }





        [HttpPost]
        public shrinkageBatch Post(shrinkageBatch value)
        {
            shrinkageBatch toBeSaved = null;
            //If ShrinkageBatch validattion returns false, an exception is thrown
            if (ValidateshrinkageBatch(value) == false)
            {
                throw new ApplicationException(errorMessage);
            }

            //If shrinkage batch validate returns true, continue execution, Else throw an exception. 
            if (ValidateshrinkageBatch(value))
            {
                //If shrinkageBatchId is > 0 Its an update, and perform a PUT operation
                if (value.shrinkageBatchId > 0)
                {
                    //If the record has not been approved, then update
                    if (value.approved == false)
                    {
                        toBeSaved = le.shrinkageBatches
                                .Include(p => p.shrinkages)
                                .First(p => p.shrinkageBatchId == value.shrinkageBatchId);
                        populateFields(toBeSaved, value);
                    }//If record has been approved already then, send an error message.
                    else throw new ApplicationException("This record has already been approve & you cann't update the details.");//
                }
                else //Else its a new record, and perform a POST
                {
                    toBeSaved = new shrinkageBatch();
                    populateFields(toBeSaved, value);
                    le.shrinkageBatches.Add(toBeSaved);
                }
                
                //For the child table
                foreach (var shrink in value.shrinkages)
                {
                    //If Shrinkage validation returns false We set save to false and throw an exception
                    if (Validateshrinkage(shrink) == false)
                    {
                        throw new ApplicationException(errorMessage);
                    }
                    //If shrinkage validate returns true, continue execution, Else throw an exception. 
                    if (Validateshrinkage(shrink))
                    {
                        shrinkage shrinkToBeSaved = null;

                        //If shrinkageId is > 0 Its an update, and perform a PUT operation
                        if (shrink.shrinkageId > 0)
                        {
                             
                            shrinkToBeSaved = toBeSaved.shrinkages
                                .First(p => p.shrinkageId == shrink.shrinkageId);
                             
                            populateShrinkageFields(shrinkToBeSaved, shrink);
                             
                        }
                        //Else its a new record, and perform a POST
                        else  
                        {
                            shrinkToBeSaved = new shrinkage();
                            populateShrinkageFields(shrinkToBeSaved, shrink);
                            toBeSaved.shrinkages.Add(shrinkToBeSaved);
                        }
                    }
                }

                for (var i = toBeSaved.shrinkages.Count - 1; i >= 0; i--)
                {
                    var inDb = toBeSaved.shrinkages.ToList()[i];
                    if (!value.shrinkages.Any(p => p.shrinkageId == inDb.shrinkageId))
                    {
                        le.shrinkages.Remove(inDb);
                    }
                }

                    le.SaveChanges();                
            }
            return toBeSaved;
        }

        //UPDATE SHRINKAGE TO APPROVED 
        [HttpPost]
        public shrinkageBatch PostApproval(shrinkageBatch value)
        {
            shrinkageBatch toBeSaved = null;

            if (value.shrinkageBatchId > 0)
            {
                toBeSaved = le.shrinkageBatches
                    .First(p => p.shrinkageBatchId == value.shrinkageBatchId);
                toBeSaved.approved = true;
                toBeSaved.approvedBy = LoginHelper.getCurrentUser(new coreSecurityEntities());
                toBeSaved.approvalDate = DateTime.Now;
                toBeSaved.approvalComments = value.approvalComments;
            }

            le.SaveChanges();
            return toBeSaved;
        }

        //UPDATE SHRINKAGE TO POSTED 
        [HttpPost]
        public shrinkageBatch ShrinkagePost(shrinkageBatch value)
        {
            jnl_batch toBePosted;
            shrinkageBatch toBeSaved = null;
            inventoryItemDetail invtItmToBeSaved = null;


            if (value.shrinkageBatchId > 0)
            {
                //IF SHRINKAGE IS APPROVED, THEN POST
                if (value.approved)
                {
                    foreach (var shrink in value.shrinkages)
                    {
                        var shrinkId = le.shrinkages.FirstOrDefault(p => p.inventoryItemId == shrink.inventoryItemId).inventoryItemId;
                        var shrinkAcctId = (int)le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == shrinkId).shrinkageAccountId;
                        var acctId = (int)le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == shrinkId).accountId;
                        var invtItmDet = (int)le.inventoryItemDetails.FirstOrDefault(p => p.batchNumber == shrink.batchNumber).inventoryItemDetailId;
                        var unitPrice = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == shrinkId).unitPrice;
                        var descript = value.postingComments;
                        var curryId = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == shrinkId).currencyId;
                        var shrinkDate = value.shrinkageDate;
                        var refNum = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == shrinkId).itemNumber;
                        var user = LoginHelper.getCurrentUser(new coreSecurityEntities());
                        var amount = unitPrice * shrink.quantityShrunk;

                        toBePosted = (new JournalExtensions()).Post("IV", shrinkAcctId, acctId, amount, descript, curryId, shrinkDate,
                            refNum, ctx, user, null);
                        ctx.jnl_batch.Add(toBePosted);

                        invtItmToBeSaved = le.inventoryItemDetails
                                    .First(p => p.inventoryItemDetailId == invtItmDet);

                        invtItmToBeSaved.quantityOnHand = invtItmToBeSaved.quantityOnHand - shrink.quantityShrunk;
                    }

                    toBeSaved = le.shrinkageBatches
                        .First(p => p.shrinkageBatchId == value.shrinkageBatchId);
                    toBeSaved.posted = true;
                    toBeSaved.postedDate = DateTime.Now;
                    toBeSaved.postedBy = LoginHelper.getCurrentUser(new coreSecurityEntities()); ;
                    toBeSaved.postingComments = value.postingComments;
                }
                else// IF SHRINKAGE IS NOT APPROVED, THROW AN EXCEPTION
                {
                    throw new ApplicationException("Sorry, unapproved shrinkage cann't be posted");                    
                }
            }

            //SAVE CHANGES
            ctx.SaveChanges();
            le.SaveChanges();
            return toBeSaved;
        }


        //populate shrinkageBatch the fields to be saved
        private void populateFields(shrinkageBatch toBeSaved, shrinkageBatch value)
        {
            toBeSaved.shrinkageDate = value.shrinkageDate;
            toBeSaved.locationId = value.locationId;
            if (value.shrinkageBatchId < 1)
            {
                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
                toBeSaved.created = DateTime.Now;
            }
            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities()); 
            toBeSaved.modified = DateTime.Now;
            toBeSaved.approved = false;
            toBeSaved.approvedBy = LoginHelper.getCurrentUser(new coreSecurityEntities());
            toBeSaved.enteredBy = LoginHelper.getCurrentUser(new coreSecurityEntities());
            toBeSaved.posted = false;
            toBeSaved.postingComments = value.postingComments;
            toBeSaved.approvalComments = value.approvalComments;
            toBeSaved.approvalDate = value.approvalDate;
        }

        //populate shrinkage the fields to be saved
        private void populateShrinkageFields(shrinkage toBeSaved, shrinkage value)
        {
            //toBeSaved.shrinkageBatchId = value.shrinkageBatchId;
            toBeSaved.inventoryItemId = value.inventoryItemId;
            toBeSaved.batchNumber = value.batchNumber;
            toBeSaved.quantityShrunk = value.quantityShrunk;
            toBeSaved.unitOfMeasurementId = value.unitOfMeasurementId;
            if (value.shrinkageId < 1)
            {
                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
                toBeSaved.created = DateTime.Now;
            }
            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities()); 
            toBeSaved.modified = DateTime.Now;
            toBeSaved.shrinkageReasonId = value.shrinkageReasonId;
        }


        [HttpDelete]
        // DELETE: api/productCategory/5
        public void Delete([FromBody]shrinkageBatch value)
        {
            var forDelete = le.shrinkageBatches
                .Include(p => p.shrinkages)
                .FirstOrDefault(p => p.shrinkageBatchId == value.shrinkageBatchId);
            if (forDelete != null)
            {
                le.shrinkageBatches.Remove(forDelete);
                le.SaveChanges();
            }
        }

        //Validate Fields in Shrinkage batch Form
        private bool ValidateshrinkageBatch(shrinkageBatch shrinkBatch)
        {
            //If Shrinkage Grid is empty, Catch the error and return false
            if (shrinkBatch.shrinkages.Any())
            {
                //Execute the List validation tests
                ValidateShrinkageBatchLocationDropDown(shrinkBatch);
                ValidateShrinkageBatchEmptyFields(shrinkBatch);
                ValidateShrinkageBatchDate(shrinkBatch);

                //If errorMessage is empty test Pass
                if (errorMessage == "")
                    {
                        return true;
                    }else
                    {
                        return false;
                    }
             }
            else
            {
                errorMessage += SHRINKAGE_GRID_WITHOUT_DATA_ERROR_MESSAGE;
                return false;
            }
        }

        private bool Validateshrinkage(shrinkage shrinkage)
        {
            ValidateShrinkageDropDown(shrinkage);
            ValidateShrinkageEmptyFields(shrinkage);
            ValidateShrinkageQuantityShrunkSize(shrinkage);
            if (errorMessage == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //validate shrinkagebatch Location Drop Down to ensure User selected from the Drop down
        private void ValidateShrinkageBatchLocationDropDown(shrinkageBatch shrinkBatch)
        {
            if (locationExists(shrinkBatch.locationId) == false)
            {
                errorMessage += SHRINKAGE_BATCH_DROP_DOWN_ERROR_MESSAGE;
            }
       }

        //validate to ensure shrinkageBatch fields are not empty
        private void ValidateShrinkageBatchEmptyFields(shrinkageBatch shrinkBatch)
        {
            if ((shrinkBatch.shrinkageDate == DateTime.MinValue)
                || (shrinkBatch.locationId < 1))
            {
                errorMessage += SHRINKAGE_BATCH_EMPTY_FIELDS_ERROR_MESSAGE;
            }
        }

        //validate to ensure shrinkageBatch date is not a future date
        private void ValidateShrinkageBatchDate(shrinkageBatch shrinkBatch)
        {
            if (shrinkBatch.shrinkageDate > DateTime.Today)
            {
                errorMessage += SHRINKAGE_BATCH_SHRINKKAGE_DATE_ERROR_MESSAGE;
            }
        }

        //validate shrinkage inventoryItem Drop Down to ensure User selected from the Drop down
        private void ValidateShrinkageDropDown(shrinkage shrinkage)
        {
            if ((inventoryItemExists(shrinkage.inventoryItemId) == false)
                || (unitOfMeasurementExists(shrinkage.unitOfMeasurementId) == false))
            {
                errorMessage += SHRINKAGE_DROP_DOWN_ERROR_MESSAGE;
            }
        }

        //validate to ensure shrinkage fields are not empty
        private void ValidateShrinkageEmptyFields(shrinkage shrinkage)
        {
            if ((shrinkage.inventoryItemId <= 0) || (shrinkage.quantityShrunk <= 0) || (shrinkage.unitOfMeasurementId <= 0))
            {
                errorMessage += SHRINKAGE_EMPTY_FIELDS_ERROR_MESSAGE;
            }
        }

        //validate to ensure shrinkage shrinkageQuantityShrunk size is reasonable
        private void ValidateShrinkageQuantityShrunkSize(shrinkage shrinkage)
        {
            if (shrinkage.quantityShrunk < 1)
            {
                errorMessage += SHRINKAGE_QUANTITY_SHRUNK_SIZE_ERROR_MESSAGE;
            }
        }

        //Validate to ensure location selected has an Id
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

        //Validate to ensure inventoryItem selected has an Id
        private bool inventoryItemExists(long? invtId)
        {
            if (le.inventoryItems.Any(p => p.inventoryItemId == invtId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Validate to ensure unitOfMeasurement selected has an Id
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

    }
}
