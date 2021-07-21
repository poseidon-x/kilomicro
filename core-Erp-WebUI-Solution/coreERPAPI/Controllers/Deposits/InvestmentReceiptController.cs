using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using coreData.Constants;
using coreErpApi.Controllers.Models;
using coreERP.Models;
using System.Web.Http.Cors;


namespace coreErpApi.Controllers.Controllers.Deposits
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class InvestmentReceiptController : ApiController
    {
        IcoreLoansEntities le;
        ErrorMessages error = new ErrorMessages();

        public InvestmentReceiptController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public InvestmentReceiptController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        // GET: api/ Returns Clients with Investment Deposit
        [HttpGet]
        public IEnumerable<clientInvestmentReceipt> Get()
        {
            List<clientInvestmentReceipt> data = le.clientInvestmentReceipts
                .Include(p => p.clientInvestmentReceiptDetails)
                .ToList();
            List<clientInvestmentReceipt> dataList = new List<clientInvestmentReceipt>();
            if(data.Count > 0)
            foreach (var record in data)
            {
                 List<clientInvestmentReceiptDetail> dataDetail = le.clientInvestmentReceiptDetails
                .Where(p => p.clientInvestmentReceiptId == record.clientInvestmentReceiptId && !p.invested)
                .ToList();
                
                if (dataDetail.Count > 0)
                {
                    record.clientInvestmentReceiptDetails = dataDetail;
                    dataList.Add(record);
                }
            }

            return dataList;
        }

        // GET: api/ Returns Clients with Investment Deposit
        [HttpGet]
        public IEnumerable<ClientInvestmentReceiptDetailModel> GetUnAppliedChecks()
        {
            var data = le.clientInvestmentReceiptDetails
                .Where(p => !p.invested && p.paymentModeId == 2)
                .Select(p => new ClientInvestmentReceiptDetailModel
                {
                    clientInvestmentReceiptDetailId = p.clientInvestmentReceiptDetailId,
                    clientInvestmentReceiptId = p.clientInvestmentReceiptId,
                    clientId = p.clientInvestmentReceipt.clientId,
                    amountReceived = p.amountReceived,
                    receiptDate = p.created,
                    paymentModeId = p.paymentModeId,
                    bankId = p.bankId,
                    chequeNumber = p.chequeNumber
                })
                .ToList();
            return data;
        }

        // GET: api/ Returns Clients with Investment Deposit
        [HttpGet]
        public clientInvestmentReceipt Get(int id)
        {
            //If id > 0 retrieve Investment with the id, else return new instance of InvestmentReceipt
            var data = le.clientInvestmentReceipts
                    //.Include(p => p.clientInvestmentReceiptDetails)//TODO check for uninvested receipt
                    .FirstOrDefault(p => p.clientInvestmentReceiptId == id);
            
            if (data == null)
            {
                data = new clientInvestmentReceipt
                {
                    clientInvestmentReceiptDetails = new List<clientInvestmentReceiptDetail>()
                };
            }
            else
            {
                var dataDetail = le.clientInvestmentReceiptDetails
                .Where(p => p.clientInvestmentReceiptId == data.clientInvestmentReceiptId && !p.invested)
                .ToList();
                data.clientInvestmentReceiptDetails = dataDetail;
            }
            return data;
        }
        //Return Investment Detail
        [HttpGet]
        public clientInvestmentReceiptDetail GetInvestmentDetail(int id)
        {
            //If id > 0 retrieve Investment with the id, else return new instance of InvestmentReceipt
            var data = le.clientInvestmentReceiptDetails
                    .Include(p => p.clientInvestmentReceipt)
                    .FirstOrDefault(p => p.clientInvestmentReceiptDetailId == id);
            if (data == null)
            {
                data = new clientInvestmentReceiptDetail();
            }
            return data;
        }

        // GET: api/ Returns Clients with Investment Deposit
        [HttpPost]
        public IEnumerable<clientInvestmentReceiptDetail> GetClientReceipts(int id)
        {
            //If id > 0 retrieve Investment with the client id, else return new instance of InvestmentReceipt
            return le.clientInvestmentReceiptDetails
                .Where(p => p.clientInvestmentReceipt.clientId == id && !p.invested)
                .ToList();
            
        }

        // POST && PUT Together
        [AuthorizationFilter()]
        [HttpPost]
        public clientInvestmentReceipt Post(clientInvestmentReceipt value)
        {
            if (value == null) return null;
            var loginUser = LoginHelper.getCurrentUser(new coreSecurityEntities());
            var currentUser =
                (new coreLogic.coreSecurityEntities()).users
                .First(p => p.user_name.ToLower().Trim() == loginUser.ToLower().Trim());
            var ct = le.cashiersTills.FirstOrDefault(p => p.userName.ToLower().Trim() == User.Identity.Name.Trim().ToLower());

            if (ct == null)
            {
                throw new ApplicationException("There is no till defined for the currently logged in user (" +
                                                User.Identity.Name + ")");
            }
            
            //clientInvestmentReceipt toBeSaved;
            var clientReceipts = le.clientInvestmentReceipts.Include(p => p.clientInvestmentReceiptDetails).FirstOrDefault(p => p.clientId == value.clientId);
            if (clientReceipts != null)
            {
                populateFields(clientReceipts, value, ct);
            }
            else
            {
                clientInvestmentReceipt toBeSaved = new clientInvestmentReceipt();
                populateFields(toBeSaved, value, ct);
                le.clientInvestmentReceipts.Add(toBeSaved);
            }
            
            try
            {  le.SaveChanges(); }
            catch (Exception x)
            { throw x; }
            return value;
        }

        private void populateFields(clientInvestmentReceipt target, clientInvestmentReceipt source, cashiersTill cashierTil)
        {
            validateReceipt(source);
            target.clientId = source.clientId;
            foreach (var det in source.clientInvestmentReceiptDetails.Where(p => !p.invested))
            {
                

                validateReceiptDetail(det);
                if (det.clientInvestmentReceiptDetailId > 0)
                {
                    var detailToUpdate = target.clientInvestmentReceiptDetails
                        .FirstOrDefault(p => p.clientInvestmentReceiptDetailId == det.clientInvestmentReceiptDetailId);
                    populateReceiptDetail(detailToUpdate, det);
                }
                else
                {
                    DateTime ch = new DateTime(det.receiptDate.Year, det.receiptDate.Month, det.receiptDate.Day);
                    var ctd = le.cashiersTillDays.FirstOrDefault(p => p.cashiersTillID == cashierTil.cashiersTillID && p.tillDay == ch
                    && p.open == true);
                    if (ctd == null)
                    {
                        throw new ApplicationException("The till for the receipt date " + det.receiptDate.ToString("dd-MMM-yyyy") + " has not been opened for this user (" +
                                                        User.Identity.Name + ")");
                    }

                    clientInvestmentReceiptDetail detailToSave = new clientInvestmentReceiptDetail();
                    populateReceiptDetail(detailToSave, det);
                    target.clientInvestmentReceiptDetails.Add(detailToSave);
                }
            }

        }

        private void populateReceiptDetail(clientInvestmentReceiptDetail target, clientInvestmentReceiptDetail source)
        {
            target.amountReceived = source.amountReceived;
            target.receivedBy = LoginHelper.getCurrentUser(new coreSecurityEntities());
            target.receiptDate = source.receiptDate;
            target.invested = false;
            target.paymentModeId = source.paymentModeId;
            if (source.paymentModeId == 2)
            {
                target.bankId = source.bankId;
                target.chequeNumber = source.chequeNumber;
            }
            if (source.clientInvestmentReceiptDetailId < 1) target.created = DateTime.Now;
            else
            {
                target.modifier = LoginHelper.getCurrentUser(new coreSecurityEntities()); 
                target.modified = DateTime.Now;

            }
        }
    

        private void validateReceipt(clientInvestmentReceipt receipt)
        {// validation for allowance type attributes
            if (!clientExist(receipt.clientId))
            {
                StringBuilder errors = new StringBuilder();
                if (!clientExist(receipt.clientId))
                    errors.Append(error.InvalidClient);
                throw new ApplicationException(errors.ToString());               
            }   
        }

        private void validateReceiptDetail(clientInvestmentReceiptDetail detail)
        {// validation for allowance type attributes
            if (detail.amountReceived < 0 || detail.receiptDate == DateTime.MinValue)
            {
                StringBuilder errors = new StringBuilder();
                if (detail.amountReceived < 0)
                    errors.Append(error.InvalidAmount);
                if (detail.receiptDate == DateTime.MinValue)
                    errors.Append(error.InvalidReceiptDate);
                throw new ApplicationException(errors.ToString());
            }
        }

        private bool clientExist(int id)
        {//check if the allowancetypename already exists

            if (le.clients.Any(p => p.clientID == id))
            {
                return true;
            }
            return false;

        }
    }
}
