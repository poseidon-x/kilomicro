using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using coreLogic.Models.ArCustomers;
using coreLogic.Models.NumberGenerator;

namespace coreErpApi.Controllers.Controllers.AccountsReceivable.Invoice
{
    [AuthorizationFilter()]
    public class ArInvoiceController : ApiController
    {
        //Declaration of constant variables for error messages
        private const string SHRINKAGE_BATCH_DROP_DOWN_ERROR_MESSAGE = "Be sure you have selected from the provided Drop Down list in the form<br />";
        //private const string SHRINKAGE_BATCH_SHRINKKAGE_DATE_ERROR_MESSAGE = "Shrinkage Date cannot be later than today<br />";
        //private const string SHRINKAGE_BATCH_EMPTY_FIELDS_ERROR_MESSAGE = "Please make sure all the form fields are not Empty<br />";
        //private const string SHRINKAGE_DROP_DOWN_ERROR_MESSAGE = "Be sure you have selected from the provided Drop Down list in the Details Grid<br />";
        //private const string SHRINKAGE_EMPTY_FIELDS_ERROR_MESSAGE = "Please make sure all the Grid fields are not Empty<br />";
        //private const string SHRINKAGE_QUANTITY_SHRUNK_SIZE_ERROR_MESSAGE = "Please make sure all quantity shrunk is a real number<br />";
        //private const string SHRINKAGE_GRID_WITHOUT_DATA_ERROR_MESSAGE = "Shrinkage Details Grid cannot be empty<br />";

        private string errorMessage = "";
        private string nextInvoiceNumber;
        private int lineNum = 0;

        
        //Declare a Database(Db) context variable 
        ICommerceEntities le;
        Icore_dbEntities ctx;


        //call a constructor to instialize a the Dv context 
        public ArInvoiceController()
        {
            le = new CommerceEntities();
            ctx = new core_dbEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
            ctx.Configuration.LazyLoadingEnabled = false;
            ctx.Configuration.ProxyCreationEnabled = false;
        }

        //A constructor wiith a parameter
        public ArInvoiceController(ICommerceEntities lent, Icore_dbEntities ent)
        {
            le = lent;
            ctx = ent;
        }

        // GET: api/arInvoices
        [HttpGet]
        public async Task<IEnumerable<arInvoice>> Get()
        {
            return await le.arInvoices
                .Include(p => p.arInvoiceLines)
                .Where(p => (p.customerName.ToLower() != "cash customer" && p.balance > 0))
                .OrderBy(p => p.arInvoiceId)
                .ToListAsync();
        }

        [HttpGet]
        public async Task<IEnumerable<InvoiceDetailViewModel>> GetInvoiceByCustomer(long custId)
        {
            return await le.arInvoices
                .Where(p => (p.customerId == custId && p.balance > 0))
                .Select(p => new InvoiceDetailViewModel
                {
                    arInvoiceId = p.arInvoiceId,
                    invoiceNumber = p.invoiceNumber,
                    totalTotal = p.totalAmount,
                    balance = p.balance,
                    invoiceNumberNBalance = p.invoiceNumber + ",  Total: " + p.balance
                })
                .OrderBy(p => p.arInvoiceId)
                .ToListAsync();
        }

        [HttpGet]
        public async Task<IEnumerable<InvoiceCustomersViewModel>> GetInvoiceCustomers()
        {
            return await le.arInvoices
                .Include(p => p.customer)
                .Where(p => (p.customerName.ToLower() != "cash customer" && p.balance > 0))
                .Select(p => new InvoiceCustomersViewModel
                {
                    customerId = p.customerId,
                    customerName = p.customerName,
                })
                .Distinct()
                .OrderBy(p => p.customerName)
                .ToListAsync();

        }

        [HttpGet]
        public arInvoice GetInvoiceBySalesOrderId(long id)
        {
            arInvoice invoice = new arInvoice(); 
            
            var salesToInvoice = le.salesOrderlines
                .Where(p => p.salesOrderId == id)
                .ToList();
            foreach (var salesLin in salesToInvoice)
            {
                arInvoiceLine arInvoiceToReturn = new arInvoiceLine();
                populateInvoiceFields(arInvoiceToReturn, salesLin);
                invoice.arInvoiceLines.Add(arInvoiceToReturn);
            }

            return invoice;
        }

        //Get an Invoice with details
        [HttpGet]
        public arInvoice Get(int Id)
        {
            arInvoice value = le.arInvoices
                .Include(p => p.arInvoiceLines)
                .FirstOrDefault(p => p.arInvoiceId == Id);

            if (value == null)
            {
                value = new arInvoice();
            }
            return value;
        }

        [HttpPost]
        public KendoResponse Get([FromBody]KendoRequest req)
        {
            string order = "invoiceNumber";

            KendoHelper.getSortOrder(req, ref order);
            var parameters = new List<object>();
            var whereClause = KendoHelper.getWhereClause<arInvoice>(req, parameters);

            var query = le.arInvoices.AsQueryable();
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
        public arInvoice Post(arInvoice value)
        {
            arInvoice toBeSaved = null;

            arInvoice orderCheck = le.arInvoices
                .FirstOrDefault(p => p.salesOrderId == value.salesOrderId);
            if (orderCheck == null)
            {

                ////If ShrinkageBatch validattion returns false, an exception is thrown
                //if (ValidateshrinkageBatch(value) == false)
                //{
                //    throw new ApplicationException(errorMessage);
                //}

                ////If shrinkage batch validate returns true, continue execution, Else throw an exception. 
                //if (ValidateshrinkageBatch(value))
                //{
                //If shrinkageBatchId is > 0 Its an update, and perform a PUT operation
                if (value.arInvoiceId > 0)
                {
                    toBeSaved = le.arInvoices
                        .Include(p => p.arInvoiceLines)
                        .First(p => p.arInvoiceId == value.arInvoiceId);
                    populateFields(toBeSaved, value);
                }
                else //Else its a new record, and perform a POST
                {
                    toBeSaved = new arInvoice();
                    populateFields(toBeSaved, value);
                    le.arInvoices.Add(toBeSaved);
                }

                //For the child table
                if (value.arInvoiceLines.Any())
                {
                    foreach (var invoiceLin in value.arInvoiceLines)
                    {
                        lineNum++;

                        arInvoiceLine invoiceLinToBeSaved = null;

                        //If shrinkageId is > 0 Its an update, and perform a PUT operation
                        if (invoiceLin.arInvoiceLineId > 0)
                        {

                            invoiceLinToBeSaved = toBeSaved.arInvoiceLines
                                .First(p => p.arInvoiceLineId == invoiceLin.arInvoiceLineId);
                            populateArInvoiceLineFields(invoiceLinToBeSaved, invoiceLin);

                        }
                        //Else its a new record, and perform a POST
                        else
                        {
                            if (le.arInvoices.Any(p => p.salesOrderId == value.salesOrderId))
                            {
                                throw new ApplicationException("The Selected Sales Order Has been Invoiced Already");
                            }
                            else
                            {
                                invoiceLinToBeSaved = new arInvoiceLine();
                                populateArInvoiceLineFields(invoiceLinToBeSaved, invoiceLin);
                                toBeSaved.arInvoiceLines.Add(invoiceLinToBeSaved);
                            }
                        }
                    }
            }

            for (var i = toBeSaved.arInvoiceLines.Count - 1; i >= 0; i--)
            {
                var inDb = toBeSaved.arInvoiceLines.ToList()[i];
                if (!value.arInvoiceLines.Any(p => p.arInvoiceLineId == inDb.arInvoiceLineId))
                {
                    le.arInvoiceLines.Remove(inDb);
                }
            }

            if (toBeSaved.salesOrderId > 0)
            {
                salesOrder order = le.salesOrders
                .FirstOrDefault(p => p.salesOrderId == value.salesOrderId);

                order.isInvoiced = true;
                order.invoicedDate = DateTime.Now;
            }
            if (toBeSaved.jobCardId > 0)
            {
                jobCard card = le.jobCards
                .FirstOrDefault(p => p.jobCardId == value.jobCardId);

                card.invoiced = true;
                card.invoiceDate = DateTime.Now;
                card.invoicedBy = LoginHelper.getCurrentUser(new coreSecurityEntities());
            }
            le.SaveChanges();

        }
            return toBeSaved;
        }


        //populate shrinkageBatch the fields to be saved
        private void populateFields(arInvoice toBeSaved, arInvoice value)
        {


            toBeSaved.salesOrderId = value.salesOrderId;
            toBeSaved.jobCardId = value.jobCardId;
            toBeSaved.customerId = value.customerId;
            toBeSaved.invoiceNumber = generateInvoiceNumber();
            toBeSaved.customerName = value.customerName;
            toBeSaved.invoiceDate = value.invoiceDate;
            toBeSaved.totalAmount = value.totalAmount;
            toBeSaved.balance = value.totalAmount;
            toBeSaved.paid = false;
            toBeSaved.isVat = value.isVat;
            toBeSaved.isNHIL = value.isNHIL;
            toBeSaved.vatRate = value.vatRate;
            toBeSaved.nhilRate = value.nhilRate;
            toBeSaved.withRate = value.withRate;
            toBeSaved.isWith = value.isWith;
            toBeSaved.paymentTermId = value.paymentTermId;
            toBeSaved.invoiceStatusId = 1;
            toBeSaved.posted = false;
            toBeSaved.currencyId = value.currencyId;
            toBeSaved.balanceLocal = 0.00;
            toBeSaved.totalAmountLocal = 0.00;
            toBeSaved.buyRate = 0.00;
            toBeSaved.sellRate = 0.00;
            toBeSaved.accountId = le.customers.FirstOrDefault(p => p.customerId == value.customerId).glAccountId;
            if (value.arInvoiceId < 1)
            {
                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
                toBeSaved.created = DateTime.Now;
            }
            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities()); 
            toBeSaved.modified = DateTime.Now;

        }

        //populate Account Recieveable InvoiceLine the fields to be saved
        private void populateArInvoiceLineFields(arInvoiceLine toBeSaved, arInvoiceLine value)
        {
            toBeSaved.arInvoiceId = value.arInvoiceId;
            toBeSaved.inventoryItemId = value.inventoryItemId;
            toBeSaved.lineNumber = lineNum;
            toBeSaved.description = value.description;
            toBeSaved.unitPrice = value.unitPrice;
			toBeSaved.quantity = value.quantity;
            toBeSaved.unitOfMeasurementId = value.unitOfMeasurementId;
            toBeSaved.discountAmount = value.discountAmount;
            toBeSaved.discountPercentage = value.discountPercentage;
            toBeSaved.totalAmount = value.totalAmount;            
			toBeSaved.netAmount = value.netAmount;
            toBeSaved.isVat = value.isVat;
            toBeSaved.isNHIL = value.isNHIL;
            toBeSaved.isWith = value.isWith;
            toBeSaved.unitPriceLocal = value.unitPriceLocal;
            toBeSaved.totalAmountLocal = value.totalAmountLocal;
            toBeSaved.netAmountLocal = value.netAmountLocal;
            toBeSaved.accountId = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == value.inventoryItemId).accountId;;
            toBeSaved.discountAccountId = value.discountAccountId;
            toBeSaved.vatAccountId = value.vatAccountId;
            toBeSaved.nhilAccountId = value.nhilAccountId;
            toBeSaved.withAccountId = value.withAccountId;			
			if (value.arInvoiceLineId < 1)
            {
            toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities()); ;
            toBeSaved.created = DateTime.Now;			
			}
            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities()); ;
            toBeSaved.modified = DateTime.Now;
        }


        private void populateInvoiceFields(arInvoiceLine toBeSaved, salesOrderline value)
        {
            toBeSaved.inventoryItemId = value.inventoryItemId;
            toBeSaved.lineNumber = value.lineNumber;
            toBeSaved.description = value.description;
            toBeSaved.unitPrice = value.unitPrice;
			toBeSaved.quantity = value.quantity;
            toBeSaved.unitOfMeasurementId = value.unitOfMeasurementId;
            toBeSaved.discountAmount = value.discountAmount;
            toBeSaved.discountPercentage = value.discountPercentage;
            toBeSaved.totalAmount = value.totalAmount;            
			toBeSaved.netAmount = value.netAmount;
            toBeSaved.unitPriceLocal = value.unitPriceLocal;
            toBeSaved.totalAmountLocal = value.totalAmountLocal;
            toBeSaved.netAmountLocal = value.netAmountLocal;
            toBeSaved.accountId = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == value.inventoryItemId).accountId;;
            toBeSaved.discountAccountId = 0;
            toBeSaved.vatAccountId = 0;
            toBeSaved.nhilAccountId = 0;
            toBeSaved.withAccountId = 0;			
        }
		
		
        [HttpPost]
        public arInvoice InvoicePost(arInvoice value)
        {
            arInvoice toBeSaved = null;
            if (value.arInvoiceId > 0)
            {
				toBeSaved = le.arInvoices
                    .First(p => p.arInvoiceId == value.arInvoiceId);
                var custId = toBeSaved.customerId;

                foreach (var line in toBeSaved.arInvoiceLines)
                {
                    var lineId =
                        le.arInvoiceLines.FirstOrDefault(p => p.inventoryItemId == line.inventoryItemId).inventoryItemId;
                    var invoiceAcctId = le.customers.FirstOrDefault(p => p.customerId == custId).glAccountId;
                    var acctId = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == lineId).accountId;
                    var unitPrice = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == lineId).unitPrice;
                    var descript =
                        le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == lineId).inventoryItemName + "Invoice";
                    var curryId = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == lineId).currencyId;
                    var invoiceDate = toBeSaved.invoiceDate;
                    var refNum = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == lineId).itemNumber;
                    var user = LoginHelper.getCurrentUser(new coreSecurityEntities());
                    var amount = unitPrice*line.quantity;

                    jnl_batch toBePosted = (new JournalExtensions()).Post("IV", invoiceAcctId, acctId, amount, descript,
                        curryId, invoiceDate,
                        refNum, ctx, user, null);
                    ctx.jnl_batch.Add(toBePosted);
                }
                toBeSaved.posted = true;
                toBeSaved.postedDate = DateTime.Now;
            }

            //SAVE CHANGES
            le.SaveChanges();
            return toBeSaved;
        }

        [HttpPost]
        public IEnumerable<arInvoice> PostCashInvoice()
        {
            var cashCustomer = le.customers.FirstOrDefault(p => p.customerName == "CASH CUSTOMER").customerId;
            IEnumerable<arInvoice> invoicesToBeSaved = le.arInvoices.Where(p => (p.customerId == cashCustomer) && (p.posted == false));

            if (invoicesToBeSaved != null)
            {
                foreach (var invoice in invoicesToBeSaved)
                {
                    foreach (var line in invoice.arInvoiceLines)
                    {
                        var lineId = le.arInvoiceLines.FirstOrDefault(p => p.inventoryItemId == line.inventoryItemId).inventoryItemId;
                        var invoiceAcctId = le.customers.FirstOrDefault(p => p.customerId == cashCustomer).glAccountId;
                        var acctId = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == lineId).accountId;
                        var unitPrice = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == lineId).unitPrice;
                        var descript = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == lineId).inventoryItemName + "Invoice";
                        var curryId = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == lineId).currencyId;
                        var invoiceDate = invoice.invoiceDate;
                        var refNum = le.inventoryItems.FirstOrDefault(p => p.inventoryItemId == lineId).itemNumber;
                        var user = LoginHelper.getCurrentUser(new coreSecurityEntities());
                        var amount = unitPrice * line.quantity;

                        jnl_batch toBePosted = (new JournalExtensions()).Post("IV", invoiceAcctId, acctId, amount, descript, curryId, invoiceDate,
                            refNum, ctx, user, null);
                        ctx.jnl_batch.Add(toBePosted);
                    }
                    invoice.posted = true;
                    invoice.postedDate = DateTime.Now;
                }
            }
            //SAVE CHANGES
            ctx.SaveChanges();
            le.SaveChanges();
            return invoicesToBeSaved;
        }


        [HttpDelete]
        // DELETE: api/productCategory/5
        public void Delete([FromBody]arInvoice value)
        {
            var forDelete = le.arInvoices
                .Include(p => p.arInvoiceLines)
                .FirstOrDefault(p => p.arInvoiceId == value.arInvoiceId);
            if (forDelete != null)
            {
                le.arInvoices.Remove(forDelete);
                le.SaveChanges();
            }
        }

        private string generateInvoiceNumber()
        {
            nextInvoiceNumber = !le.arInvoices.Any()
                ? "INV0000000001"
                : // if it's empty, start with INV0000000001
                "INV" +
                (int.Parse(
                    le.arInvoices.OrderByDescending(i => i.invoiceNumber) // order by code descending
                    .First() // get first one (last code)
                    .invoiceNumber.Split('V')[1]) // get only the number part
                + 1).ToString("0000000000"); // add 1 and format with 6 digits

            return nextInvoiceNumber;
        }
    }
}
