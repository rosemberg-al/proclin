using AutoMapper;
using Clinicas.Application.Services.Interfaces;
using Clinicas.Domain.Model;
using Clinicas.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Clinicas.Api.Controllers
{
    [RoutePrefix("medicamentos")]
    [Authorize]
    public class MedicamentoController : BaseController
    {
        private readonly IMedicamentoService _serviceMedicamento;
        private readonly IUsuarioService _usuarioService;

        public MedicamentoController(IMedicamentoService serviceMedicamento, IUsuarioService usuarioService) : base(usuarioService)
        {
            _serviceMedicamento = serviceMedicamento;
            _usuarioService = usuarioService;
        }

        [HttpGet]
        [Route("getMedicamentoById")]
        public HttpResponseMessage GetMedicamentoById(int id)
        {
            try
            {
                var model = new MedicamentoViewModel();
                var result = _serviceMedicamento.GetMedicamentoById(id);

                if (result != null)
                {
                    model.Indicacao = result.Indicacao;
                    model.IdMedicamento = result.IdMedicamento;
                    model.Nome = result.Nome;
                    model.Fabricante = result.Fabricante;
                    model.PrincipioAtivo = result.PrincipioAtivo;
                    model.RegistroMS = result.RegistroMS;
                    model.Posologia = result.Posologia;
                    model.ContraIndicacao = result.ContraIndicacao;
                    model.Preco = result.Preco;
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do medicamento.");
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("pesquisarMedicamentos")]
        public HttpResponseMessage PesquisarMedicamentos(string nome)
        {
            try
            {
                var model = new List<MedicamentoViewModel>();
                var result = _serviceMedicamento.PesqusiarMedicamentos(nome);

                if (result != null)
                {
                    foreach(var item in result)
                    {
                        model.Add(new MedicamentoViewModel()
                        {
                            Indicacao = item.Indicacao,
                            IdMedicamento = item.IdMedicamento,
                            Nome = item.Nome,
                            Fabricante = item.Fabricante,
                            PrincipioAtivo = item.PrincipioAtivo,
                            RegistroMS = item.RegistroMS,
                            ContraIndicacao = item.ContraIndicacao,
                            Posologia = item.Posologia,
                            Preco = item.Preco
                        });
                    }
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do medicamento.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("listarMedicamentos")]
        public HttpResponseMessage ListarMedicamentos()
        {
            try
            {
                var model = new List<MedicamentoViewModel>();
                var result = _serviceMedicamento.ListarMedicamentos();

                if (result != null)
                {
                   foreach(var item in result)
                        model.Add(new MedicamentoViewModel() {
                            Indicacao = item.Indicacao,
                            IdMedicamento = item.IdMedicamento,
                            Fabricante = item.Fabricante,
                            Nome = item.Nome,
                            PrincipioAtivo = item.PrincipioAtivo,
                            RegistroMS = item.RegistroMS,
                            Posologia = item.Posologia,
                            ContraIndicacao = item.ContraIndicacao,
                            Preco = item.Preco
                        });
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do medicamento.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        
    }
}