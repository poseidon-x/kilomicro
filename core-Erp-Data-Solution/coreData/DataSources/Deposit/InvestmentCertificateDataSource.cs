using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using coreLibrary;
using coreLogic;
using coreLogic.HelperClasses;
using coreLogic.Models;
using coreLogic.Models.CompanyProfile;
using coreLogic.Models.Payment;
using Telerik.Reporting;


namespace coreData.DataSources.Deposit
{
    [DataObject]
    public class InvestmentCertificateDataSource
    {
        private readonly IcoreLoansEntities le;
        private readonly Icore_dbEntities ctx;

        //call a constructor to instialize a the  context 
        public InvestmentCertificateDataSource()
        {
            var db2 = new coreLoansEntities();
            var db3 = new core_dbEntities();

            le = db2;
            ctx = db3;

            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public DepositCertificateViewModel GetClientStatement(int depositId)
        {
            DepositCertificateData data = new DepositCertificateData(depositId);

            return data.getDepositDetails();
        }


    }
}

