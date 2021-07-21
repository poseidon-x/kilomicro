using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using coreLibrary;
using coreLogic;
using coreLogic.HelperClasses;
using coreLogic.Models;
using coreLogic.Models.CompanyProfile;
using coreLogic.Models.Loans;
using coreLogic.Models.Payment;
using Telerik.Reporting;


namespace coreData.DataSources.Loans
{
    [DataObject]
    public class DailyCollectionSheetDataSource
    {
        private readonly IcoreLoansEntities le;
        private readonly Icore_dbEntities ctx;

        //call a constructor to instialize a the  context 
        public DailyCollectionSheetDataSource()
        {
            var db2 = new coreLoansEntities();
            var db3 = new core_dbEntities();

            le = db2;
            ctx = db3;

            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public DailyCollectionSheetViewModel GetCollectionByDate(int branchId, DateTime collectionDate)
        {
            DailyCollectionSheetData data = new DailyCollectionSheetData(branchId,collectionDate);

            return data.GetData();
        }


    }
}

