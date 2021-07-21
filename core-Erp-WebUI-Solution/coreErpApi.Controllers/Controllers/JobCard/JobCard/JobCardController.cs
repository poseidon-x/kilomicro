//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Web.Http;
//using coreERP;
//using coreLogic;
//using coreERP.Providers;
//using System.Linq.Dynamic;
//using System.Threading.Tasks;
//using System.Windows.Forms.VisualStyles;

//namespace coreErpApi.Controllers.Controllers.JobCard.JobCard
//{
//    [AuthorizationFilter()]
//    public class JobCardController : ApiController
//    {
//        //Declaration of constant variables for error messages
//        private const string JOB_CARD_NULL_ERROR_MESSAGE = "Job Card cannot be null<br />";
//        private const string JOB_CARD_DROP_DOWN_ERROR_MESSAGE = "Invalid Drop Down value from the form<br />";
//        private const string JOB_CARD_DETAILS_GRID_WITHOUT_DATA_ERROR_MESSAGE = "One or more Grid(s) is/are empty<br />";
//        private const string JOB_CARD_EMPTY_FIELDS_ERROR_MESSAGE = "Please make sure all the form fields are not Empty<br />";
//        private const string JOB_ORDER_DATE_ERROR_MESSAGE = "Job Order Date cannot be a future date<br />";
//        private const string JOB_ORDER_STARTING_DATE_ERROR_MESSAGE = "Job Order Start Date cannot be a past date<br />";    
//        private const string JOB_ORDERALREADY_APPROVED_ERROR_MESSAGE_ = "This record has already been approve & you cann't update the details.<br />";
//        private const string MATERIALS_DETAILS_DROP_DOWN_ERROR_MESSAGE = "Invalid Drop Down value for Materials Grid<br />";
//        private const string MATERIALS_DETAILS_FIELDS_ERROR_MESSAGE = "Material Grid has a record with an empty field<br />";
//        private const string MATERIAL_DETAILS_DATA_LENGTH_ERROR_MESSAGE = "Material Grid has a record with a field's length out of range<br />";
//        private const string MATERIAL_DETAILS_DATA_NUMBERIC_FIELD_ERROR_MESSAGE = "Material Grid has a record with a Numeric field's length out of range";
//        private const string LABOUR_DETAILS_FIELDS_ERROR_MESSAGE = "Labour Grid has a record with an empty field<br />";
//        private const string LABOUR_DETAILS_DATETIME_ERROR_MESSAGE = "Labour Grid has a record with an Invalid Date<br />";
//        private const string LABOUR_DETAILS_STRING_DATA_RANGE_ERROR_MESSAGE = "Labour Grid has a record with a field's length out of range<br />";
//        private const string LABOUR_DETAILS_NUMERIC_DATA_RANGE_ERROR_MESSAGE = "Labour Grid has a record with a a Numeric field's length out of range<br />";
//        private const string LABOUR_DETAILS_DROP_DOWN_ERROR_MESSAGE = "Invalid Drop Down value for Labour Grid<br />";
//        private const string JOB_CARD_APPROVAL_ERROR_MESSAGE = "Job card already approved<br />";


        
//        private string errorMessage = "";
//        //Declare a Database(Db) context variable 
//        private ICommerceEntities le;
        
//        //call a constructor to instialize a the Dv context 
//        public JobCardController()
//        {
//            le = new CommerceEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        //A constructor wiith a parameter
//        public JobCardController(ICommerceEntities lent, Icore_dbEntities ent)
//        {
//            le = lent;
//        }

//        // GET: api/jobCard
//        public async Task<IEnumerable<jobCard>> Get()
//        {
//            return await le.jobCards
//                .Include(p => p.jobCardMaterialDetails)
//                .Include(p => p.jobCardLabourDetails)
//                .OrderBy(p => p.jobCardId)
//                .ToListAsync();            
//        }

//	    // GET: api/JobCard

//         [HttpGet]
//        public jobCard Get(int Id)
//        {
//            jobCard value = le.jobCards
//                .Include(p => p.jobCardMaterialDetails)
//                .Include(p => p.jobCardLabourDetails)
//                .FirstOrDefault(p => p.jobCardId == Id);



//            if (value == null)
//            {
//                value = new jobCard();
//            }
//            return value;
//        }

//        [HttpPost]
//        public KendoResponse Get([FromBody]KendoRequest req)
//        {
//            string order = "jobCardId";

//            KendoHelper.getSortOrder(req, ref order);
//            var parameters = new List<object>();
//            var whereClause = KendoHelper.getWhereClause<brand>(req, parameters);

//            var query = le.jobCards.AsQueryable();
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
//        public jobCard Post(jobCard value)
//        {
//            jobCard toBeSaved = null;
//            //If JobCard validattion returns false, an exception is thrown
//            if (ValidateJobCard(value) == false)
//            {
//                throw new ApplicationException(errorMessage);
//            }

//            //If JobCard validate returns true, continue execution
//            {
//                //If jobCardId is > 0 Its an update, and perform a PUT operation
//                if (value.jobCardId > 0)
//                {
//                    //If the record has not been approved, then update
//                    if (value.approved == false)
//                    {
//                        toBeSaved = le.jobCards
//                                .Include(p => p.jobCardMaterialDetails)
//								.Include(p => p.jobCardLabourDetails)
//                                .First(p => p.jobCardId == value.jobCardId);
//                        populateFields(toBeSaved, value);
//                    }
//                    //If record has been approved already then, throw exception.
//                    else throw new ApplicationException(JOB_ORDERALREADY_APPROVED_ERROR_MESSAGE_);
//                }
//                else //Else its a new record, and perform a POST
//                {
//                    toBeSaved = new jobCard();
//                    populateFields(toBeSaved, value);
//                    le.jobCards.Add(toBeSaved);
//                }
                
//                //For the child table
//                foreach (var materDetail in value.jobCardMaterialDetails)
//                {
//                    //If jobCardMaterialDetails validation fails throw an exception
//                    if (ValidateMaterialDetails(materDetail) == false)
//                    {
//                        throw new ApplicationException(errorMessage);
//                    }
//                    //If jobCardMaterialDetails validate returns true, continue execution, Else throw an exception. 
//                    if (ValidateMaterialDetails(materDetail))
//                    {
//                        jobCardMaterialDetail materDetailToBeSaved = null;

//                        //If shrinkageId is > 0 Its an update, and perform a PUT operation
//                        if (materDetail.jobCardMaterialDetailId > 0)
//                        {
                             
//                            materDetailToBeSaved = toBeSaved.jobCardMaterialDetails
//                                .First(p => p.jobCardMaterialDetailId == materDetail.jobCardMaterialDetailId);
                             
//                            populateMaterialDetailFields(materDetailToBeSaved, materDetail);
                             
//                        }
//                        //Else its a new record, and perform a POST
//                        else  
//                        {
//                            materDetailToBeSaved = new jobCardMaterialDetail();
//                            populateMaterialDetailFields(materDetailToBeSaved, materDetail);
//                            toBeSaved.jobCardMaterialDetails.Add(materDetailToBeSaved);
//                        }
//                    }
//                }
				
//				foreach (var labDetail in value.jobCardLabourDetails)
//                {
//                    //If jobCardLabourDetails validation returns false We set save to false and throw an exception
//                    if (ValidateLabourDetails(labDetail) == false)
//                    {
//                        throw new ApplicationException(errorMessage);
//                    }
//                    //If jobCardLabourDetails validate returns true, continue execution, Else throw an exception. 
//                    if (ValidateLabourDetails(labDetail))
//                    {
//                        jobCardLabourDetail labDetailToBeSaved = null;

//                        //If jobCardLabourDetailId is > 0 Its an update, and perform a PUT operation
//                        if (labDetail.jobCardLabourDetailId > 0)
//                        {
//                            labDetailToBeSaved = toBeSaved.jobCardLabourDetails
//                                .First(p => p.jobCardLabourDetailId == labDetail.jobCardLabourDetailId);
                             
//                            populateLabourDetailFields(labDetailToBeSaved, labDetail);
                             
//                        }
//                        //Else its a new record, and perform a POST
//                        else  
//                        {
//                            labDetailToBeSaved = new jobCardLabourDetail();
//                            populateLabourDetailFields(labDetailToBeSaved, labDetail);
//                            toBeSaved.jobCardLabourDetails.Add(labDetailToBeSaved);
//                        }
//                    }
//                }

//                for (var i = toBeSaved.jobCardLabourDetails.Count - 1; i >= 0; i--)
//                {
//                    var inDb = toBeSaved.jobCardLabourDetails.ToList()[i];
//                    if (!value.jobCardLabourDetails.Any(p => p.jobCardLabourDetailId == inDb.jobCardLabourDetailId))
//                    {
//                        le.jobCardLabourDetails.Remove(inDb);
//                    }
//                }

//                    le.SaveChanges();                
//            }
//            return toBeSaved;
//        }

//        [HttpPost]
//        public jobCard ApproveJobCard(jobCard value)
//        {
//            jobCard toBeSaved = null;

//            if (!value.approved)
//            {
//                throw new ApplicationException(JOB_CARD_APPROVAL_ERROR_MESSAGE);
//            }
//            if (!value.approved)
//            {

//                toBeSaved = le.jobCards
//                    .First(p => p.jobCardId == value.jobCardId);

//                toBeSaved.approved = true;
//                toBeSaved.approvelDate = DateTime.Now;
//                toBeSaved.approvedBy = LoginHelper.getCurrentUser(new coreSecurityEntities());

//                workOrder work = le.workOrders.FirstOrDefault(p => p.workOrderNumber == value.workOrderNumber);
//                work.status = true;
//                //SAVE CHANGES
//                le.SaveChanges();
//            }
//            return toBeSaved;
//        }

//        [HttpPost]
//        public jobCard SignJobCard(jobCard value)
//        {
//            jobCard toBeSaved = null;

//            if (value.fulfilled && !value.signed)
//            {
//                toBeSaved = le.jobCards
//                    .First(p => p.jobCardId == value.jobCardId);

//                toBeSaved.signed = true;
//                toBeSaved.signDate = DateTime.Now;
//                toBeSaved.signedBy = LoginHelper.getCurrentUser(new coreSecurityEntities());

//                //SAVE CHANGES
//                le.SaveChanges();
//            }

//            return toBeSaved;
//        }

//        [HttpPost]
//        public jobCard FulfillJobCard(jobCard value)
//        {
//            jobCard toBeSaved = null;

//            if (value.invoiced && !value.fulfilled)
//            {
//                toBeSaved = le.jobCards
//                    .First(p => p.jobCardId == value.jobCardId);

//                toBeSaved.fulfilled = true;
//                toBeSaved.fulfillmentDate = DateTime.Now;
//                toBeSaved.fulfilledBy = LoginHelper.getCurrentUser(new coreSecurityEntities());

//                //SAVE CHANGES
//                le.SaveChanges();
//            }

//            return toBeSaved;
//        }

//        //populate Job Card the fields to be saved
//        private void populateFields(jobCard toBeSaved, jobCard value)
//        {
//            toBeSaved.jobNumber = generateInvtTransJobNumber();
//            toBeSaved.jobDate = value.jobDate;
//            toBeSaved.orderStartingDate = value.orderStartingDate;
//            toBeSaved.customerId = value.customerId;
//            toBeSaved.workOrderNumber = value.workOrderNumber;
//            toBeSaved.standardMarkUpRate = value.standardMarkUpRate;
//            toBeSaved.standardHourlyBillingRate = value.standardHourlyBillingRate;
//            toBeSaved.invoiced = false;
//            toBeSaved.fulfilled = false;
//            toBeSaved.signed = false;
//            toBeSaved.approved = false;
//            toBeSaved.totalLabour = value.totalLabour;
//            toBeSaved.totalMaterial = value.totalMaterial;
//            toBeSaved.vat = 0;
//            toBeSaved.nhil = 0;
//            if (value.jobCardId < 1)
//            {
//                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                toBeSaved.created = DateTime.Now;
//            }
//            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            toBeSaved.modified = DateTime.Now;
//        }

//        //populate jobCardMaterialDetail the fields to be saved
//        private void populateMaterialDetailFields(jobCardMaterialDetail toBeSaved, jobCardMaterialDetail value)
//        {
//            toBeSaved.serialNumber = value.serialNumber;
//            toBeSaved.materialDescription = value.materialDescription;
//            toBeSaved.inventoryItemId = value.inventoryItemId;
//            toBeSaved.partNumber = value.partNumber;
//            toBeSaved.quantity = value.quantity;
//            toBeSaved.unitOfMeasurementId = value.unitOfMeasurementId;
//            toBeSaved.unitCost = value.unitCost;
//            toBeSaved.materialCost = value.materialCost;
//            toBeSaved.markup = value.markup;
//            toBeSaved.materialCharge = value.materialCharge;
//            if (value.jobCardMaterialDetailId < 1)
//            {
//                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                toBeSaved.created = DateTime.Now;
//            }
//            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            toBeSaved.modified = DateTime.Now;
//        }

//        //populate shrinkage the fields to be saved
//        private void populateLabourDetailFields(jobCardLabourDetail toBeSaved, jobCardLabourDetail value)
//        {
//            toBeSaved.labourDate = value.labourDate;
//            toBeSaved.productionLineDescription = value.productionLineDescription;
//            toBeSaved.starTime = value.starTime;
//            toBeSaved.endTime = value.endTime;
//            toBeSaved.activityCode = value.activityCode;
//            toBeSaved.totalHours = value.totalHours;
//            toBeSaved.billable = value.billable;
//            if (toBeSaved.billable)
//            {
//                toBeSaved.billableHours = value.billableHours;
//                toBeSaved.billingRate = value.billingRate;
//                toBeSaved.billingValue = value.billingValue;
//            }
//            if (value.jobCardLabourDetailId < 1)
//            {
//                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                toBeSaved.created = DateTime.Now;
//            }
//            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            toBeSaved.modified = DateTime.Now;
//        }


//        public string generateInvtTransJobNumber()
//        {
//            string nextJobNumber = !le.jobCards.Any()
//                ? "JB000000001"
//                : // if it's empty, start with BHN-000001
//                "JB" +
//                (int.Parse(
//                    le.jobCards.OrderByDescending(i => i.jobNumber) // order by code descending
//                    .First() // get first one (last code)
//                    .jobNumber.Split('B')[1]) // get only the number part
//                + 1).ToString("000000000"); // add 1 and format with 6 digits

//            return nextJobNumber;
//        }

//        [HttpDelete]
//        // DELETE: api/productCategory/5
//        public void Delete([FromBody]jobCard value)
//        {
//            var forDelete = le.jobCards
//                .Include(p => p.jobCardMaterialDetails)
//                .Include(p => p.jobCardLabourDetails)
//                .FirstOrDefault(p => p.jobCardId == value.jobCardId);
//            if (forDelete != null)
//            {
//                le.jobCards.Remove(forDelete);
//                le.SaveChanges();
//            }
//        }


//        //Validate Fields in Shrinkage batch Form
//        private bool ValidateJobCard(jobCard jc)
//        {
//            if (jc == null)
//            {
//                errorMessage += JOB_CARD_NULL_ERROR_MESSAGE;
//                throw new ApplicationException(errorMessage);
//            }
//            //If JobCard Grid is empty, Catch the error and return false
//            if (jc.jobCardLabourDetails.Any() && jc.jobCardMaterialDetails.Any())
//            {
//                //Execute the  other validation tests
//                ValidateJobCardDropDown(jc);
//                ValidateJobCardEmptyFields(jc);
//                ValidateJobCardDateFields(jc);

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
//                errorMessage += JOB_CARD_DETAILS_GRID_WITHOUT_DATA_ERROR_MESSAGE;
//                return false;
//            }
//        }

//        private bool ValidateMaterialDetails(jobCardMaterialDetail matDet)
//        {

//            //ValidateMaterialDetailDropDown(matDet);
//            ValidateMaterialDetailEmptyFields(matDet);
//            ValidateMaterialDetailDataLength(matDet);
//            if (errorMessage == "")
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
           
//        }

//        private bool ValidateLabourDetails(jobCardLabourDetail labDet)
//        {
//            ValidateLabourDetailDropDown(labDet);
//            ValidateLabourDetailEmptyFields(labDet);
//            ValidateLabourDetailDataLength(labDet);

//            if (errorMessage == "")
//            {
//                return true;
//            }
            
//            return false;
//        }

//        //validate Job Card Drop Down to ensure User selected from the Drop down
//        private void ValidateJobCardDropDown(jobCard jobCard)
//        {
//            if (CustomerExists(jobCard.customerId) == false || WorkOrderExists(jobCard.workOrderNumber) == false)
//            {
//                errorMessage += JOB_CARD_DROP_DOWN_ERROR_MESSAGE;
//            }
//        }

//        //validate to ensure JobCard fields are not empty
//        private void ValidateJobCardEmptyFields(jobCard jobCard)
//        {
//            if (jobCard.jobDate == DateTime.MinValue || jobCard.orderStartingDate == DateTime.MinValue
//                || jobCard.customerId < 1 || String.IsNullOrEmpty(jobCard.workOrderNumber) || jobCard.workOrderNumber.Trim().Length == 0
//                || jobCard.standardMarkUpRate < 1 || jobCard.standardHourlyBillingRate < 1 || jobCard.totalLabour < 0 
//                || jobCard.totalMaterial < 0 || jobCard.vat < 0 || jobCard.nhil < 0)
//            {
//                errorMessage += JOB_CARD_EMPTY_FIELDS_ERROR_MESSAGE;
//            }
//        }


//        //validate to ensure JobCard date is not a future date and starting Date is not a past date
//        private void ValidateJobCardDateFields(jobCard jobCard)
//        {
//            if (jobCard.jobDate > DateTime.Now.AddDays(1))
//            {
//                errorMessage += JOB_ORDER_DATE_ERROR_MESSAGE;
//            }
//            if (jobCard.orderStartingDate < DateTime.Today)
//            {
//                errorMessage += JOB_ORDER_STARTING_DATE_ERROR_MESSAGE;
//            }
//        }

//        //validate  Material Detail Drop Down to ensure User selected from the Drop down
//        private void ValidateMaterialDetailDropDown(jobCardMaterialDetail matDet)
//        {
//            if (InventoryItemExists(matDet.inventoryItemId) == false || UnitOfMeasurementExists(matDet.unitOfMeasurementId))
//            {
//                errorMessage += MATERIALS_DETAILS_DROP_DOWN_ERROR_MESSAGE;
//            }
//        }



//        //validate to ensure MaterialDetail fields are not empty
//        private void ValidateMaterialDetailEmptyFields(jobCardMaterialDetail matDet)
//        {
//            if (matDet.inventoryItemId <= 0 || String.IsNullOrEmpty(matDet.serialNumber) || matDet.serialNumber.Trim().Length == 0
//                || String.IsNullOrEmpty(matDet.materialDescription) || matDet.materialDescription.Trim().Length == 0
//                || String.IsNullOrEmpty(matDet.partNumber) || matDet.partNumber.Trim().Length == 0 || matDet.quantity < 1)
//            {
//                errorMessage += MATERIALS_DETAILS_FIELDS_ERROR_MESSAGE;
//            }
//        }

//        //validate to ensure MaterialDetail data length are with the size specified in the database
//        private void ValidateMaterialDetailDataLength(jobCardMaterialDetail matDet)
//        {
//            if (matDet.serialNumber.Length > 38 || matDet.materialDescription.Length > 48 || matDet.partNumber.Length > 48)
//            {
//                errorMessage += MATERIAL_DETAILS_DATA_LENGTH_ERROR_MESSAGE;
//            }
//            if (matDet.quantity < 0 || matDet.unitCost < 0 || matDet.materialCost < 0 || matDet.markup < 0
//                || matDet.materialCharge < 0)
//            {
//                errorMessage += MATERIAL_DETAILS_DATA_NUMBERIC_FIELD_ERROR_MESSAGE;
//            }
//        }

//        //validate to ensure LabourDetail fields are not empty
//        private void ValidateLabourDetailEmptyFields(jobCardLabourDetail labDet)
//        {
//            if (labDet.totalHours <= 1 || String.IsNullOrEmpty(labDet.productionLineDescription) || labDet.productionLineDescription.Trim().Length == 0
//                || String.IsNullOrEmpty(labDet.activityCode) || labDet.activityCode.Trim().Length == 0
//                || labDet.starTime == DateTime.MinValue || labDet.endTime == DateTime.MinValue || labDet.labourDate == DateTime.MinValue)
//            {
//                errorMessage += LABOUR_DETAILS_FIELDS_ERROR_MESSAGE;
//            }
//        }

//        private void ValidateLabourDetailDataLength(jobCardLabourDetail labDet)
//        {
//            DateTime value;
//            if (!DateTime.TryParse(labDet.labourDate.ToString(), out value) || !DateTime.TryParse(labDet.starTime.ToString(), out value) 
//                || !DateTime.TryParse(labDet.endTime.ToString(), out value) )
//            {
//                errorMessage += LABOUR_DETAILS_DATETIME_ERROR_MESSAGE;
//            }

//            if (labDet.productionLineDescription.Length > 99 || labDet.activityCode.Length > 9)
//            {
//                errorMessage += LABOUR_DETAILS_STRING_DATA_RANGE_ERROR_MESSAGE;
//            }

//            if (labDet.activityCode.Length > 9 || labDet.totalHours < 0 || labDet.billableHours < 0 
//                || labDet.billingRate < 0 || labDet.billingValue < 0)
//            {
//                errorMessage += LABOUR_DETAILS_NUMERIC_DATA_RANGE_ERROR_MESSAGE;
//            }
//        }

//        //validate Labour Detail Drop Down to ensure User selected from the Drop down
//        private void ValidateLabourDetailDropDown(jobCardLabourDetail labDet)
//        {
//            if (ActivityExists(labDet.activityCode) == false)
//            {
//                errorMessage += LABOUR_DETAILS_DROP_DOWN_ERROR_MESSAGE;
//            }
//        }

//        //Validate to ensure customer selected has an exist
//        private bool CustomerExists(long custId)
//        {
//            if (le.customers.Any(p => p.customerId == custId))
//            {
//                return true;
//            }
            
//            return false;
//        }

//        //Validate to ensure selected invetory Item exist
//        private bool InventoryItemExists(long invtItemId)
//        {
//            if (le.inventoryItems.Any(p => p.inventoryItemId == invtItemId))
//            {
//                return true;
//            }
            
//            return false;
//        }

//        //Validate to ensure selected WorkOrder exist
//        private bool WorkOrderExists(string workOrdNum)
//        {
//            if (le.workOrders.Any(p => p.workOrderNumber == workOrdNum))
//            {
//                return true;
//            }
            
//            return false;
//        }

//        //Validate to ensure selected unitOfMeasurements exist
//        private bool UnitOfMeasurementExists(int unitOfMeasmId)
//        {
//            if (le.unitOfMeasurements.Any(p => p.unitOfMeasurementId == unitOfMeasmId))
//            {
//                return true;
//            }
            
//            return false;
//        }

//        //Validate to ensure selected workOrder Activity exist
//        private bool ActivityExists(string actCode)
//        {
//            if (le.workOrderActivities.Any(p => p.activityCode == actCode))
//            {
//                return true;
//            }

//            return false;
//        }

//    }
//}


