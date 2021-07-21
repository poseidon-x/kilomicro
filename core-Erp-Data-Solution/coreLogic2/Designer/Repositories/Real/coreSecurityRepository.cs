using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity; 
using System.Linq.Expressions;

namespace coreLogic
{
    public class coreSecurityRepository<TEntity>
        : genericRepository<TEntity>, ICoreSecurityRepository<TEntity> where TEntity : class
    {
        public coreSecurityRepository(coreSecurityEntities context)
            : base(context)
        { 
        }
    }
}