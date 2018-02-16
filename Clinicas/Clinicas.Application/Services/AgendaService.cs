using Clinicas.Application.Services.Interfaces;
using Clinicas.Domain.Model;
using Clinicas.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clinicas.Domain.DTO;
using Clinicas.Domain.ViewModel;
using Clinicas.Domain.Mail;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Net.Mail;

namespace Clinicas.Application.Services
{
    public class AgendaService : IAgendaService
    {
        private readonly IAgendaRepository _repository;
        private readonly ICadastroRepository _caRepository;
        private readonly string _primaryAddress;
        private string _templateDirectory;
        private readonly Uri _ewsUri;

        public AgendaService(IAgendaRepository repository, ICadastroRepository caRepository)
        {
            _repository = repository;
            _caRepository = caRepository;
        }

        #region Mail

        /// <summary>
        /// Adiciona um e-mail na fila
        /// </summary>
        /// <param name="mensagem">Mensagem a ser enviada</param>
        /// <returns></returns>
        public MailQueue AddToQueue(MailQueue mensagem)
        {
            return _repository.AddToQueue(mensagem);
        }

        /// <summary>
        /// Busca um template pelo ID
        /// </summary>
        /// <param name="templateId">ID do Template</param>
        /// <returns></returns>
        public MailTemplate GetTemplateById(int templateId)
        {
            return _repository.GetTemplateById(templateId);
        }

        /// <summary>
        /// Lista todos os templates disponíveis
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MailTemplate> GetTemplates()
        {
            return _repository.GetTemplates();
        }

        /// <summary>
        /// Remove um e-mail da fila
        /// </summary>
        /// <param name="idMailQueue">ID do e-mail na fila</param>
        public void RemoveFromQueue(int idMailQueue)
        {
            _repository.RemoveFromQueue(idMailQueue);
        }

        #region [Bloquio de agenda]			
        public List<BloqueioAgenda> ListarBloqueioAgenda(int idClinica)
        {
            return _repository.ListarBloqueioAgenda(idClinica);
        }
        public BloqueioAgenda ObterBloqueioAgenda(int id)
        {
            return _repository.ObterBloqueioAgenda(id);
        }
        public BloqueioAgenda SalvarBloqueioAgenda(BloqueioAgenda model)
        {
            return _repository.SalvarBloqueioAgenda(model);
        }
        public void ExcluirBloqueioAgenda(BloqueioAgenda model)
        {
            _repository.ExcluirBloqueioAgenda(model);
        }
        #endregion

        public MailTemplate GetTemplateByIdentifier(string identifier)
        {
            return _repository.GetTemplateByIdentifier(identifier);
        }

        /// <summary>
        /// Processa a fila de e-mails
        /// </summary>
        public void ProcessQueue()
        {
            _templateDirectory = ConfigurationManager.AppSettings["TemplatesDirectory"];

            //Busca todos os emails na fila que não estão agendados
            var queue = _repository.RetornarEmailsPendentesEnvio();

            if (queue != null && queue.Any())
            {
                queue = queue.Take(10).ToList();

                //Envia os emails da fila
                foreach (var mail in queue)
                {
                    try
                    {
                        if (!_templateDirectory.EndsWith("\\"))
                            _templateDirectory += "\\";

                        string templateHtml = File.ReadAllText(_templateDirectory + mail.Template.Folder + "\\" + mail.Template.Filename);
                        string subject = mail.Template.Subject;
                        var json = JObject.Parse(mail.Parameters);

                        templateHtml = ProcessTemplate(templateHtml, json);
                        subject = ProcessSubject(subject, json);

                        var result = EnviarEmail(subject, templateHtml, mail.To);
                        if (result)
                            _repository.AtualizaStatusEnvio(MailStatus.Sent, mail.Id);
                        else
                            _repository.AtualizaStatusEnvio(MailStatus.Error, mail.Id);

                    }
                    catch (Exception ex)
                    {
                        _repository.AtualizaStatusEnvio(MailStatus.Error, mail.Id);
                    }
                }
            }
        }

        public bool EnviarEmail(string subject, string templateHtml, string email)
        {
            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Host = ConfigurationManager.AppSettings["SmtpEmail"]; //"smtp.gmail.com";
            client.EnableSsl = false;
            client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]);
            client.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["EmailEnvio"], ConfigurationManager.AppSettings["SenhaEnvio"]);
            MailMessage smail = new MailMessage();
            smail.Sender = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings["EmailEnvio"], "ProClin");
            smail.From = new MailAddress(ConfigurationManager.AppSettings["EmailEnvio"], "ProClin");
            smail.To.Add(new MailAddress(email, "Paciente"));
            smail.Subject = subject;
            smail.Body = templateHtml;
            smail.IsBodyHtml = true;
            try
            {
                client.Send(smail);
            }
            catch (System.Exception ex)
            {
                return false;
            }
            finally
            {
                smail = null;
            }
            return true;
        }
        #endregion

        #region Métodos privados
        private string ProcessTemplate(string templateHtml, JObject json)
        {
            if (json.Count > 0)
            {
                var body = json["BODY"];

                foreach (var segment in body)
                {
                    string propName = ((JProperty)segment).Name.ToUpper();
                    var propValue = segment.Children().First();

                    if (templateHtml.IndexOf("{" + propName + "}") == -1)
                        continue;

                    if (propValue is JValue && ((JValue)propValue).Value != null) //Variavel simples
                    {
                        templateHtml = templateHtml.Replace("{" + propName + "}", ((JValue)propValue).Value.ToString());
                    }
                    else if (propValue is JObject) //Variavel Especial
                    {
                        var tipo = ((JValue)propValue["TYPE"]).Value.ToString();
                        var sb = new StringBuilder();

                        switch (tipo)
                        {
                            case "TABLE_ROW":

                                var rowStyle = (JValue)propValue["ROW_STYLE"];
                                var cellStyle = (JValue)propValue["CELL_STYLE"];
                                var jRows = (JArray)propValue["ITEMS"];

                                if (jRows != null)
                                {
                                    foreach (var row in jRows)
                                    {
                                        if (rowStyle != null)
                                            sb.AppendLine("<tr style=\"" + rowStyle.Value + "\">");
                                        else
                                            sb.AppendLine("<tr>");

                                        foreach (var rowValue in row.Children().OrderBy(t => t))
                                        {
                                            foreach (var cell in rowValue.Children().First())
                                            {
                                                if (cellStyle != null)
                                                    sb.Append("<td style=\"" + cellStyle.Value + "\">");
                                                else
                                                    sb.Append("<td>");
                                                sb.Append(cell);
                                                sb.AppendLine("</td>");
                                            }
                                        }
                                        sb.AppendLine("</tr>");
                                    }

                                    templateHtml = templateHtml.Replace("{" + propName + "}", sb.ToString());
                                }

                                break;

                            case "REPEAT":

                                var prefix = (JValue)propValue["PREFIX"];
                                var separator = (JValue)propValue["SEPARATOR"];
                                var jRepeat = (JArray)propValue["ITEMS"];

                                if (jRepeat != null)
                                {
                                    foreach (var item in jRepeat)
                                    {
                                        if (prefix != null)
                                            sb.Append(prefix.Value);
                                        sb.Append(item);
                                        if (separator != null)
                                            sb.Append(separator.Value);
                                    }
                                    templateHtml = templateHtml.Replace("{" + propName + "}", sb.ToString());
                                }
                                break;
                            default:
                                throw new Exception(string.Format("Tipo de variavel especial '{0}' não encontrado", tipo));
                        }
                    }
                }
            }
            return templateHtml;
        }

        private string ProcessSubject(string subject, JObject json)
        {
            if (json.Count > 0)
            {
                var jSubject = json["SUBJECT"];
                if (jSubject != null)
                {
                    foreach (var segment in jSubject)
                    {
                        string propName = ((JProperty)segment).Name.ToUpper();
                        var propValue = segment.Children().First();
                        if (propValue is JValue)
                            subject = subject.Replace("{" + propName + "}", ((JValue)propValue).Value.ToString());
                    }
                }
            }
            return subject;
        }


        #endregion


        public ICollection<Agenda> ListarAgenda(string situacao, int? idprofissional, string salaespera, int idclinica, int idunidadeatendimento)
        {
            return _repository.ListarAgenda(situacao, idprofissional, salaespera, idclinica, idunidadeatendimento);
        }

        public ICollection<Agenda> ListarAgendaAguardando(int? idprofissional, int idclinica, int idunidadeatendimento)
        {
            return _repository.ListarAgendaAguardando(idprofissional, idclinica,idunidadeatendimento);
        }

        public void GerarAgenda(GerarAgendaViewModel model, int idClinica, Usuario usuario)
        {
            TimeSpan ts = (model.DataFim - model.DataInicio);
            if (ts.Days > 365)
            {
                throw new Exception(" Somente permitido informar períodos menores ou igual a um ano!");
            }

            if (ts.Days <= 0)
            {
                throw new Exception(" Período inválido ");
            }

            //verifica se tem bloqueio de data para gerar a agenda
           // _repository.ObterBloqueioAgenda()

            //verifico se tem agenda cadastrada no periodo selecionado
            var agenda = _repository.PesquisarAgenda(null, model.IdProfissional, string.Empty, model.DataInicio, model.DataFim, idClinica, model.IdUnidadeAtendimento,string.Empty).ToList();
            if (agenda.Count > 0)
            {
                var aguardando = agenda.Where(x => x.Situacao == "Marcado" && x.Situacao == "Realizado");
                //excluo a agenda e crio novamente, depois marco os horários que já estavam selecionado
                _repository.ExcluirAgenda(agenda);

                var datasSemana = new List<DateTime>();
                while (model.DataInicio <= model.DataFim)
                {
                    datasSemana.Add(model.DataInicio);
                    model.DataInicio = model.DataInicio.AddDays(1);
                }

                //obtem o profissional da agenda
                var profissional = _caRepository.ObterFuncionarioById(model.IdProfissional);
                //obtem a clinica da agenda
                var clinica = _caRepository.ObterClinicaById(idClinica);

                var novo = new List<Agenda>();

                foreach (var item in model.AgendaMedica)
                {

                    var HoraInicio = item.HoraInicio.ToString().Split(':');
                    var HoraTermino = item.HoraTermino.ToString().Split(':');

                    DateTime TempoInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(HoraInicio[0]), Convert.ToInt32(HoraInicio[1]), 00);
                    DateTime TempoTermino = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(HoraTermino[0]), Convert.ToInt32(HoraTermino[1]), 00);

                    double intervaloMinutos = item.IntervaloMinutos;

                    List<DateTime> arrHorario = new List<DateTime>();
                    while (TempoInicio <= TempoTermino)
                    {
                        arrHorario.Add(TempoInicio);
                        TempoInicio = TempoInicio.AddHours(0).AddMinutes(intervaloMinutos).AddSeconds(0);
                    }

                    if (item.IntervaloInicio != null && item.IntervaloTermino != null)
                    {
                        var IntInicio = item.IntervaloInicio.ToString().Split(':');
                        var Inttermino = item.IntervaloTermino.ToString().Split(':');
                        DateTime IntervaloInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(IntInicio[0]), Convert.ToInt32(IntInicio[1]), 00);
                        DateTime IntervaloTermino = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(Inttermino[0]), Convert.ToInt32(Inttermino[1]), 00);

                        List<DateTime> arrIntervalo = new List<DateTime>();
                        while (IntervaloInicio < IntervaloTermino)
                        {
                            arrIntervalo.Add(IntervaloInicio);
                            IntervaloInicio = IntervaloInicio.AddHours(0).AddMinutes(intervaloMinutos).AddSeconds(0);
                        }
                        var listaAgendaAguardando = arrHorario.Where(x => !arrIntervalo.Contains(x)).ToList();

                        //aqui só pego as datas(ex: somentes as segundas entre as datas selecionadas) referentes ao dia da semana escolhido
                        var datadodia = datasSemana.Where(s => s.DayOfWeek == RetornaDiaSemana(item.Dia));

                        if (model.IdUnidadeAtendimento > 0)
                        {
                            var unidade = _caRepository.ObterUnidadeAtendimentoPorId(model.IdUnidadeAtendimento);

                            foreach (var data in datadodia)
                            {
                                foreach (var _item in listaAgendaAguardando)
                                {
                                    novo.Add(new Agenda(data, _item.TimeOfDay, profissional, clinica, usuario, unidade));
                                }
                            }
                        }
                        else
                        {
                            foreach (var data in datadodia)
                            {
                                foreach (var _item in listaAgendaAguardando)
                                {
                                    novo.Add(new Agenda(data, _item.TimeOfDay, profissional, clinica, usuario));
                                }
                            }
                        }
                    }
                    else
                    {
                        var listaAgendaAguardando = arrHorario.ToList();

                        var datadodia = datasSemana.Where(s => s.DayOfWeek == RetornaDiaSemana(item.Dia));

                        if (model.IdUnidadeAtendimento > 0)
                        {
                            var unidade = _caRepository.ObterUnidadeAtendimentoPorId(model.IdUnidadeAtendimento);
                            foreach (var data in datadodia)
                            {
                                foreach (var _item in listaAgendaAguardando)
                                {
                                    novo.Add(new Agenda(data, _item.TimeOfDay, profissional, clinica, usuario, unidade));
                                }
                            }
                        }
                        else
                        {
                            foreach (var data in datadodia)
                            {
                                foreach (var _item in listaAgendaAguardando)
                                {
                                    novo.Add(new Agenda(data, _item.TimeOfDay, profissional, clinica, usuario));
                                }
                            }
                        }
                    }
                }
                //agora pego a nova agenda e coloco os horarios que estavam marcados anteriormente
                foreach (var item in aguardando)
                {
                    var horariomarcado = novo.Where(x => x.Data == item.Data && x.Hora == item.Hora).FirstOrDefault();
                    if (horariomarcado != null)
                    {
                        horariomarcado.SetSituacao("Marcado");
                        horariomarcado.SetUsuarioMarcado(item.UsuarioMarcado);
                        horariomarcado.DataMarcado = DateTime.Now;
                        horariomarcado.SetPaciente(item.Paciente);
                        horariomarcado.SetProfissionalSaude(item.ProfissionalSaude);
                        horariomarcado.SetEspecialidade(item.Especialidade);
                        horariomarcado.SetProcedimento(item.Procedimento);
                        horariomarcado.SetValor((decimal)item.Valor);
                        horariomarcado.SetTipoAgendamento(item.Tipo);
                        horariomarcado.SetValorProfissional((decimal)item.ValorProfissional);
                        horariomarcado.SetTipoAtendimento(item.TipoAtendimento);
                        horariomarcado.SetObservacao(item.Observacao);
                        horariomarcado.SetConvenio(item.Convenio);
                        horariomarcado.SetSalaEspera("Nao");
                    }
                }
                _repository.SalvarAgendaCompleta(novo);
            }
            else
            {
                var datasSemana = new List<DateTime>();
                while (model.DataInicio <= model.DataFim)
                {
                    datasSemana.Add(model.DataInicio);
                    model.DataInicio = model.DataInicio.AddDays(1);
                }

                //obtem o profissional da agenda
                var profissional = _caRepository.ObterFuncionarioById(model.IdProfissional);
                //obtem a clinica da agenda
                var clinica = _caRepository.ObterClinicaById(idClinica);

                var novo = new List<Agenda>();

                foreach (var item in model.AgendaMedica)
                {

                    var HoraInicio = item.HoraInicio.ToString().Split(':');
                    var HoraTermino = item.HoraTermino.ToString().Split(':');

                    DateTime TempoInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(HoraInicio[0]), Convert.ToInt32(HoraInicio[1]), 00);
                    DateTime TempoTermino = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(HoraTermino[0]), Convert.ToInt32(HoraTermino[1]), 00);

                    double intervaloMinutos = item.IntervaloMinutos;

                    List<DateTime> arrHorario = new List<DateTime>();
                    while (TempoInicio <= TempoTermino)
                    {
                        arrHorario.Add(TempoInicio);
                        TempoInicio = TempoInicio.AddHours(0).AddMinutes(intervaloMinutos).AddSeconds(0);
                    }

                    if (item.IntervaloInicio != null && item.IntervaloTermino != null)
                    {
                        var IntInicio = item.IntervaloInicio.ToString().Split(':');
                        var Inttermino = item.IntervaloTermino.ToString().Split(':');
                        DateTime IntervaloInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(IntInicio[0]), Convert.ToInt32(IntInicio[1]), 00);
                        DateTime IntervaloTermino = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(Inttermino[0]), Convert.ToInt32(Inttermino[1]), 00);

                        List<DateTime> arrIntervalo = new List<DateTime>();
                        while (IntervaloInicio < IntervaloTermino)
                        {
                            arrIntervalo.Add(IntervaloInicio);
                            IntervaloInicio = IntervaloInicio.AddHours(0).AddMinutes(intervaloMinutos).AddSeconds(0);
                        }
                        var listaAgendaAguardando = arrHorario.Where(x => !arrIntervalo.Contains(x)).ToList();

                        //aqui só pego as datas(ex: somentes as segundas entre as datas selecionadas) referentes ao dia da semana escolhido
                        var datadodia = datasSemana.Where(s => s.DayOfWeek == RetornaDiaSemana(item.Dia)).ToList();
                        if (model.IdUnidadeAtendimento > 0)
                        {
                            var unidade = _caRepository.ObterUnidadeAtendimentoPorId(model.IdUnidadeAtendimento);

                            foreach (var data in datadodia)
                            {
                                foreach (var _item in listaAgendaAguardando)
                                {
                                    novo.Add(new Agenda(data, _item.TimeOfDay, profissional, clinica, usuario, unidade));
                                }
                            }
                        }
                        else
                        {
                            foreach (var data in datadodia)
                            {
                                foreach (var _item in listaAgendaAguardando)
                                {
                                    novo.Add(new Agenda(data, _item.TimeOfDay, profissional, clinica, usuario));
                                }
                            }
                        }
                    }
                    else
                    {
                        var listaAgendaAguardando = arrHorario.ToList();

                        var datadodia = datasSemana.Where(s => s.DayOfWeek == RetornaDiaSemana(item.Dia));

                        if (model.IdUnidadeAtendimento > 0)
                        {
                            var unidade = _caRepository.ObterUnidadeAtendimentoPorId(model.IdUnidadeAtendimento);

                            foreach (var data in datadodia)
                            {
                                foreach (var _item in listaAgendaAguardando)
                                {
                                    novo.Add(new Agenda(data, _item.TimeOfDay, profissional, clinica, usuario, unidade));
                                }
                            }
                        }
                        else
                        {
                            foreach (var data in datadodia)
                            {
                                foreach (var _item in listaAgendaAguardando)
                                {
                                    novo.Add(new Agenda(data, _item.TimeOfDay, profissional, clinica, usuario));
                                }
                            }

                        }
                    }
                    //_repository.SalvarAgendaCompleta(novo);
                }
                _repository.SalvarAgendaCompleta(novo);
            }
        }

        public DayOfWeek RetornaDiaSemana(string dia)
        {
            switch (dia)
            {
                case "Segunda":
                    return DayOfWeek.Monday;
                case "Terca":
                    return DayOfWeek.Tuesday;
                case "Quarta":
                    return DayOfWeek.Wednesday;
                case "Quinta":
                    return DayOfWeek.Thursday;
                case "Sexta":
                    return DayOfWeek.Friday;
                case "Sabado":
                    return DayOfWeek.Saturday;
                case "Domingo":
                    return DayOfWeek.Sunday;
                default:
                    return DayOfWeek.Monday;
            }
        }

        public void LiberarAgenda(LiberarAgendaViewModel liberaragenda)
        {

            TimeSpan ts = (liberaragenda.DataTermino - liberaragenda.DataInicio);
            if (ts.Days > 15)
            {
                throw new Exception(" Somente permitido informar períodos anteriores ou igual a 15 dias ");
            }

            if (ts.Days < 0)
            {
                throw new Exception(" Período inválido ");
            }

            // monta list dia da semana
            var intList = new List<int>();
            if (liberaragenda.DiaSemana == null)
            {
                throw new Exception(" Nenhum dia da semana selecionado ");
            }

            #region [Dia Semana]
            if (liberaragenda.DiaSemana.Domingo)
            {
                intList.Add(0);
            }
            if (liberaragenda.DiaSemana.Segunda)
            {
                intList.Add(1);
            }
            if (liberaragenda.DiaSemana.Terca)
            {
                intList.Add(2);
            }
            if (liberaragenda.DiaSemana.Quarta)
            {
                intList.Add(3);
            }
            if (liberaragenda.DiaSemana.Quinta)
            {
                intList.Add(4);
            }
            if (liberaragenda.DiaSemana.Sexta)
            {
                intList.Add(5);
            }
            if (liberaragenda.DiaSemana.Sabado)
            {
                intList.Add(6);
            }
            #endregion

            // deleta agenda Aguardando
            #region [ LIBERAR AGENDA ]
            if (liberaragenda.PossuiIntervalo)
            {
                var HoraInicio = liberaragenda.HoraInicio.Split(':');
                var HoraTermino = liberaragenda.HoraTermino.Split(':');

                DateTime TempoInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(HoraInicio[0]), Convert.ToInt32(HoraInicio[1]), 00);
                DateTime TempoTermino = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(HoraTermino[0]), Convert.ToInt32(HoraTermino[1]), 00);

                double intervaloMinutos = liberaragenda.IntervaloMinutos;

                List<DateTime> arrHorario = new List<DateTime>();
                while (TempoInicio <= TempoTermino)
                {
                    arrHorario.Add(TempoInicio);
                    TempoInicio = TempoInicio.AddHours(0).AddMinutes(intervaloMinutos).AddSeconds(0);
                }

                var IntInicio = liberaragenda.IntervaloInicio.Split(':');
                var Inttermino = liberaragenda.IntervaloTermino.Split(':');
                DateTime IntervaloInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(IntInicio[0]), Convert.ToInt32(IntInicio[1]), 00);
                DateTime IntervaloTermino = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(Inttermino[0]), Convert.ToInt32(Inttermino[1]), 00);

                List<DateTime> arrIntervalo = new List<DateTime>();
                while (IntervaloInicio < IntervaloTermino)
                {
                    arrIntervalo.Add(IntervaloInicio);
                    IntervaloInicio = IntervaloInicio.AddHours(0).AddMinutes(intervaloMinutos).AddSeconds(0);
                }
                var listaAgendaAguardando = arrHorario.Where(x => !arrIntervalo.Contains(x)).ToList();



                while (liberaragenda.DataInicio <= liberaragenda.DataTermino)
                {
                    int dia = (int)liberaragenda.DataInicio.DayOfWeek;
                    if (intList.Contains(dia))
                    {


                        foreach (var item in listaAgendaAguardando)
                        {
                            // .ToString("yyyy-MM-dd")
                            //.ToString(),
                            _repository.LiberarAgenda(liberaragenda.DataInicio, item.TimeOfDay, "", liberaragenda.IdProfissional, liberaragenda.Usuario.IdUsuario, liberaragenda.IdClinica);
                        }
                    }
                    liberaragenda.DataInicio = liberaragenda.DataInicio.AddDays(1);
                }
            }
            else
            {
                // cadastra uma agenda sem inervalo 

                var HoraInicio = liberaragenda.HoraInicio.Split(':');
                var HoraTermino = liberaragenda.HoraTermino.Split(':');

                DateTime TempoInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(HoraInicio[0]), Convert.ToInt32(HoraInicio[1]), 00);
                DateTime TempoTermino = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, Convert.ToInt32(HoraTermino[0]), Convert.ToInt32(HoraTermino[1]), 00);

                double intervaloMinutos = liberaragenda.IntervaloMinutos;

                List<DateTime> arrHorario = new List<DateTime>();
                while (TempoInicio <= TempoTermino)
                {
                    arrHorario.Add(TempoInicio);
                    TempoInicio = TempoInicio.AddHours(0).AddMinutes(intervaloMinutos).AddSeconds(0);
                }

                var listaAgendaAguardando = arrHorario.ToList();

                while (liberaragenda.DataInicio <= liberaragenda.DataTermino)
                {
                    int dia = (int)liberaragenda.DataInicio.DayOfWeek;
                    if (intList.Contains(dia))
                    {
                        foreach (var item in listaAgendaAguardando)
                        {
                            //.ToString("yyyy-MM-dd") .ToString()
                            _repository.LiberarAgenda(liberaragenda.DataInicio, item.TimeOfDay, "", liberaragenda.IdProfissional, liberaragenda.Usuario.IdUsuario, liberaragenda.IdClinica);
                        }
                    }
                    liberaragenda.DataInicio = liberaragenda.DataInicio.AddDays(1);
                }
            }
            #endregion
        }

        public ICollection<Agenda> PesquisarAgendaAguardando(int? idagenda, string profissionalSaude, int idclinica,int idunidadeatendimento)
        {
            return _repository.PesquisarAgendaAguardando(idagenda, profissionalSaude, idclinica, idunidadeatendimento);
        }

        public Agenda AgendaAvulsa(Agenda avulsa)
        {
            return _repository.AgendaAvulsa(avulsa);
        }
        public Agenda ObterAgendaPorId(int idagenda)
        {
            return _repository.ObterAgendaPorId(idagenda);
        }

        public Agenda CancelarAtendimento(int idagenda, int idusuario)
        {
            return _repository.CancelarAtendimento(idagenda, idusuario);
        }

        public void ConfirmarAtendimento(int idagenda, int idusuario)
        {
            _repository.ConfirmarAtendimento(idagenda, idusuario);
        }

        public ICollection<Agenda> ListarAgendaProfissionalSaudePorMes(int mes, int idprofissional)
        {
            return _repository.ListarAgendaProfissionalSaudePorMes(mes, idprofissional);
        }

        public ICollection<Agenda> PesquisarAgenda(int? idagenda, int? idprofissional, string paciente, DateTime dataInicio, DateTime dataTermino, int idclinica, int idunidadeatendimento, string situacao)
        {
            TimeSpan ts = (dataTermino - dataInicio);

            if (ts.Days < 0)
                throw new Exception(" Informe um período válido ");

            if (ts.Days > 180)
                throw new Exception(" Somente permitido informar períodos anteriores ou igual a 180 dias ");


            return _repository.PesquisarAgenda(idagenda, idprofissional, paciente, dataInicio, dataTermino, idclinica, idunidadeatendimento, situacao);
        }

        public TipoAtendimento ObterTipoAtendimentoPorId(int idTipo)
        {
            return _repository.ObterTipoAtendimentoPorId(idTipo);
        }

        public void Marcar(int idagenda, int idpaciente, int idprofissional, int idespecialidade, int idprocedimento, int? idconvenio, string tipo, string observacoes, string solicitante, decimal valor, decimal valorProfissional, int? idtipoAtendimento, int idusuario, int idUnidadeAtendimento)
        {
            _repository.Marcar(idagenda, idpaciente, idprofissional, idespecialidade, idprocedimento, idconvenio, tipo, observacoes, solicitante, valor, valorProfissional, idtipoAtendimento, idusuario, idUnidadeAtendimento);
        }

        public Agenda ObterAgendaDiaHora(DateTime data, TimeSpan hora, int idProfissional, int idClinica,int idunidadeatendimento)
        {
            return _repository.ObterAgendaDiaHora(data, hora, idProfissional, idClinica, idunidadeatendimento);
        }

        public void EncaminharSalaEspera(int idagenda)
        {
            _repository.EncaminharSalaEspera(idagenda);
        }
        public ICollection<Agenda> ListarAgendaMedicaPorData(int idMedico, DateTime data, int idClinica, int idunidadeatendimento)
        {
            return _repository.ListarAgendaMedicaPorData(idMedico, data, idClinica, idunidadeatendimento);
        }
        public ICollection<Agenda> ListarAgendaMedicaPorPeriodo(int idMedico, DateTime dataInicio, DateTime dataFim)
        {
            return _repository.ListarAgendaMedicaPorPeriodo(idMedico, dataInicio, dataFim);
        }
        public void ExcluirAgendasAnteriores(int idClinica)
        {
            _repository.ExcluirAgendasAnteriores(idClinica);
        }

        public Agenda SalvarAgenda(Agenda agenda)
        {
            return _repository.SalvarAgenda(agenda);
        }

        public ICollection<Agenda> ListarPacientesConvocados()
        {
            return _repository.ListarPacientesConvocados();
        }
    }
}