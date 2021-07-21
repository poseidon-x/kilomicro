using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity; 
using System.Linq.Expressions;

namespace coreLogic
{
    public class coreReportsRepository<TEntity> 
        : genericRepository<TEntity>, ICoreReportsRepository<TEntity> where TEntity : class
    {
        public coreReportsRepository(coreReports.reportEntities context)
            : base(context)
        { 
        }
    }
}