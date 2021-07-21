using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity; 
using System.Linq.Expressions;

namespace coreLogic
{
    public class coreLoansRepository<TEntity>
        : genericRepository<TEntity>, ICoreLoansRepository<TEntity> where TEntity : class
    { 
        public coreLoansRepository(coreLoansEntities context) : base(context)
        { 
        }
    }
}