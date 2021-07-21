using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using coreLogic;

namespace coreService
{
    public class DepreciationModule
    {
        IJournalExtensions journalextensions = new JournalExtensions();
        public bool StopFlag;
        public bool Stopped;

        public void Main()
        {
            Stopped = false;
            StopFlag = false;

            while (!StopFlag)
            {
                try
                {
                    coreLoansEntities le = new coreLoansEntities();
                    coreLogic.core_dbEntities ent = new coreLogic.core_dbEntities();
                    var pro = ent.comp_prof.FirstOrDefault();
                    var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    var lns = le.assets.Where(p => (p.lastDepreciationDate == null 
                        || p.lastDepreciationDate < date)).ToList();
                    foreach (var ln in lns)
                    {
                        //ln.assetSubCategoryReference.Load();
                        //ln.assetSubCategory.assetCategoryReference.Load();
                        DateTime? date2 = null;
                        if (ln.lastDepreciationDate == null)
                            date2 = ln.assetPurchaseDate;
                        else
                            date2 = ln.lastDepreciationDate;

                        DateTime date3 = date2.Value.AddMonths(1);
                        date3 = new DateTime(date3.Year, date3.Month, DateTime.DaysInMonth(date3.Year, date3.Month),
                            23, 59, 59);
                        if (date3 > date2 && date3 < date)
                        {
                            var dep = 0.0;
                            if (ln.assetSubCategory.assetCategory.depreciationMethod == 1)
                            {
                                dep += (1.0 / (12.0 * ln.assetLifetime)) * ln.assetPrice;
                            }
                            else
                            {
                            }

                            if (dep > 0)
                            {
                                if (ln.assetCurrentValue == 0) ln.assetCurrentValue = ln.assetPrice;
                                ln.assetCurrentValue -= dep;
                                var inte = new assetDepreciation
                                {
                                    assetID=ln.assetID,
                                    assetValue=ln.assetCurrentValue,
                                    depreciationAmount=dep,
                                    drepciationDate=date3,
                                    period=1,
                                    startDate=date2                                    
                                };
                                ln.assetDepreciations.Add(inte);

                                var jb = journalextensions.Post("LN", ln.assetSubCategory.assetCategory.depreciationAccountID.Value,
                                    ln.assetSubCategory.assetCategory.accumulatedDepreciationAccountID.Value, (dep),
                                    "Depreciation - " + date3.ToString("dd-MMM-yyyy")+ " - " + (dep).ToString("#,###.#0"),
                                    pro.currency_id.Value, date, ln.assetTag, ent, "SYSTEM",null);

                                ent.jnl_batch.Add(jb);

                            }
                            ln.lastDepreciationDate = date3;
                        }
                    }
                    le.SaveChanges();
                    ent.SaveChanges();
                }
                catch (Exception x)
                {
                    ExceptionManager.LogException(x, "DepreciationModule.Main");
                }

                Thread.Sleep(30000);

                Stopped = true;
            }
        }
    }
}
