using Clinicas.Application.Services.Interfaces;
using Clinicas.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clinicas.Domain.Model;

namespace Clinicas.Application.Services
{
    public class FuncionarioService : IFuncionarioService
    {
        private readonly IFuncionarioRepository _repository;

        #region [Cadastro de Funcionários]
        public FuncionarioService(IFuncionarioRepository repository)
        {
            _repository = repository;
        }

        public List<Funcionario> ListarFuncionariosDeSaude()
        {
            return _repository.ListarFuncionariosDeSaude();
        }

        public List<Funcionario> ListarFuncionariosDeSaudeAtivos()
        {
            return _repository.ListarFuncionariosDeSaudeAtivos();
        }

        public List<Especialidade> ListarEspecialidadesFuncionario(int idFuncionario)
        {
            return _repository.ListarEspecialidadesFuncionario(idFuncionario);
        }

        public Funcionario ObterFuncionariPorId(int id)
        {
            return _repository.ObterFuncionariPorId(id);
        }
        public Funcionario SalvarFuncionario(Funcionario model)
        {
            return _repository.SalvarFuncionario(model);
        }
        public List<Funcionario> ListarFuncionarios()
        {
            return _repository.ListarFuncionarios();
        }
        public List<Funcionario> ListarFuncionariosPorNome(string nome)
        {
            return _repository.ListarFuncionariosPorNome(nome);
        }

        #endregion

        #region [Cadastro de Médicos]

        public Medico ObterMedicoPorId(int id)
        {
            return _repository.ObterMedicoPorId(id);
        }

        public List<Medico> ListarMedicos()
        {
            return _repository.ListarMedicos();
        }
        public List<Medico> ListarMedicosPorNome(string nome)
        {
            return _repository.ListarMedicosPorNome(nome);
        }

        public Medico SalvarMedico(Medico medico)
        {
            return _repository.SalvarMedico(medico);
        }

        #endregion
    }
}