using Clinicas.Api.Models;
using Clinicas.Application.Services.Interfaces;
using Clinicas.Domain.Mail;
using Clinicas.Domain.Model;
using Clinicas.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Clinicas.Api.Controllers
{
    [RoutePrefix("agenda")]
    [Authorize]
    public class AgendaController : BaseController
    {
        private readonly IAgendaService _serviceAgenda;
        private readonly IUsuarioService _usuarioService;
        private readonly ICadastroService _cadastroService;

        public AgendaController(IAgendaService serviceAgenda, IUsuarioService usuarioservice, ICadastroService cadastroService) : base(usuarioservice)
        {
            _serviceAgenda = serviceAgenda;
            _usuarioService = usuarioservice;
            _cadastroService = cadastroService;
        }

        [HttpGet]
        [Route("encaminharsalaespera")]
        public HttpResponseMessage EncaminharSalaEspera(int idagenda)
        {
            try
            {
                _serviceAgenda.EncaminharSalaEspera(idagenda);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        [Route("gerarAgenda")]
        public HttpResponseMessage GerarAgenda(GerarAgendaViewModel model)
        {
            try
            {
                _serviceAgenda.GerarAgenda(model, base.GetUsuarioLogado().IdClinica, base.GetUsuarioLogado());
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("convocarPaciente")]
        public HttpResponseMessage ConvocarPaciente(int idagenda)
        {
            try
            {
                var agenda = _serviceAgenda.ObterAgendaPorId(idagenda);
                agenda.SetConvocarPaciente();
                _serviceAgenda.SalvarAgenda(agenda);

                return Request.CreateResponse(HttpStatusCode.OK, agenda);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("listarPacientesConvocados")]
        public HttpResponseMessage ListarPacientesConvocados()
        {
            try
            {
                var lista = _serviceAgenda.ListarPacientesConvocados();
                var model = new List<PacientesConvocadosViewModel>();

                foreach (var item in lista)
                {
                    model.Add(new PacientesConvocadosViewModel()
                    {
                        NmPaciente = item.Paciente.Pessoa.Nome,
                        NmProfissionalSaude = item.ProfissionalSaude.Pessoa.Nome,
                        Data = item.Data,
                        Hora = item.Hora,
                        Consultorio = item.Consultorio != null ? item.Consultorio.NmConsultorio : "",
                        IdAgenda = item.IdAgenda
                    });
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("liberarAgenda")]
        public HttpResponseMessage LiberarAgenda(LiberarAgendaViewModel obj)
        {
            try
            {
                obj.IdClinica = base.GetUsuarioLogado().IdClinica;
                obj.Usuario = base.GetUsuarioLogado();
                _serviceAgenda.LiberarAgenda(obj);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("listarAgenda")]
        public HttpResponseMessage ListarAgenda(string situacao = "Todos", int? idprofissional = 0)
        {
            try
            {
                var result = _serviceAgenda.ListarAgenda(situacao: situacao, idprofissional: idprofissional, idclinica: base.GetUsuarioLogado().IdClinica);
                var model = new List<AgendaViewModel>();
                foreach (var item in result)
                {
                    model.Add(new AgendaViewModel()
                    {
                        IdAgenda = item.IdAgenda,
                        Valor = item.Valor,
                        ValorProfissional = item.ValorProfissional,
                        NmProcedimento = item.Procedimento.NomeProcedimento,
                        SalaEspera = item.SalaEspera,
                        NmProfissionalSaude = item.ProfissionalSaude.Pessoa.Nome,
                        NmPaciente = item.Paciente != null ? item.Paciente.Pessoa.Nome : "",  //Todo: Navegação
                        Data = item.Data,
                        Tipo = item.Tipo,
                        Hora = item.Hora,
                        Situacao = item.Situacao
                    });
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("listarAgendaAguardando")]
        public HttpResponseMessage ListarAgendaAguardando(int? idprofissional = 0)
        {
            try
            {
                var result = _serviceAgenda.ListarAgendaAguardando(idprofissional, base.GetUsuarioLogado().IdClinica,base.GetUsuarioLogado().IdUnidadeAtendimento);
                var model = new List<AgendaViewModel>();
                foreach (var item in result)
                {
                    model.Add(new AgendaViewModel()
                    {
                        IdAgenda = item.IdAgenda,
                        NmProfissionalSaude = item.ProfissionalSaude.Pessoa.Nome,
                        Data = item.Data,
                        Hora = item.Hora,
                        Situacao = item.Situacao
                    });
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("pesquisarAgendaAguardando")]
        public HttpResponseMessage PesquisarAgendaAguardando(int? idagenda, string profissionalsaude)
        {
            try
            {
                var result = _serviceAgenda.PesquisarAgendaAguardando(idagenda, profissionalsaude, base.GetUsuarioLogado().IdClinica,base.GetUsuarioLogado().IdUnidadeAtendimento);
                var model = new List<AgendaViewModel>();
                foreach (var item in result)
                {
                    model.Add(new AgendaViewModel()
                    {
                        IdAgenda = item.IdAgenda,
                        NmProfissionalSaude = item.ProfissionalSaude.Pessoa.Nome,
                        Data = item.Data,
                        Hora = item.Hora,
                        Situacao = item.Situacao
                    });
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("salaespera")]
        public HttpResponseMessage SalaEspera(int idprofissional)
        {
            try
            {
                var result = _serviceAgenda.ListarAgenda(situacao: "Marcado", idprofissional: idprofissional, salaEspera: "Sim", idclinica: base.GetUsuarioLogado().IdClinica);
                var model = new List<AgendaViewModel>();
                foreach (var item in result)
                {
                    model.Add(new AgendaViewModel()
                    {
                        IdAgenda = item.IdAgenda,
                        Valor = item.Valor,
                        ValorProfissional = item.ValorProfissional,
                        SalaEspera = item.SalaEspera,
                        NmProfissionalSaude = item.ProfissionalSaude.Pessoa.Nome,
                        NmPaciente = item.Paciente.Pessoa.Nome,
                        IdPaciente = item.Paciente.IdPaciente,
                        NmProcedimento = item.Procedimento.NomeProcedimento,
                        Data = item.Data,
                        Hora = item.Hora,
                        Situacao = item.Situacao
                    });
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("obteragendaporid")]
        public HttpResponseMessage ObterAgendaPorId(int id)
        {
            try
            {
                var result = _serviceAgenda.ObterAgendaPorId(id);
                var model = new AgendaViewModel();

                model.IdAgenda = result.IdAgenda;
                model.Valor = result.Valor;
                model.ValorProfissional = result.ValorProfissional;
                model.IdPaciente = result.Paciente == null ? 0 : result.Paciente.IdPaciente;
                model.SalaEspera = result.SalaEspera;
                model.NmProfissionalSaude = result.ProfissionalSaude == null ? "" : result.ProfissionalSaude.Pessoa.Nome;
                model.NmPaciente = result.Paciente == null ? "" : result.Paciente.Pessoa.Nome;
                model.NmConvenio = result.Convenio != null ? result.Convenio.Pessoa.Nome : null;
                model.Data = result.Data;
                model.Hora = result.Hora;
                model.RG = result.Paciente == null ? "" : result.Paciente.Pessoa.RG;
                model.Mae = result.Paciente == null ? "" : result.Paciente.Pessoa.Mae;
                model.DtNascimento = result.Paciente == null ? null : result.Paciente.Pessoa.DataNascimento;
                model.Email = result.Paciente == null ? "" : result.Paciente.Pessoa.Email;
                model.NmProcedimento = result.Procedimento == null ? "" : result.Procedimento.NomeProcedimento;
                model.Tipo = result.Tipo == "C" ? "Convênio" : "Particular";
                model.UnidadeAtendimento = result.UnidadeAtendimento.Nome;
                model.Situacao = result.Situacao;
                model.Telefone1 = result.Paciente == null ? "" : result.Paciente.Pessoa.Telefone1;
                model.Telefone2 = result.Paciente == null ? "" : result.Paciente.Pessoa.Telefone1;
                model.SalaEspera = result.SalaEspera;
                model.Foto = result.Paciente == null ? null : result.Paciente.Foto;


                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("realizado")]
        public HttpResponseMessage ConfirmarAtendimento(int idagenda)
        {
            try
            {
                _serviceAgenda.ConfirmarAtendimento(idagenda, base.GetUsuarioLogado().IdUsuario);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("cancelado")]
        public HttpResponseMessage CancelarAtendimento(int idagenda)
        {
            try
            {
                var agenda = _serviceAgenda.CancelarAtendimento(idagenda, base.GetUsuarioLogado().IdUsuario);
                CriarAgendaNovaCancelado(agenda);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public void CriarAgendaNovaCancelado(Agenda agenda)
        {
            var funcionario = _cadastroService.ObterFuncionarioById(agenda.IdFuncionario);
            if (funcionario == null)
                throw new Exception("Não foi possível obter dados do profissional.");

            var clinica = _cadastroService.ObterClinicaById(base.GetUsuarioLogado().IdClinica);
            if (clinica == null)
                throw new Exception("Não foi possível obter dados da clinica.");

            var novo = new Agenda(Convert.ToDateTime(agenda.Data), agenda.Hora, funcionario, clinica, base.GetUsuarioLogado());

            novo.SetAvulsa("Nao");
            novo.SetSalaEspera("Nao");
            novo.SetSituacao("Aguardando");
            _serviceAgenda.AgendaAvulsa(novo);
        }

        [HttpGet]
        [Route("ListarAgendaProfissionalSaudePorMes")]
        public HttpResponseMessage ListarAgendaProfissionalSaudePorMes(int mes, int idprofissional)
        {
            try
            {
                var result = _serviceAgenda.ListarAgendaProfissionalSaudePorMes(mes, idprofissional);
                var model = new List<CalendarioViewModel>();

                string classe = "";
                foreach (var item in result)
                {
                    switch (item.Situacao)
                    {
                        case "Aguardando":
                            classe = "fc-event-warning";
                            break;
                        case "Marcado":
                            classe = "fc-event-success";
                            break;
                        case "Realizado":
                            classe = "fc-event-info";
                            break;
                        case "Cancelado":
                            classe = "fc-event-danger";
                            break;
                    }

                    model.Add(new CalendarioViewModel()
                    {
                        id = item.IdAgenda,
                        start = new DateTime(item.Data.Year, item.Data.Month, item.Data.Day, item.Hora.Hours, item.Hora.Minutes, 00),
                        title = item.Paciente != null ? item.Paciente.Pessoa.Nome : null,
                        className = classe
                    });
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("pesquisarAgenda")]
        public HttpResponseMessage PesquisarAgenda(int? idagenda, int? idprofissional, string paciente, DateTime dataInicio, DateTime dataTermino, string situacao)
        {
            try
            {
                var result = _serviceAgenda.PesquisarAgenda(idagenda, idprofissional, paciente, dataInicio, dataTermino, base.GetUsuarioLogado().IdClinica,base.GetUsuarioLogado().IdUnidadeAtendimento, situacao);
                var model = new List<AgendaViewModel>();
                foreach (var item in result)
                {
                    model.Add(new AgendaViewModel()
                    {
                        IdAgenda = item.IdAgenda,
                        Valor = item.Valor,
                        ValorProfissional = item.ValorProfissional,
                        NmProcedimento = item.Procedimento.NomeProcedimento,
                        SalaEspera = item.SalaEspera,
                        NmProfissionalSaude = item.ProfissionalSaude.Pessoa.Nome,
                        NmPaciente = item.Paciente != null ? item.Paciente.Pessoa.Nome : "",  //Todo: Navegação
                        Data = item.Data,
                        Tipo = item.Tipo,
                        Hora = item.Hora,
                        Situacao = item.Situacao
                    });
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [Route("marcado")]
        public HttpResponseMessage Marcado(NovoAgendamentoViewModel agenda)
        {
            try
            {
                var paciente = _cadastroService.ObterPacienteById(agenda.idpaciente);
                if (paciente == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possivel recuperar dados do paciente.");

                var clinica = _cadastroService.ObterClinicaById(base.GetUsuarioLogado().IdClinica);
                var unidade = _cadastroService.ObterUnidadeAtendimentoPorId(base.GetUsuarioLogado().IdUnidadeAtendimento);
                if (clinica == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possivel recuperar dados da clinica.");


                _serviceAgenda.Marcar(agenda.idagenda, agenda.idpaciente, agenda.idprofissional,
                    agenda.idespecialidade, agenda.idprocedimento, agenda.idconvenio, agenda.tipo, agenda.observacoes,
                    agenda.solicitante, agenda.valor, agenda.valorProfissional, agenda.idtipoatendimento, base.GetUsuarioLogado().IdUsuario, agenda.idUnidadeAtendimento);



                if (!String.IsNullOrEmpty(paciente.Pessoa?.Email))
                {

                    var template = _serviceAgenda.GetTemplateByIdentifier("AGENDAMENTO_ONLINE");
                    if (template == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possivel recuperar o Templete de e-mail.");

                    string endereco = unidade.Logradouro + ", " + unidade.Numero + "Bairro: " + unidade.Bairro;
                    string protocolo = agenda.idagenda.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();

                    var obj = new JsonTemplate()
                    {
                        SUBJECT = new JsonTemplateSubject() { PROTOCOLO = protocolo },
                        BODY = new JsonTemplateBody()
                        {
                            PACIENTE = paciente.Pessoa.Nome.ToUpper(),
                            CLINICA = clinica.Nome + " - " + endereco,
                            DATA_HORA = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString(),
                            TELEFONE = unidade.Telefone1,
                            PROTOCOLO = protocolo,
                        }
                    };
                    var json = new JavaScriptSerializer().Serialize(obj);

                    _serviceAgenda.AddToQueue(new MailQueue(template, paciente.Pessoa.Email, json));
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("datasdisponiveis")]
        public object DatasDisponiveisPorMedico(int idMedico)
        {
            object datas;
            //var agenda = _serviceAgenda.ListarAgenda("Todos", idMedico, "Nao", base.GetUsuarioLogado().IdClinica);

            var agenda = _serviceAgenda.ListarAgendaAguardando(idMedico, base.GetUsuarioLogado().IdClinica, base.GetUsuarioLogado().IdUnidadeAtendimento);

            datas = new
            {
                Meses = agenda.Select(d => d.Data.Month).Distinct(),
                Datas = agenda.Select(a => new { date = a.Data.ToString("yyyy-MM-dd") })
            };

            return datas;
        }

        [HttpGet]
        [Route("agendaMedica")]
        public List<AgendaMedicaViewModel> AgendaMedica(int idMedico, DateTime data)
        {
            var agenda = _serviceAgenda.ListarAgendaMedicaPorData(idMedico, data, base.GetUsuarioLogado().IdClinica,base.GetUsuarioLogado().IdUnidadeAtendimento);
            var model = new List<AgendaMedicaViewModel>();
            foreach (var item in agenda)
            {
                model.Add(new AgendaMedicaViewModel
                {
                    Data = item.Data.ToShortDateString(),
                    Hora = item.Hora.ToString(@"hh\:mm"),
                    IdAgenda = item.IdAgenda,
                });
            }

            return model;
        }

        [HttpPost]
        [Route("agendar")]
        public HttpResponseMessage AgendarPaciente(AgendaProcedimentoViewModel model)
        {
            try
            {
                if (model.Avulsa)
                {
                    TimeSpan time = TimeSpan.Parse(model.Hora);
                    //verifica se já tem uma agenda para o dia e o horario
                    var agenda = _serviceAgenda.ObterAgendaDiaHora(Convert.ToDateTime(model.Data), time, model.IdProfissional, base.GetUsuarioLogado().IdClinica,base.GetUsuarioLogado().IdUnidadeAtendimento);
                    if (agenda != null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Já existem uma agenda para essa data e hora!");

                    var clinica = _cadastroService.ObterClinicaById(base.GetUsuarioLogado().IdClinica);
                    if (clinica == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados da clinica.");

                    var funcionario = _cadastroService.ObterFuncionarioById(model.IdProfissional);
                    if (funcionario == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do profissional.");

                    var novo = new Agenda(Convert.ToDateTime(model.Data), time, funcionario, clinica, base.GetUsuarioLogado());

                    novo.SetTipoAgendamento(model.Tipo);

                    var atendimento = _serviceAgenda.ObterTipoAtendimentoPorId(model.IdTipoAtendimento);
                    if (atendimento == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do tipo de atendimento.");

                    novo.SetTipoAtendimento(atendimento);

                    var paciente = _cadastroService.ObterPacienteById(model.IdPaciente);
                    if (paciente == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do paciente.");

                    novo.SetPaciente(paciente);

                    var procedimento = _cadastroService.ObterProcedimentoById(model.IdProcedimento);
                    if (procedimento == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do procedimento.");

                    novo.SetProcedimento(procedimento);

                    var especialidade = _cadastroService.GetEspecialidadeById(model.IdEspecialidade);
                    if (especialidade == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados da especialidade.");

                    novo.SetEspecialidade(especialidade);

                    if (model.IdConvenio > 0)
                    {
                        var convenio = _cadastroService.ObterConvenioById(model.IdConvenio);
                        if (convenio == null)
                            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do convenio.");

                        novo.SetConvenio(convenio);
                    }


                    novo.SetAvulsa("Sim");
                    novo.SetSituacao("Marcado");
                    novo.SetValor(model.Valor);
                    novo.SetValorProfissional(model.ValorProfissional);

                    if (model.IdUnidadeAtendimento > 0)
                    {
                        var unidade = _cadastroService.ObterUnidadeAtendimentoPorId(model.IdUnidadeAtendimento);
                        if (unidade == null)
                            throw new Exception("Erro ao recuperar a unidade de atendimento.");

                        novo.SetUnidadeAtendimento(unidade);
                    }

                    _serviceAgenda.AgendaAvulsa(novo);
                }
                else
                {
                    _serviceAgenda.Marcar(model.IdAgenda, model.IdPaciente, model.IdProfissional,
                        model.IdEspecialidade, model.IdProcedimento, model.IdConvenio, model.Tipo,
                        model.Observacao, model.Solicitante, model.Valor, model.ValorProfissional,
                        model.IdTipoAtendimento, base.GetUsuarioLogado().IdClinica, model.IdUnidadeAtendimento);
                }

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #region [Bloqueio de agenda]
        [HttpGet]
        [Route("listarBloqueioAgenda")]
        public List<BloqueioAgendaViewModel> ListarBloqueioAgenda()
        {
            var bloqueios = _serviceAgenda.ListarBloqueioAgenda(base.GetUsuarioLogado().IdClinica);
            var model = new List<BloqueioAgendaViewModel>();
            foreach (var item in bloqueios)
            {
                model.Add(new BloqueioAgendaViewModel
                {
                    DataFim = item.DataFim,
                    DataInicio = item.DataInicio,
                    Funcionario = item.Funcionario.Pessoa.Nome,
                    IdBloqueio = item.IdBloqueio,
                    IdFuncionario = item.IdFuncionario,
                    Motivo = item.Motivo
                });
            }

            return model;
        }

        [HttpGet]
        [Route("obterBloqueioAgenda")]
        public HttpResponseMessage ObterBloqueioAgenda(int id)
        {
            try
            {
                var model = new BloqueioAgendaViewModel();
                var result = _serviceAgenda.ObterBloqueioAgenda(id);

                if (result == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do bloqueio de agenda.");

                model.DataFim = result.DataFim;
                model.DataInicio = result.DataInicio;
                model.Funcionario = result.Funcionario.Pessoa.Nome;
                model.IdBloqueio = result.IdBloqueio;
                model.IdFuncionario = result.IdFuncionario;
                model.Motivo = result.Motivo;

                return Request.CreateResponse(HttpStatusCode.OK, model);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("salvarBloqueioAgenda")]
        public HttpResponseMessage SalvarBloqueioAgenda(BloqueioAgendaViewModel model)
        {
            var result = new BloqueioAgendaAvisoViewModel();
            try
            {
                if (model.IdBloqueio > 0)
                {
                    var bloqueio = _serviceAgenda.ObterBloqueioAgenda(model.IdBloqueio);
                    bloqueio.SetDataFim(model.DataFim);
                    bloqueio.SetDataInicio(model.DataInicio);
                    bloqueio.SetMotivo(model.Motivo);

                    _serviceAgenda.SalvarBloqueioAgenda(bloqueio);
                }
                else
                {
                    var funcionario = _cadastroService.ObterFuncionarioById(model.IdFuncionario);
                    if (funcionario == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do funcionário.");

                    var clinica = _cadastroService.ObterClinicaById(base.GetUsuarioLogado().IdClinica);
                    if (clinica == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados da clinica.");

                    //recupero a agenda do funcionário no periódo selecionado
                    var agendaperiodo = _serviceAgenda.ListarAgendaMedicaPorPeriodo(funcionario.IdFuncionario, model.DataInicio, model.DataFim);
                    if (agendaperiodo.Count == 0)
                    {
                        var bloqueio = new BloqueioAgenda(funcionario, clinica, model.DataInicio, model.DataFim, model.Motivo);
                        _serviceAgenda.SalvarBloqueioAgenda(bloqueio);
                        result.Aceito = true;
                    }
                    else
                    {
                        result.Aceito = false;

                        var marcados = agendaperiodo.Where(x => x.Situacao == "Marcado").ToList();
                        foreach (var item in marcados)
                        {
                            result.PacientesMarcados.Add(new PacientesMarcados
                            {
                                Data = item.Data.ToShortDateString() + " as " + item.Hora.ToString(@"hh\:mm"),
                                Email = item.Paciente.Pessoa.Email,
                                Nome = item.Paciente.Pessoa.Nome,
                                Telefone = item.Paciente.Pessoa.Telefone1
                            });
                        }

                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("excluirBloqueioAgenda")]
        public HttpResponseMessage ExcluirBloqueioAgenda(int id)
        {
            try
            {
                var result = _serviceAgenda.ObterBloqueioAgenda(id);
                if (result == null)
                    Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do bloqueio de agenda.");

                _serviceAgenda.ExcluirBloqueioAgenda(result);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion


    }
}