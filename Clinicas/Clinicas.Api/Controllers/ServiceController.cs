using AutoMapper;
using Clinicas.Application.Services.Interfaces;
using Clinicas.Domain.Model;
using Clinicas.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Web.Http;
using System.Drawing.Imaging;
using System.Drawing;
using Clinicas.Api.Models;
using System.Web.Script.Serialization;
using Clinicas.Domain.Mail;

namespace Clinicas.Api.Controllers
{
    [RoutePrefix("servico")]
    public class ServiceController : BaseController
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IAgendaService _service;
        private readonly ICadastroService _serviceCadastro;

        public ServiceController(IUsuarioService usuarioService, ICadastroService serviceCadastro, IAgendaService service) : base(usuarioService)
        {
            _usuarioService = usuarioService;
            _serviceCadastro = serviceCadastro;
            _service = service;
        }


        [HttpGet]
        [Route("emailteste")]
        [AllowAnonymous]
        public HttpResponseMessage EmailTeste()
        {
            try
            {
                var template = _service.GetTemplateByIdentifier("AGENDAMENTO_ONLINE");
                if (template == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possivel recuperar o Templete de e-mail.");

                var obj = new JsonTemplate()
                {
                    SUBJECT = new JsonTemplateSubject() { PROTOCOLO = "12346789" },
                    BODY = new JsonTemplateBody()
                    {
                        PACIENTE = "RENATO AYRES",
                        CLINICA = "CLINICA TESTE",
                        DATA_HORA = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString(),
                        TELEFONE = "(31) 3355-6688",
                        PROTOCOLO = "12346789",
                    }
                };
                var json = new JavaScriptSerializer().Serialize(obj);

                _service.AddToQueue(new MailQueue(template, "romneymoreira@gmail.com", json));
                _service.AddToQueue(new MailQueue(template, "renatouai@gmail.com", json));

                _service.ProcessQueue();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("ProcessQueueEmail")]
        public HttpResponseMessage ProcessQueueEmail()
        {
            try
            {
                _service.ProcessQueue();
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("enviarSms")]
        public HttpResponseMessage EnviarSms()
        {
            try
            {
                // 


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Route("esqueciminhasenha")]
        public HttpResponseMessage EsqueciMinhaSenha(string login)
        {
            try
            {

                var user = _usuarioService.ObterUsuarioLogin(login);
                if (user == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possivel recuperar dados do usuário.");

                var template = _service.GetTemplateByIdentifier("ESQUECI_MINHA_SENHA");
                if (template == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possivel recuperar o Templete de e-mail.");

                user.ResetarSenha();

                _usuarioService.SalvarUsuario(user);

                string protocolo = user.IdUsuario.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
                var obj = new JsonTemplateSenha()
                {
                    SUBJECT = new JsonTemplateSubjectSenha() { PROTOCOLO = protocolo },
                    BODY = new JsonTemplateBodySenha()
                    {
                        NOVA_SENHA = user.Senha,
                        USUARIO = user.Login
                    }
                };
                var json = new JavaScriptSerializer().Serialize(obj);

                _service.AddToQueue(new MailQueue(template, user.Login, json));
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// serviço para excluir a agenda dos medicos que ficaram sem paciente 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Route("excluirAgendaAguardando")]
        public HttpResponseMessage ExcluirAgendaAguardando()
        {
            try
            {
                //var clinica = _serviceCadastro.ObterClinicaById(base.GetUsuarioLogado().IdClinica);
                _service.ExcluirAgendasAnteriores(base.GetUsuarioLogado().IdClinica);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("getAllEspecialidades")]
        public HttpResponseMessage ListarEspecialidades()
        {
            try
            {
                var model = new List<EspecialidadeViewModel>();
                var result = _serviceCadastro.ListarEspecialidades();

                if (result != null)
                {
                    /* Mapper.CreateMap<Especialidade, EspecialidadeViewModel>();
                    model = Mapper.Map<List<Especialidade>, List<EspecialidadeViewModel>>(result); */
                    foreach (var item in result)
                    {
                        var procedimento = new List<ProcedimentosViewModel>();
                        if (item.Procedimentos.Count > 0)
                        {
                            foreach (var x in item.Procedimentos)
                            {
                                procedimento.Add(new ProcedimentosViewModel()
                                {
                                    IdProcedimento = x.IdProcedimento,
                                    Codigo = x.Codigo,
                                    NmProcedimento = x.NomeProcedimento
                                });
                            }
                        }
                        model.Add(new EspecialidadeViewModel()
                        {
                            IdEspecialidade = item.IdEspecialidade,
                            NmEspecialidade = item.NmEspecialidade,
                            Procedimentos = procedimento
                        });
                    }

                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados da especialidade.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("solicitaracesso")]
        public HttpResponseMessage SolicitarAcesso(SolicitarAcessoViewModel model)
        {
            try
            {
                //valida se já existe um usuário
                var usuario = _usuarioService.ObterUsuarioLogin(model.Email);
                if(usuario != null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Já existe um usuário criado com esse login.");

                //valida se já existe uma clinica cadastrada
                 var clinica = _serviceCadastro.ObterUnidadeDeAtendimentoByCnpj(model.Cnpj);
                if (clinica != null)
                  return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Já existe uma clinica cadastrada no sistema, utilize a opção 'esqueci minha senha'.");
                 
                var cidade = _serviceCadastro.ObterCidadePorId(model.IdCidade);
                if (cidade == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao recuperar dados da cidade.");

                var estado = _serviceCadastro.ObterEstadoPorId(model.IdEstado);
                if (estado == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao recuperar dados do estado.");
                
                // adiciona uma clinica
                var novaclinica = new Domain.Model.Clinica(model.Nome);

                // adiciona a unidade principal da clínica 
                novaclinica.AddUnidade(new Domain.Model.UnidadeAtendimento(model.Nome, model.Cnpj, novaclinica,estado,cidade,model.Email,model.Telefone, _serviceCadastro.GetEspecialidadeById(model.IdEspecialidade)));
                
                //insere dados da nova clinica
                var clin = _serviceCadastro.SalvarClinica(novaclinica);

                var grupo = _usuarioService.ObterGrupoUsuarioAdministrador("Administrador");
                if (grupo == null)
                {
                    grupo = new Domain.Model.GrupoUsuario("Administrador");
                    _usuarioService.SalvarGrupoUsuario(grupo);
                }

                var unidade = _serviceCadastro.ObterClinicaById(clin.IdClinica).Unidades.First();

                var novousuario = new Domain.Model.Usuario(model.Email, model.Email, model.Nome, grupo, clin, unidade);
                
                string senha = novousuario.CriarSenhaNovoAcesso();
                novousuario.SetSenha(senha);
                novousuario.SetTipo("Administrador");
                novousuario.SetPrimeiroAcesso("S");


                //insere dados do novo usuário
                _usuarioService.SalvarUsuario(novousuario);
                //dispara email com a senha 
                var template = _service.GetTemplateByIdentifier("SOLICITAR_ACESSO");
                if (template == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possivel recuperar o Templete de e-mail.");

                var obj = new JsonTemplateAcesso()
                {
                    SUBJECT = new JsonTemplateSubjectAcesso() { DATA_HORA = DateTime.Now.ToShortDateString() + " - " + DateTime.Now.ToShortTimeString() },
                    BODY = new JsonTemplateBodyAcesso()
                    {
                        NOME = model.Nome,
                        LOGIN = model.Email,
                        SENHA = senha
                    }
                };
                var json = new JavaScriptSerializer().Serialize(obj);
                _service.AddToQueue(new MailQueue(template, model.Email, json));

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}