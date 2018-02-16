using Clinicas.Application.Services.Interfaces;
using Clinicas.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clinicas.Domain.Model;

namespace Clinicas.Application.Services
{
    public class ProntuarioService : IProntuarioService
    {
        private readonly IProntuarioRepository _repository;

        public ProntuarioService(IProntuarioRepository repository)
        {
            _repository = repository;
        }

        #region [Ultimos Atendimentos]
        public ICollection<Agenda> UltimosAtendimentos(int idpaciente)
        {
            return _repository.UltimosAtendimentos(idpaciente);
        }
        #endregion

        #region [Anamnese]
        public Anamnese ObterAnamnesePorId(int id)
        {
            return _repository.ObterAnamnesePorId(id);
        }

        public ICollection<Anamnese> ListarAnamnese(int id)
        {
            return _repository.ListarAnamnese(id);
        }

        public void SalvarAnamnese(Anamnese anamnese)
        {
            _repository.SalvarAnamnese(anamnese);
        }

        public void ExcluirAnamnese(Anamnese anamnese)
        {
            _repository.ExcluirAnamnese(anamnese);
        }
        #endregion

        #region [Atestado]
        public Atestado GetAtestadoById(int id)
        {
            return _repository.GetAtestadoById(id);
        }

        public List<Atestado> ListarAtestadoByPaciente(int id)
        {
            return _repository.ListarAtestadoByPaciente(id);
        }

        public ModeloProntuario ObterModeloAtestado(int id)
        {
            return _repository.ObterModeloAtestado(id);
        }

        public List<ModeloProntuario> ListarModelosAtestadosAtivos()
        {
            return _repository.ListarModelosAtestadosAtivos();
        }

        public Atestado SalvarAtestadoPaciente(Atestado atestado)
        {
            return _repository.SalvarAtestadoPaciente(atestado);
        }

        public void ExcluirAtestado(int id)
        {
            _repository.ExcluirAtestado(id);
        }
        #endregion

        #region [Modelo de Prontuário]
        public ModeloProntuario ObterModeloProntuarioPorId(int id)
        {
            return _repository.ObterModeloProntuarioPorId(id);
        }

        public ModeloProntuario SalvarModeloProntuario(ModeloProntuario modelo)
        {
            return _repository.SalvarModeloProntuario(modelo);
        }

        public ICollection<ModeloProntuario> ListarModelosProntuario()
        {
            return _repository.ListarModelosProntuario();
        }

        public ICollection<ModeloProntuario> ListarModeloProntuarioPorTipo(string tipo)
        {
            return _repository.ListarModeloProntuarioPorTipo(tipo);
        }

        public void ExcluirModelo(ModeloProntuario modelo)
        {
            _repository.ExcluirModelo(modelo);
        }

        public ICollection<ModeloProntuario> PesquisarModelos(string nome)
        {
            return _repository.PesquisarModelos(nome);
        }
        #endregion

        #region [História Pregressa]

        public HistoriaPregressa SalvarHistoriaPregressa(HistoriaPregressa historia)
        {
            return _repository.SalvarHistoriaPregressa(historia);
        }

        public HistoriaPregressa ObterHistoriaPregressaPorId(int id)
        {
            return _repository.ObterHistoriaPregressaPorId(id);
        }

        public ICollection<HistoriaPregressa> ListarHistoriaPregressa(int id)
        {
            return _repository.ListarHistoriaPregressa(id);
        }

        public void ExcluirHistoriaPregressa(int id)
        {
            _repository.ExcluirHistoriaPregressa(id);
        }
        #endregion

        #region [Hospital]
        public Hospital ObterHospitalPorId(int id)
        {
            return _repository.ObterHospitalPorId(id);
        }

        public Hospital SalvarHospital(Hospital hospital)
        {
            return _repository.SalvarHospital(hospital);
        }

        public ICollection<Hospital> ObterHospitais()
        {
            return _repository.ObterHospitais();
        }

        public List<Hospital> ListarHospitaisPorNome(string nome)
        {
            return _repository.ListarHospitaisPorNome(nome);
        }
        #endregion

        #region [Medida Antropométrica]

        public ICollection<MedidasAntropometricas> ObterMedidasPorPaciente(int id)
        {
            return _repository.ObterMedidasPorPaciente(id);
        }

        public MedidasAntropometricas ObterMedidasPorId(int id)
        {
            return _repository.ObterMedidasPorId(id);
        }

        public void ExcluirMedidas(int id)
        {
            _repository.ExcluirMedidas(id);
        }

        public MedidasAntropometricas SalvarMedidaAntropometrica(MedidasAntropometricas medida)
        {
            return _repository.SalvarMedidaAntropometrica(medida);
        }

        #endregion

        #region [Receituário]
        public Receituario ObterReceituarioPorId(int id)
        {
            return _repository.ObterReceituarioPorId(id);
        }
        
        public List<Receituario> ListarReceituariosByPaciente(int id)
        {
            return _repository.ListarReceituariosByPaciente(id);
        }

        public List<ModeloProntuario> ListarModelosReceituariosAtivos()
        {
            return _repository.ListarModelosReceituariosAtivos();
        }

        public Receituario SalvarReceituario(Receituario receituario)
        {
            return _repository.SalvarReceituario(receituario);
        }

        public List<ReceituarioMedicamento> ObterMedicamentosReceituario(int id)
        {
            return _repository.ObterMedicamentosReceituario(id);
        }

        public void ExcluirReceituario(int id)
        {
            _repository.ExcluirReceituario(id);
        }
        #endregion

        #region [Cadastro de Vacinas]
        public Vacina ObterVacinaPorId(int id)
        {
            return _repository.ObterVacinaPorId(id);
        }
        public List<Vacina> ListarVacinas()
        {
            return _repository.ListarVacinas();
        }
        public Vacina SalvarVacina(Vacina vacina)
        {
            return _repository.SalvarVacina(vacina);
        }
        #endregion

        #region [Requisições de Exames]
        public RequisicaoExames ObterRequisicaoExameById(int id)
        {
            return _repository.ObterRequisicaoExameById(id);
        }
        public List<RequisicaoExames> ListarRequisicoesByPaciente(int id)
        {
            return _repository.ListarRequisicoesByPaciente(id);
        }
        public RequisicaoExames SalvarRequisicaoExame(RequisicaoExames requisicao)
        {
            return _repository.SalvarRequisicaoExame(requisicao);
        }


        #endregion

        #region [Odontograma]
        public Odontograma ObterOdontogramaPorId(int id)
        {
            return _repository.ObterOdontogramaPorId(id);
        }

        public ICollection<Odontograma> ListarOdontogramaPorIdPaciente(int id)
        {
            return _repository.ListarOdontogramaPorIdPaciente(id);
        }

        public Odontograma SalvarOdontograma(Odontograma odontograma)
        {
            return _repository.SalvarOdontograma(odontograma);
        }

        public Odontograma ExcluirOdontograma(Odontograma odontograma)
        {
            return _repository.ExcluirOdontograma(odontograma);
        }
        #endregion
    }
}