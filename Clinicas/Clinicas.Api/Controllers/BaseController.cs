using Clinicas.Application.Services.Interfaces;
using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Clinicas.Api.Controllers
{
    public class BaseController : ApiController
    {
        public Usuario _usuario { get; set; }
        private readonly IUsuarioService _usuarioservice;
        public BaseController(IUsuarioService usuarioservice)
        {
            this._usuarioservice = usuarioservice;
            var usuario = User.Identity.Name;

            if (User.Identity.Name == null)
            {
                var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized) { ReasonPhrase = "Não Autorizado " };
                throw new HttpResponseException(msg);
            } 
        }
        public Usuario GetUsuarioLogado()
        {
            if (User.Identity.Name != null)
                this._usuario = _usuarioservice.ObterUsuarioLogin(User.Identity.Name);
                return _usuario;
        }
    }
}