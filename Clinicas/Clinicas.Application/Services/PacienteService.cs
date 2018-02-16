using Clinicas.Application.Services.Interfaces;
using Clinicas.Domain.Model;
using Clinicas.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Application.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _repository;

        public PacienteService(IPacienteRepository repository)
        {
            _repository = repository;
        }

        public List<Anamnese> ListarAnamnesesPorPaciente(int idPaciente)
        {
            return _repository.ListarAnamnesesPorPaciente(idPaciente);
        }

        public IEnumerable<Paciente> ListarPacientes()
        {
            return _repository.ListarPacientes();
        }

        public List<Paciente> ListarPacientesPorNome(string nome)
        {
            return _repository.ListarPacientesPorNome(nome);
        }

        public DadosNascimento ObterDadosNascimentoPorIdPaciente(int id)
        {
            return _repository.ObterDadosNascimentoPorIdPaciente(id);
        }

        public List<RegistroVacina> ListarRegistroVacinaPorPaciente(int idPaciente)
        {
            return _repository.ListarRegistroVacinaPorPaciente(idPaciente);
        }

        public List<Carteira> ListarCarteirasPaciente(int id)
        {
            return _repository.ListarCarteirasPaciente(id);
        }

        public Anamnese ObterAnamnesePorId(int id)
        {
            return _repository.ObterAnamnesePorId(id);
        }

        public Paciente SalvarPaciente(Paciente paciente)
        {
            return _repository.SalvarPaciente(paciente);
        }

        public void ExcluirCarteiraPaciente(List<Carteira> carteiras)
        {
            _repository.ExcluirCarteiraPaciente(carteiras);
        }

        public Paciente ObterPacientePorId(int id)
        {
            return _repository.ObterPacientePorId(id);
        }

        public RegistroVacina ObterRegistroVacinaPorId(int id)
        {
            return _repository.ObterRegistroVacinaPorId(id);
        }

        public Vacina ObterVacinaPorId(int id)
        {
            return _repository.ObterVacinaPorId(id);
        }

        public List<Vacina> ObterVacinasAtivas()
        {
            return _repository.ObterVacinasAtivas();
        }

        public Anamnese SalvarAnamnese(Anamnese model)
        {
            return _repository.SalvarAnamnese(model);
        }

        public RegistroVacina SalvarRegistroVacina(RegistroVacina model)
        {
            return _repository.SalvarRegistroVacina(model);
        }

        public DadosNascimento SalvarDadosNascimento(DadosNascimento dados)
        {
            return _repository.SalvarDadosNascimento(dados);
        }

        #region [Estados e Cidades]

        public List<Estado> ListarEstados()
        {
            return _repository.ListarEstados();
        }
        public List<Cidade> ListarCidadesByEstado(int idEstado)
        {
            return _repository.ListarCidadesByEstado(idEstado);
        }

        public Estado ObterEstadoPorId(int idEstado)
        {
            return _repository.ObterEstadoPorId(idEstado);
        }
        public Cidade ObterCidadePorId(int idCidade)
        {
            return _repository.ObterCidadePorId(idCidade);
        }

        #endregion
    }
}