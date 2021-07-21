using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace coreLogic
{
    public class CheckCoreDbEntities : core_dbEntities
    {
        protected bool ValidateTotalDebit()
        {
            //Retrieve all changes in journal batch
            var changes = this.ChangeTracker.Entries<jnl_batch>().Where(
                p => p.State == EntityState.Added);

            //Validate changes and save
            foreach (var change in changes)
            {
                var batch = change.Entity;
                var totalDiff = batch.jnl.Sum(p => p.dbt_amt) - batch.jnl.Sum(p => p.crdt_amt);

                if (totalDiff > 0.1)
                {
                    return false;
                }
            }

            return true;
        }

        protected bool ValidateTotalDebitTemp()
        {
            //Retrieve all changes in journal batch
            var changes = this.ChangeTracker.Entries<jnl_batch_tmp>().Where(
                p => p.State == EntityState.Added || p.State == EntityState.Modified);
        
            //Validate changes and save
            foreach (var change in changes)
            {
                var batch = change.Entity;
                var totalDiff = batch.jnl_tmp.Sum(p => p.dbt_amt) - batch.jnl_tmp.Sum(p => p.crdt_amt);

                if (totalDiff > 0.1)
                {
                    return false;
                }
            }

            return true;
        }

        public bool saveChangesWithChecks()
        {
            if (ValidateTotalDebit()&& ValidateTotalDebitTemp())
            {
                SaveChanges();
                return true;
            }

            return false;

        }
    }
}
