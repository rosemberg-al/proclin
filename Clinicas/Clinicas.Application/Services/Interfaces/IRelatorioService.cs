using Clinicas.Domain.Model;
using Clinicas.Domain.ViewModel;
using Clinicas.Domain.ViewModel.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Application.Services.Interfaces
{
    public interface IRelatorioService
    {
        ICollection<RelAgendaMedica> RelAgendaMedica(DateTime datainicio, DateTime datatermino, int? idprofissional, int? idpaciente, string situacao, int idclinica);
        ICollection<RelProcedimentosRealizados> ProcedimentosRealizados(DateTime periodoInicial, DateTime periodoFinal,int idclinica);
        ICollection<RelAniversariante> RelAniversariantes(string mes,int idclinica);
        ICollection<RelCheque> RelCheque(DateTime datainicio, DateTime datatermino, string situacao,int idclinica);
        ICollection<RelOcupacao> RelOcupacao();
        ICollection<RelEspecialidade> RelEspecialidade();
        ICollection<RelFinanceiro> RelFinanceiro(DateTime datainicio, DateTime datatermino, string tipo, string situacao, int idpessoa,int idclinica);
        ICollection<RelFornecedor> RelFornecedor(int idclinica);
        ICollection<RelConvenio> RelConvenio(int idclinica);
        ICollection<RelProcedimento> RelProcedimentos();
        ICollection<RelPaciente> RelPacientes(int idclinica);
        ICollection<RelAgendaMedica> RelFaturamento(DateTime datainicio, DateTime datatermino, int? idprofissional, int? idpaciente, string situacao, string tipo, int idclinica);
        ICollection<RelPlanoAgenda> RelPlanoDeAgenda(DateTime datainicio, DateTime datatermino, int? idprofissional, int? idUnidade, int idclinica);
        
    }
}