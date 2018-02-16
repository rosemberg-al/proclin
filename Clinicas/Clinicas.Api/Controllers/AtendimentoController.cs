using AutoMapper;
using Clinicas.Application.Services.Interfaces;
using Clinicas.Domain.Model;
using Clinicas.Domain.ViewModel;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
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
    [RoutePrefix("atendimento")]
    public class AtendimentoController : ApiController
    {
        private readonly IPacienteService _service;

        public AtendimentoController(IPacienteService service)
        {
            _service = service;
        }

        //#region [Anamnese]
        //[HttpGet]
        //[Route("getAnamneseById")]
        //public HttpResponseMessage ObterAnamnesePorId(int id)
        //{
        //    try
        //    {
        //        var model = new AnamneseViewModel();

        //        var anamnese = _service.ObterAnamnesePorId(id);

        //        if (anamnese != null)
        //        {
        //            Mapper.CreateMap<Anamnese, AnamneseViewModel>();
        //            model = Mapper.Map<Anamnese, AnamneseViewModel>(anamnese);

        //        }
        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpGet]
        //[Route("getAnamneseByPaciente")]
        //public HttpResponseMessage ObterAnamnesePorPaciente(int id)
        //{
        //    try
        //    {
        //        var model = new List<AnamneseViewModel>();

        //        var anamnese = _service.ListarAnamnesesPorPaciente(id);

        //        if (anamnese.Count > 0)
        //        {
        //            Mapper.CreateMap<Anamnese, AnamneseViewModel>();
        //            model = Mapper.Map<List<Anamnese>, List<AnamneseViewModel>>(anamnese);

        //        }
        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpPost]
        //[Route("saveAnamnese")]
        //public HttpResponseMessage SalvarAnamnese(AnamneseViewModel model)
        //{
        //    try
        //    {
        //        if (model.IdAnamnese > 0)
        //        {
        //            var anamnese = _service.ObterAnamnesePorId(model.IdAnamnese);
        //            if (anamnese == null)
        //                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados da anamnese.");

        //            anamnese.SetDiagnostico(model.Diagnostico);
        //            anamnese.SetHma(model.Hma);
        //            string situacao = model.Situacao == "A" ? "Ativo" : "Inativo";
        //            anamnese.SetSituacao(situacao);
        //            anamnese.SetCondutaMedica(model.CondutaMedica);

        //            _service.SalvarAnamnese(anamnese);
        //        }
        //        else
        //        {
        //            var ppaciente = _service.ObterPacientePorId(model.IdPaciente);
        //            if (ppaciente == null)
        //                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do paciente.");

        //            var profissional = _service(model.IdPaciente);
        //            if (ppaciente == null)
        //                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do profissional de saúde");

        //            var anamnese = new Anamnese(model.Hma, model.Diagnostico, model.CondutaMedica, ppaciente);
        //            _service.SalvarAnamnese(anamnese);
        //        }

        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}
        //#endregion

        //#region [Vacinas]
        //[HttpGet]
        //[Route("getVacinasAtivas")]
        //public HttpResponseMessage ObterVacinasAtivas()
        //{
        //    try
        //    {
        //        var model = new List<VacinaViewModel>();

        //        var vacinas = _service.ObterVacinasAtivas();

        //        if (vacinas != null)
        //        {
        //            Mapper.CreateMap<Vacina, VacinaViewModel>();
        //            model = Mapper.Map<List<Vacina>, List<VacinaViewModel>>(vacinas);

        //        }
        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpPost]
        //[Route("saveRegistroVacina")]
        //public HttpResponseMessage SalvarRegistroVacina(RegistroVacinaViewModel model)
        //{
        //    try
        //    {
        //        if (model.IdRegistroVacina > 0)
        //        {
        //            var vacina = _service.ObterVacinaPorId(model.IdVacina);
        //            if (vacina == null)
        //                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados da vacina.");

        //            var registro = _service.ObterRegistroVacinaPorId(model.IdRegistroVacina);
        //            if (registro == null)
        //                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do registro.");

        //            registro.SetData(model.Data);
        //            registro.SetDose(model.Dose);
        //            registro.SetHora(model.Hora);
        //            registro.SetLote(model.Lote);

        //            _service.SalvarRegistroVacina(registro);
        //        }
        //        else
        //        {
        //            var ppaciente = _service.ObterPacientePorId(model.IdPaciente);
        //            if (ppaciente == null)
        //                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do paciente.");

        //            var vacina = _service.ObterVacinaPorId(model.IdVacina);
        //            if (vacina == null)
        //                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados da vacina.");

        //            var registro = new RegistroVacina(ppaciente, vacina, model.Data, model.Hora, model.Dose, model.Lote);
        //            _service.SalvarRegistroVacina(registro);
        //        }

        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpGet]
        //[Route("getRegistroVacinaById")]
        //public HttpResponseMessage ObterRegistroVacinaPorId(int id)
        //{
        //    try
        //    {
        //        var model = new RegistroVacinaViewModel();

        //        var anamnese = _service.ObterRegistroVacinaPorId(id);

        //        if (anamnese != null)
        //        {
        //            Mapper.CreateMap<RegistroVacina, RegistroVacinaViewModel>();
        //            model = Mapper.Map<RegistroVacina, RegistroVacinaViewModel>(anamnese);

        //            model.HoraVacina = model.Hora.ToShortTimeString();

        //        }
        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpGet]
        //[Route("getVacinasByPaciente")]
        //public HttpResponseMessage ListarRegistroVacinaPorPaciente(int id)
        //{
        //    try
        //    {
        //        var model = new List<RegistroVacinaViewModel>();

        //        var registros = _service.ListarRegistroVacinaPorPaciente(id);

        //        if (registros != null)
        //        {
        //            Mapper.CreateMap<RegistroVacina, RegistroVacinaViewModel>();
        //            Mapper.CreateMap<Vacina, VacinaViewModel>();

        //            model = Mapper.Map<List<RegistroVacina>, List<RegistroVacinaViewModel>>(registros);

        //        }
        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //#endregion
        
        [HttpGet]
        [Route("pdf")]
        public HttpResponseMessage GeraPdf()
        {
            //HtmlForm form = new HtmlForm();
            Document document = new Document(PageSize.A4, 20, 20, 20, 20);
            MemoryStream ms = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            var obj = new HTMLWorker(document);

            string html = " <html> " +
              " <body> " +
              " 	<span style=color:#f00; font-size:10pt>Nosso HTML básico.</span> <br/><br/>" +
              "  <p style='color: red; font - size:18px;'>teste teste teste <b>teste</b>  teste</p><br/><br/>" +
              "     <table border=1 bordercolor=#222 cellpadding=2 cellspacing=0> " +
              "       <tr> " +
              "       	<td># 1</td> " +
              "         <td># 2</td> " +
              "       </tr> " +
              "       <tr> " +
              "       	<td>Coluna n° 1</td> " +
              "         <td><strong>Coluna n° 2</strong></td> " +
              "       </tr> " +
              "     </table> " +
              " </body> " +
              " </html> ";
            StringReader se = new StringReader(html);
            document.Open();
            obj.Parse(se);
            document.Close();
            byte[] bytes = ms.ToArray();

            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(bytes) };

            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline")
            {
                FileName = "CIP_" +
                           DateTime.Now.ToString("G")
                               .Replace(":", "")
                               .Replace("-", "")
                               .Replace("/", "")
                               .Replace(" ", "") + ".pdf"
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            return response;

        }
    }
}