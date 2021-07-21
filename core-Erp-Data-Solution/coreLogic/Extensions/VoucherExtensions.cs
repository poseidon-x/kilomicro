using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace coreLogic
{
    public static class VoucherExtensions
    {
        public static string recipient(this v_head head)
        {
            var recipient = "";

            if (head != null)
            {
                using (var ent = new core_dbEntities())
                {
                    if (head.v_type == "C" && head.cust_id > 0)
                    {
                        var c = ent.custs.FirstOrDefault(p => p.cust_id == head.cust_id);
                        if (c != null) recipient = c.cust_name + " (" + c.acc_num + ")";
                    }
                    else if (head.v_type == "S" && head.sup_id > 0)
                    {
                        var c = ent.sups.FirstOrDefault(p => p.sup_id == head.sup_id);
                        if (c != null) recipient = c.sup_name + " (" + c.acc_num + ")";
                    }
                }
            }

            return recipient;
        }

        public static bool AllowedOU(this v_head batch, string userName)
        {
            var rtr = false;
            if (batch.creator.ToUpper().Trim() == userName.ToUpper().Trim())
            {
                rtr = true;
            }
            else
            {
                using (var ent = new coreGLEntities())
                {
                    var items = (from j in ent.v_dtl
                                 where j.v_head.v_head_id == batch.v_head_id
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
            return rtr;
        }
        public static double sum_crdt_amt(this jnl_batch_stg batch)
        {
            var dtls = (from p in (new coreGLEntities()).jnl_stg
                        where (p.jnl_batch_stg.jnl_batch_id == batch.jnl_batch_id)
                        select p.crdt_amt).Sum();
            return Math.Round(dtls, 2);
        }
        public static double sum_dbt_amt(this jnl_batch_stg batch)
        {
            var dtls = (from p in (new coreGLEntities()).jnl_stg
                        where (p.jnl_batch_stg.jnl_batch_id == batch.jnl_batch_id)
                        select p.dbt_amt).Sum();
            return Math.Round(dtls,2);
        }
    }
}
