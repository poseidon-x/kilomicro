using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Linq.Expressions;

namespace coreLogic
{
    public interface ICoreCreditUnionRepository<TEntity> : ICoreRepository<TEntity> where TEntity : class
    {
    }
}