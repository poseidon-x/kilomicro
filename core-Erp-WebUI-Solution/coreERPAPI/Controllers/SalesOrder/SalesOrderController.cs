using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;


namespace coreErpApi.Controllers.Controllers.SalesOrder
{
    [AuthorizationFilter()]
    public class SalesOrderController : ApiController
    {
        ////Declaration of constant variables for error messages
        private const string SALES_ORDER_DROP_DOWN_ERROR_MESSAGE = "One or More form drop down field(s) have invalid value<br />";
        private const string SALES_ORDER_DATES_ERROR_MESSAGE = "One or More form date field(s) have invalid value/Empty<br />";
        private const string SALES_ORDER_INVALID_FIELDS_DATE_ERROR_MESSAGE = "One or More form date field(s) is later than today<br />";
        private const string SALES_ORDER_EMPTY_FIELDS_ERROR_MESSAGE = "One or More form date field(s) are Empty<br />";
        private const string SALES_ORDER_BILLING_DROP_DOWN_ERROR_MESSAGE = "Billing Details City doesn't exist<br />";
        private const string SALES_ORDER_BILLING_EMPTY_FIELDS_ERROR_MESSAGE = "One or More Billing Details fields are  Empty<br />";
        private const string SALES_ORDER_BILLING_FIELDS_LENGTH_SIZE_ERROR_MESSAGE = "A Billing Details field maximum characters has been exceeded <br />";
        private const string SALES_ORDER_SHIPPING_DROP_DOWN_ERROR_MESSAGE = "Shipping Details has invalid drop down value<br />";
        private const string SALES_ORDER_SHIPPING_EMPTY_FIELDS_ERROR_MESSAGE = "One or More Shipping Details fields are  Empty<br />";
        private const string SALES_ORDER_SHIPPING_FIELDS_LENGTH_SIZE_ERROR_MESSAGE = "A Shipping Details field maximum characters has been exceeded <br />";
        private const string SALES_ORDER_LINE_DROP_DOWN_ERROR_MESSAGE = "Order Line Details has invalid drop down value<br />";
        private const string SALES_ORDER_LINE_EMPTY_FIELDS_ERROR_MESSAGE = "One or More Order Line Details fields are  Empty<br />";
        private const string SALES_ORDER_LINE_FIELDS_LENGTH_SIZE_ERROR_MESSAGE = "A Order Line Details field maximum characters has been exceeded <br />";
        private const string SALES_ORDER_GRID_WITHOUT_DATA_ERROR_MESSAGE = "Sales Order Details Grid cannot be empty<br />";

        private string errorMessage = "";
        private string nextOrderNumber = "";
        private int  lineNum = 0; 

        //Declare a Database(Db) context variable 
        ICommerceEntities le;
        Icore_dbEntities ctx;


        //call a constructor to instialize a the Dv context 
        public SalesOrderController()
        {
            le = new CommerceEntities();
            ctx = new core_dbEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
            ctx.Configuration.LazyLoadingEnabled = false;
            ctx.Configuration.ProxyCreationEnabled = false;
        }

        //A constructor wiith a parameter
        public SalesOrderController(ICommerceEntities lent, Icore_dbEntities ent)
        {
            le = lent;
            ctx = ent;
        }

        // GET: api/salesOrder
        public IEnumerable<salesOrder> Get()
        {
            return le.salesOrders
                .Where(p => p.customerName.ToLower() != "cash customer")
                .Include(p => p.salesOrderBillings)
                .Include(p => p.salesOrderlines)
                .Include(p => p.salesOrderShippings)
                .OrderBy(p => p.orderNumber)
                .ToList();
        }

        public IEnumerable<salesOrder> GetCashCustomers()
        {
            return le.salesOrders
                .Where(p => (p.customerName == "CASH CUSTOMER") && (p.isInvoiced == false))
                .Include(p => p.salesOrderlines)
                .OrderBy(p => p.orderNumber)
                .ToList();
        }

        [HttpPost]
        public KendoResponse Get([FromBody]KendoRequest req)
        {
            string order = "salesDate";

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<brand>(req, parameters);

            var query = le.salesOrders.AsQueryable();
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
        public salesOrder Get(int id)
        {
            salesOrder value = le.salesOrders
                .Include(p => p.salesOrderBillings)
                .Include(p => p.salesOrderlines)
                .Include(p => p.salesOrderShippings)
                .FirstOrDefault(p => p.salesOrderId == id);

            if (value == null)
            {
                value = new salesOrder();
            }
            return value;
        }





        [HttpPost]
        public salesOrder Post(salesOrder value)
        {
            salesOrder toBeSaved = null;
            //If SalesOrde validattion returns false, an exception is thrown
            if (ValidateSalesOrderFields(value) == false)
            {
                throw new ApplicationException(errorMessage);
            }

            //If SalesOrde validate returns true, continue execution, Else throw an exception. 
            if (ValidateSalesOrderFields(value))
            {
                //If shrinkageBatchId is > 0 Its an update, and perform a PUT operation
                if (value.salesOrderId > 0)
                {

                    toBeSaved = le.salesOrders
                        .Include(p => p.salesOrderBillings)
                        .Include(p => p.salesOrderlines)
                        .Include(p => p.salesOrderShippings)
                        .First(p => p.salesOrderId == value.salesOrderId);
                    populateFields(toBeSaved, value);

                }
                else //Else its a new record, and perform a POST
                {
                    toBeSaved = new salesOrder();
                    populateFields(toBeSaved, value);
                    le.salesOrders.Add(toBeSaved);
                }

                if (value.salesOrderBillings.Count > 0)
                {
                    //For the child table
                    foreach (var salBill in value.salesOrderBillings)
                    {
                        //If salesOrderBillings validation returns false We set save to false and throw an exception
                        if (ValidateOrderBilling(salBill) == false)
                        {
                            throw new ApplicationException(errorMessage);
                        }
                        //If shrinkage validate returns true, continue execution, Else throw an exception. 
                        if (ValidateOrderBilling(salBill))
                        {
                            salesOrderBilling salBillToBeSaved = null;

                            //If shrinkageId is > 0 Its an update, and perform a PUT operation
                            if (salBill.salesOrderBillingId > 0)
                            {

                                salBillToBeSaved = toBeSaved.salesOrderBillings
                                    .First(p => p.salesOrderBillingId == salBill.salesOrderBillingId);

                                populateSalesOrderBillingFields(salBillToBeSaved, salBill);

                            }
                            //Else its a new record, and perform a POST
                            else
                            {
                                salBillToBeSaved = new salesOrderBilling();
                                populateSalesOrderBillingFields(salBillToBeSaved, salBill);
                                toBeSaved.salesOrderBillings.Add(salBillToBeSaved);
                            }
                        }
                    }
                }

                //For the child table
                foreach (var salLin in value.salesOrderlines)
                {
                    lineNum ++;
                    //If Shrinkage validation returns false We set save to false and throw an exception
                    if (ValidateOrderline(salLin) == false)
                    {
                        throw new ApplicationException(errorMessage);
                    }
                    //If shrinkage validate returns true, continue execution, Else throw an exception. 
                    if (ValidateOrderline(salLin))
                    {
                        salesOrderline salLinToBeSaved = null;

                        //If shrinkageId is > 0 Its an update, and perform a PUT operation
                        if (salLin.salesOrderLineId > 0)
                        {

                            salLinToBeSaved = toBeSaved.salesOrderlines
                                .First(p => p.salesOrderLineId == salLin.salesOrderLineId);

                            populateSalesOrderLineFields(salLinToBeSaved, salLin);

                        }
                        //Else its a new record, and perform a POST
                        else
                        {
                            salLinToBeSaved = new salesOrderline();
                            populateSalesOrderLineFields(salLinToBeSaved, salLin);
                            toBeSaved.salesOrderlines.Add(salLinToBeSaved);
                        }
                    }
                }

                if (value.salesOrderShippings.Count > 0)
                {
                    //For the child table
                    foreach (var salShipp in value.salesOrderShippings)
                    {
                        //If Shrinkage validation returns false We set save to false and throw an exception
                        if (ValidateShippingMethod(salShipp) == false)
                        {
                            throw new ApplicationException(errorMessage);
                        }
                        //If shrinkage validate returns true, continue execution, Else throw an exception. 
                        if (ValidateShippingMethod(salShipp))
                        {
                            salesOrderShipping salShippToBeSaved = null;

                            //If shrinkageId is > 0 Its an update, and perform a PUT operation
                            if (salShipp.salesOrderShippingId > 0)
                            {
                                salShippToBeSaved = toBeSaved.salesOrderShippings
                                    .First(p => p.salesOrderShippingId == salShipp.salesOrderShippingId);
                                populateSalesOrderShippingFields(salShippToBeSaved, salShipp);
                            }
                            //Else its a new record, and perform a POST
                            else
                            {
                                salShippToBeSaved = new salesOrderShipping();
                                populateSalesOrderShippingFields(salShippToBeSaved, salShipp);
                                toBeSaved.salesOrderShippings.Add(salShippToBeSaved);
                            }
                        }
                    }
                }
            }



            for (var i = toBeSaved.salesOrderBillings.Count - 1; i >= 0; i--)
                {
                    var inDb = toBeSaved.salesOrderBillings.ToList()[i];
                    if (!value.salesOrderBillings.Any(p => p.salesOrderBillingId == inDb.salesOrderBillingId))
                    {
                        le.salesOrderBillings.Remove(inDb);
                    }
                }

                for (var i = toBeSaved.salesOrderlines.Count - 1; i >= 0; i--)
                {
                    var inDb = toBeSaved.salesOrderlines.ToList()[i];
                    if (!value.salesOrderlines.Any(p => p.salesOrderLineId == inDb.salesOrderLineId))
                    {
                        le.salesOrderlines.Remove(inDb);
                    }
                }

                for (var i = toBeSaved.salesOrderShippings.Count - 1; i >= 0; i--)
                {
                    var inDb = toBeSaved.salesOrderShippings.ToList()[i];
                    if (!value.salesOrderShippings.Any(p => p.salesOrderShippingId == inDb.salesOrderShippingId))
                    {
                        le.salesOrderShippings.Remove(inDb);
                    }
                }

                    le.SaveChanges();                
            //}
            return toBeSaved;
        }

        private string generateOrderNumber()
        {
            nextOrderNumber = !le.salesOrders.Any()
                ? "ORD0000000001"
                : // if it's empty, start with ORD000001
                "ORD" +
                (int.Parse(
                    le.salesOrders.OrderByDescending(i => i.orderNumber) // order by code descending
                    .First() // get first one (last code)
                    .orderNumber.Split('D')[1]) // get only the number part
                + 1).ToString("0000000000"); // add 1 and format with 6 digits

            return nextOrderNumber;
        }


        //populate shrinkageBatch the fields to be saved
        private void populateFields(salesOrder toBeSaved, salesOrder value)
        {
            toBeSaved.customerId = value.customerId;
            toBeSaved.customerName = value.customerName;
            toBeSaved.orderNumber = generateOrderNumber();
            toBeSaved.salesDate = value.salesDate;
            toBeSaved.totalAmount = value.totalAmount;
            toBeSaved.balance = 0.00;
            toBeSaved.requiredDate = value.requiredDate;
            toBeSaved.shippedDate = value.shippedDate;
            toBeSaved.locationId = le.customers.FirstOrDefault(p => p.customerId == value.customerId).locationId;
            toBeSaved.salesTypeId = value.salesTypeId;
            toBeSaved.currencyId = value.currencyId;
            toBeSaved.buyRate = ctx.currencies.FirstOrDefault(p => p.currency_id == value.currencyId).current_buy_rate;
            toBeSaved.sellRate = ctx.currencies.FirstOrDefault(p => p.currency_id == value.currencyId).current_sell_rate;
            toBeSaved.totalAmountLocal = 0.00;
            toBeSaved.balanceLocal = 0.00;
            toBeSaved.accountId = le.customers.FirstOrDefault(p => p.customerId == value.customerId).glAccountId;
            toBeSaved.paymentTermId = value.paymentTermId;
            if (value.salesOrderId < 1)
            {
                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
                toBeSaved.created = DateTime.Now;
            }
            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities()); 
            toBeSaved.modified = DateTime.Now;
        }

        //populate shrinkage the fields to be saved
        private void populateSalesOrderBillingFields(salesOrderBilling toBeSaved, salesOrderBilling value)
        {
            toBeSaved.salesOrderId = value.salesOrderId;
            toBeSaved.billTo = value.billTo;
            toBeSaved.addressLine1 = value.addressLine1;
            toBeSaved.addressLine2 = (String.IsNullOrEmpty(value.addressLine2)) ? "" : value.addressLine2;
            toBeSaved.cityName = value.cityName;
            if (value.salesOrderBillingId < 1)
            {
                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
                toBeSaved.created = DateTime.Now;
            }
            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities()); ;
            toBeSaved.modified = DateTime.Now;
        }

        private void populateSalesOrderLineFields(salesOrderline toBeSaved, salesOrderline value)
        {
            toBeSaved.salesOrderId = value.salesOrderId;
            toBeSaved.inventoryItemId = value.inventoryItemId;
            toBeSaved.lineNumber = value.lineNumber;
            toBeSaved.description = value.description;
            toBeSaved.accountId = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == value.inventoryItemId).accountId;
            toBeSaved.unitPrice = value.unitPrice;
            toBeSaved.lineNumber = lineNum;
            toBeSaved.quantity = value.quantity; 
            toBeSaved.unitOfMeasurementId = value.unitOfMeasurementId;
            toBeSaved.discountAmount = value.discountAmount;
            toBeSaved.discountPercentage = value.discountPercentage;
            toBeSaved.totalAmount = value.totalAmount;
            toBeSaved.netAmount = value.netAmount;
            toBeSaved.unitPriceLocal = 0.00;
            toBeSaved.totalAmountLocal = 0.00;
            toBeSaved.netAmountLocal = 0.00;
            toBeSaved.discountAmountLocal =  0.00;
            if (value.salesOrderLineId < 1)
            {
                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
                toBeSaved.created = DateTime.Now;
            }
            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities()); ;
            toBeSaved.modified = DateTime.Now;
        }

        private void populateSalesOrderShippingFields(salesOrderShipping toBeSaved, salesOrderShipping value)
        {
            toBeSaved.salesOrderId = value.salesOrderId;
            toBeSaved.shippingMethodId = value.shippingMethodId;
            toBeSaved.shipTo = value.shipTo;
            toBeSaved.addressLine1 = value.addressLine1;
            toBeSaved.addressLine2 = (String.IsNullOrEmpty(value.addressLine2)) ? "" : value.addressLine2;
            toBeSaved.cityName = value.cityName;
            toBeSaved.countryName = value.countryName;
            if (value.salesOrderShippingId < 1)
            {
                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
                toBeSaved.created = DateTime.Now;
            }
            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities()); ;
            toBeSaved.modified = DateTime.Now;
        }

        


        [HttpDelete]
        // DELETE: api/productCategory/5
        public void Delete([FromBody]salesOrder value)
        {
            var forDelete = le.salesOrders
                .Include(p => p.salesOrderBillings)
                .Include(p => p.salesOrderlines)
                .Include(p => p.salesOrderShippings)
                .FirstOrDefault(p => p.salesOrderId == value.salesOrderId);
            if (forDelete != null)
            {
                le.salesOrders.Remove(forDelete);
                le.SaveChanges();
            }
        }

        //Validate Fields in Sales Order Form
        private bool ValidateSalesOrderFields(salesOrder salOrd)
        {
            //If Sales Order Grid is empty, Catch the error and return false
            if //(salOrd.salesOrderBillings.Any() && salOrd.salesOrderShippings.Any()
                (salOrd.salesOrderlines.Any())
            {
                ValidateSalesOrderDropDowns(salOrd);
                ValidateSalesOrderEmptyDate(salOrd);
                ValidateSalesOrderEmptyFields(salOrd);
                //ValidateSalesOrderInavlidDateRangeFields(salOrd);

                //If errorMessage is empty test Pass
                if (errorMessage == "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                errorMessage += SALES_ORDER_GRID_WITHOUT_DATA_ERROR_MESSAGE;
                return false;
            }
        }


        private bool ValidateOrderBilling(salesOrderBilling salBill)
        {
            
                ValidateOrderBillingDropDown(salBill);
                ValidateOrderBillingEmptyFields(salBill);
                ValidateOrderBillingFieldLength(salBill);
                if (errorMessage == "")
                {
                    return true;
                }
                else
                {
                    return false;
                }
        }

        private bool ValidateShippingMethod(salesOrderShipping salShipp)
        {
            ValidateOrderShippingDropDown(salShipp);
            ValidateOrderShippingEmptyFields(salShipp);
            ValidateOrderShippingFieldLength(salShipp);
            if (errorMessage == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool ValidateOrderline(salesOrderline salLin)
        {
            ValidateOrderlineDropDown(salLin);
            ValidateOrderlineEmptyFields(salLin);
            ValidateOrderlineFieldLength(salLin);
            if (errorMessage == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        

        //validate SalesOrder Drop Down to ensure User selected from the Drop down

        private void ValidateSalesOrderDropDowns(salesOrder salOrd)
        {
            if (customerExists(salOrd.customerId) == false || currencyExists(salOrd.currencyId) == false
                || paymentTermExists(salOrd.paymentTermId) == false || salesExists(salOrd.salesTypeId) == false)
            {
                errorMessage += SALES_ORDER_DROP_DOWN_ERROR_MESSAGE;
            }
        }

        private void ValidateSalesOrderEmptyDate(salesOrder salOrd)
        {
            if (salOrd.salesDate == DateTime.MinValue || salOrd.requiredDate == DateTime.MinValue
                || salOrd.shippedDate == DateTime.MinValue)
            {
                errorMessage += SALES_ORDER_DATES_ERROR_MESSAGE;
            }
        }

        private void ValidateSalesOrderEmptyFields(salesOrder salOrd)
        {
            if (salOrd.customerId < 1 || String.IsNullOrEmpty(salOrd.customerName) || salOrd.salesTypeId < 1
                || salOrd.currencyId < 1 || salOrd.paymentTermId < 1)
            {
                errorMessage += SALES_ORDER_EMPTY_FIELDS_ERROR_MESSAGE;
            }
        }

        private void ValidateSalesOrderInavlidDateRangeFields(salesOrder salOrd)
        {
            if (salOrd.salesDate > DateTime.Now.Date || salOrd.requiredDate < DateTime.Now.Date
                || salOrd.shippedDate < DateTime.Now.Date)
            {
                errorMessage += SALES_ORDER_INVALID_FIELDS_DATE_ERROR_MESSAGE;
            }
        }

        //validate to ensure shrinkageBatch fields are not empty
        private void ValidateOrderBillingEmptyFields(salesOrderBilling salBill)
        {
            if (String.IsNullOrEmpty(salBill.billTo) || String.IsNullOrEmpty(salBill.addressLine1)
                || String.IsNullOrEmpty(salBill.cityName))
            {
                errorMessage += SALES_ORDER_BILLING_EMPTY_FIELDS_ERROR_MESSAGE;
            }
        }

        private void ValidateOrderBillingDropDown(salesOrderBilling salBill)
        {
            if (cityExists(salBill.cityName) == false)
            {
                errorMessage += SALES_ORDER_BILLING_DROP_DOWN_ERROR_MESSAGE;
            }
        }

        private void ValidateOrderBillingFieldLength(salesOrderBilling salBill)
        {
            if (salBill.billTo.Length > 399 || salBill.addressLine1.Length > 399
                || salBill.cityName.Length > 99)
            {
                errorMessage += SALES_ORDER_BILLING_FIELDS_LENGTH_SIZE_ERROR_MESSAGE;
            }
        }

        private void ValidateOrderShippingDropDown(salesOrderShipping salShipp)
        {
            if (shippingMethodExists(salShipp.shippingMethodId) == false || cityExists(salShipp.cityName) == false
                || countryExists(salShipp.countryName) == false)
            {
                errorMessage += SALES_ORDER_SHIPPING_DROP_DOWN_ERROR_MESSAGE;
            }
        }
        
        private void ValidateOrderShippingEmptyFields(salesOrderShipping salShipp)
        {
            if (salShipp.shippingMethodId < 1 || String.IsNullOrEmpty(salShipp.shipTo) 
                || String.IsNullOrEmpty(salShipp.addressLine1) || String.IsNullOrEmpty(salShipp.cityName) 
                || String.IsNullOrEmpty(salShipp.countryName))
            {
                errorMessage += SALES_ORDER_SHIPPING_EMPTY_FIELDS_ERROR_MESSAGE;
            }
        }

        private void ValidateOrderShippingFieldLength(salesOrderShipping salShipp)
        {
            if (salShipp.shipTo.Length > 399 || salShipp.addressLine1.Length > 399
                || salShipp.cityName.Length > 99 || salShipp.countryName.Length > 99)
            {
                errorMessage += SALES_ORDER_SHIPPING_FIELDS_LENGTH_SIZE_ERROR_MESSAGE;
            }
        }   
     
        private void ValidateOrderlineDropDown(salesOrderline salLin)
        {
            if (inventoryItemExists(salLin.inventoryItemId) == false || unitOfMeasuremenExists(salLin.unitOfMeasurementId) == false)
            {
                errorMessage += SALES_ORDER_LINE_DROP_DOWN_ERROR_MESSAGE;
            }
        }

        private void ValidateOrderlineEmptyFields(salesOrderline salLin)
        {
            if ( salLin.inventoryItemId < 1 ||  String.IsNullOrEmpty(salLin.description) ||
                salLin.unitPrice < 1 || salLin.quantity < 1 || salLin.unitOfMeasurementId < 1 || salLin.totalAmount < 1 )
            {
                errorMessage += SALES_ORDER_LINE_EMPTY_FIELDS_ERROR_MESSAGE;
            }
        }

        private void ValidateOrderlineFieldLength(salesOrderline salLin)
        {
            if (salLin.description.Length > 399 )
            {
                errorMessage += SALES_ORDER_LINE_FIELDS_LENGTH_SIZE_ERROR_MESSAGE;
            }
        }
        


        private bool customerExists(long? cusId)
        {
            if (le.customers.Any(p => p.customerId == cusId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool currencyExists(long? currId)
        {
            if (ctx.currencies.Any(p => p.currency_id == currId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool paymentTermExists(long? payTerId)
        {
            if (le.paymentTerms.Any(p => p.paymentTermID == payTerId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool salesExists(long? saTypId)
        {
            if (le.salesTypes.Any(p => p.salesTypeID == saTypId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool cityExists(string cityName)
        {
            if (ctx.cities.Any(p => p.city_name == cityName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool countryExists(string countryName)
        {
            if (ctx.countries.Any(p => p.country_name == countryName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool shippingMethodExists(long? shipMethId)
        {
            if (le.shippingMethods.Any(p => p.shippingMethodID == shipMethId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool inventoryItemExists(long? invtItemId)
        {
            if (le.inventoryItems.Any(p => p.inventoryItemId == invtItemId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool unitOfMeasuremenExists(long? unitOfMeasId)
        {
            if (le.unitOfMeasurements.Any(p => p.unitOfMeasurementId == unitOfMeasId))
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
