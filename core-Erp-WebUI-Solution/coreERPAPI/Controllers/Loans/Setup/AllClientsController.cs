using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;
using System.Web.Http.Cors;
using coreERP.Models.Loan;
using coreErpApi;
using Microsoft.Ajax.Utilities;


namespace coreERP.Controllers.Loans.Setup
{
    //[AuthorizationFilter()]
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class AllClientsController : ApiController
    {
        IcoreLoansEntities le;

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
            var data = le.loans.Where(p => p.loanStatusID == 4 && p.balance > 100)
                .Join(le.clients, l => l.clientID, c => c.clientID, (l,c) => new ClientViewModel
                {
                    clientID = l.clientID,
                    clientName = c.surName + ", " + c.otherNames,
                    clientNameWithAccountNO = c.surName + " " + c.otherNames + ", " + c.accountNumber,
                    accountNO =  c.accountNumber
                })
                .OrderBy(p => p.clientName)
                .ToList();
            return data;
        }

        

        //Get only saving Clients
        [HttpGet]
        public IEnumerable<ClientViewModel> GetSavingsClient()
        {
            var data = le.savings
                .Join(le.clients, s => s.clientID, c => c.clientID, (s, c) => new ClientViewModel
                {
                    clientID = s.clientID,
                    clientName = c.surName + ", " + c.otherNames,
                    clientNameWithAccountNO = c.surName + " " + c.otherNames + ", " + s.savingNo,
                    accountNO = s.savingNo,
                    savingId = s.savingID
                })
                .OrderBy(p => p.clientID)
                .ToList();
            return data;
        }

        [HttpPost]
        public IEnumerable<ClientViewModel> GetSearchedSavingClient(ClientSearchModel input)
        {
            var data = le.savings
                .Join(le.clients, s => s.clientID, c => c.clientID, (s, c) => new ClientViewModel
                {
                    clientID = s.clientID,
                    clientName = c.surName + ", " + c.otherNames,
                    clientNameWithAccountNO = c.surName + " " + c.otherNames + ", " + s.savingNo,
                    accountNO = s.savingNo,
                    savingId = s.savingID
                })
                .Where(p => p.clientNameWithAccountNO.ToLower().Contains(input.searchString.ToLower()))
                .OrderBy(p => p.clientID)
                .ToList();
            return data;
        }

        [HttpPost]
        public IEnumerable<ClientViewModel> GetSearchedDepositClient(ClientSearchModel input)
        {
            var data = le.deposits
                .Join(le.clients, d => d.clientID, c => c.clientID, (s, c) => new ClientViewModel
                {
                    clientID = s.clientID,
                    clientName = c.surName + ", " + c.otherNames,
                    clientNameWithAccountNO = c.surName + " " + c.otherNames + ", " + c.accountNumber,
                    accountNO = c.accountNumber,
                    savingId = 0
                })
                .Where(p => p.clientNameWithAccountNO.ToLower().Contains(input.searchString.ToLower()))
                .DistinctBy(p => p.clientID)
                .OrderBy(p => p.clientID)
                .ToList();
            return data;
        }

        [HttpPost]
        public IEnumerable<ClientViewModel> GetSearchedLoanClient(ClientSearchModel input)
        {
            var data = le.loans
                .Where(p => p.loanStatusID == 4 && p.balance > 0)
                .Join(le.clients, d => d.clientID, c => c.clientID, (s, c) => new ClientViewModel
                {
                    clientID = s.clientID,
                    clientName = c.surName + ", " + c.otherNames,
                    clientNameWithAccountNO = c.surName + " " + c.otherNames + ", " + c.accountNumber,
                    accountNO = c.accountNumber
                })
                .Where(p => p.clientNameWithAccountNO.ToLower().Contains(input.searchString.ToLower()))
                .DistinctBy(p => p.clientID)
                .OrderBy(p => p.clientID)
                .ToList();
            return data;
        }

        [HttpGet]
        public IEnumerable<SavingAccountViewModel> GetClientSavingsAccounts(int id)
        {
            var data = le.savings
                .Where(p => p.clientID == id)
                .Select(p => new SavingAccountViewModel
                {
                    clientId = p.clientID,
                    savingId = p.savingID,
                    savingAccountNo = p.savingNo
                })
                .OrderBy(p => p.savingAccountNo)
                .ToList();
            return data;
        }

        [HttpPost]
        public IEnumerable<ClientViewModel> GetSearchedClient(ClientSearchModel input)
        {
            var data = le.savings
                .Join(le.clients, s => s.clientID, c => c.clientID, (s, c) => new ClientViewModel
                {
                    clientID = s.clientID,
                    clientName = c.surName + ", " + c.otherNames,
                    clientNameWithAccountNO = c.surName + " " + c.otherNames + ", " + c.accountNumber,
                    accountNO = c.accountNumber,
                    savingId = 0
                })
                .Where(p => p.clientNameWithAccountNO.ToLower().Contains(input.searchString.ToLower()))
                .DistinctBy(p => p.clientID)
                .OrderBy(p => p.clientID)
                .ToList();
            return data;
        }
        [AuthorizationFilter()]
        [HttpPost]
        public IEnumerable<ClientViewModel> GetAllClientsBySearch(ClientSearchModel input)
        {
            //Check the login user's branch
            HelperMethod helper = new HelperMethod();
            var user = User?.Identity?.Name?.Trim()?.ToLower();
            var clients = le.clients.ToList();
            if (!helper.IsOwner(user))
            {
                //Get the user branch ID
                var staffBranchId = helper.GetBranchIdForUser(user);
                clients = clients.Where(p => p.branchID.Value == staffBranchId).ToList();
            }
            var clientData = clients
                .Select(p => new ClientViewModel
                {
                    clientID = p.clientID,
                    clientName = p.surName + ", " + p.otherNames,
                    clientNameWithAccountNO = p.surName + " " + p.otherNames + ", " + p.accountNumber,
                    accountNO = p.accountNumber
                })
                .Where(p => p.clientNameWithAccountNO.ToLower().Contains(input.searchString.ToLower()))
                .DistinctBy(p => p.clientID)
                .OrderBy(p => p.clientID)
                .ToList();
            return clientData;
        }

        [HttpGet]
        public IEnumerable<DepositClientViewModel> GetDepositClient()
        {
            var data = le.deposits
                .Join(le.clients, d => d.clientID, c => c.clientID, (d, c) => new DepositClientViewModel
                {
                    clientID = d.clientID,
                    depositId = d.depositID,
                    clientName = c.clientTypeID == 5 ? c.companyName : c.surName + ", " + c.otherNames,
                    clientNameWithDepositNO = c.surName + " " + c.otherNames + ", " + d.depositNo
                })
                .DistinctBy(p => p.clientID)
                .OrderBy(p => p.clientNameWithDepositNO)
                .ToList();
            return data;
        }

        [HttpPost]
        public IEnumerable<ClientViewModel> GetSavingsClient(SavingClientSearchViewModel searchInput)
        {
            List<ClientViewModel> data;
            if (searchInput.isAccountNumber)
            {
                data = le.savings
                .Where(i => i.savingNo.ToLower().Contains(searchInput.searchValue.ToLower()))
                .Join(le.clients, s => s.clientID, c => c.clientID, (s, c) => new ClientViewModel
                {
                    clientID = s.clientID,
                    clientName = c.surName + ", " + c.otherNames,
                    clientNameWithAccountNO = c.surName + " " + c.otherNames + ", " + s.savingNo,
                    accountNO = s.savingNo,
                    savingId = s.savingID
                })
                .OrderBy(p => p.clientID)
                .ToList();
            }
            else if (searchInput.isName)
            {
                data = le.savings
                    .Join(le.clients, s => s.clientID, c => c.clientID, (s, c) => new ClientViewModel
                    {
                        clientID = s.clientID,
                        clientName = c.surName + ", " + c.otherNames,
                        clientNameWithAccountNO = c.surName + " " + c.otherNames + ", " + s.savingNo,
                        accountNO = s.savingNo,
                        savingId = s.savingID
                    })
                    .Where(p => p.clientName.ToLower().Contains(searchInput.searchValue.ToLower()))
                    .OrderBy(p => p.clientID)
                    .ToList();
            }
            else
            {
                return null;
            }
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

        [HttpGet]
        public IEnumerable<ClientViewModel> GetLoanTypeClients()
        {
            var loanClientType = le.clientTypes.Where(p => p.clientTypeName.ToLower().Contains("loan"))
                .Select(p => p.clientTypeId)
                .ToList();
            return le.clients
                .Where(p => loanClientType.Contains(p.clientTypeID))
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
                .Include(r=>r.loanGroupClients)
                .Include(p=>p.loanGroupClients.Select(w=>w.loanGroup))
                .Where(p => p.clientTypeID == 7)
                .Select(p => new ClientViewModel
                {
                    clientID = p.clientID,
                    clientName = p.surName + " " + p.otherNames,
                    clientNameWithAccountNO = p.surName + " " + p.otherNames + ", " + p.accountNumber,
                    groupAddedDate = p.loanGroupClients.FirstOrDefault().created
                })
                .Distinct()
                .OrderBy(p => p.groupAddedDate)
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

        [HttpGet]
        public List<ClientViewModel> GetProvisionClients(int id)
        {
            var loanIds = le.loanProvisions
                .Where(p => p.provisionBatchId == id)
                .Select(p => p.loanID)
                .ToList();

            return le.loans.Where(p => loanIds.Contains(p.loanID))
                .Join(le.clients, l => l.clientID, c => c.clientID, (l,c) => new ClientViewModel
                {
                    clientID = l.clientID,
                    loanNo =  l.loanNo,
                    loanId = l.loanID,
                    clientNameWithAccountNO = c.surName+", "+c.otherNames+" - "+c.accountNumber
                })
                .OrderBy(i => i.clientNameWithAccountNO)
                .ToList();

        }

        [HttpGet]
        public List<LoanDetailViewModel> GetProvisionLoans(int id)
        {
            return le.loanProvisions.Where(p => p.provisionBatchId == id)
                .Join(le.loans, lp => lp.loanID, l => l.loanID, (lp,l) => new LoanDetailViewModel
                {
                    loanId = lp.loanID,
                    loanNo = l.loanNo
                })
                .OrderBy(p => p.loanNo)
                .ToList();              
        }


        [HttpGet]
        public List<LoanDetailViewModel> GetClientUndisbursedLoans(int id)
        {
            return le.loans
                .Where(p => p.clientID == id && (p.loanStatusID == 3 || p.loanStatusID == 1 || p.loanStatusID == 2))
                .Select(p => new LoanDetailViewModel
                {
                    loanId = p.loanID,
                    loanNo = p.loanNo
                })
                .OrderBy(p => p.loanNo)
                .ToList();
        }


        [HttpGet]
        public List<ClientViewModel> GetClientForGroup(int groupId)
        {
            var group = le.loanGroups
                .Include(p => p.loanGroupClients)
                .Include(p=>p.client)
                .Include(p=>p.loanGroupClients.Select(r=>r.client))
                .Include(p=>p.loanGroupClients.Select(r=>r.client.loans))
                .FirstOrDefault(p => p.loanGroupId == groupId);

            if (group == null) throw new ApplicationException("Group doesn't exist");
            var groupClients = group.loanGroupClients
                .Where(p => p.loanGroupId == groupId)
                .Select(p => new ClientViewModel
                {
                    clientID = p.clientId,
                    clientName = p.client.surName + " " + p.client.otherNames,
                })
                //.Distinct()
                .OrderBy(p => p.clientID)
                .ToList();
            return groupClients;
        }

        [HttpPost]
        public IEnumerable<ClientViewModel> GetClientsById(int Id)
        {
            var data = le.clients
                .Select(p => new ClientViewModel
                {
                    clientID = p.clientID,
                    clientName = p.surName + ", " + p.otherNames,
                    clientNameWithAccountNO = p.surName + " " + p.otherNames + ", " + p.accountNumber,
                    accountNO = p.accountNumber
                })
                .Where(p => p.clientID==Id)
                .DistinctBy(p => p.clientID)
                .OrderBy(p => p.clientID)
                .ToList();
            return data;
        }

        

        [HttpGet]
        public List<ClientViewModel> GetClientsByDay(int id)
        {
            var dayClients = le.loanGroupClients
                .Include(p=>p.loanGroup)
                .Include(p => p.loanGroup.client)
                .Include(p=>p.loanGroup.loanGroupDay)
                .Where(p=>p.loanGroup.loanGroupDayId==id)
                .Select(p => new ClientViewModel
                {
                    clientID = p.clientId,
                    clientName = p.client.surName + " " + p.client.otherNames,
                })
                .OrderBy(p => p.clientID)
                .ToList();
            return dayClients;
        }

    }
}
