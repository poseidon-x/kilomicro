using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using coreLogic;
using coreERP.Providers;

namespace coreERP.Controllers.Currency
{
    [AuthorizationFilter()]
    public class CurrencyController : ApiController
    {
        core_dbEntities le = new core_dbEntities();

        public CurrencyController()
        {
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        // GET: api/currencies
        public IEnumerable<coreLogic.currencies> Get()
        {
            return le.currencies
                .OrderBy(p => p.currency_id)
                .ToList();
        }

        // GET: api/currencies/5
        [HttpGet]
        public currencies Get(int id)
        {
            return le.currencies
                .FirstOrDefault(p => p.currency_id == id);
        }

        [HttpPost]
        // POST: api/currencies
        public currencies Post([FromBody]currencies value)
        {
            le.currencies.Add(value);
            le.SaveChanges();

            return value;
        }

        [HttpPut]
        // PUT: api/currencies/5
        public currencies Put([FromBody]currencies value)
        {
            var toBeUpdated = new currencies
            {
                currency_id = value.currency_id,
                major_name = value.major_name,
                minor_name = value.minor_name,
                major_symbol = value.major_symbol,
                minor_symbol = value.minor_symbol,
                //
                current_buy_rate = value.current_buy_rate,
                current_sell_rate = value.current_sell_rate,
                creation_date = value.creation_date,
                creator = value.creator,
                modification_date = value.modification_date,
                last_modifier = value.last_modifier,

            };
            le.Entry(toBeUpdated).State = System.Data.Entity.EntityState.Modified;
            le.SaveChanges();

            return toBeUpdated;
        }

        [HttpDelete]
        // DELETE: api/currencies/5
        public void Delete([FromBody]currencies value)
        {
            var forDelete = le.currencies.FirstOrDefault(p => p.currency_id == value.currency_id);
            if (forDelete != null)
            {
                le.currencies.Remove(forDelete);
                le.SaveChanges();
            }
        }
    }
}
