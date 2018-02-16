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
using System.Net.Mail;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Clinicas.Api.Controllers
{
    [RoutePrefix("seguranca")]
    [Authorize]
    public class SegurancaController : BaseController
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ISegurancaService _segurancaService;
        private readonly IAgendaService _service;
        private readonly ICadastroService _cadastroService;

        public SegurancaController(IUsuarioService usuarioService, ICadastroService cadastroService, ISegurancaService segurancaService,  IAgendaService service) : base(usuarioService)
        {
            this._usuarioService = usuarioService;
            this._segurancaService = segurancaService;
            _service = service;
            _cadastroService = cadastroService;
        }

        #region [Funcionalidades]

        [HttpGet]
        [Route("recuperarsenha")]
        public HttpResponseMessage RecuperarSenha(string email)
        {
            try
            {
                var usuario = _usuarioService.ObterUsuarioLogin(email);

                if (usuario == null)
                {
                    throw new Exception(" Nenhum usuário encontrado ");
                }
                else
                {
                    // envia email recuperação de senha
                    string sUserName = "renato@genialsoft.com.br";
                    string sPassword = "r15008956";
                    string sBobdy = " Sua senha é "+usuario.Senha;

                    MailMessage objEmail = new MailMessage();
                    objEmail.To.Add(email);
                    objEmail.From = new MailAddress(sUserName.Trim());
                    objEmail.Subject = "Proclin - Recuperação de Senha";
                    objEmail.Body = sBobdy;

                    SmtpClient smtp = new SmtpClient("smtp.genialsoft.com.br", 587 /* TLS */);
                    smtp.EnableSsl = false;
                    smtp.Credentials = new System.Net.NetworkCredential(sUserName, sPassword, "");
                    //smtp.UseDefaultCredentials = false;
                    smtp.Send(objEmail);


                    return Request.CreateResponse(HttpStatusCode.OK, email);

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("obterUsuarioPorLogin")]
        public HttpResponseMessage ObterUsuarioPorLogin(string login)
        {
            try
            {
                var usuario = _usuarioService.ObterUsuarioLogin(login);
                var model = new UsuarioViewModel();
                model.Nome = usuario.Nome;
                model.NmGrupoUsuario = usuario.GrupoUsuario.Nome;
                model.NmClinica = usuario.Clinica.Nome;
                model.Tipo = usuario.Tipo;
                model.PrimeiroAcesso = usuario.PrimeiroAcesso == "S" ? true : false;
                model.IdGrupoUsuario = usuario.IdGrupoUsuario;
                model.Email = usuario.Email;
                model.IdUsuario = usuario.IdUsuario;
                model.Login = usuario.Login;
                model.Situacao = usuario.Situacao;
                model.IdClinica = usuario.IdClinica;
                model.IdFuncionario = usuario.IdFuncionario ?? 0;
                model.NmUnidadeAtendimento = usuario.UnidadeAtendimento.Nome;

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        
        [HttpGet]
        [Route("obterFuncionalidadesPorGrupoUsuario")]
        public HttpResponseMessage ObterFuncionalidadesPorGrupoUsuario(int idgrupoUsuario)
        {
            try
            {
                var model = _segurancaService.ObterFuncionalidadesPorGrupoUsuario(idgrupoUsuario);
                return Request.CreateResponse(HttpStatusCode.OK, model);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #endregion

        #region [Usuario]

        [HttpGet]
        [Route("listarUsuarios")]
        public HttpResponseMessage ListarUsuarios()
        {
            try
            {
                var consulta = _usuarioService.ListarUsuarios(base.GetUsuarioLogado().IdClinica);
                var model = new List<UsuarioViewModel>();
                foreach (var item in consulta)
                {
                    model.Add(new UsuarioViewModel()
                    {
                        IdClinica = item.IdClinica,
                        NmClinica = item.Clinica.Nome,
                        Nome = item.Nome,
                        IdUsuario = item.IdUsuario,
                        Login = item.Login,
                        Email = item.Email,
                        Situacao = item.Situacao,
                        IdGrupoUsuario = item.GrupoUsuario.IdGrupoUsuario,
                        NmGrupoUsuario = item.GrupoUsuario.Nome
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
        [Route("salvarUsuario")]
        public HttpResponseMessage SalvarUsuario(UsuarioViewModel model)
        {
            try
            {
                if (model.IdUsuario > 0)
                {
                    var usuario = _usuarioService.ObterUsuarioPorId(model.IdUsuario);
                    if (usuario == null)
                    {
                        throw new Exception("Não foi possivel recuperar os dados do usuário ");
                    }
                    else
                    {
                        usuario.SetNome(model.Nome);
                        usuario.SetGrupoUsuario(_segurancaService.ObterGrupoUsuarioPorId(model.IdGrupoUsuario));
                        usuario.SetUnidadeAtendimento(_cadastroService.ObterUnidadeAtendimentoPorId(model.IdUnidadeAtendimento));
                        _usuarioService.SalvarUsuario(usuario);
                    }
                }
                else
                {
                    var consulta = _usuarioService.ObterUsuarioLogin(model.Login);
                    if (consulta != null)
                    {
                        throw new Exception("O Login informado já possui um usuário ");
                    }
                    var usuario = new Usuario(model.Login,model.Email,model.Nome,_segurancaService.ObterGrupoUsuarioPorId(model.IdGrupoUsuario),base.GetUsuarioLogado().Clinica, _cadastroService.ObterUnidadeAtendimentoPorId(model.IdUnidadeAtendimento));
                    _usuarioService.SalvarUsuario(usuario);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
                
        [HttpGet]
        [Route("alterarSenha")]
        public HttpResponseMessage AlterarSenha(string novasenha,string confirmar)
        {
            try
            {
                if (novasenha.Length < 6)
                {
                    throw new Exception("Sua senha deve conter no mínimo 6 caracteres ");
                }
                if (novasenha != confirmar)
                {
                    throw new Exception("Senhas não conferem ");
                }


                var usuario = _usuarioService.ObterUsuarioLogin(base.GetUsuarioLogado().Login);
                if (usuario == null)
                {
                    throw new Exception("Não foi possivel recuperar os dados do usuário ");
                }
                usuario.AlterarSenha(novasenha);
                _usuarioService.SalvarUsuario(usuario);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("desativarUsuario")]
        public HttpResponseMessage DesativarUsuario(int idusuario)
        {
            try
            {
                var usuario = _usuarioService.ObterUsuarioPorId(idusuario);
                if (usuario == null)
                {
                    throw new Exception("Não foi possivel recuperar os dados do usuário ");
                }
                usuario.Desativar();
                _usuarioService.SalvarUsuario(usuario);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("excluirUsuario")]
        public HttpResponseMessage ExcluirUsuario(int idusuario)
        {
            try
            {
                var usuario = _usuarioService.ObterUsuarioPorId(idusuario);
                if (usuario == null)
                {
                    throw new Exception("Não foi possivel recuperar os dados do usuário ");
                }
                _usuarioService.ExcluirUsuario(usuario);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("obterUsuarioPorId")]
        public HttpResponseMessage ObterUsuarioPorId(int idusuario)
        {
            try
            {
                var usuario = _usuarioService.ObterUsuarioPorId(idusuario);
                if (usuario == null)
                {
                    throw new Exception("Não foi possivel recuperar os dados do usuário ");
                }
                var model = new UsuarioViewModel();
                model.Email = usuario.Email;
                model.IdUsuario = usuario.IdUsuario;
                model.Nome = usuario.Nome;
                model.Login = usuario.Login;
                model.Situacao = usuario.Situacao;
                model.NmGrupoUsuario = usuario.GrupoUsuario.Nome;
                model.IdGrupoUsuario = usuario.IdGrupoUsuario;
                model.IdClinica = usuario.IdClinica;
                model.NmClinica = usuario.Clinica.Nome;
                model.Tipo = usuario.Tipo;
                model.IdUnidadeAtendimento = usuario.IdUnidadeAtendimento;
                model.PrimeiroAcesso = usuario.PrimeiroAcesso == "S" ? true : false;

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("ativarUsuario")]
        public HttpResponseMessage AtivarUsuario(int idusuario)
        {
            try
            {
                var usuario = _usuarioService.ObterUsuarioPorId(idusuario);
                if (usuario == null)
                {
                    throw new Exception("Não foi possivel recuperar os dados do usuário ");
                }
                usuario.Ativar();
                _usuarioService.SalvarUsuario(usuario);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("obterGrupoUsuarioPorId")]
        public HttpResponseMessage ObterGrupoUsuarioId(int idgrupousuario)
        {
            try
            {
                var grupo = _segurancaService.ObterGrupoUsuarioPorId(idgrupousuario);
                if (grupo == null)
                {
                    throw new Exception("Não foi possivel recuperar os dados do grupo de usuário ");
                }
                var model = new GrupoUsuarioViewModel()
                {
                    IdGrupoUsuario = grupo.IdGrupoUsuario,
                    Nome = grupo.Nome
                };
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("listarGrupoUsuario")]
        public HttpResponseMessage ListarGrupoUsuario()
        {
            try
            {
                var grupo = _segurancaService.ListarGrupoUsuario();
                var model = new List<GrupoUsuarioViewModel>();
                foreach (var item in grupo)
                {
                    model.Add(new GrupoUsuarioViewModel()
                    {
                        IdGrupoUsuario = item.IdGrupoUsuario,
                        Nome = item.Nome
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
        [AllowAnonymous]
        [Route("validalogin")]
        public HttpResponseMessage ValidaLogin(string login)
        {
            bool model = false;
            try
            {
                var user = _usuarioService.ObterUsuarioLogin(login);
                if (user != null)
                    model = true;

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("resetarSenha")]
        public HttpResponseMessage ResetarSenha(string login)
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
                _service.ProcessQueue();
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