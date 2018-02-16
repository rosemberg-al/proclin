using Clinicas.Application.Services.Interfaces;
using Clinicas.Domain.Model;
using Clinicas.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Clinicas.Auth.Api.Controllers
{
    //[Authorize]
    [RoutePrefix("paciente")]
    public class PacienteController : ApiController
    {
        private readonly IPacienteService _service;

        public PacienteController(IPacienteService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("getById")]
        public PacienteViewModel ObterPacientePorId(int id)
        {
            try
            {
                var model = new PacienteViewModel();

                //var paciente = _service.ObterPacientePorId(id);

                //if (paciente != null)
                //{
                //    CreateMaps();
                //    model = Mapper.Map<paciente, PacienteViewModel>(paciente);
                //}
                return model;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void CreateMaps()
        {
            //Mapper.CreateMap<paciente, PacienteViewModel>();
            //Mapper.CreateMap<Anamnese, AnamneseViewModel>();
            //Mapper.CreateMap<Atestado, AtestadoViewModel>();
            //Mapper.CreateMap<Vacina, VacinaViewModel>();
            //Mapper.CreateMap<RequisicaoExames, RequisicaoExamesViewModel>();
            //Mapper.CreateMap<RegistroVacina, RegistroVacinaViewModel>();
        }

    }
}