using Clinicas.Application.Services.Interfaces;
using Clinicas.Domain.Model;
using Clinicas.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Clinicas.Api.Controllers
{
    [RoutePrefix("busca")]
    [Authorize]
    public class BuscaController : BaseController
    {
        private readonly IBuscaService _serviceBusca;
        private readonly IUsuarioService _usuarioService;

        public BuscaController(IBuscaService serviceBusca, IUsuarioService usuarioservice) : base(usuarioservice)
        {
            _serviceBusca = serviceBusca;
            _usuarioService = usuarioservice;
        }

        [HttpGet]
        [Route("funcionalidade")]
        public HttpResponseMessage Buscar(string search)
        {
            try
            {
                var consulta = _serviceBusca.Busca(search);
                return Request.CreateResponse(HttpStatusCode.OK,consulta);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


    }
}