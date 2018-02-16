using Clinicas.Domain.DTO;
using Clinicas.Domain.Model;
using Clinicas.Domain.ViewModel;
using Clinicas.Infrastructure.Base;
using Clinicas.Infrastructure.Context;
using Clinicas.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Linq.Expressions;
using Clinicas.Domain.Mail;

namespace Clinicas.Infrastructure.Repository
{
    public class AgendaRepository : RepositoryBase<ClinicasContext>, IAgendaRepository
    {
        public AgendaRepository(IUnitOfWork<ClinicasContext> unit)
            : base(unit)
        {
        }

        #region Mail

        /// <summary>
        /// Adiciona um e-mail na fila
        /// </summary>
        /// <param name="mensagem">Mensagem a ser enviada</param>
        /// <returns></returns>
        public MailQueue AddToQueue(MailQueue mensagem)
        {
            Context.MailQueue.Add(mensagem);
            Context.SaveChanges();
            return mensagem;
        }

        /// <summary>
        /// Busca um template pelo ID
        /// </summary>
        /// <param name="templateId">ID do Template</param>
        /// <returns></returns>
        public MailTemplate GetTemplateById(int templateId)
        {
            return Context.MailTemplates.FirstOrDefault(t => t.Id == templateId);
        }

        /// <summary>
        /// Lista todos os templates disponíveis
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MailTemplate> GetTemplates()
        {
            return Context.Set<MailTemplate>();
        }

        /// <summary>
        /// Remove um e-mail da fila
        /// </summary>
        /// <param name="idMailQueue">ID do e-mail na fila</param>
        public void RemoveFromQueue(int idMailQueue)
        {
            var mail = Context.Set<MailQueue>().FirstOrDefault(m => m.Id == idMailQueue);
            if (mail != null)
            {
                Context.Set<MailQueue>().Remove(mail);
                Context.SaveChanges();
            }
        }

        /// <summary>
        /// Busca um template pelo seu identificador global
        /// </summary>
        /// <param name="identifier">Identificador global</param>
        /// <returns></returns>
        public MailTemplate GetTemplateByIdentifier(string identifier)
        {
            return Context.MailTemplates.FirstOrDefault(t => t.Identifier.ToUpper() == identifier.ToUpper());
        }

        public void AtualizaStatusEnvio(string status, int idMailQueue)
        {
            var mail = Context.MailQueue.FirstOrDefault(t => t.Id == idMailQueue);
            if (mail == null)
                throw new Exception("Erro ao recuperar email.");

            mail.SetStatus(status);
            Context.Entry(mail).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public List<MailQueue> RetornarEmailsPendentesEnvio()
        {
            var queue = Context.MailQueue
                .Include(p => p.Template)
                .Where(m => m.Scheduled == null && m.Status == MailStatus.Pending)
                .OrderBy(o => o.Date).ToList();

            return queue;
        }

        #endregion

        public ICollection<Agenda> ListarAgenda(string situacao, int? idprofissional, string salaespera, int idclinica,int idunidadeatendimento)
        {
            ICollection<Agenda> resultado = null;

            Expression<Func<Agenda, bool>> filtroSituacao = registro => true;
            Expression<Func<Agenda, bool>> filtroProfissional = registro => true;
            Expression<Func<Agenda, bool>> filtroSalaEspera = registro => true;
            Expression<Func<Agenda, bool>> filtroUndiadeAtendimento = registro => true;

            if ((situacao != "Todos") && (situacao != "Hoje"))
                filtroSituacao = (Agenda registro) =>
                           registro.Situacao == situacao;

            if (situacao == "Hoje")
            {
                var data = DateTime.Now.Date;
                filtroSituacao = (Agenda registro) =>
                                           registro.Situacao == "Marcado" && registro.Data == data;
            }

            if (idprofissional > 0)
                filtroProfissional = (Agenda registro) =>
                                          registro.ProfissionalSaude.IdFuncionario == idprofissional;

            if (idunidadeatendimento > 0)
                filtroUndiadeAtendimento = (Agenda registro) =>
                                          registro.IdUnidadeAtendimento == idunidadeatendimento;

            if (!string.IsNullOrEmpty(salaespera))
                filtroSalaEspera = (Agenda registro) =>
                           registro.SalaEspera == salaespera;

            resultado = Context.Agenda.Include(x=>x.UnidadeAtendimento).Include(x => x.Paciente.Pessoa).Include(x => x.ProfissionalSaude.Pessoa).Include(x => x.Procedimento)
                .Where(filtroSituacao).Where(filtroUndiadeAtendimento).Where(filtroProfissional).Where(filtroSalaEspera).Where(x=>x.IdClinica==idclinica).Where(x => x.Situacao != "Aguardando").ToList();

            return resultado;
        }

        public Agenda ObterAgendaPorId(int idagenda)
        {
            return Context.Agenda.Include(x=>x.UnidadeAtendimento).Include(x => x.Paciente.Pessoa).Include(x => x.Procedimento).Include(x => x.ProfissionalSaude.Pessoa).Include(x => x.Convenio.Pessoa).First(x => x.IdAgenda == idagenda);
        }
        public Agenda AgendaAvulsa(Agenda avulsa)
        {
            Context.Agenda.Add(avulsa);
            Context.SaveChanges();
            return avulsa;
        }

        public TipoAtendimento ObterTipoAtendimentoPorId(int idTipo)
        {
            return Context.TipoAtendimento.First(x => x.IdTipoAtendimento == idTipo);
        }

        public void LiberarAgenda(DateTime data, TimeSpan hora, string observacao, int cdprofissional, int idusuario, int idclinica)
        {
            var usuarioInclusao = Context.Usuarios.Find(idusuario);
            var funcionario = Context.Funcionario.First(x => x.Tipo == "Profissional de Saúde" && x.IdFuncionario == cdprofissional);
            var clinica = Context.Clinica.Find(idclinica);

            var agenda = new Agenda();
            agenda.Aguardando(data, hora, usuarioInclusao, funcionario, clinica, observacao);

            Agenda consulta = Context.Agenda.FirstOrDefault(x => x.Data == data && x.Hora == hora && x.IdFuncionario == cdprofissional);
            if (consulta == null)
            {
                Context.Agenda.Add(agenda);
                Context.SaveChanges();
            }


            // deleta a agenda do profissional e atualiza
            // Context.Database.ExecuteSqlCommand(" DELETE FROM `Agenda` WHERE(`IdAgenda`= '4') ");
            // Context.SaveChanges();

            // verifica se a agenda ja existe 
            /* var consulta = Context.Database.SqlQuery<AgendaDTO>(" SELECT Agenda.IdAgenda FROM Agenda INNER JOIN AgendaProcedimento ON Agenda.IdAgenda = AgendaProcedimento.IdAgenda where IdFuncionario = '" + cdprofissional + "' and `Data` = '" + data + "' and Hora = '" + hora + "' and Situacao='Aguardando' ").ToList();

            if (consulta.Count == 0)
            {
                int? cd = Context.Database.SqlQuery<int?>(" SELECT MAX(IdAgenda)+1 FROM Agenda as Last_ID ").First();

                string sql = " INSERT INTO `Agenda` (`Data`, `Hora`, `Observacao`, `IdUsuarioInclusao`, `DataInclusao`, `IdClinica`)  VALUES ( '" + data + "', '" + hora + "', '" + observacao + "', '" + idusuario + "', NOW(),'" + idclinica + "' ) ";
                
                Context.Database.ExecuteSqlCommand(sql);
                Context.SaveChanges();

                sql = "  INSERT INTO `AgendaProcedimento` (`IdAgenda`,  `IdFuncionario`) VALUES('" + cd + "', '" + cdprofissional + "'); ";

                Context.Database.ExecuteSqlCommand(sql);
                Context.SaveChanges();
            } */
        }

        public ICollection<Agenda> ListarAgendaAguardando(int? idprofissional, int idclinica,int idunidadeatendimento)
        {
            ICollection<Agenda> resultado = null;
            Expression<Func<Agenda, bool>> filtroProfissional = registro => true;

            Expression<Func<Agenda, bool>> filtroData = registro => true;
            Expression<Func<Agenda, bool>> filtroUnidadeAtendimento = registro => true;

            if (idprofissional > 0)
                filtroProfissional = (Agenda registro) =>
                                          registro.ProfissionalSaude.IdFuncionario == idprofissional;

            if (idunidadeatendimento > 0)
                filtroUnidadeAtendimento = (Agenda registro) =>
                                          registro.IdUnidadeAtendimento == idunidadeatendimento;

            filtroData = (Agenda registro) =>
                                          registro.Data >= DateTime.Now;

            resultado = Context.Agenda.Include(x => x.ProfissionalSaude.Pessoa)
                .Where(filtroProfissional).Where(filtroData).Where(x => x.Situacao == "Aguardando").ToList();

            return resultado;
        }

        public ICollection<Agenda> PesquisarAgendaAguardando(int? idagenda, string profissionalSaude, int idclinica, int idunidadeatendimento)
        {
            ICollection<Agenda> resultado = null;
            Expression<Func<Agenda, bool>> filtroIdAgenda = registro => true;
            Expression<Func<Agenda, bool>> filtroProfissionalSaude = registro => true;
            Expression<Func<Agenda, bool>> filtroUnidadeAtendimento = registro => true;

            if (idagenda > 0)
                filtroIdAgenda = (Agenda registro) =>
                                          registro.IdAgenda == idagenda;


            if (idunidadeatendimento > 0)
                filtroUnidadeAtendimento = (Agenda registro) =>
                                          registro.IdUnidadeAtendimento == idunidadeatendimento;

            if (!string.IsNullOrEmpty(profissionalSaude))
                filtroProfissionalSaude = (Agenda registro) =>
                                          registro.ProfissionalSaude.Pessoa.Nome.ToUpper() == profissionalSaude.ToUpper();

            resultado = Context.Agenda.Include(x => x.ProfissionalSaude.Pessoa)
                .Where(filtroIdAgenda).Where(filtroUnidadeAtendimento).Where(filtroProfissionalSaude).Where(x => x.Situacao == "Aguardando").ToList();

            return resultado;
        }

        public Agenda CancelarAtendimento(int idagenda, int idusuario)
        {
            var agenda = Context.Agenda.First(x => x.IdAgenda == idagenda);
            if (agenda == null)
                throw new Exception("Erro ao obter dados da agenda ");

            var usuario = Context.Usuarios.First(x => x.IdUsuario == idusuario);
            if (usuario == null)
                throw new Exception("Erro ao obter dados do usuário ");

            agenda.CancelarAtendimento(usuario);
            Context.Entry(agenda).State = EntityState.Modified;
            Context.SaveChanges();
            return agenda;
        }

        public void ConfirmarAtendimento(int idagenda, int idusuario)
        {
            var agenda = Context.Agenda.First(x => x.IdAgenda == idagenda);
            if (agenda == null)
                throw new Exception("Erro ao obter dados da agenda ");

            var usuario = Context.Usuarios.First(x => x.IdUsuario == idusuario);
            if (usuario == null)
                throw new Exception("Erro ao obter dados do usuário ");

            agenda.ConfirmarAtendimento(usuario);
            Context.Entry(agenda).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public ICollection<Agenda> ListarAgendaProfissionalSaudePorMes(int mes, int idprofissional)
        {
            return Context.Agenda
                .Include(x => x.Paciente.Pessoa)
                .Include(x => x.ProfissionalSaude.Pessoa)
                .Where(x => x.Data.Month.Equals(mes) && x.IdFuncionario == idprofissional && x.Situacao!="Aguardando").ToList();
        }

        public ICollection<Agenda> PesquisarAgenda(int? idagenda, int? idprofissional, string paciente, DateTime dataInicio, DateTime dataTermino, int idclinica, int idUnidade, string situacao)
        {
            ICollection<Agenda> resultado = null;

            Expression<Func<Agenda, bool>> filtroIdAgenda = registro => true;
            Expression<Func<Agenda, bool>> filtroProfissional = registro => true;
            Expression<Func<Agenda, bool>> filtroPaciente = registro => true;
            Expression<Func<Agenda, bool>> filtroPeriodo = registro => true;
            Expression<Func<Agenda, bool>> filtroSituacao = registro => true;
            Expression<Func<Agenda, bool>> filtroUnidadeAtendimento = registro => true;

            if (idagenda > 0)
                filtroIdAgenda = (Agenda registro) =>
                           registro.IdAgenda == idagenda;

            if (idUnidade > 0)
                filtroUnidadeAtendimento = (Agenda registro) =>
                           registro.IdUnidadeAtendimento == idUnidade;

            if (situacao != "Todos")
                filtroSituacao = (Agenda registro) =>
                           registro.Situacao == situacao;

            if (!string.IsNullOrEmpty(paciente))
                filtroPaciente = (Agenda registro) =>
                           registro.Paciente.Pessoa.Nome.ToUpper().Contains(paciente.ToUpper());

            if (idprofissional > 0)
                filtroProfissional = (Agenda registro) =>
                                          registro.ProfissionalSaude.IdFuncionario == idprofissional;

            var dataIn = dataInicio.Date;
            var dataTer = dataTermino.Date;

            resultado = Context.Agenda.Include(x => x.Paciente.Pessoa).Include(x => x.ProfissionalSaude.Pessoa).Include(x => x.Procedimento)
                .Where(filtroSituacao).Where(filtroUnidadeAtendimento).Where(filtroProfissional).Where(filtroPaciente).Where(filtroSituacao).Where(filtroIdAgenda).Where(x => x.Data >= dataInicio && x.Data <= dataTermino).Where(x => x.Situacao != "Aguardando").ToList();

            return resultado;
        }

        public void Marcar(int idagenda, int idpaciente, int idprofissional, int idespecialidade, int idprocedimento, int? idconvenio, string tipo, string observacoes, string solicitante, decimal valor, decimal valorProfissional, int? idtipoatendimento, int idusuario, int idUnidadeAtendimento)
        {
            var agenda = Context.Agenda.Find(idagenda);
            if (agenda == null)
                throw new Exception("Erro ao recuperar dados da agenda");

            var hoje = DateTime.Now.Date;
            if (agenda.Data <= hoje)
                throw new Exception("Não foi possivel agendar um horário inferior a data atual ");

            var usuario = Context.Usuarios.Find(idusuario);
            if (usuario == null)
                throw new Exception("Erro ao recuperar dados do usuário ");

            var paciente = Context.Paciente.Find(idpaciente);
            if (paciente == null)
                throw new Exception("Erro ao recupear dados do paciente ");

            var profissional = Context.Funcionario.Find(idprofissional);
            if (profissional == null)
                throw new Exception("Erro ao recupear dados do profissional ");

            var especialidade = Context.Especialidade.Find(idespecialidade);
            if (especialidade == null)
                throw new Exception("Erro ao recuperar a especialidade do profissional de saúde ");

            var procedimento = Context.Procedimento.Find(idprocedimento);
            if (procedimento == null)
                throw new Exception("Erro ao recuperar dados do procedimento ");

            var convenio = Context.Convenio.Find(idconvenio);
            if ((tipo == "C") && (convenio==null))
                    throw new Exception("Erro ao recuperar dados do convenio");

           
            
           // valida agenda repetida
           var consulta = Context.Agenda.FirstOrDefault(x => x.Data == agenda.Data && x.Hora == agenda.Hora && x.Situacao == "Marcado" && x.IdFuncionario == agenda.IdFuncionario);

           if (consulta != null)
               throw new Exception("Erro ao realizar um novo agendamento. Existe uma marcação para este horário no  sistema ");


            var tipoAtendimento = Context.TipoAtendimento.Find(idtipoatendimento);

            agenda.Marcado(usuario, paciente, profissional, especialidade, procedimento, convenio, tipo, observacoes, solicitante, valor, valorProfissional, tipoAtendimento);
            if (idUnidadeAtendimento > 0)
            {
                var unidade = Context.Unidades.FirstOrDefault(x => x.IdUnidadeAtendimento == idUnidadeAtendimento);
                if (unidade == null)
                    throw new Exception("Erro ao recuperar a unidade de atendimento.");

                agenda.SetUnidadeAtendimento(unidade);
            }
            Context.Entry(agenda).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public void SalvarAgendaCompleta(List<Agenda> agendas)
        {
            Context.Agenda.AddRange(agendas);
            Context.SaveChanges();
        }

        public void EncaminharSalaEspera(int idagenda)
        {
            var agenda = Context.Agenda.Find(idagenda);
            if (agenda == null)
                throw new Exception("Erro ao recuperar dados da agenda ");

            if (agenda.Situacao != "Marcado")
                throw new Exception(" Erro ao encaminhar o paciente para sala de espera. O Paciente não se encontra com a agenda marcado ");

            agenda.SetSalaEspera("Sim");
            Context.Entry(agenda).State = EntityState.Modified;
            Context.SaveChanges();

        }

        public Agenda ObterAgendaDiaHora(DateTime data, TimeSpan hora, int idProfissional, int idClinica, int idunidadeatendimento)
        {

            Expression<Func<Agenda, bool>> filtroData = registro => true;
            Expression<Func<Agenda, bool>> filtroHora = registro => true;
            Expression<Func<Agenda, bool>> filtroProfissional = registro => true;
            Expression<Func<Agenda, bool>> filtroclinica = registro => true;
            Expression<Func<Agenda, bool>> filtroUnidadeAtendimento = registro => true;

            filtroProfissional = (Agenda registro) =>
                                          registro.ProfissionalSaude.IdFuncionario == idProfissional;

            filtroData = (Agenda registro) =>
                       registro.Data == data;

            filtroHora = (Agenda registro) =>
                       registro.Hora == hora;

            filtroclinica = (Agenda registro) =>
                       registro.IdClinica == idClinica;

            filtroUnidadeAtendimento = (Agenda registro) =>
                      registro.IdUnidadeAtendimento == idunidadeatendimento;

            var resultado = Context.Agenda.Include(x => x.Paciente.Pessoa).Include(x => x.ProfissionalSaude.Pessoa).Include(x => x.Procedimento)
               .Where(filtroData).Where(filtroUnidadeAtendimento).Where(filtroHora).Where(filtroclinica).Where(filtroProfissional).FirstOrDefault();

            return resultado;
        }
        public void ExcluirAgendasAnteriores(int idClinica)
        {
            ICollection<Agenda> resultado = null;
            DateTime dataIncio = DateTime.Now.AddDays(-1);

            Expression<Func<Agenda, bool>> filtroData = registro => true;
            Expression<Func<Agenda, bool>> filtroClinica = registro => true;

            filtroClinica = (Agenda registro) =>
                      registro.IdClinica == idClinica;


            filtroData = (Agenda registro) =>
                       registro.Data <= dataIncio;

            resultado = Context.Agenda.Include(x => x.Paciente.Pessoa).Include(x => x.ProfissionalSaude.Pessoa).Include(x => x.Procedimento)
               .Where(filtroData).Where(filtroClinica).Where(x => x.Situacao == "Aguardando").ToList();

            Context.Agenda.RemoveRange(resultado);

            Context.Entry(resultado).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public ICollection<Agenda> ListarAgendaMedicaPorData(int idMedico, DateTime data, int idClinica,int idunidadeAtendimento)
        {
            ICollection<Agenda> resultado = null;

            Expression<Func<Agenda, bool>> filtroData = registro => true;
            Expression<Func<Agenda, bool>> filtroProfissional = registro => true;
            Expression<Func<Agenda, bool>> filtroUnidadeAtendimento = registro => true;

            if (idMedico > 0)
            {
                filtroProfissional = (Agenda registro) =>
                                          registro.ProfissionalSaude.IdFuncionario == idMedico;
            }

            if (idunidadeAtendimento > 0)
            {
                filtroUnidadeAtendimento = (Agenda registro) =>
                                          registro.IdUnidadeAtendimento == idunidadeAtendimento;
            }

            filtroData = (Agenda registro) => registro.Data == data;

            resultado = Context.Agenda.Include(x=>x.UnidadeAtendimento).Include(x => x.ProfissionalSaude.Pessoa).Include(x => x.Procedimento)
               .Where(filtroData).Where(filtroUnidadeAtendimento).Where(filtroProfissional).Where(x=>x.Situacao=="Aguardando").OrderBy(x => x.Hora).ToList();
            return resultado;
        }

        public ICollection<Agenda> ListarAgendaMedicaPorPeriodo(int idMedico, DateTime dataInicio, DateTime dataFim)
        {
            ICollection<Agenda> resultado = null;

            Expression<Func<Agenda, bool>> filtroData = registro => true;
            Expression<Func<Agenda, bool>> filtroDataFim = registro => true;
            Expression<Func<Agenda, bool>> filtroProfissional = registro => true;

            if (idMedico > 0)
                filtroProfissional = (Agenda registro) =>
                                          registro.ProfissionalSaude.IdFuncionario == idMedico;

            filtroData = (Agenda registro) =>
                       registro.Data >= dataInicio;

            filtroDataFim = (Agenda registro) =>
                      registro.Data <= dataFim;

            resultado = Context.Agenda.Include(x => x.ProfissionalSaude.Pessoa).Include(x => x.Paciente.Pessoa)
                .Where(filtroData).Where(filtroProfissional)
                .Where(x => x.Situacao == "Marcado" || x.Situacao == "Realizado")
                .Where(filtroDataFim).ToList();

            return resultado;
        }

        public void ExcluirAgenda(List<Agenda> agenda)
        {
            Context.Agenda.RemoveRange(agenda);
            Context.SaveChanges();
        }

        #region [Bloquio de agenda]			
        public List<BloqueioAgenda> ListarBloqueioAgenda(int idClinica)
        {
            return Context.BloqueioAgenda.Include(x => x.Funcionario).Include(x => x.Funcionario.Pessoa).Where(x => x.IdClinica == idClinica).Take(200).OrderByDescending(x => x.DataInicio).ToList();
        }
        public BloqueioAgenda ObterBloqueioAgenda(int id)
        {
            return Context.BloqueioAgenda.Include(x => x.Funcionario).Include(x => x.Funcionario.Pessoa).FirstOrDefault(x => x.IdBloqueio == id);
        }
        public BloqueioAgenda ObterBloqueioAgendaPorPeriodo(int idProfissional, DateTime dataInicio, DateTime dataFim)
        {
            return Context.BloqueioAgenda.Include(x => x.Funcionario).Include(x => x.Funcionario.Pessoa).FirstOrDefault(x => x.IdFuncionario == idProfissional);
        }
        public BloqueioAgenda SalvarBloqueioAgenda(BloqueioAgenda model)
        {
            if (model.IdBloqueio > 0)
            {
                Context.Entry(model).State = EntityState.Modified;
            }
            else
            {
                Context.BloqueioAgenda.Add(model);
            }
            Context.SaveChanges();
            return model;
        }
        public void ExcluirBloqueioAgenda(BloqueioAgenda model)
        {
            Context.BloqueioAgenda.Remove(model);
            Context.SaveChanges();
        }


        #endregion

        public Agenda SalvarAgenda(Agenda agenda)
        {
            if (agenda.IdAgenda > 0)
            {
                Context.Entry(agenda).State = EntityState.Modified;
            }
            else
            {
                Context.Agenda.Add(agenda);
            }
            Context.SaveChanges();
            return agenda;
        }

        public ICollection<Agenda> ListarPacientesConvocados()
        {
            return Context.Agenda.Include(m=>m.Paciente.Pessoa)
                .Include(m=>m.ProfissionalSaude.Pessoa)
                .Include(m=>m.Consultorio)
                .Include(m=>m.UnidadeAtendimento)
                .Where(x => x.ConvocarPaciente == "Sim" && x.Situacao == "Marcado").OrderByDescending(x=>x.DataConvocacaoPaciente).ToList();
        }
    }
}