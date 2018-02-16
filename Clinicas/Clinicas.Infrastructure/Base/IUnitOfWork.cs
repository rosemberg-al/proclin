using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace Clinicas.Infrastructure.Base
{
    public interface IUnitOfWork<TContext>
         where TContext : DbContext
    {
        DbContextTransaction BeginTransation();

        bool SaveChanges();
        void AddToContext<T>(T entity)
            where T : class;
        void LoadReference<TEntity, TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> property)
            where TEntity : class
            where TProperty : class;
        TContext Context { get; }
    }
}