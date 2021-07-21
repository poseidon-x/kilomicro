using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity; 
using System.Linq.Expressions;

namespace coreLogic
{
    public class coreMomoRepository<TEntity>
        : genericRepository<TEntity>, ICoreMomoRepository<TEntity> where TEntity : class
    {
        public coreMomoRepository(momoModelsConnectionString context)
            : base(context)
        { 
        }
    }
}