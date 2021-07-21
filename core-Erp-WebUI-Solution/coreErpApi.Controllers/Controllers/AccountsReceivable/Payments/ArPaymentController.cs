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
//using coreLogic.Models.ArCustomers;


//namespace coreErpApi.Controllers.Controllers.AccountsReceivable.Payments

//{
//    [AuthorizationFilter()]
//    public class ArPaymentController : ApiController
//    {

//        private string errorMessage = "";
//        private string nextOrderNumber = "";
//        private int  lineNum = 0; 

//        //Declare a Database(Db) context variable 
//        ICommerceEntities le;
//        Icore_dbEntities ctx;


//        //call a constructor to instialize a the Dv context 
//        public ArPaymentController()
//        {
//            le = new CommerceEntities();
//            ctx = new core_dbEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//            ctx.Configuration.LazyLoadingEnabled = false;
//            ctx.Configuration.ProxyCreationEnabled = false;
//        }

//        //A constructor wiith a parameter
//        public ArPaymentController(ICommerceEntities lent, Icore_dbEntities ent)
//        {
//            le = lent;
//            ctx = ent;
//        }

//        // GET: api/Payment
//        public IEnumerable<arPayment> Get()
//        {
//            return le.arPayments
//                .Include(p => p.arPaymentLines)
//                .OrderBy(p => p.paymentDate)
//                .ToList();
//        }

//        [HttpGet]
//        public arPayment Get(long id)
//        {
//            arPayment value = le.arPayments
//                .Include(p => p.arPaymentLines)
//                .FirstOrDefault(p => p.arPaymentId == id);

//            if (value == null)
//            {
//                value = new arPayment();
//            }
//            return value;
//        }



//        [HttpGet]
//        public IEnumerable<InvoiceCustomersViewModel> GetPaymentCustomers()
//        {
//            var payments = le.arPayments.ToList();
//            var customers = new List<InvoiceCustomersViewModel>();
            
//            foreach (var payment in payments)
//            {
//                var cust =le.customers
//                        .Select(p => new InvoiceCustomersViewModel
//                        {
//                            customerId = p.customerId,
//                            customerName = p.customerName,
//                        }).Distinct()
//                        .FirstOrDefault(p => p.customerId == payment.customerId);

//                customers.Add(cust);
//            }

//            return customers;
//        }



//        [HttpPost]
//        public KendoResponse Get([FromBody]KendoRequest req)
//        {
//            string order = "paymentDate";

//            KendoHelper.getSortOrder(req, ref order);
//            var parameters = new List<object>();
//            var whereClause = KendoHelper.getWhereClause<brand>(req, parameters);

//            var query = le.arPayments.AsQueryable();
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
//        public arPayment Post(arPayment value)
//        {
//            arPayment toBeSaved = null;
            
//            if (value.arPaymentId > 0)
//                {

//                    toBeSaved = le.arPayments
//                        .Include(p => p.arPaymentLines)
//                        .First(p => p.arPaymentId == value.arPaymentId);
//                    populateFields(toBeSaved, value);

//                }
//                else //Else its a new record, and perform a POST
//                {   

//                    toBeSaved = new arPayment();
//                    populateFields(toBeSaved, value);
//                    le.arPayments.Add(toBeSaved);
//                }

//            if (value.arPaymentLines.Count > 0)
//                {
//                    //For the child table
//                    foreach (var payLin in value.arPaymentLines)
//                    {
                        
//                        arPaymentLine payLinToBeSaved = null;
//                        arInvoice tobeUpdated = null;


//                        //If arPaymentLineId is > 0 Its an update, and perform a PUT operation
//                        if (payLin.arPaymentLineId > 0)
//                            {
//                                tobeUpdated = le.arInvoices.FirstOrDefault(p => p.arInvoiceId == payLin.arinvoiceId);
//                                tobeUpdated.balance = payLin.balanceLeft;

//                                payLinToBeSaved = toBeSaved.arPaymentLines
//                                    .First(p => p.arPaymentLineId == payLin.arPaymentLineId);

//                                populatePaymentLineFields(payLinToBeSaved, payLin);

//                            }
//                            //Else its a new record, and perform a POST
//                            else
//                            {
//                                tobeUpdated = le.arInvoices.FirstOrDefault(p => p.arInvoiceId == payLin.arinvoiceId);
//                                tobeUpdated.balance = payLin.balanceLeft;

//                                payLinToBeSaved = new arPaymentLine();
//                                populatePaymentLineFields(payLinToBeSaved, payLin);
//                                toBeSaved.arPaymentLines.Add(payLinToBeSaved);
//                            }
//                        //}
//                    }
//                }
               
//            //}



//            for (var i = toBeSaved.arPaymentLines.Count - 1; i >= 0; i--)
//                {
//                    var inDb = toBeSaved.arPaymentLines.ToList()[i];
//                    if (!value.arPaymentLines.Any(p => p.arPaymentLineId == inDb.arPaymentLineId))
//                    {
//                        le.arPaymentLines.Remove(inDb);
//                    }
//                }

//                    le.SaveChanges();                
//            //}
//            return toBeSaved;
//        }



//        //populate Payment the fields to be saved
//        private void populateFields(arPayment toBeSaved, arPayment value)
//        {
//            toBeSaved.customerId = value.customerId;
//            toBeSaved.paymentDate = value.paymentDate;
//            toBeSaved.totalAmountPaid = value.totalAmountPaid;
//	    toBeSaved.paymentCreditBalance = value.paymentCreditBalance;
//            toBeSaved.paymentMethodId = value.paymentMethodId;
//            toBeSaved.checkNumber = value.checkNumber;
//            toBeSaved.creditCardNumber = value.creditCardNumber;
//            toBeSaved.mobileMoneyNumber = value.mobileMoneyNumber;
//            toBeSaved.currencyId = value.currencyId;
//            toBeSaved.totalAmountPaidLocal = 0; 
//            toBeSaved.buyRate = 0;
//            toBeSaved.sellRate = 0;
//            toBeSaved.posted = false;
//            if (value.arPaymentId < 1)
//            {
//                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                toBeSaved.created = DateTime.Now;
//            }
//            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities()); 
//            toBeSaved.modified = DateTime.Now;
//        }

//        //populate PaymentLine the fields to be saved
//        private void populatePaymentLineFields(arPaymentLine toBeSaved, arPaymentLine value)
//        {
//            arInvoice invoice = le.arInvoices.FirstOrDefault(p => p.arInvoiceId == value.arinvoiceId);

//            toBeSaved.arinvoiceId = value.arinvoiceId;
//            toBeSaved.amountPaid = value.amountPaid;
//            toBeSaved.balanceLeft = value.balanceLeft;
//            toBeSaved.amountLPaidLocal = 0;
//            toBeSaved.balanceLeftLocal = 0;
//            toBeSaved.accountId = invoice.accountId; 
//            if (value.arPaymentLineId < 1)
//            {
//                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                toBeSaved.created = DateTime.Now;
//            }
//            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities()); ;
//            toBeSaved.modified = DateTime.Now;
//        }



//        [HttpDelete]
//        // DELETE: api/productCategory/5
//        public void Delete([FromBody]arPayment value)
//        {
//            var forDelete = le.arPayments
//                .Include(p => p.arPaymentLines)
//                .FirstOrDefault(p => p.arPaymentId == value.arPaymentId);
//            if (forDelete != null)
//            {
//                le.arPayments.Remove(forDelete);
//                le.SaveChanges();
//            }
//        }

       


//    }
//}
