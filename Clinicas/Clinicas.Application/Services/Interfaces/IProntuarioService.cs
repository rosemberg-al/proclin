using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Application.Services.Interfaces
{
    public interface IProntuarioService
    {
        #region [Ultimos Atendimentos]
        ICollection<Agenda> UltimosAtendimentos(int idpaciente);
        #endregion

        #region [Anamnese]
        Anamnese ObterAnamnesePorId(int id);
        ICollection<Anamnese> ListarAnamnese(int id);
        void SalvarAnamnese(Anamnese anamnese);
        void ExcluirAnamnese(Anamnese anamnese);
        #endregion

        #region [História Pregressa]

        HistoriaPregressa SalvarHistoriaPregressa(HistoriaPregressa historia);
        HistoriaPregressa ObterHistoriaPregressaPorId(int id);
        void ExcluirHistoriaPregressa(int id);
        ICollection<HistoriaPregressa> ListarHistoriaPregressa(int id);

        #endregion
        
        #region [Atestado]

        Atestado GetAtestadoById(int id);
        List<Atestado> ListarAtestadoByPaciente(int id);
        Atestado SalvarAtestadoPaciente(Atestado atestado);
        ModeloProntuario ObterModeloAtestado(int id);
        List<ModeloProntuario> ListarModelosAtestadosAtivos();
        void ExcluirAtestado(int id);
        #endregion

        #region [Modelos Prontuários]
        ModeloProntuario ObterModeloProntuarioPorId(int id);
        ModeloProntuario SalvarModeloProntuario(ModeloProntuario modelo);
        void ExcluirModelo(ModeloProntuario modelo);
        ICollection<ModeloProntuario> PesquisarModelos(string nome);
        ICollection<ModeloProntuario> ListarModelosProntuario();
        ICollection<ModeloProntuario> ListarModeloProntuarioPorTipo(string tipo);
        #endregion

        #region [hospital]
        Hospital ObterHospitalPorId(int id);
        Hospital SalvarHospital(Hospital hospital);
        ICollection<Hospital> ObterHospitais();
        List<Hospital> ListarHospitaisPorNome(string nome);
        
        #endregion

        #region [Medida Antropométrica]
        ICollection<MedidasAntropometricas> ObterMedidasPorPaciente(int id);
        MedidasAntropometricas ObterMedidasPorId(int id);
        MedidasAntropometricas SalvarMedidaAntropometrica(MedidasAntropometricas medida);
        void ExcluirMedidas(int id);
        #endregion

        #region [Receituário]
        Receituario ObterReceituarioPorId(int id);
        List<Receituario> ListarReceituariosByPaciente(int id);
        List<ModeloProntuario> ListarModelosReceituariosAtivos();
        Receituario SalvarReceituario(Receituario receituario);
        List<ReceituarioMedicamento> ObterMedicamentosReceituario(int id);
        void ExcluirReceituario(int id);
        #endregion

        #region [Cadastro de Vacinas]
        Vacina ObterVacinaPorId(int id);
        List<Vacina> ListarVacinas();
        Vacina SalvarVacina(Vacina vacina);
        #endregion

        #region [Requisições de Exames]
        RequisicaoExames ObterRequisicaoExameById(int id);
        List<RequisicaoExames> ListarRequisicoesByPaciente(int id);
        RequisicaoExames SalvarRequisicaoExame(RequisicaoExames requisicao);
        #endregion

        #region [Odontograma]
        Odontograma ObterOdontogramaPorId(int id);
        ICollection<Odontograma> ListarOdontogramaPorIdPaciente(int id);
        Odontograma SalvarOdontograma(Odontograma odontograma);
        Odontograma ExcluirOdontograma(Odontograma odontograma);
        #endregion
    }
}