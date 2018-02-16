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
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;

        public DashboardService(IDashboardRepository repository)
        {
            _repository = repository;
        }

        public AtendimentosDTO AgendamentoDia(int idclinica, int idunidade)
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
        //   return _repository.QtdeAtendimentosParticular();
        //}

        public int QtdePacientes(int idclinica, int idunidade)
        {
            return _repository.QtdePacientes(idclinica, idunidade);
        }

        public AtendimentosDTO ResumoAtendimentosPorMes(int idclinica, int idunidade)
        {
            throw new NotImplementedException();
        }

        public decimal TotalPagar(int idclinica,int idunidade)
        {
            return _repository.TotalPagar(idclinica,idunidade);
        }

        public decimal TotalReceber(int idclinica, int idunidade)
        {
            return _repository.TotalReceber(idclinica, idunidade);
        }
        public decimal TotalaContasPagas(int idclinica, int idunidade)
        {
            return _repository.TotalaContasPagas(idclinica, idunidade);
        }

        public decimal TotalContasRecebidas(int idclinica, int idunidade)
        {
            return _repository.TotalContasRecebidas(idclinica, idunidade);
        }
        public List<RelDespesasCategoria> DespesasPorCategoria(int idclinica, int idunidade)
        {
            return _repository.DespesasPorCategoria(idclinica, idunidade);
        }
        public RelReceitasPorDespesas ReceitasPorDespesas(int idclinica, int idunidade)
        {
            return _repository.ReceitasPorDespesas(idclinica, idunidade);
        }
    }
}