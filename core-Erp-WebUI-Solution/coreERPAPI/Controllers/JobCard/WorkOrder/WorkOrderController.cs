using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace coreErpApi.Controllers.Controllers.JobCard.WorkOrder
{
    [AuthorizationFilter()]
    public class WorkOrderController : ApiController
    {
        //Declaration of constant variables for error messages
        private const string WORK_ORDER_GRID_WITHOUT_DATA_ERROR_MESSAGE = "Work Order Activities grid cannot be empty<br />";
        private const string WORK_ORDER_DROP_DOWN_ERROR_MESSAGE = "Invalid customer for work order<br />";
        private const string WORK_ORDER_EMPTY_DATE_ERROR_MESSAGE = "Invalid work order Date<br />";
        private const string WORK_ORDER_DATE_EALIER_ERROR_MESSAGE = "Work order Date cannot be a past Date<br />";
        private const string ACTIVITTY_SPECIALITY_DROP_DOWN_ERROR_MESSAGE = "Work order Activity has invalid speciality<br />";
        private const string ACTIVITY_INVALID_NUMERIC_DETAILS_FIELDS_ERROR_MESSAGE = "Work order Activity has invalid numeric value<br />";

        private string errorMessage = "";
        //Declare a Database(Db) context variable 
        private ICommerceEntities le;
        

        //call a constructor to instialize a the Dv context 
        public WorkOrderController()
        {
            le = new CommerceEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        //A constructor wiith a parameter
        public WorkOrderController(ICommerceEntities lent)
        {
            le = lent;
        }

        // GET: api/jobCard
        public async Task<IEnumerable<workOrder>> Get()
        {
            return await le.workOrders
                .Include(p => p.workOrderActivities)
                .OrderBy(p => p.workOrderId)
                .ToListAsync();
        }

	// GET: api/Batches/
        [HttpGet]
        public workOrder Get(int id)
        {
            workOrder value = le.workOrders
                .Include(p => p.workOrderActivities)
                .FirstOrDefault(p => p.workOrderId == id);

            if (value == null)
            {
                value = new workOrder();
            }
            return value;
        }

        [HttpPost]
        public KendoResponse Get([FromBody]KendoRequest req)
        {
            string order = "workOrderId";

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<brand>(req, parameters);

            var query = le.workOrders.AsQueryable();
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
        public workOrder Post(workOrder value)
        {
            workOrder toBeSaved = null;
            //If workOrder validattion returns false, an exception is thrown
            if (ValidateWorkOrder(value) == false)
            {
                throw new ApplicationException(errorMessage);
            }

            //If workOrder validate returns true, continue execution, Else throw an exception. 
            if (ValidateWorkOrder(value))
            {
            //If workOrderId is > 0 Its an update, and perform a PUT operation
            if (value.workOrderId > 0)
                {
                    toBeSaved = le.workOrders
                                .Include(p => p.workOrderActivities)
                                .First(p => p.workOrderId == value.workOrderId);
                        populateFields(toBeSaved, value);
                }
                else //Else its a new record, and perform a POST
                {
                    toBeSaved = new workOrder();
                    populateFields(toBeSaved, value);
                    le.workOrders.Add(toBeSaved);
                }
                
                //For the child table
                foreach (var orderAct in value.workOrderActivities)
                {
                    //If workOrderActivity validation returns false We set save to false and throw an exception
                    if (ValidateActivity(orderAct) == false)
                    {
                        throw new ApplicationException(errorMessage);
                    }
                    //If shrinkage validate returns true, continue execution, Else throw an exception. 
                    //if (Validateshrinkage(shrink))
                    //{
                    workOrderActivity orderActToBeSaved = null;

                        //If shrinkageId is > 0 Its an update, and perform a PUT operation
                    if (orderAct.workOrderActivityId > 0)
                        {

                            orderActToBeSaved = toBeSaved.workOrderActivities
                                .First(p => p.workOrderActivityId == orderAct.workOrderActivityId);

                            populateOrderActivityFields(orderActToBeSaved, orderAct);
                             
                        }
                        //Else its a new record, and perform a POST
                        else  
                        {
                            orderActToBeSaved = new workOrderActivity();
                            populateOrderActivityFields(orderActToBeSaved, orderAct);
                            toBeSaved.workOrderActivities.Add(orderActToBeSaved);
                        }
                    //}
                }


                for (var i = toBeSaved.workOrderActivities.Count - 1; i >= 0; i--)
                {
                    var inDb = toBeSaved.workOrderActivities.ToList()[i];
                    if (!value.workOrderActivities.Any(p => p.workOrderActivityId == inDb.workOrderActivityId))
                    {
                        le.workOrderActivities.Remove(inDb);
                    }
                }

                    le.SaveChanges();                
            }
            return toBeSaved;
        }

        //populate Work Order
        private void populateFields(workOrder toBeSaved, workOrder value)
        {
            toBeSaved.customerId = value.customerId;
            toBeSaved.workOrderNumber = generateWorkOrderNumber();
            toBeSaved.workOrderDate = value.workOrderDate;
            toBeSaved.status = false;
            if (value.workOrderId < 1)
            {
                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
                toBeSaved.created = DateTime.Now;
            }
            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
            toBeSaved.modified = DateTime.Now;
        }

        //populate workOrderActivity the fields to be saved
        private void populateOrderActivityFields(workOrderActivity toBeSaved, workOrderActivity value)
        {
            toBeSaved.activityCode = generateActivityCode();
            toBeSaved.specialityId = value.specialityId;
            toBeSaved.hourlyBillingRate = value.hourlyBillingRate;
            toBeSaved.hourlyCost = value.hourlyCost;
            toBeSaved.markup = value.markup;
            toBeSaved.billable = value.billable;
            if (value.workOrderActivityId < 1)
            {
                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
                toBeSaved.created = DateTime.Now;
            }
            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities()); ;
            toBeSaved.modified = DateTime.Now;
        }        

        public string generateWorkOrderNumber()
        {
            string nextWorkOrderNumber = !le.workOrders.Any()
                ? "WK000000001"
                : // if it's empty, start with WK000000001
                "WK" +
                (int.Parse(
                    le.workOrders.OrderByDescending(i => i.workOrderNumber) // order by code descending
                    .First() // get first one (last code)
                    .workOrderNumber.Split('K')[1]) // get only the number part
                + 1).ToString("000000000"); // add 1 and format with 6 digits

            return nextWorkOrderNumber;
        }

        public string generateActivityCode()
        {
            string nextActivityCode = !le.workOrderActivities.Any()
                ? "AT0000001"
                : // if it's empty, start with WK000000001
                "AT" +
                (int.Parse(
                    le.workOrderActivities.OrderByDescending(i => i.activityCode) // order by code descending
                    .First() // get first one (last code)
                    .activityCode.Split('T')[1]) // get only the number part
                + 1).ToString("0000000"); // add 1 and format with 6 digits

            return nextActivityCode;
        }

        [HttpDelete]
        // DELETE: api/productCategory/5
        public void Delete([FromBody]workOrder value)
        {
            var forDelete = le.workOrders
                .Include(p => p.workOrderActivities)
                .FirstOrDefault(p => p.workOrderId == value.workOrderId);
            if (forDelete != null)
            {
                le.workOrders.Remove(forDelete);
                le.SaveChanges();
            }
        }

        //Validate Fields in Work order Form
        private bool ValidateWorkOrder(workOrder wkOrd)
        {
            //If workOrderActivities Grid is empty, Catch the error and return false
            if (wkOrd.workOrderActivities.Any())
            {
                //Execute the List validation tests
                ValidateWorkOrderDropDown(wkOrd);
                ValidateWorkOrderEmptyFields(wkOrd);

                //If errorMessage is empty test Pass
                if (errorMessage == "")
                {
                    return true;
                }
                
                return false;
            }

            errorMessage += WORK_ORDER_GRID_WITHOUT_DATA_ERROR_MESSAGE;
            return false;
        }

        //validate Work order Drop Down to ensure User selected from the Drop down
        private void ValidateWorkOrderDropDown(workOrder workOrder)
        {
            if (customerExists(workOrder.customerId) == false || workOrder.customerId < 1)
            {
                errorMessage += WORK_ORDER_DROP_DOWN_ERROR_MESSAGE;
            }
        }

        //validate to ensure WOrk order fields are not empty
        private void ValidateWorkOrderEmptyFields(workOrder workOrder)
        {
            if ((workOrder.workOrderDate == DateTime.MinValue))
            {
                errorMessage += WORK_ORDER_EMPTY_DATE_ERROR_MESSAGE;
            }
            if (workOrder.workOrderDate < DateTime.Today.AddDays(-1))
            {
                errorMessage += WORK_ORDER_DATE_EALIER_ERROR_MESSAGE;
            }
        }

        private bool ValidateActivity(workOrderActivity wkOrdAct)
        {
            ValidateOrderActivityDropDown(wkOrdAct);
            ValidateOrderActivityEmptyFields(wkOrdAct);
            if (errorMessage == "")
            {
                return true;
            }
                
            return false;
        }

        //validate shrinkage inventoryItem Drop Down to ensure User selected from the Drop down
        private void ValidateOrderActivityDropDown(workOrderActivity wkOrdAct)
        {
            if ((specialityExist(wkOrdAct.specialityId) == false || wkOrdAct.specialityId < 1))
            {
                errorMessage += ACTIVITTY_SPECIALITY_DROP_DOWN_ERROR_MESSAGE;
            }
        }

        //validate to ensure shrinkage fields are not empty
        private void ValidateOrderActivityEmptyFields(workOrderActivity wkOrdAct)
        {
            if ((wkOrdAct.hourlyBillingRate < 0) || (wkOrdAct.hourlyCost < 0) || (wkOrdAct.markup < 0))
            {
                errorMessage += ACTIVITY_INVALID_NUMERIC_DETAILS_FIELDS_ERROR_MESSAGE;
            }
        }
        //Validate to ensure customer selected exist
        private bool customerExists(long custId)
        {
            if (le.customers.Any(p => p.customerId == custId))
            {
                return true;
            }
           
            return false;            
        }

        //Validate to ensure speciality selected exist
        private bool specialityExist(long specitId)
        {
            if (le.specialities.Any(p => p.specialityId == specitId))
            {
                return true;
            }
            return false;
        }

    }
}


