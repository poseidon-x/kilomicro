using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity; 
using System.Linq.Expressions;

namespace coreLogic
{
    public class coreCommerceRepository<TEntity> 
        : genericRepository<TEntity>, ICoreCommerceRepository<TEntity> where TEntity : class
    {
        public coreCommerceRepository(CommerceEntities context)
            : base(context)
        { 
        }
    }
}