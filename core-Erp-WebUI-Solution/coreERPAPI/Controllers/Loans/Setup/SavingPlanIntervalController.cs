﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using coreERP;
using coreLogic;
using coreERP.Providers;
using System.Linq.Dynamic;

namespace coreErpApi.Controllers.Controllers.Loans.Setup
{
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    [AuthorizationFilter()]
    public class SavingPlanIntervalController : ApiController
    {
        IcoreLoansEntities le;

        public SavingPlanIntervalController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public SavingPlanIntervalController(IcoreLoansEntities lent)
        {
            le = lent; 
        }

        // GET: api/
        public IEnumerable<savingPlanInterval> Get()
        {
            return le.savingPlanIntervals
                .OrderBy(p => p.planIntervalName)
                .ToList();
        }

        // GET: api/
        [HttpGet]
        public savingPlanInterval Get(int id)
        {
            return le.savingPlanIntervals
                .FirstOrDefault(p => p.savingPlanIntervalId == id);
        }
         
    }
}