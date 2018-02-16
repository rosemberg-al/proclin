using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Repository.Interfaces
{
    public interface IFuncionarioRepository
    {
        #region [Funcionario]
        Funcionario ObterFuncionariPorId(int id);
        List<Funcionario> ListarFuncionariosDeSaudeAtivos();
        List<Funcionario> ListarFuncionariosDeSaude();
        Funcionario SalvarFuncionario(Funcionario model);
        List<Funcionario> ListarFuncionarios();
        List<Funcionario> ListarFuncionariosPorNome(string nome);
        List<Especialidade> ListarEspecialidadesFuncionario(int idFuncionario);
        #endregion

        #region [Cadastro de Médicos]

        Medico ObterMedicoPorId(int id);
        List<Medico> ListarMedicos();
        List<Medico> ListarMedicosPorNome(string nome);
        Medico SalvarMedico(Medico medico);

        #endregion
    }
}