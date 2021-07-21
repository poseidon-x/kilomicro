//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Web.Http;
//using coreERP;
//using coreLogic;
//using coreERP.Providers;
//using System.Linq.Dynamic;
//using System.Text;
//using System.Threading.Tasks;
//using coreData.Constants;
//using coreErpApi.Controllers.Models;
//using coreERP.Models;
//using DocumentFormat.OpenXml.Wordprocessing;

//namespace coreErpApi.Controllers.Controllers.Deposits
//{
//    public class InvestmentReceiptController : ApiController
//    {
//        IcoreLoansEntities le;
//        ErrorMessages error = new ErrorMessages();

//        public InvestmentReceiptController()
//        {
//            le = new coreLoansEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        public InvestmentReceiptController(IcoreLoansEntities lent)
//        {
//            le = lent; 
//        }

//        // GET: api/ Returns Clients with Investment Deposit
//        [HttpGet]
//        public IEnumerable<clientInvestmentReceipt> Get()
//        {
//            return le.clientInvestmentReceipts
//                .Include(p => p.clientInvestmentReceiptDetails.Where(i => !i.invested))
//                .ToList();
//        }

//        // GET: api/ Returns Clients with Investment Deposit
//        [HttpGet]
//        public clientInvestmentReceipt Get(int id)
//        {
//            //If id > 0 retrieve Investment with the id, else return new instance of InvestmentReceipt
//            clientInvestmentReceipt data;
//            if (id > 0)
//            {
//                data = le.clientInvestmentReceipts
//                    .Include(p => p.clientInvestmentReceiptDetails.Where(i => !i.invested))
//                    .FirstOrDefault(p => p.clientInvestmentReceiptId == id);
//            }
//            else
//            {
//                data = new clientInvestmentReceipt
//                {
//                    clientInvestmentReceiptDetails =  new List<clientInvestmentReceiptDetail>()               
//                };
//            }
//            return data;
//        }

//        // GET: api/ Returns Clients with Investment Deposit
//        [HttpPost]
//        public IEnumerable<clientInvestmentReceiptDetail> GetClientReceipts(int id)
//        {
//            //If id > 0 retrieve Investment with the client id, else return new instance of InvestmentReceipt
//            return le.clientInvestmentReceiptDetails
//                .Where(p => !p.invested && p.clientInvestmentReceipt.clientId == id)
//                .ToList();
            
//        }

//        // POST && PUT Together
//        [AuthorizationFilter()]
//        [HttpPost]
//        public clientInvestmentReceipt Post(clientInvestmentReceipt value)
//        {
//            if (value == null) return null;
//            //Validate the input value
//            //validateLoanGroup(value);
//            clientInvestmentReceipt toBeSaved;

//            if (value.clientInvestmentReceiptId > 0)
//            {
//                toBeSaved = le.clientInvestmentReceipts
//                    .Include(p => p.clientInvestmentReceiptDetails)
//                    .FirstOrDefault(p => p.clientInvestmentReceiptId == value.clientInvestmentReceiptId);
//                populateFields(toBeSaved, value);
//            }
//            else
//            {
//                toBeSaved = new clientInvestmentReceipt();
//                populateFields(toBeSaved, value);
//                le.clientInvestmentReceipts.Add(toBeSaved);
//            }
            
//            try
//            {  le.SaveChanges(); }
//            catch (Exception x)
//            { throw x; }
//            return toBeSaved;
//        }

//        private void populateFields(clientInvestmentReceipt target, clientInvestmentReceipt source)
//        {
//            validateReceipt(source);
//            target.clientId = source.clientId;
//            foreach (var det in source.clientInvestmentReceiptDetails.Where(p => !p.invested))
//            {
//                validateReceiptDetail(det);
//                if (det.clientInvestmentReceiptDetailId > 0)
//                {
//                    var detailToUpdate = target.clientInvestmentReceiptDetails
//                        .FirstOrDefault(p => p.clientInvestmentReceiptDetailId == det.clientInvestmentReceiptDetailId);
//                    populateReceiptDetail(detailToUpdate, det);
//                }
//                else
//                {
//                    clientInvestmentReceiptDetail detailToSave = new clientInvestmentReceiptDetail();
//                    populateReceiptDetail(detailToSave, det);
//                    target.clientInvestmentReceiptDetails.Add(detailToSave);
//                }
//            }

//        }

//        private void populateReceiptDetail(clientInvestmentReceiptDetail target, clientInvestmentReceiptDetail source)
//        {
//            target.amountReceived = source.amountReceived;
//            target.receivedBy = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            target.receiptDate = DateTime.Now;
//            target.invested = false;
//            target.paymentModeId = source.paymentModeId;
//            if (source.paymentModeId == 2)
//            {
//                target.bankId = source.bankId;
//                target.chequeNumber = source.chequeNumber;
//            }
//            if (source.clientInvestmentReceiptDetailId < 1) target.created = DateTime.Now;
//            else
//            {
//                target.modified = DateTime.Now;
//                target.modified = DateTime.Now;

//            }
//        }
    

//        private void validateReceipt(clientInvestmentReceipt receipt)
//        {// validation for allowance type attributes
//            if (!clientExist(receipt.clientId))
//            {
//                StringBuilder errors = new StringBuilder();
//                if (!clientExist(receipt.clientId))
//                    errors.Append(error.InvalidClient);
//                throw new ApplicationException(errors.ToString());               
//            }   
//        }

//        private void validateReceiptDetail(clientInvestmentReceiptDetail detail)
//        {// validation for allowance type attributes
//            if (detail.amountReceived < 0 || detail.receiptDate == DateTime.MinValue)
//            {
//                StringBuilder errors = new StringBuilder();
//                if (detail.amountReceived < 0)
//                    errors.Append(error.InvalidAmount);
//                if (detail.receiptDate == DateTime.MinValue)
//                    errors.Append(error.InvalidReceiptDate);
//                throw new ApplicationException(errors.ToString());
//            }
//        }

//        private bool clientExist(int id)
//        {//check if the allowancetypename already exists

//            if (le.clients.Any(p => p.clientID == id))
//            {
//                return true;
//            }
//            return false;

//        }
//    }
//}
