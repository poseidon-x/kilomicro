﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreERP;
using coreLogic;
using agencyAPI.Providers;
using System.Threading.Tasks;
using System.Web.Http.Cors;


namespace agencyAPI.Controllers.Setup
{
    [AuthorizationFilter()]
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class DepositRepaymentModeController : ApiController
    {
        IcoreLoansEntities ent;


        public DepositRepaymentModeController()
        {
            ent = new coreLoansEntities();
            ent.Configuration.LazyLoadingEnabled = false;
            ent.Configuration.ProxyCreationEnabled = false;
        }

        public DepositRepaymentModeController(IcoreLoansEntities cent)
        {
            ent = cent;
        }

        [HttpGet]
        // GET: api/City
        public async Task<IEnumerable<depositRepaymentMode>> Get()
        {
            return await ent.depositRepaymentModes
                .OrderBy(p => p.repaymentModeName)
                .ToListAsync();
        }

        [HttpGet]
        // GET: api/City
        public depositRepaymentMode Get(long id)
        {
            return ent.depositRepaymentModes
                .FirstOrDefault(p => p.depositRepaymentModeId == id);
        }

        

    }
}




  



