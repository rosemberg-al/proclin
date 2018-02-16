using Clinicas.Domain.DTO;
using Clinicas.Domain.Model;
using Clinicas.Domain.ViewModel;
using Clinicas.Infrastructure.Base;
using Clinicas.Infrastructure.Context;
using Clinicas.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Linq.Expressions;
using Clinicas.Domain.Mail;

namespace Clinicas.Infrastructure.Repository
{
    public class BuscaRepository : RepositoryBase<ClinicasContext>, IBuscaRepository
    {
        public BuscaRepository(IUnitOfWork<ClinicasContext> unit)
            : base(unit)
        {
        }

        public ICollection<BuscaViewModel> Busca(string search)
        {
            return Context.Database.SqlQuery<BuscaViewModel>(" select * from Busca where Busca.Descricao LIKE '%" + search + "%'  ").ToList();
        }
    }
}