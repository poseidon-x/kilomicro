using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity; 
using System.Linq.Expressions;

namespace coreLogic
{
    public class coreGLRepository<TEntity>
        : genericRepository<TEntity>, ICoreGLRepository<TEntity> where TEntity : class
    {
        public coreGLRepository(momoModelsConnectionString context)
            : base(context)
        { 
        }
    }
}