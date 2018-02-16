using Clinicas.Infrastructure.Base;
using Clinicas.Infrastructure.Context;
using Clinicas.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clinicas.Domain.Model;
using System.Data.Entity;
using Clinicas.Domain.ViewModel;
using Clinicas.Domain.ViewModel.Relatorios;

namespace Clinicas.Infrastructure.Repository
{
    public class RelatorioRepository : RepositoryBase<ClinicasContext>, IRelatorioRepository
    {
        public RelatorioRepository(IUnitOfWork<ClinicasContext> unit)
            : base(unit)
        {
        }

        public ICollection<RelAgendaMedica> RelAgendaMedica(DateTime datainicio, DateTime datatermino, int? idprofissional, int? idpaciente, string situacao,int idclinica)
        {
            string sql = " select * from vw_agenda where Data BETWEEN '" + datainicio.ToString("yyyy-MM-dd") + "' AND '" + datatermino.ToString("yyyy-MM-dd") + "' and IdClinica = '"+ idclinica + "'   ";
            if (idprofissional > 0)
                sql += " and vw_agenda.IdProfissional = '" + idprofissional + "' ";

            if (idpaciente > 0)
                sql += " and vw_agenda.IdPaciente = '" + idpaciente + "' ";

            if(situacao!="Todos")
                sql +=" and vw_agenda.Situacao='"+situacao+"' ";
            
            return Context.Database.SqlQuery<RelAgendaMedica>(sql).ToList();
        }

        public ICollection<RelAniversariante> RelAniversariantes(string mes,int idclinica)
        {
            string sql = "  select * from vw_rel_aniversariantes where MONTH(DataNascimento) = '" + mes +"' ";
            return Context.Database.SqlQuery<RelAniversariante>(sql).ToList();
        }

        public ICollection<RelCheque> RelCheque(DateTime datainicio, DateTime datatermino, string situacao,int idclinica)
        {
            string sql = " select * from vw_rel_cheque where BomPara BETWEEN '" + datainicio.ToString("yyyy-MM-dd") + "' AND '" + datatermino.ToString("yyyy-MM-dd") + "' ";

            sql += " and vw_rel_cheque.Situacao != 'Excluido' ";

            if (situacao != "Todos")
                sql += " and vw_rel_cheque.Situacao = '" + situacao + "' ";
            
            return Context.Database.SqlQuery<RelCheque>(sql).ToList();
        }

        public ICollection<RelConvenio> RelConvenio(int idclinica)
        {
           string sql = " select * from vw_rel_convenio order by vw_rel_convenio.Nome ";
           return Context.Database.SqlQuery<RelConvenio>(sql).ToList();
        }

        public ICollection<RelEspecialidade> RelEspecialidade()
        {
            string sql = " select * from vw_especialidade order by vw_especialidade.NmEspecialidade ";
            return Context.Database.SqlQuery<RelEspecialidade>(sql).ToList();
        }

        public ICollection<RelFornecedor> RelFornecedor(int idclinica)
        {
            string sql = " select * from vw_rel_fornecedores order by vw_rel_fornecedores.Nome ";
            return Context.Database.SqlQuery<RelFornecedor>(sql).ToList();
        }

        public ICollection<RelOcupacao> RelOcupacao()
        {
            string sql = " select * from vw_ocupacao order by vw_ocupacao.NmOcupacao ";
            return Context.Database.SqlQuery<RelOcupacao>(sql).ToList();
        }

        public ICollection<RelFinanceiro> RelFinanceiro(DateTime datainicio, DateTime datatermino, string tipo, string situacao, int idpessoa,int idclinica)
        {
            string sql = " select * from vw_rel_financeiro where DataVencimento BETWEEN '" + datainicio.ToString("yyyy-MM-dd") + "' AND '" + datatermino.ToString("yyyy-MM-dd") + "' ";
            sql += " and vw_rel_financeiro.Tipo = '" + tipo + "' ";

            if (idpessoa>0)
                sql += " and vw_rel_financeiro.IdPessoa = '" + idpessoa + "' ";

            if (situacao != "Todos")
                sql += " and vw_rel_financeiro.Situacao = '" + situacao + "' ";

            return Context.Database.SqlQuery<RelFinanceiro>(sql).ToList();
        }

        public ICollection<RelProcedimentosRealizados> ProcedimentosRealizados(DateTime periodoInicial, DateTime periodoFinal,int idclinica)
        {
           return Context.Database.SqlQuery<RelProcedimentosRealizados>(" select `Funcionario`.`IdFuncionario` AS `IdFuncionario`,`Pessoa`.`Nome` AS `Nome`,count(0) AS `Total` from ((`Agenda` join `Funcionario` on((`Agenda`.`IdFuncionario` = `Funcionario`.`IdFuncionario`))) join `Pessoa` on((`Funcionario`.`IdFuncionario` = `Pessoa`.`IdPessoa`))) " +
                "where (`Agenda`.`Situacao` = 'Realizado') and Data BETWEEN '" + periodoInicial.ToString("yyyy-MM-dd") + "' AND '" + periodoFinal.ToString("yyyy-MM-dd") + "' group by `Pessoa`.`Nome` ").ToList();
        }

        public ICollection<RelProcedimento> RelProcedimento()
        {
            string sql = "  select * from vw_rel_procedimentos  ";
            return Context.Database.SqlQuery<RelProcedimento>(sql).ToList();
        }

        public ICollection<RelPaciente> RelPacientes(int idclinica)
        {
            string sql = "  select * from vw_rel_pacientes  ";
            return Context.Database.SqlQuery<RelPaciente>(sql).ToList();
        }

        public ICollection<RelPlanoAgenda> RelPlanoDeAgenda(DateTime datainicio, DateTime datatermino, int? idprofissional, int? idUnidade, int idclinica)
        {
            string sql = "SET lc_time_names = 'pt_PT'; select * from vw_rel_plano_agenda where Data BETWEEN '" + datainicio.ToString("yyyy-MM-dd") + "' AND '" + datatermino.ToString("yyyy-MM-dd") + "' and IdClinica = '" + idclinica + "' ";
            if (idprofissional > 0)
                sql += " and vw_rel_plano_agenda.IdProfissional = '" + idprofissional + "' ";

            if (idUnidade > 0)
                sql += " and vw_rel_plano_agenda.IdUnidadeAtendimento = '" + idUnidade + "' ";


            return Context.Database.SqlQuery<RelPlanoAgenda>(sql).ToList();
        }

        public ICollection<RelAgendaMedica> RelFaturamento(DateTime datainicio, DateTime datatermino, int? idprofissional, int? idpaciente, string situacao,string tipo, int idclinica)
        {
            string sql = " select * from vw_rel_faturamento where Data BETWEEN '" + datainicio.ToString("yyyy-MM-dd") + "' AND '" + datatermino.ToString("yyyy-MM-dd") + "' and IdClinica = '" + idclinica + "' and Tipo = '" + tipo + "'   ";
            if (idprofissional > 0)
                sql += " and vw_rel_faturamento.IdProfissional = '" + idprofissional + "' ";

            if (idpaciente > 0)
                sql += " and vw_rel_faturamento.IdPaciente = '" + idpaciente + "' ";

            if (situacao != "Todos")
                sql += " and vw_rel_faturamento.Situacao='" + situacao + "' ";

            return Context.Database.SqlQuery<RelAgendaMedica>(sql).ToList();
        }

    }
}