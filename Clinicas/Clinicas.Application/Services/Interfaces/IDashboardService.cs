using Clinicas.Domain.DTO;
using Clinicas.Domain.ViewModel.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Application.Services.Interfaces
{
    public interface IDashboardService
    {
        // todos os meses 
        decimal TotalPagar(int idClinica, int idunidadeatendimento);
        decimal TotalReceber(int idClinica, int idunidadeatendimento);
        decimal TotalaContasPagas(int idClinica, int idunidadeatendimento);
        decimal TotalContasRecebidas(int idClinica, int idunidadeatendimento);
        List<RelDespesasCategoria> DespesasPorCategoria(int idClinica, int idunidadeatendimento);
        RelReceitasPorDespesas ReceitasPorDespesas(int idClinica, int idunidadeatendimento);
        //int QtdeAtendimentosParticular();
        int QtdeAtendimentosConvenio(int idClinica, int idunidadeatendimento);
        int QtdePacientes(int idClinica, int idunidadeatendimento);
        int QtdeAgendamentos(int idClinica, int idunidadeatendimento);
        AtendimentosDTO ResumoAtendimentosPorMes(int idClinica, int idunidadeatendimento);
        AtendimentosDTO AgendamentoDia(int idClinica, int idunidadeatendimento);


    }
}