using Clinicas.Application.Services.Interfaces;
using Clinicas.Domain.Model;
using Clinicas.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clinicas.Domain.DTO;
using Clinicas.Domain.ViewModel;
using Clinicas.Domain.ViewModel.Relatorios;

namespace Clinicas.Application.Services
{
    public class RelatorioService : IRelatorioService
    {
        private readonly IRelatorioRepository _repository;

        public RelatorioService(IRelatorioRepository repository)
        {
            _repository = repository;
        }

        public ICollection<RelProcedimentosRealizados> ProcedimentosRealizados(DateTime periodoInicial, DateTime periodoFinal, int idclinica)
        {
            return _repository.ProcedimentosRealizados(periodoInicial, periodoFinal,idclinica);
        }

        public ICollection<RelAgendaMedica> RelAgendaMedica(DateTime datainicio, DateTime datatermino, int? idprofissional, int? idpaciente, string situacao, int idclinica)
        {
            return _repository.RelAgendaMedica(datainicio, datatermino, idprofissional, idpaciente, situacao, idclinica);
        }

        public ICollection<RelAniversariante> RelAniversariantes(string mes,int idclinica)
        {
            return _repository.RelAniversariantes(mes,idclinica);
        }

        public ICollection<RelCheque> RelCheque(DateTime datainicio, DateTime datatermino, string situacao,int idclinica)
        {
            return _repository.RelCheque(datainicio, datatermino,situacao,idclinica);
        }

        public ICollection<RelConvenio> RelConvenio(int idclinica)
        {
            return _repository.RelConvenio(idclinica);
        }

        public ICollection<RelEspecialidade> RelEspecialidade()
        {
            return _repository.RelEspecialidade();
        }

        public ICollection<RelProcedimento> RelProcedimentos()
        {
            return _repository.RelProcedimento();
        }

        public ICollection<RelPaciente> RelPacientes(int idclinica)
        {
            return _repository.RelPacientes(idclinica);
        }

        public ICollection<RelFinanceiro> RelFinanceiro(DateTime datainicio, DateTime datatermino, string tipo, string situacao, int idpessoa,int idclinica)
        {
            return _repository.RelFinanceiro(datainicio,datatermino,tipo,situacao,idpessoa,idclinica);
        }

        public ICollection<RelFornecedor> RelFornecedor(int idclinica)
        {
            return _repository.RelFornecedor(idclinica);
        }

        public ICollection<RelOcupacao> RelOcupacao()
        {
            return _repository.RelOcupacao();
        }

        public ICollection<RelAgendaMedica> RelFaturamento(DateTime datainicio, DateTime datatermino, int? idprofissional, int? idpaciente, string situacao, string tipo, int idclinica)
        {
            return _repository.RelFaturamento(datainicio, datatermino, idprofissional, idpaciente, situacao,tipo, idclinica);
        }
        public ICollection<RelPlanoAgenda> RelPlanoDeAgenda(DateTime datainicio, DateTime datatermino, int? idprofissional, int? idUnidade, int idclinica)
        {
            return _repository.RelPlanoDeAgenda(datainicio, datatermino, idprofissional, idUnidade, idclinica);
        }

    }
}