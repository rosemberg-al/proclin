using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Base
{
    public interface IRepositoryBase<TContext>
        where TContext : DbContext
    {
        IUnitOfWork<TContext> UnitOfWork { get; }
    }
}