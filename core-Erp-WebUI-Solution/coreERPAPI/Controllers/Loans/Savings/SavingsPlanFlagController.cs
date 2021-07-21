using coreLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web.Http;
using coreERP.Providers;


namespace coreERP.Controllers.Loans.Setups
{
    public class SavingsPlanFlagController : ApiController
    {
        coreLoansEntities le = new coreLoansEntities();
        core_dbEntities ent = new core_dbEntities();

        public SavingsPlanFlagController()
        {
            le.Configuration.LazyLoadingEnabled = false; 
            le.Configuration.ProxyCreationEnabled = false;
        }

        [HttpGet]
        public IEnumerable<savingPlanFlag> Get()
        {
            var flags = le.savingPlanFlags
                .Include(p=> p.saving)
                .Include(p=> p.saving.client)
                .Include(p=> p.saving.savingType)
                .Where(p => p.approved == null)
                .ToList();

            return flags;
        }

        [HttpGet]
        public IEnumerable<savingPlanFlag> GetUnApplied()
        {
            var flags = le.savingPlanFlags
                .Include(p => p.saving)
                .Include(p => p.saving.client)
                .Include(p => p.saving.savingType)
                .Where(p => p.approved == true && p.applied == null)
                .ToList();

            return flags;
        }

        [HttpPost]
        public savingPlanFlag Approve(long id)
        {
            var flag = le.savingPlanFlags.FirstOrDefault(p => p.savingPlanFlagID == id);
            if (flag != null)
            {
                flag.approved = true;
                flag.approvedBy = "TO_BE_SET";
                flag.approvedDate = DateTime.Now;
            }
            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                if (x.InnerException != null) x = x.InnerException;
                if (x.InnerException != null) x = x.InnerException;
                if (x.InnerException != null) x = x.InnerException;
                if (x.InnerException != null) x = x.InnerException;
                throw new ApplicationException(x.Message);
            }
            return flag;
        }

        

        [HttpPost]
        public savingPlanFlag Dispprove(long id)
        {
            var flag = le.savingPlanFlags.FirstOrDefault(p => p.savingPlanFlagID == id);
            if (flag != null)
            {
                flag.approved = false;
                flag.approvedBy = LoginHelper.getCurrentUser(new coreSecurityEntities()); 
                flag.approvedDate = DateTime.Now;
            }
            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                if (x.InnerException != null) x = x.InnerException;
                if (x.InnerException != null) x = x.InnerException;
                if (x.InnerException != null) x = x.InnerException;
                if (x.InnerException != null) x = x.InnerException;
                throw new ApplicationException(x.Message);
            }
            return flag;
        }

        

        [HttpPost]
        public savingPlanFlag Apply(long id)
        { 
            var flag = le.savingPlanFlags
                .Include(p=> p.saving)
                .FirstOrDefault(p => p.savingPlanFlagID == id && p.approved==true && (p.applied == null || p.applied==false));
            if (flag != null)
            {
                var type=le.savingTypes.First(p => p.savingTypeID == flag.proposedPlanId);
                flag.applied = true;
                flag.saving.savingTypeID = flag.proposedPlanId;
                flag.saving.savingPlanID = type.planID.Value;
                flag.saving.interestRate = type.interestRate;
                flag.appliedDate = DateTime.Now;
                flag.appliedBy = "TO_BE_SET";
            }
            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                if (x.InnerException != null) x = x.InnerException;
                if (x.InnerException != null) x = x.InnerException;
                if (x.InnerException != null) x = x.InnerException;
                if (x.InnerException != null) x = x.InnerException;
                throw new ApplicationException(x.Message);
            }
            return flag;
        }

        [HttpPost]
        public savingPlanFlag Reject(long id)
        {
            var flag = le.savingPlanFlags
                .Include(p => p.saving)
                .FirstOrDefault(p => p.savingPlanFlagID == id && p.approved == true && (p.applied == null || p.applied == false));
            if (flag != null)
            {
                flag.applied = false;
                flag.appliedDate = DateTime.Now;
                flag.appliedBy = "TO_BE_SET";
            }
            try
            {
                le.SaveChanges();
            }
            catch (Exception x)
            {
                if (x.InnerException != null) x = x.InnerException;
                if (x.InnerException != null) x = x.InnerException;
                if (x.InnerException != null) x = x.InnerException;
                if (x.InnerException != null) x = x.InnerException;
                throw new ApplicationException(x.Message);
            }
            return flag;
        }

    }
}
