using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Application.Services.Interfaces
{
    public interface IFuncionarioService
    {
        #region [Funcionario]
        Funcionario ObterFuncionariPorId(int id);
        List<Funcionario> ListarFuncionariosDeSaudeAtivos();
        List<Funcionario> ListarFuncionariosDeSaude();
        Funcionario SalvarFuncionario(Funcionario model);
        List<Funcionario> ListarFuncionarios();
        List<Especialidade> ListarEspecialidadesFuncionario(int idFuncionario);
        List<Funcionario> ListarFuncionariosPorNome(string nome);
        #endregion

        #region [Cadastro de Médicos]

        Medico ObterMedicoPorId(int id);
        List<Medico> ListarMedicos();
        List<Medico> ListarMedicosPorNome(string nome);
        Medico SalvarMedico(Medico medico);

        #endregion
    }
}