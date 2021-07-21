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
using System.Web.Http.Cors;
using coreERP;

namespace coreErpApi.Controllers.Controllers.Loans.LoanType
{
    //[AuthorizationFilter()]
    //[EnableCors(origins: "*", headers: "*", methods: "*", exposedHeaders: "X-Custom-Header")]
    public class LoanTypeApprovalController : ApiController
    {
        IcoreLoansEntities le;

        public LoanTypeApprovalController()
        {
            le = new coreLoansEntities();
            le.Configuration.LazyLoadingEnabled = false;
            le.Configuration.ProxyCreationEnabled = false;
        }

        public LoanTypeApprovalController(IcoreLoansEntities cent)
        {
            le = cent;
        }

        [HttpGet]
        // GET: api/
        public  IEnumerable<loanType> Get()
        {
            var lnTypes = le.loanTypes
                .OrderBy(p => p.loanTypeName)
                .ToList();

            return lnTypes;
        }

        [HttpPost]
        // GET: api/
        public loanType GetApproval(int id)
        {
            loanType loanTyp = le.loanTypes
                .Include(p => p.loanApprovalStages)
                .Include(p => p.loanApprovalStages.Select(q => q.loanApprovalStageOfficers))
                .FirstOrDefault(p => p.loanTypeID == id);
            if (loanTyp == null)
            {
                loanTyp = new loanType
                {
                    loanApprovalStages = new List<loanApprovalStage>()
                };
            }
            return loanTyp;
        }


        [HttpPost]
        public string Post(loanType lnType)
        {
            var loanType = le.loanTypes.FirstOrDefault(p => p.loanTypeID == lnType.loanTypeID);
            if(loanType == null) { throw new ApplicationException("Loan Product cannot be null");}
            foreach (var value in lnType.loanApprovalStages)
            {
                if (value == null) return null;
                //Validate the input value

                if (value.loanApprovalStageId > 0)
                {
                    var toBeSaved = le.loanApprovalStages
                        .Include(p => p.loanApprovalStageOfficers)
                        .FirstOrDefault(p => p.loanApprovalStageId == value.loanApprovalStageId);
                    populateFields(toBeSaved, value);
                }
                else
                {
                    loanApprovalStage toBeSaved = new loanApprovalStage {loanTypeId = loanType.loanTypeID};
                    populateFields(toBeSaved, value);
                    //loanType.loanApprovalStages.Add(toBeSaved);
                    le.loanApprovalStages.Add(toBeSaved);
                }
                
            }
            

            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                throw x;
            }
            return "Approval stage saved succesfully";
        }

        private void populateFields(loanApprovalStage target, loanApprovalStage source)
        {
            //target.loanTypeId = source.loanTypeId;
            target.name = source.name;
            target.isMandatory = source.isMandatory;
            target.ordinal = source.ordinal;
            
            if (source.loanApprovalStageOfficers.Count < 1)
                throw new ApplicationException("Please add Approval Officer");

            foreach (var stageOfficer in source.loanApprovalStageOfficers)
            {
                //Remove deleted loanApprovalStageOfficers from loanApprovalStage
                List<int> stageOfficerIds = source.loanApprovalStageOfficers.Select(p => p.loanApprovalStageOfficerId).ToList();
                foreach (loanApprovalStageOfficer staOff in target.loanApprovalStageOfficers)
                {
                    if (!stageOfficerIds.Contains(staOff.loanApprovalStageOfficerId))
                        le.loanApprovalStageOfficers.Remove(staOff);
                }

                if (stageOfficer.loanApprovalStageOfficerId < 1)
                {
                    var tobeSaved = new loanApprovalStageOfficer();
                    populateStageOfficerFields(tobeSaved, stageOfficer);
                    target.loanApprovalStageOfficers.Add(tobeSaved);
                }
            }

        }

        private void populateStageOfficerFields(loanApprovalStageOfficer target, loanApprovalStageOfficer source)
        {
            target.profileType = source.profileType;
            target.profileValue = source.profileValue;
        }



    }
}









