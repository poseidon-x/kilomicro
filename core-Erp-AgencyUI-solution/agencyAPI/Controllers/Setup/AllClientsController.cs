using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreLogic;
using agencyAPI.Providers;
using System.Web.Http.Cors;
using agencyAPI.Models;
using coreData.Constants;
using System.Text;


namespace agencyAPI.Controllers.Setup
{
    [AuthorizationFilter()]
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class AllClientsController : ApiController
    {
        IcoreLoansEntities le;
        private StringBuilder errors = new StringBuilder();
        private ErrorMessages error = new ErrorMessages();

        public AllClientsController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public AllClientsController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        // GET: api/
        [HttpGet]
        public IEnumerable<ClientViewModel> Get()
        {
            var data = le.clients
                .Select(p => new ClientViewModel
                {
                    clientID = p.clientID,
                    clientName = p.surName + ", " + p.otherNames,
                    clientNameWithAccountNO = p.surName + " " + p.otherNames+", "+p.accountNumber,
                })
                .OrderBy(p => p.clientID)
                .ToList();
            return data;
        }

        //Get only loan Clients
        [HttpGet]
        public IEnumerable<ClientViewModel> GetRunningLoanClient()
        {
            var data = le.loans.Where(p => p.loanStatusID == 4 && p.balance > 0)
                .Join(le.clients, l => l.clientID, c => c.clientID, (l,c) => new ClientViewModel
                {
                    clientID = l.clientID,
                    clientName = c.surName + ", " + c.otherNames,
                    clientNameWithAccountNO = c.surName + " " + c.otherNames + ", " + c.accountNumber,
                    accountNO =  c.accountNumber
                })
                .OrderBy(p => p.clientID)
                .ToList();
            return data;
        }

        //Get only loan Clients
        [HttpGet]
        public IEnumerable<ClientViewModel> GetSavingsClient()
        {
            var data = le.savings
                .Join(le.clients, s => s.clientID, c => c.clientID, (s, c) => new ClientViewModel
                {
                    clientID = s.clientID,
                    clientName = c.surName + ", " + c.otherNames,
                    clientNameWithAccountNO = c.surName + " " + c.otherNames + ", " + c.accountNumber,
                    accountNO = c.accountNumber,
                    savingId = s.savingID
                })
                .OrderBy(p => p.clientID)
                .ToList();
            return data;
        }

        // GET: api/
        [HttpGet]
        public byte[] GetClientImage(int id)
        {
            var clientImage = le.clientImages.FirstOrDefault(p => p.clientID == id);
            if (clientImage != null && (le.images.FirstOrDefault(p => p.imageID == clientImage.imageID) != null))
            {
                return le.images.FirstOrDefault(p => p.imageID == clientImage.imageID).image1;
            }
            return null;
        }

        // GET: api/
        [HttpGet]
        public IEnumerable<ClientViewModel> GetGroupClients()
        {
            var data =  le.clients
                .Where(p => p.clientTypeID == 7)
                .Select(p => new ClientViewModel
                {
                    clientID = p.clientID,
                    clientName = p.surName + " " + p.otherNames,
                    clientNameWithAccountNO = p.surName + " " + p.otherNames+", "+p.accountNumber
                })
                .Distinct()
                .OrderBy(p => p.clientID)
                .ToList();

            return data;            
        }

        // GET: api/
        [HttpGet]
        public IEnumerable<ClientViewModel> GetClientWithoutGroup()
        {
            return le.clients
                .Where(p => p.clientTypeID == 7 && !p.loanGroupClients.Any())
                .Select(p => new ClientViewModel
                {
                    clientID = p.clientID,
                    clientName = p.surName + " " + p.otherNames,
                    clientNameWithAccountNO = p.surName + " " + p.otherNames + ", " + p.accountNumber
                })
                .Distinct()
                .OrderBy(p => p.clientID)
                .ToList();
        }

        // GET: api/
        [HttpGet]
        public IEnumerable<ClientViewModel> GetAllGroupClients()
        {
            return le.clients
                .Where(p => p.clientTypeID == 7)
                .Select(p => new ClientViewModel
                {
                    clientID = p.clientID,
                    clientName = p.surName + " " + p.otherNames,
                    clientNameWithAccountNO = p.surName + " " + p.otherNames + ", " + p.accountNumber
                })
                .Distinct()
                .OrderBy(p => p.clientID)
                .ToList();
        }

        // GET: api/
        //[HttpGet]
        //public IEnumerable<ClientViewModel> GetCreditLineClients()
        //{
        //    var clientIds = le.creditLines
        //        .Select(p => p.clientId)
        //        .ToList();

        //    return le.clients
        //        .Where(p => clientIds.Contains(p.clientID))
        //        .Select(p => new ClientViewModel
        //        {
        //            clientID = p.clientID,
        //            clientName = p.surName + " " + p.otherNames
        //        })
        //        .Distinct()
        //        .OrderBy(p => p.clientID)
        //        .ToList();
        //}

        // GET: api/clients
        [HttpGet]
        public ClientViewModel Get(int id)
        {
            return le.clients
                .Select(p => new ClientViewModel
                {
                    clientID = p.clientID,
                    clientName =  p.surName + " " + p.otherNames
                })
                .FirstOrDefault(p => p.clientID == id);
        }

        // GET: Search for Client
        [HttpPost]
        public IEnumerable<CSViewModel> SearchClient(CSModel CSVModel)
        {
            validateSearch(CSVModel);

            switch (CSVModel.searchCriteria)
            {
                case "accountnumber":
                    var data = le.savings
                       .Join(le.clients, s => s.clientID, c => c.clientID, (s, c) => new CSViewModel
                       {
                           clientName =  c.surName + " " + c.otherNames,
                           clientNameWithAccountNO = c.surName + " " + c.otherNames + ", " + c.accountNumber,
                           savingNo = s.savingNo,
                           branch = c.branch.branchName,
                           savingTypeName = s.savingType.savingTypeName,
                           firstDepositDate = s.firstSavingDate,
                           currentBalance = s.principalBalance
                       })
                       .OrderBy(p => p.clientNameWithAccountNO)
                       .Where(p => p.savingNo.ToLower().Contains(CSVModel.searchData))
                       .ToList();
                    return data;
                case "accountname":
                    var sNameData = le.savings
                       .Join(le.clients, s => s.clientID, c => c.clientID, (s, c) => new CSViewModel
                       {
                           clientName = c.surName + " " + c.otherNames,
                           clientNameWithAccountNO = c.surName + " " + c.otherNames + ", " + c.accountNumber,
                           savingNo = s.savingNo,
                           branch = c.branch.branchName,
                           savingTypeName = s.savingType.savingTypeName,
                           firstDepositDate = s.firstSavingDate,
                           currentBalance = s.principalBalance
                       })
                       .OrderBy(p => p.clientNameWithAccountNO)
                       .Where(p => p.clientName.ToLower().Contains(CSVModel.searchData))
                       .ToList();
                    return sNameData;
            }

            return null;
        }

        private void validateSearch(CSModel input)
        {// validation for client search attributes
            if (input.searchCriteria == null || input.searchData == null)
            {
                if (input.searchCriteria == null)
                    errors.Append(error.SearchCriteriaEmpty);
                if (input.searchData == null)
                    errors.Append(error.SearchDataEmpty);
                throw new ApplicationException(errors.ToString());
            }
        }
    }
}
