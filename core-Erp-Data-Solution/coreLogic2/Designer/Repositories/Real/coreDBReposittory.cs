using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity; 
using System.Linq.Expressions;

namespace coreLogic
{
    public class coreDBReposittory<TEntity>
        : genericRepository<TEntity>, ICoreDBRepository<TEntity> where TEntity : class
    { 
        public coreDBReposittory(core_dbEntities context) : base (context)
        { 
        }
    }
}