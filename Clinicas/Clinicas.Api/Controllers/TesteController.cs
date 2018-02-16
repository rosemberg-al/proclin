using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Clinicas.Api.Controllers
{
    [RoutePrefix("teste")]
    [Authorize]
    public class TesteController : ApiController
    {
        [HttpGet]
        [Authorize]
        [Route("get")]
        public string Get()
        {
            return HttpContext.Current.User.Identity.Name;

        }
    }
}