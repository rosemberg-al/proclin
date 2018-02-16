using Clinicas.Domain.DTO;
using Clinicas.Domain.Model;
using Clinicas.Domain.ViewModel;
using Clinicas.Infrastructure.Base;
using Clinicas.Infrastructure.Context;
using Clinicas.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Clinicas.Domain.ViewModel.Relatorios;

namespace Clinicas.Infrastructure.Repository
{
    public class DashboardRepository : RepositoryBase<ClinicasContext>, IDashboardRepository
    {
        public DashboardRepository(IUnitOfWork<ClinicasContext> unit)
            : base(unit)
        {
        }

        public AtendimentosDTO AgendamentoDia(int idclinica,int idunidade)
        {
            throw new NotImplementedException();
        }

        public int QtdeAgendamentos(int idclinica, int idunidade)
        {
            throw new NotImplementedException();
        }

        public int QtdeAtendimentosConvenio(int idclinica, int idunidade)
        {
            throw new NotImplementedException();
        }

        //public int QtdeAtendimentosParticular()
        //{
        //    return Context.AgendaProcedimento.Include(x=>x.Agenda).Where(x=>x.Tipo=="P" && x.Agenda.Situacao=="Realizado").Count();
        //}

        public int QtdePacientes(int idclinica, int idunidade)
        {
            return Context.Paciente.Include(x => x.Pessoa).Where(x => x.Situacao == "Ativo" 
            && x.Pessoa.IdClinica==idclinica).Count();
        }

        public AtendimentosDTO ResumoAtendimentosPorMes(int idclinica,int idunidadeatendimento)
        {
            throw new NotImplementedException();
        }

        #region Financeiro

        public decimal TotalPagar(int idclinica,int idunidade)
        {
            return Context.FinanceiroParcelas.Include(x => x.Financeiro)
                .Where(x => x.Situacao == "Aberto" 
                && x.Financeiro.Tipo == "Contas a Pagar" 
                && x.Financeiro.IdClinica == idclinica && x.Financeiro.IdUnidadeAtendimento == idunidade).Sum(x => x.Valor);
        }

        public decimal TotalReceber(int idclinica,int idunidade)
        {
            return Context.FinanceiroParcelas
                .Include(x => x.Financeiro)
                .Where(x => x.Situacao == "Aberto" && x.Financeiro.Tipo == "Contas a Receber"
                && x.Financeiro.IdClinica == idclinica && x.Financeiro.IdUnidadeAtendimento == idunidade).Sum(x => x.Valor);
        }

        public decimal TotalaContasPagas(int idclinica, int idunidade)
        {
            return Context.FinanceiroParcelas
                .Include(x => x.Financeiro)
                .Where(x => x.Situacao == "Baixado" 
                && x.Financeiro.Tipo == "Contas a Pagar" 
                && x.Financeiro.IdClinica == idclinica && x.Financeiro.IdUnidadeAtendimento == idunidade).Sum(x => x.Valor);
        }

        public decimal TotalContasRecebidas(int idclinica, int idunidade)
        {
            return Context.FinanceiroParcelas.Include(x => x.Financeiro)
                .Where(x => x.Situacao == "Baixado" && x.Financeiro.Tipo == "Contas a Receber" 
                && x.Financeiro.IdClinica == idclinica && x.Financeiro.IdUnidadeAtendimento == idunidade).Sum(x => x.Valor);
        }

        public List<RelDespesasCategoria> DespesasPorCategoria(int idclinica,int idunidade)
        {
            string sql = "select SUM(P.Valor) Valor, C.NmPlanoConta PlanoConta  from Financeiro F, FinanceiroParcela P, PlanoConta C " +
                          "  where F.IdFinanceiro = P.IdFinanceiro " +
                          "  and C.IdPlanoConta = P.IdPlanoConta " +
                          "  and F.IdClinica = " + idclinica + " " +
                          "  and F.IdUnidadeAtendimento = " + idunidade + " " +
                          "  and F.Tipo = 'Contas a Pagar' " +
                           " AND P.Situacao = 'Baixado' " +
                          "  GROUP BY C.NmPlanoConta";

            return Context.Database.SqlQuery<RelDespesasCategoria>(sql).ToList();
        }

        public RelReceitasPorDespesas ReceitasPorDespesas(int idclinica,int idunidade)
        {
            //somente do ano atual
            var ano = DateTime.Now.Year;

            var model = new RelReceitasPorDespesas();
            string sqlReceber = "SELECT month(P.DataAcerto) as Mes, " +
                         "            year(P.DataAcerto) as Ano, " +
                         "            Sum(P.Valor) Valor " +
                         "      from Financeiro F, FinanceiroParcela P " +
                         "      WHERE F.IdFinanceiro = P.IdFinanceiro " +
                         "   and F.IdClinica = " + idclinica + " " +
                         "   and F.IdUnidadeAtendimento = " + idunidade + " " +
                         "   and F.Tipo = 'Contas a Receber' " +
                         "   AND P.Situacao = 'Baixado' " +
                         "   and year(P.DataAcerto) = " + ano + " " +
                         "   GROUP BY ano, mes " +
                         "   ORDER BY ano, mes asc";

            var receber = Context.Database.SqlQuery<DadosReceita>(sqlReceber).ToList();
            model.Receitas = receber;

            string sqlPagar = "SELECT month(P.DataAcerto) as Mes, " +
                         "            year(P.DataAcerto) as Ano, " +
                         "            Sum(P.Valor) Valor " +
                         "      from Financeiro F, FinanceiroParcela P " +
                         "      WHERE F.IdFinanceiro = P.IdFinanceiro " +
                         "   and F.IdClinica = " + idclinica + " " +
                         "   and F.IdUnidadeAtendimento = " + idunidade + " " +
                         "   and F.Tipo = 'Contas a Pagar' " +
                         " AND P.Situacao = 'Baixado' " +
                         "   and year(P.DataAcerto) = " + ano + " " +
                         "   GROUP BY ano, mes " +
                         "   ORDER BY ano, mes asc";

            var pagar = Context.Database.SqlQuery<DadosReceita>(sqlPagar).ToList();
            model.Despesas = pagar;

            return model;
        }
        #endregion
    }
}