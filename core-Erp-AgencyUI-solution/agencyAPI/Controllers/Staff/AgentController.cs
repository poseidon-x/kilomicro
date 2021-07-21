using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using coreLogic;
using agencyAPI.Providers;
using System.Web.Http.Cors;
using agencyAPI.Models;

namespace agencyAPI.Controllers.Staff
{
    [AuthorizationFilter()]
    [EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class AgentController : ApiController
    {
        IcoreLoansEntities le;

        public AgentController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public AgentController(IcoreLoansEntities lent)
        {
            le = lent;
        }

        // GET: api/
        public IEnumerable<AgentViewModel> Get()
        {
            return le.agents
                .Select(p => new AgentViewModel
                {
                    agentId = p.agentID,
                    agentNameWithNo = p.surName + ", " + p.otherNames + " (" + p.agentNo + ")"
                })
                .OrderBy(i => i.agentNameWithNo)
                .ToList();
        }

        // GET: api/
        [HttpGet]
        public AgentViewModel Get(int id)
        {
            return le.agents
                .Select(p => new AgentViewModel
                {
                    agentId = p.agentID,
                    agentNameWithNo = p.surName + ", " + p.otherNames + " (" + p.agentNo + ")",
                })
                .FirstOrDefault(i => i.agentId == id);
        }

    }
}
