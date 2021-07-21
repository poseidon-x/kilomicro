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
using coreLogic.Models.ArCustomers;


namespace coreErpApi.Controllers.Controllers.AccountsReceivable.CreditMemo

{
    [AuthorizationFilter()]
    public class ArCreditMemoReasonController : ApiController
    {

        private string errorMessage = "";
        private string nextOrderNumber = "";
        private int  lineNum = 0; 

        //Declare a Database(Db) context variable 
        ICommerceEntities le;


        //call a constructor to instialize a the Dv context 
        public ArCreditMemoReasonController()
        {
            le = new CommerceEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        //A constructor wiith a parameter
        public ArCreditMemoReasonController(ICommerceEntities lent, Icore_dbEntities ent)
        {
            le = lent;
        }

        // GET: api/Payment
        public IEnumerable<creditMemoReason> Get()
        {
            return le.creditMemoReasons
                .OrderBy(p => p.creditMemoReasonName)
                .ToList();
        }

        [HttpGet]
        public creditMemoReason Get(int id)
        {
            creditMemoReason value = le.creditMemoReasons
                .FirstOrDefault(p => p.creditMemoReasonId == id);

            if (value == null)
            {
                value = new creditMemoReason();
            }
            return value;
        }



       
       


    }
}
