using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinicas.Infrastructure.Repository.Interfaces
{
    public interface IPacienteRepository
    {
        Paciente ObterPacientePorId(int id);
        Paciente SalvarPaciente(Paciente paciente);
        void ExcluirCarteiraPaciente(List<Carteira> carteiras);
        IEnumerable<Paciente> ListarPacientes();
        List<Paciente> ListarPacientesPorNome(string nome);
        Anamnese SalvarAnamnese(Anamnese model);
        Anamnese ObterAnamnesePorId(int id);
        List<Anamnese> ListarAnamnesesPorPaciente(int idPaciente);
        List<Vacina> ObterVacinasAtivas();
        Vacina ObterVacinaPorId(int id);
        RegistroVacina ObterRegistroVacinaPorId(int id);
        RegistroVacina SalvarRegistroVacina(RegistroVacina model);
        List<RegistroVacina> ListarRegistroVacinaPorPaciente(int idPaciente);
        DadosNascimento ObterDadosNascimentoPorIdPaciente(int id);
        DadosNascimento SalvarDadosNascimento(DadosNascimento dados);
        List<Carteira> ListarCarteirasPaciente(int id);
        #region [Estados e Cidades]

        List<Estado> ListarEstados();
        List<Cidade> ListarCidadesByEstado(int idEstado);
        Estado ObterEstadoPorId(int idEstado);
        Cidade ObterCidadePorId(int idCidade);
        #endregion
    }
}
