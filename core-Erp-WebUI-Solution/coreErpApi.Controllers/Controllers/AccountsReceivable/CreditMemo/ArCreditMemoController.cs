//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Web.Http;
//using coreERP;
//using coreLogic;
//using coreERP.Providers;
//using System.Linq.Dynamic;
//using coreLogic.Models.NumberGenerator;
//using coreErpApi.Controllers.Models;


//namespace coreErpApi.Controllers.Controllers.AccountsReceivable.CreditMemo

//{
//    [AuthorizationFilter()]
//    public class ArCreditMemoController : ApiController
//    {

//        private string errorMessage = "";
//        private string nextOrderNumber = "";
//        private int  lineNum = 0; 

//        //Declare a Database(Db) context variable 
//        ICommerceEntities le;
//        Icore_dbEntities ctx;


//        //call a constructor to instialize a the Dv context 
//        public ArCreditMemoController()
//        {
//            le = new CommerceEntities();
//            ctx = new core_dbEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//            ctx.Configuration.LazyLoadingEnabled = false;
//            ctx.Configuration.ProxyCreationEnabled = false;
//        }

//        //A constructor wiith a parameter
//        public ArCreditMemoController(ICommerceEntities lent, Icore_dbEntities ent)
//        {
//            le = lent;
//            ctx = ent;
//        }

//        // GET: api/Payment
//        public IEnumerable<creditMemo> Get()
//        {
//            return le.creditMemoes
//                .Include(p => p.creditMemoLines)
//                .OrderBy(p => p.memoDate)
//                .ToList();
//        }

//        [HttpGet]
//        public creditMemo Get(long id)
//        {
//            creditMemo value = le.creditMemoes
//                .Include(p => p.creditMemoLines)
//                .FirstOrDefault(p => p.creditMemoId == id);

//            if (value == null)
//            {
//                value = new creditMemo();
//            }
//            return value;
//        }

//        [HttpGet]
//        public creditMemo GetMemoByCustomer(long id)
//        {
//            creditMemo value = le.creditMemoes
//		.Where(p => p.customerId == id && p.totalBalanceLeft > 0)
//                .FirstOrDefault(p => p.creditMemoId == id);

//            if (value == null)
//            {
//                value = new creditMemo();
//            }
//            return value;
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
//        public creditMemo Post(creditMemo value)
//        {
//            creditMemo toBeSaved = null;

//            if (value.creditMemoId > 0)
//                {

//                    toBeSaved = le.creditMemoes
//                        .Include(p => p.creditMemoLines)
//                        .First(p => p.creditMemoId == value.creditMemoId);
//                    populateMemoFields(toBeSaved, value);

//                }
//                else //Else its a new record, and perform a POST
//                {

//                    toBeSaved = new creditMemo();
//                    populateMemoFields(toBeSaved, value);
//                    le.creditMemoes.Add(toBeSaved);
//                }

//            if (value.creditMemoLines.Any())
//                {
//                    //For the child table
//                    foreach (var memoLine in value.creditMemoLines)
//                    {

//                        creditMemoLine memoLineToBeSaved = null;

//                        //If arPaymentLineId is > 0 Its an update, and perform a PUT operation
//                        if (memoLine.creditMemoLineId > 0)
//                            {


//                                memoLineToBeSaved = toBeSaved.creditMemoLines
//                                    .First(p => p.creditMemoLineId == memoLine.creditMemoLineId);

//                                populateMemoLineFields(memoLineToBeSaved, memoLine);

//                            }
//                            //Else its a new record, and perform a POST
//                            else
//                            {
//                                memoLineToBeSaved = new creditMemoLine();
//                                populateMemoLineFields(memoLineToBeSaved, memoLine);
//                                toBeSaved.creditMemoLines.Add(memoLineToBeSaved);
//                            }
//                        //}
//                    }
//                }
               
//            //}



//            for (var i = toBeSaved.creditMemoLines.Count - 1; i >= 0; i--)
//                {
//                    var inDb = toBeSaved.creditMemoLines.ToList()[i];
//                    if (!value.creditMemoLines.Any(p => p.creditMemoLineId == inDb.creditMemoLineId))
//                    {
//                        le.creditMemoLines.Remove(inDb);
//                    }
//                }

//                    le.SaveChanges();                
//            //}
//            return toBeSaved;
//        }



//        //populate Memo the fields to be saved
//        private void populateMemoFields(creditMemo toBeSaved, creditMemo value)
//        {
//            NumberGenerator num = new NumberGenerator();
//            var hasContent = le.creditMemoes.Any();
//            var lastMemoNun = "";
//            if (hasContent)
//            {
//                lastMemoNun = le.creditMemoes.OrderByDescending(i => i.memoNumber).First().memoNumber;
//            }

//            if (value.arInvoiceId != null && value.arInvoiceId > 0)
//            { toBeSaved.arInvoiceId = value.arInvoiceId; }
//            if (value.arPaymentId != null && value.arPaymentId > 0)
//            { toBeSaved.arPaymentId = value.arPaymentId; }
//            toBeSaved.memoDate = value.memoDate;
//            toBeSaved.memoNumber = num.Generate("memoNumber", hasContent, "CRM", 8, lastMemoNun);
//            toBeSaved.totalAmountReturned = value.totalAmountReturned;
//            toBeSaved.totalBalanceLeft = value.totalBalanceLeft;
//            toBeSaved.approved = false;
//            toBeSaved.posted = false;
//            toBeSaved.creditMemoReasonId = value.creditMemoReasonId;
//            toBeSaved.currencyId = value.currencyId;
//            toBeSaved.totalAmountReturnedLocal = 0;
//            toBeSaved.buyRate = ctx.currencies.FirstOrDefault(p => p.currency_id == value.currencyId).current_buy_rate;
//            toBeSaved.sellRate = ctx.currencies.FirstOrDefault(p => p.currency_id == value.currencyId).current_sell_rate;
//            toBeSaved.posted = false;
//            if (value.creditMemoId < 1)
//            {
//                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                toBeSaved.created = DateTime.Now;
//            }
//            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities()); 
//            toBeSaved.modified = DateTime.Now;
//        }

//        //populate MemoLine the fields to be saved
//        private void populateMemoLineFields(creditMemoLine toBeSaved, creditMemoLine value)
//        {
            
//            if (value.arinvoiceLineId != null && value.arinvoiceLineId > 0) { toBeSaved.arinvoiceLineId = value.arinvoiceLineId; }

//            toBeSaved.amountReturned = value.amountReturned;
//            toBeSaved.quantityReturned = value.quantityReturned;
//            toBeSaved.balanceLeft = value.balanceLeft;
//            toBeSaved.balanceLeftLocal = 0;
//            toBeSaved.amountReturnedLocal = 0;
//            if (value.creditMemoLineId < 1)
//            {
//                toBeSaved.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                toBeSaved.created = DateTime.Now;
//            }
//            toBeSaved.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities()); ;
//            toBeSaved.modified = DateTime.Now;
//        }



//        [HttpDelete]
//        // DELETE: api/productCategory/5
//        public void Delete([FromBody]creditMemo value)
//        {
//            var forDelete = le.creditMemoes
//                .Include(p => p.creditMemoLines)
//                .FirstOrDefault(p => p.creditMemoId == value.creditMemoId);
//            if (forDelete != null)
//            {
//                le.creditMemoes.Remove(forDelete);
//                le.SaveChanges();
//            }
//        }

       


//    }
//}
