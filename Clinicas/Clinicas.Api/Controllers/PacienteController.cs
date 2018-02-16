using AutoMapper;
using Clinicas.Application.Services.Interfaces;
using Clinicas.Domain.Model;
using Clinicas.Domain.ViewModel;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace Clinicas.Api.Controllers
{
    [RoutePrefix("paciente")]
    [Authorize]
    public class PacienteController : ApiController
    {
        private readonly IPacienteService _service;
        private readonly IProntuarioService _serviceProntuario;
        private readonly ICadastroService _servicecadastro;

        public PacienteController(IPacienteService service, IProntuarioService serviceProntuario, ICadastroService servicecadastro)
        {
            _service = service;
            _serviceProntuario = serviceProntuario;
            _servicecadastro = servicecadastro;
        }

       

        public class f
        {
            public string foto { get; set; }
        }

        [HttpPost]
        [Route("alterarfoto")]
        public HttpResponseMessage AlterarFotoPaciente(AlterarFotoPacienteViewModel model)
        {
            try
            {
                var paciente = _service.ObterPacientePorId(model.IdPaciente);

                if(paciente == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do paciente!");

                paciente.SetFoto(model.Foto);

                _service.SalvarPaciente(paciente);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("uploadFoto")]
        public HttpResponseMessage ObterPacientePorId(f obj)
        {
            try
            {
                var s = obj.foto.IndexOf(',');
                var fa = obj.foto.Substring(s+1);
                var file = Convert.FromBase64String(fa);
                File.WriteAllBytes(@"C:\Temp\a.jpg", file);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getPacientesPorNome")]
        public HttpResponseMessage ListarPacientesPorNome(string nome)
        {
            try
            {
                var pacientes = _service.ListarPacientesPorNome(nome);

                var model = new List<PacienteViewModel>();

                foreach (var item in pacientes)
                {
                    
                    model.Add(new PacienteViewModel()
                    {
                        Mae = item.Pessoa.Mae,
                        Telefone1 = item.Pessoa.Telefone1,
                        CartaoSus = item.CartaoSus,
                        DataNascimento = item.Pessoa.DataNascimento,
                        Email = item.Pessoa.Email,
                        Nome = item.Pessoa.Nome,
                        IdPaciente = item.Pessoa.IdPessoa
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
        [Route("getConveniosByPaciente")]
        public HttpResponseMessage ListarCarteirasPaciente(int id)
        {
            try
            {
                var model = new List<CarteiraViewModel>();
                if (id > 0)
                {
                    var carteiras = _service.ListarCarteirasPaciente(id);
                    if (carteiras != null)
                    {
                        foreach(var item in carteiras)
                        {
                            model.Add(new CarteiraViewModel()
                            {
                                IdCarteira = item.IdCarteira,
                                Convenio = item.Convenio.Pessoa.Nome,
                                NumeroCarteira = item.NumeroCarteira,
                                IdConvenio = item.IdConvenio,
                                Plano = item.Plano,
                                RegistroAns = item.Convenio.RegistroAns,
                                ValidadeCarteira = item.ValidadeCarteira
                            });
                        }
                    }
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
        [Route("getEstados")]
        public HttpResponseMessage ListarEstados()
        {
            try
            {
                var estados = _service.ListarEstados();
                var model = new List<EstadosViewModel>();

                foreach (var item in estados)
                {
                    model.Add(new EstadosViewModel()
                    {
                        Id = item.IdEstado,
                        Nome = item.Nome,
                        Uf = item.Nome
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
        [Route("getCidadesByEstado")]
        public HttpResponseMessage ListarCidadesByEstado(int id)
        {
            try
            {
                var estados = _service.ListarCidadesByEstado(id);
                var model = new List<CidadesViewModel>();

                foreach (var item in estados)
                {
                    model.Add(new CidadesViewModel()
                    {
                        Id = item.IdCidade,
                        Nome = item.Nome,
                        Uf = item.Estado.Nome,
                        IdEstado = item.IdEstado
                    });
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}