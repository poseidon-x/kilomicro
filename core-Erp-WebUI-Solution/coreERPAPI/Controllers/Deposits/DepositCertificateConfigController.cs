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
using System.Web.Http.Cors;
using coreErpApi.Controllers.Models;
using coreData.Constants;

namespace coreErpApi.Controllers.Controllers.Deposits
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [AuthorizationFilter()]
    public class DepositCertificateConfigController : ApiController
    {
        IcoreLoansEntities le;
        ErrorMessages error = new ErrorMessages();

        public DepositCertificateConfigController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public DepositCertificateConfigController(IcoreLoansEntities lent)
        {
            le = lent;
        }

        [HttpGet]
        public depositCertificateConfig Get(int id)
        {
            var data = le.depositCertificateConfigs.FirstOrDefault(p => p.depositCertificateConfigId == id);
            if(data == null) data = new depositCertificateConfig();
            return data;
        }

        [HttpPost]
        public depositCertificateConfig Post(depositCertificateConfig input)
        {
            if (input == null) return null;

            if (input.depositCertificateConfigId > 0)
            {
                var toBeBeUpdated = le.depositCertificateConfigs
                    .FirstOrDefault(p => p.depositCertificateConfigId == input.depositCertificateConfigId);
                populateFields(toBeBeUpdated, input);
            }
            else
            {
                depositCertificateConfig toBesaved = new depositCertificateConfig();
                populateFields(toBesaved, input);
                le.depositCertificateConfigs.Add(toBesaved);
            }

            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                throw new ApplicationException(ErrorMessages.ErrorSavingToServer);
            }
            
            return input;
        }
        
        private void populateFields(depositCertificateConfig tobeSaved, depositCertificateConfig input)
        {
            if (input.depositCertificateConfigId <= 0) tobeSaved.depositCertificateConfigId = 1;
            tobeSaved.earlyRedemptionText = input.earlyRedemptionText;
            tobeSaved.trustText = input.trustText;
            tobeSaved.authorityText = input.authorityText;
            tobeSaved.riskDisclosureText = input.riskDisclosureText;
        }

        

    }
}
