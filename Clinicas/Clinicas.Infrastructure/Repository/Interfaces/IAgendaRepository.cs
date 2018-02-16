using Clinicas.Domain.DTO;
using Clinicas.Domain.Mail;
using Clinicas.Domain.Model;
using Clinicas.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinicas.Infrastructure.Repository.Interfaces
{
    public interface IAgendaRepository
    {
        #region Email
        List<MailQueue> RetornarEmailsPendentesEnvio();
        MailQueue AddToQueue(MailQueue mensagem);
        MailTemplate GetTemplateById(int templateId);
        MailTemplate GetTemplateByIdentifier(string identifier);
        void AtualizaStatusEnvio(string status, int idMailQueue);
        IEnumerable<MailTemplate> GetTemplates();
        void RemoveFromQueue(int idMailQueue);
        #endregion

        Agenda ObterAgendaPorId(int idagenda);
        Agenda AgendaAvulsa(Agenda avulsa);
        void ExcluirAgenda(List<Agenda> agenda);
        void SalvarAgendaCompleta(List<Agenda> agendas);
        ICollection<Agenda> ListarAgenda(string situacao, int? idprofissional, string salaEspera,int idclinica,int idunidadeatendimento);
        void LiberarAgenda(DateTime data, TimeSpan hora, string observacao, int cdprofissional,int idusuario, int idclinica);
        ICollection<Agenda> ListarAgendaAguardando(int? idprofissional,int idclinica, int idunidadeatendimento);
        ICollection<Agenda> PesquisarAgendaAguardando(int? idagenda, string profissionalSaude,int idclinica, int idunidadeatendimento);
        void ConfirmarAtendimento(int idagenda, int idusuario);
        Agenda CancelarAtendimento(int idagenda, int idusuario);
        TipoAtendimento ObterTipoAtendimentoPorId(int idTipo);
        ICollection<Agenda> ListarAgendaProfissionalSaudePorMes(int mes, int idprofissional);
        ICollection<Agenda> PesquisarAgenda(int? idagenda, int? idprofissional, string paciente, DateTime dataInicio, DateTime dataTermino, int idclinica, int idUnidade, string situacao);
        void Marcar(int idagenda, int idpaciente, int idprofissional, int idespecialidade, int idprocedimento, int? idconvenio, string tipo, string observacoes, string solicitante, decimal valor, decimal valorProfissional, int? idtipoatendimento, int idusuario, int idUnidadeAtendimento);
        void EncaminharSalaEspera(int idagenda);
        Agenda ObterAgendaDiaHora(DateTime data, TimeSpan hora, int idProfissional, int idClinica,int idunidadeatendimento);
        ICollection<Agenda> ListarAgendaMedicaPorData(int idMedico, DateTime data, int idClinica, int idunidadeatendimento);
        ICollection<Agenda> ListarAgendaMedicaPorPeriodo(int idMedico, DateTime dataInicio, DateTime dataFim);
        void ExcluirAgendasAnteriores(int idClinica);
        Agenda SalvarAgenda(Agenda Agenda);
        ICollection<Agenda> ListarPacientesConvocados();

        #region [Bloquio de agenda]			
        List<BloqueioAgenda> ListarBloqueioAgenda(int idClinica);
        BloqueioAgenda ObterBloqueioAgenda(int id);
        BloqueioAgenda ObterBloqueioAgendaPorPeriodo(int idProfissional, DateTime datanicio, DateTime dataFim);
        BloqueioAgenda SalvarBloqueioAgenda(BloqueioAgenda model);
        void ExcluirBloqueioAgenda(BloqueioAgenda model);
        #endregion
    }
}
