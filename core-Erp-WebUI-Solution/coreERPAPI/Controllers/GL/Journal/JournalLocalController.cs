//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Web.Http;
//using coreERP;
//using coreLogic;
//using coreERP.Providers;
//using System.Linq.Dynamic;
//using System.Threading.Tasks;
//using System.Web.Http.Cors;
//using coreErpApi.Controllers.Models;
//using coreData.Constants;

//namespace coreErpApi.Controllers.GL.Journal
//{
//    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
//    [AuthorizationFilter()]
//    public class JournalLocalController : ApiController
//    {
//        private Icore_dbEntities le;
//        private ErrorMessages error = new ErrorMessages();

//        public JournalLocalController()
//        {
//            le = new core_dbEntities();
//            le.Configuration.LazyLoadingEnabled = false;
//            le.Configuration.ProxyCreationEnabled = false;
//        }

//        public JournalLocalController(Icore_dbEntities lent)
//        {
//            le = lent;
//        }

//        //[HttpGet]
//        //public IEnumerable<jnl_batch_tmp> Get()
//        //{
//        //    var data = le.jnl_batch_tmp
//        //        .Include(p => p.jnl_tmp)
//        //        .ToList();
//        //    return data;
//        //}

//        [HttpGet]
//        public jnl_batch_tmp Get(int id)
//        {
//            var data = le.jnl_batch_tmp
//                .FirstOrDefault(p => p.jnl_batch_id == id);

//            if (data == null)
//            {
//                data = new jnl_batch_tmp
//                {
//                    creation_date = DateTime.Now,
//                    batch_no = "UNASSIGNED",
//                    creator = LoginHelper.getCurrentUser(new coreSecurityEntities()),
//                    posted = false
//                };
//            }
//            return data;
//        }




//        [HttpPost]
//        public jnl_batch_tmp Post(jnl_batch_tmp input)
//        {
//            if (input == null) return null;

//            if (input.jnl_batch_id > 0)
//            {
//                var toBeUpdated = le.jnl_batch_tmp.FirstOrDefault(p => p.jnl_batch_id == input.jnl_batch_id);
//                populateBatchFields(toBeUpdated, input);
//            }
//            else
//            {
//                jnl_batch_tmp toBeSaved = new jnl_batch_tmp();
//                populateBatchFields(toBeSaved, input);
//                le.jnl_batch_tmp.Add(toBeSaved);
//            }

//            try
//            {
//                le.SaveChanges();
//            }
//            catch (Exception x)
//            {
//                throw new ApplicationException(error.ErrorSavingToServer);
//            }
//        }


//        private void populateBatchFields(jnl_batch_tmp value, jnl_batch_tmp input)
//        {
//            if (input.jnl_batch_id < 1)
//            {
//                value.posted = false;
//                value.batch_no = coreExtensions.NextGLBatchNumber();
//                value.source = "J/E";
//                value.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            }
//            else
//            {
//                value.last_modifier = LoginHelper.getCurrentUser(new coreSecurityEntities());
//                value.modification_date = DateTime.Now;
//            }
//            value.multi_currency = false;
//            value.is_adj = false;

//            foreach (var jnl in input.jnl_tmp)
//            {
//                if (jnl.jnl_id > 0)
//                {
//                    var jnlToBeUpdated = input.jnl_tmp.FirstOrDefault(p => p.jnl_id == jnl.jnl_id);
//                    populateJnlFields(jnlToBeUpdated, jnl);
//                }
//                else
//                {
//                    jnl_tmp toBeSaved = new jnl_tmp();
//                    populateJnlFields(toBeSaved, jnl);
//                    input.jnl_tmp.Add(toBeSaved);
//                }
//            }
//        }

//        private void populateJnlFields(jnl_tmp value, jnl_tmp input, jnl_batch_tmp bat)
//        {
//            if (input.jnl_id < 1)
//            {
//                var currency = le.currencies.FirstOrDefault();
//                value.currencies = new currencies
//                {
//                    currency_id = currency.currency_id,
//                    major_name = currency.major_name,
//                    minor_name = currency.minor_name
//                };
//                value.creation_date = DateTime.Now;
//                value.creator = LoginHelper.getCurrentUser(new coreSecurityEntities());
//            }
//            value.tx_date = bat.creation_date.Value;
//            value.description = input.description;
//            if (value.dbt_amt > 0)
//            {
//                value.dbt_amt = input.dbt_amt;
//                value.crdt_amt = 0;
//            }
//            else if (value.crdt_amt > 0)
//            {
//                value.dbt_amt = 0;
//                value.crdt_amt = input.crdt_amt;
//            }

//            //value.accts = new accts {acct_id = acc.acct_id, acc_name = acc.acc_name, acc_num = acc.acc_num};
//            //gl_ou ou = le.gl_ou.First<gl_ou>(p => p.ou_id == input.gl_ou.ou_id);
//            //value.gl_ou = new gl_ou {ou_id = ou.ou_id, ou_name = ou.ou_name};




//        }
//    }
//}
