using Clinicas.Domain.DTO;
using Clinicas.Domain.ViewModel;
using Clinicas.Infrastructure.Base;
using Clinicas.Infrastructure.Context;
using Clinicas.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clinicas.Domain.Tiss;
using System.Data.Entity;
using System.Linq.Expressions;
using Clinicas.Domain.Model;

namespace Clinicas.Infrastructure.Repository
{
    public class GuiaRepository : RepositoryBase<ClinicasContext>, IGuiaRepository
    {
        public GuiaRepository(IUnitOfWork<ClinicasContext> unit)
            : base(unit)
        {
        }

        public void Excluir(Guia guia)
        {
            guia.Situacao = "Excluida";
            Context.Entry(guia).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public List<Guia> ListarGuias()
        {
            return Context.Guia.ToList();
        }

        public List<Guia> ListarGuiasPorTipo(string tipoGuia, int idClinica)
        {
            return Context.Guia.Where(x => x.TipoGuia == tipoGuia && x.IdClinica == idClinica).ToList();
        }

        public List<Guia> ListarGuiasBuscaAvancada(BuscaGuiaViewModel model)
        {
            if (!string.IsNullOrEmpty(model.NumeroGuia))
                return Context.Guia.Where(x => x.CabecalhoGuia.NumeroGuia == model.NumeroGuia).ToList();
            else if (model.DataInicio != DateTime.MinValue && model.DataFim != DateTime.MinValue)
                return Context.Guia.Where(x => x.CabecalhoGuia.DataEmissaoGuia >= model.DataInicio && x.CabecalhoGuia.DataEmissaoGuia <= model.DataFim).ToList();
            else if (!string.IsNullOrEmpty(model.NomePaciente))
                return Context.Guia.Where(x => x.DadosBeneficiario.Nome.ToUpper().StartsWith(model.NomePaciente.ToUpper())).ToList();
            else if (!string.IsNullOrEmpty(model.Profissional))
                return Context.Guia.Where(x => x.DadosContratado.NomeProfissionalExecutante.ToUpper().StartsWith(model.Profissional.ToUpper())).ToList();
            else
                return new List<Guia>();
        }

        public List<Guia> ListarGuiasPorConvenio(int idConvenio, int idClinica, string tipoGuia)
        {
            return Context.Guia.Include(x => x.Lote).Include(x => x.Convenio).Include(x => x.Convenio.Pessoa).Where(x => x.IdConvenio == idConvenio && x.IdClinica == idClinica && x.TipoGuia == tipoGuia && x.Situacao != "Faturada").ToList();
        }

        public Guia ObterGuiaPorId(int idguia)
        {
            return Context.Guia.First(x => x.IdGuia == idguia);
        }

        public void Salvar(Guia guia)
        {
            if (guia.IdGuia > 0)
            {
                Context.Entry(guia).State = EntityState.Modified;
            }
            else
            {
                Context.Guia.Add(guia);
            }
            Context.SaveChanges();
        }

        public ICollection<Guia> Pesquisar(int? idGuia, string nome)
        {
            ICollection<Guia> resultado = null;

            Expression<Func<Guia, bool>> filtroIdGuia = registro => true;
            Expression<Func<Guia, bool>> filtroNome = registro => true;


            if (idGuia > 0)
                filtroIdGuia = (Guia registro) =>
                                          registro.IdGuia == idGuia;

            if (!string.IsNullOrEmpty(nome))
                filtroNome = (Guia registro) =>
                           registro.DadosBeneficiario.Nome.Contains(nome);

            resultado = Context.Guia
                        .Where(filtroIdGuia).Where(filtroNome).ToList();

            return resultado;
        }

        public void CancelarGuia(int idguia)
        {
            var guia = Context.Guia.Find(idguia);
            guia.Cancelar();
            Context.Entry(guia).State = EntityState.Modified;
            Context.SaveChanges();
        }

        #region [Lote Guias]

        public Lote SalvarLote(Lote lote)
        {
            if (lote.IdLote > 0)
            {
                Context.Entry(lote).State = EntityState.Modified;
            }
            else
            {
                Context.Lote.Add(lote);
            }
            Context.SaveChanges();
            return lote;
        }

        public void ExcluirLote(Lote lote)
        {
            Context.Lote.Remove(lote);
            Context.SaveChanges();
        }

        public List<Lote> ListarLotes(int idClinica)
        {
            return Context.Lote.Include(x => x.Convenio).Include(x => x.Convenio.Pessoa).Where(x => x.IdClinica == idClinica).ToList();
        }

        public List<Lote> ListarLotesPorConvenio(int idClinica, int idConvenio)
        {
            return Context.Lote.Include(x => x.Convenio).Where(x => x.IdClinica == idClinica && x.IdConvenio == idConvenio).ToList();
        }

        public Lote ObterLotePorId(int idLote)
        {
            return Context.Lote.Include(x => x.Convenio).Include(z => z.Guias).Include(z => z.Clinica).First(x => x.IdLote == idLote);
        }

        #endregion
    }
}