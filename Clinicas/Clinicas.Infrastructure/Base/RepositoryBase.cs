using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Base
{
    public abstract class RepositoryBase<TContext>
         where TContext : DbContext
    {
        protected readonly TContext Context;
        protected IUnitOfWork<TContext> _uow;

        public IUnitOfWork<TContext> UnitOfWork
        {
            get { return _uow; }
        }

        private RepositoryBase()
        {
        }

        public RepositoryBase(TContext context)
            : this()
        {
            Context = context;
        }

        public RepositoryBase(IUnitOfWork<TContext> uow)
            : this(uow.Context)
        {
            _uow = uow;
        }
    }
}