using Clinicas.Domain.DTO;
using Clinicas.Domain.Mail;
using Clinicas.Domain.Model;
using Clinicas.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Application.Services.Interfaces
{
    public interface IAgendaService
    {
        #region Email
        MailQueue AddToQueue(MailQueue mensagem);
        MailTemplate GetTemplateById(int templateId);
        MailTemplate GetTemplateByIdentifier(string identifier);
        IEnumerable<MailTemplate> GetTemplates();
        void RemoveFromQueue(int idMailQueue);
        void ProcessQueue();
        #endregion

        #region [Bloquio de agenda]			
        List<BloqueioAgenda> ListarBloqueioAgenda(int idClinica);
        BloqueioAgenda ObterBloqueioAgenda(int id);
        BloqueioAgenda SalvarBloqueioAgenda(BloqueioAgenda model);
        void ExcluirBloqueioAgenda(BloqueioAgenda model);
        #endregion

        Agenda ObterAgendaPorId(int idagenda);
        Agenda AgendaAvulsa(Agenda avulsa);
        ICollection<Agenda> ListarAgenda(string situacao = "Todos", int? idprofissional = 0, string salaEspera = "Nao", int idclinica = 0,int idunidadeatendimento = 0);
        TipoAtendimento ObterTipoAtendimentoPorId(int idTipo);
        ICollection<Agenda> ListarAgendaAguardando(int? idprofissional,int idclinica, int idunidadeatendimento);
        ICollection<Agenda> PesquisarAgendaAguardando(int? idagenda, string profissionalSaude, int idclinica, int idunidadeatendimento);
        void LiberarAgenda(LiberarAgendaViewModel liberaragenda);
        void GerarAgenda(GerarAgendaViewModel model, int idClinica, Usuario usuario);

        void ConfirmarAtendimento(int idagenda, int idusuario);
        Agenda CancelarAtendimento(int idagenda, int idusuario);
        ICollection<Agenda> ListarAgendaProfissionalSaudePorMes(int mes, int idprofissional);

        ICollection<Agenda> PesquisarAgenda(int? idagenda, int? idprofissional, string paciente,DateTime dataInicio,DateTime dataTermino, int idclinica, int idunidadeatendimento, string situacao);
        void Marcar(int idagenda, int idpaciente, int idprofissional, int idespecialidade, int idprocedimento, int? idconvenio, string tipo, string observacoes, string solicitante, decimal valor, decimal valorProfissional, int? idtipoatendimento, int idusuario, int idUnidadeAtendimento);
        Agenda ObterAgendaDiaHora(DateTime data, TimeSpan hora, int idProfissional, int idClinica, int idunidadeatendimento);
        void EncaminharSalaEspera(int idagenda);
        ICollection<Agenda>  ListarAgendaMedicaPorData(int idMedico, DateTime data, int idClinica, int idunidadeatendimento);
        ICollection<Agenda> ListarAgendaMedicaPorPeriodo(int idMedico, DateTime dataInicio, DateTime dataFim);
        Agenda SalvarAgenda(Agenda agenda);
        void ExcluirAgendasAnteriores(int idClinica);
        ICollection<Agenda> ListarPacientesConvocados();
    }
}