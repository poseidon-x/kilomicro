using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using coreLogic;
using System.Data.Entity;
using coreLogic.Models;

namespace coreLogic
{
    [DataObject]
    public class ProductSummaryDataSource
    {

        private readonly coreReports.reportEntities le;

        public ProductSummaryDataSource()
        {
            var db2 = new coreReports.reportEntities(); 

            le= db2;
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public IEnumerable<ProductSummary> GetProductSummary(DateTime? endDate)
        {
            var data = (
                from p in le.vwProductSummaries
                select new ProductSummary
                {
                    allActiveAccounts = p.allActiveAccounts,
                    allAccounts = p.allAccounts,
                    distinctAccount = p.distinctAccount,
                    distinctActiveAccounts = p.distinctActiveAccounts,
                    principalBalance = p.principalBalance,
                    productCategory = p.productCategory,
                    productName = p.productName,
                    totalPrincipal = p.totalPrincipal,
                    remainingBalance = p.remainingBalance,
                    totalInterest = p.totalInterest
                }
                )
                .ToList()
                .OrderBy(p => p.productName)
                .ToList();

            var ent = new core_dbEntities();
            var cp = ent.comp_prof.First();
            
            foreach (ProductSummary record in data)
            {
                record.companyLogo = cp.logo;
                record.companyAddress = cp.addr_line_1;
                record.companyPhone = cp.phon_num;
            }
             
            return data;
        }
    }
}