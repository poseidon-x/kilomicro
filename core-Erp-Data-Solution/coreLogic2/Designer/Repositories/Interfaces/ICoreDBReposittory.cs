﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Linq.Expressions;

namespace coreLogic
{
    public interface ICoreDBRepository<TEntity> : ICoreRepository<TEntity> where TEntity : class
    {
    }
}