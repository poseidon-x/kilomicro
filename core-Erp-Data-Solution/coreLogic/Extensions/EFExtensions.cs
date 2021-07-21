using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Core;

namespace coreLogic
{
    public static class coreExtensions
    {
        public const string GL_TX_BATCH_NAME = "GL_TX";
        public const string FND_TX_BATCH_NAME = "FND_TX";
        static object o = new object();

        
        public static void AttachToOrGet<T>(this ObjectContext context, string entitySetName, ref T entity)
            where T : IEntityWithKey
        {
            ObjectStateEntry entry;
            // Track whether we need to perform an attach
            bool attach = false;
            if (
                context.ObjectStateManager.TryGetObjectStateEntry
                    (
                        context.CreateEntityKey(entitySetName, entity),
                        out entry
                    )
                )
            {
                // Re-attach if necessary
                attach = entry.State == EntityState.Detached;
                // Get the discovered entity to the ref
                entity = (T)entry.Entity;
            }
            else
            {
                // Attach for the first time
                attach = true;
            }
            if (attach)
                context.AttachTo(entitySetName, entity);
        }
         
        public static bool IsAttachedTo(this ObjectContext context, EntityKey key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }
            ObjectStateEntry entry;
            if (context.ObjectStateManager.TryGetObjectStateEntry(key, out entry))
            {
                return (entry.State != EntityState.Detached);
            }
            return false;
        }

        public static string NextSystemNumber(string name, int size=5, Icore_dbEntities ent = null)
        {
            if (ent == null)
            {
                ent=new  core_dbEntities();
            }
            lock (o)
            {
                var sn = ent.sys_no.FirstOrDefault(p => p.name == name);
                int next;
                if (sn == null)
                {
                    sn = new sys_no
                    {
                        name = name,
                        creation_date = DateTime.Now,
                        creator = "SYSTEM",
                        prefix = "",
                        step = 1,
                        value = 1,
                        suffix = ""
                    };
                    next = 1;
                    ent.sys_no.Add(sn);
                }
                else
                {
                    next = sn.value + sn.step;
                    sn.value = next;
                }
                ent.SaveChanges();
                var sysNo=sn.prefix + ((sn.value.ToString().Length<size)?sn.value.ToString().PadLeft(size,'0')
                    : sn.value.ToString()) + sn.suffix;
                return sysNo;
            }
        } 
        public static string NextGLBatchNumber(Icore_dbEntities ent = null)
        {
            return NextSystemNumber(FND_TX_BATCH_NAME, 1, ent);
        }
        public static double sum_dbt_amt(this jnl_batch_tmp batch)
        {
            return (new core_dbEntities()).jnl_tmp.Where(
                p=>p.jnl_batch_tmp.jnl_batch_id==batch.jnl_batch_id).Sum(p => p.dbt_amt);
        }
        public static double sum_crdt_amt(this jnl_batch_tmp batch)
        {
            return (new core_dbEntities()).jnl_tmp.Where(
                p => p.jnl_batch_tmp.jnl_batch_id == batch.jnl_batch_id).Sum(p => p.crdt_amt);
        }
        public static bool is_valid(this jnl_batch_tmp batch)
        {
            return batch.sum_crdt_amt().Round(2) == batch.sum_dbt_amt().Round(2);
        }
        public static double Round(this double val, int decimals)
        {
            return Math.Round(val, decimals);
        }
        public static double sum_dbt_amt(this jnl_batch batch)
        {
            return (new core_dbEntities()).jnl.Where(
                p => p.jnl_batch.jnl_batch_id == batch.jnl_batch_id).Sum(p => p.dbt_amt);
        }
        public static double sum_crdt_amt(this jnl_batch batch)
        {
            return (new core_dbEntities()).jnl.Where(
                p => p.jnl_batch.jnl_batch_id == batch.jnl_batch_id).Sum(p => p.crdt_amt);
        }
        public static bool is_valid(this jnl_batch batch)
        {
            return batch.sum_crdt_amt().Round(2) == batch.sum_dbt_amt().Round(2);
        }
        public static int AccountingPeriod(this DateTime date, int fmo){
            var accPeriod = int.Parse(((date.Month < fmo) ? date.Year - 1 : date.Year).ToString()
                        + ((fmo != 1) ? ((date.Month + fmo) % 12) + (date.Month < fmo ? 0 : 1) : date.Month).ToString().PadLeft(2, '0'));
            return accPeriod;
        }
        public static bool CanClose(this DateTime date, Icore_dbEntities ent = null)
        {
            if (ent == null)
            {
                ent = new core_dbEntities();
            }
            var period = ent.acct_period.FirstOrDefault(p => p.close_date >= date);
            if (period == null)
            {
                return true;
            }

            return false;
        }
        public static DateTime MonthEnd(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }
        public static bool AllowedOU(this string userName, int? ouID)
        {
            var rtr = false;
            if (ouID == null)
            {
                rtr = false;
            }
            else
            {
                using (var ent = new core_dbEntities())
                {

                    var ou = ent.user_gl_ou_gl_ou.FirstOrDefault(p => p.gl_ou.ou_id == ouID && p.user_name == userName);
                    if (ou == null)
                    {
                        var thisOU = ent.gl_ou.FirstOrDefault(p => p.ou_id == ouID);
                        if (thisOU == null || thisOU.parent_ou_id == null)
                            rtr = false;
                        else
                        {
                            rtr = userName.AllowedOU(thisOU.parent_ou_id);
                        }
                    }
                    else
                        rtr = ou.allow;
                }
            }
            return rtr;
        }
        public static bool AllowedOU(this jnl_batch batch, string userName)
        {
            var rtr = false;
            try
            {
                if (batch.creator.ToUpper().Trim() == userName.ToUpper().Trim())
                {
                    rtr = true;
                }
                else
                {
                    using (var ent = new core_dbEntities())
                    {
                        var items = (from j in ent.jnl
                                     where j.jnl_batch.jnl_batch_id == batch.jnl_batch_id
                                     select j.gl_ou.ou_id).ToList();
                        foreach (var item in items)
                        {
                            if (item == null)
                                rtr = true;
                            else
                            {
                                rtr = userName.AllowedOU(item);
                                if (rtr == false) break;
                            }
                        }
                    }
                }
            }
            catch (Exception x)
            {
            }
            return rtr;
        }
        public static bool AllowedOU(this jnl_batch_tmp batch, string userName)
        {
            var rtr = false;
            try
            {
                if (batch.creator.ToUpper().Trim() == userName.ToUpper().Trim())
                {
                    rtr = true;
                }
                else
                {
                    using (var ent = new core_dbEntities())
                    {
                        var items = (from j in ent.jnl_tmp
                                     where j.jnl_batch_tmp.jnl_batch_id == batch.jnl_batch_id
                                     select j.gl_ou.ou_id).ToList();
                        foreach (var item in items)
                        {
                            if (item == null)
                                rtr = true;
                            else
                            {
                                rtr = userName.AllowedOU(item);
                                if (rtr == false) break;
                            }
                        }
                    }
                }
            }
            catch (Exception x)
            {
            }
            return rtr;
        }
        public static bool AllowedOU(this pc_head batch, string userName)
        {
            var rtr = false;
            try
            {
                if (batch.creator.ToUpper().Trim() == userName.ToUpper().Trim())
                {
                    rtr = true;
                }
                else
                {
                    using (var ent = new coreGLEntities())
                    {
                        var items = (from j in ent.pc_dtl
                                     where j.pc_head.pc_head_id == batch.pc_head_id
                                     select j.gl_ou_id).ToList();
                        foreach (var item in items)
                        {
                            if (item == null)
                                rtr = true;
                            else
                            {
                                rtr = userName.AllowedOU(item);
                                if (rtr == false) break;
                            }
                        }
                    }
                }
            }
            catch (Exception x)
            {
            }
            return rtr;
        }
        public static bool AllowedOU(this vw_jnl batch, string userName)
        {
            var rtr = false;
            try
            {
                if (batch.creator.ToUpper().Trim() == userName.ToUpper().Trim())
                {
                    rtr = true;
                }
                else
                {
                    using (var ent = new core_dbEntities())
                    {
                        var items = (from j in ent.jnl
                                     where j.jnl_batch.jnl_batch_id == batch.jnl_batch_id
                                     select j.gl_ou.ou_id).ToList();
                        foreach (var item in items)
                        {
                            if (item == null)
                                rtr = true;
                            else
                            {
                                rtr = userName.AllowedOU(item);
                                if (rtr == false) break;
                            }
                        }
                    }
                }
            }
            catch (Exception x)
            {
            }
            return rtr;
        }
        public static bool AllowedOU(this vw_jnl_tmp batch, string userName)
        {
            var rtr = false;
            try
            {
                if (batch.creator.ToUpper().Trim() == userName.ToUpper().Trim())
                {
                    rtr = true;
                }
                else
                {
                    using (var ent = new core_dbEntities())
                    {
                        var items = (from j in ent.jnl_tmp
                                     where j.jnl_batch_tmp.jnl_batch_id == batch.jnl_batch_id
                                     select j.gl_ou.ou_id).ToList();
                        foreach (var item in items)
                        {
                            if (item == null)
                                rtr = true;
                            else
                            {
                                rtr = userName.AllowedOU(item);
                                if (rtr == false) break;
                            }
                        }
                    }
                }
            }
            catch (Exception x)
            {   
            }
            return rtr;
        }

        public static jnl_batch ToJournal(this jnl_batch_stg batch)
        {
            return new jnl_batch
            {
                batch_no=batch.batch_no,
                creation_date=batch.creation_date,
                creator=batch.creator,
                is_adj=batch.is_adj,
                multi_currency=batch.multi_currency,
                posted=batch.posted,
                source=batch.source
            };
        }

        public static jnl ToJournal(this jnl_stg j, Icore_dbEntities ent)
        {
            return new jnl
            {
                acct_period = j.acct_period,
                accts = ent.accts.First(p => p.acct_id == j.acct_id),
                check_no = j.check_no,
                crdt_amt = j.crdt_amt,
                creation_date = j.creation_date,
                creator = j.creator,
                currencies = ent.currencies.FirstOrDefault(p => p.currency_id == j.currency_id),
                dbt_amt = j.dbt_amt,
                description = j.description,
                frgn_crdt_amt = j.frgn_crdt_amt,
                frgn_dbt_amt = j.frgn_dbt_amt,
                gl_ou = j.cost_center_id != null ? ent.gl_ou.FirstOrDefault(p => p.ou_id == j.cost_center_id) : null,
                last_modifier = j.last_modifier,
                modification_date = j.modification_date,
                rate = j.rate,
                recipient = j.recipient,
                ref_no = j.ref_no,
                tx_date = j.tx_date
            };
        }
        public static string fullname(this coreLogic.client client)
        {
            return client.surName + ", " + client.otherNames + " (" + client.accountNumber + ")";
        }
    }
}
