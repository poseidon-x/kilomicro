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

namespace coreErpApi.Controllers.Controllers.General.Setup
{
    [AuthorizationFilter()]
    public class CurrencyNoteController : ApiController
    {
        IcoreLoansEntities ent;


        public CurrencyNoteController()
        {
            ent = new coreLoansEntities();
            ent.Configuration.LazyLoadingEnabled = false;
            ent.Configuration.ProxyCreationEnabled = false;
        }

        public CurrencyNoteController(IcoreLoansEntities cent)
        {
            ent = cent;
        }

        [HttpGet]
        // GET: api/City
        public async Task<IEnumerable<currencyNote>> Get()
        {
            return await ent.currencyNotes
                .OrderBy(p => p.value)
                .ToListAsync();
        }

        [HttpGet]
        // GET: api/City
        public currencyNote Get(long id)
        {
            return ent.currencyNotes
                .FirstOrDefault(p => p.currencyNoteId == id);
        }

        

    }
}




  




