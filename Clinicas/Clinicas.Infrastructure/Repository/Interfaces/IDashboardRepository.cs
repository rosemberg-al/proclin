using Clinicas.Domain.DTO;
using Clinicas.Domain.ViewModel.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Repository.Interfaces
{
    public interface IDashboardRepository
    {
        decimal TotalPagar(int idclinica, int idunidade);
        decimal TotalReceber(int idclinica, int idunidade);
        decimal TotalaContasPagas(int idclinica, int idunidade);
        decimal TotalContasRecebidas(int idclinica, int idunidade);
        List<RelDespesasCategoria> DespesasPorCategoria(int idclinica, int idunidade);
        RelReceitasPorDespesas ReceitasPorDespesas(int idclinica, int idunidade);
        //int QtdeAtendimentosParticular();
        int QtdeAtendimentosConvenio(int idclinica, int idunidade);
        int QtdePacientes(int idclinica, int idunidade);
        int QtdeAgendamentos(int idclinica, int idunidade);
        AtendimentosDTO ResumoAtendimentosPorMes(int idclinica, int idunidade);
        AtendimentosDTO AgendamentoDia(int idclinica, int idunidade);
    }
}