using Clinicas.Application.Services;
using Clinicas.Application.Services.Interfaces;
using Clinicas.Domain.ViewModel;
using Clinicas.Infrastructure.Context;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Clinicas.Api.Controllers
{
    [System.Web.Http.RoutePrefix("relatorio")]
    [Authorize]
    public class RelatorioController : BaseController
    {
        private readonly IProntuarioService _service;
        private readonly IPacienteService _servicePaciente;
        private readonly ICadastroService _serviceCadastro;
        private readonly IFuncionarioService _serviceFuncionario;
        private readonly IRelatorioService _serviceRelatorio;
        private readonly IUsuarioService _serviceUsuario;

        public RelatorioController(IProntuarioService service, IPacienteService servicePaciente, ICadastroService serviceCadastro, IUsuarioService serviceUsuario, IFuncionarioService serviceFuncionario, IRelatorioService serviceRelatorio):base(serviceUsuario)
        {
            _service = service;
            _servicePaciente = servicePaciente;
            _serviceCadastro = serviceCadastro;
            _serviceFuncionario = serviceFuncionario;
            _serviceRelatorio = serviceRelatorio;
            _serviceUsuario = serviceUsuario;
        }


        [HttpGet]
        [Route("printRelPlanoAgenda")]
        public HttpResponseMessage RelPlanoDeAgenda(DateTime datainicio, DateTime datatermino, int? idprofissional, int? idUnidade)
        {
            try
            {
                if (datainicio > datatermino)
                {
                    throw new Exception("Periodo inválido");
                }

                var lista = _serviceRelatorio.RelPlanoDeAgenda(datainicio, datatermino, idprofissional, idUnidade, base.GetUsuarioLogado().IdClinica);
                string profissional = "";
                string unidade = "";

                if (idprofissional > 0)
                    profissional = _serviceCadastro.ObterFuncionarioById((int)idprofissional).Pessoa.Nome;
                else
                    profissional = "Todos";

                if (idUnidade > 0)
                    unidade = _serviceCadastro.ObterUnidadeAtendimentoPorId((int)idUnidade).Nome;
                else
                    unidade = "Todas";

                //vinculando dataset ao objeto relat
                var ds = new ReportDataSource();
                ds.Name = "dsdados"; ds.Value = lista.OrderBy(x => x.Data);

                var report = new LocalReport()
                {
                    ReportPath = AppDomain.CurrentDomain.BaseDirectory + "Report/RelatorioPlanoAgenda.rdlc"
                };
                report.DataSources.Add(ds);
                report.Refresh();

                ReportParameter[] parametros = new ReportParameter[] {
                    new ReportParameter("usuario",base.GetUsuarioLogado().Nome),
                    new ReportParameter("profissional",profissional),
                    new ReportParameter("unidade",unidade),
                    new ReportParameter("datainicio",datainicio.ToString("dd/MM/yyyy")),
                    new ReportParameter("datatermino",datatermino.ToString("dd/MM/yyyy"))
                 };

                report.SetParameters(parametros);

                //configurações da página ex: margin, top, left ...
                string deviceInfo =
                "<DeviceInfo>" +
                "<OutputFormat>PDF</OutputFormat>" +
                "<PageWidth>29.7cm</PageWidth>" +
                "<PageHeight>21cm</PageHeight>" +
                "<MarginTop>0.5cm</MarginTop>" +
                "<MarginLeft>0.5cm</MarginLeft>" +
                "<MarginRight>0.5cm</MarginRight>" +
                "<MarginBottom>0.5cm</MarginBottom>" +
                "</DeviceInfo>";

                string mimeType = "";
                string encoding = "";
                string filenameExtension = "";
                string[] streams = null;
                Microsoft.Reporting.WebForms.Warning[] warnings = null;
                byte[] bytes = report.Render("PDF", deviceInfo, out mimeType, out encoding, out filenameExtension, out streams, out warnings);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(bytes);
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);
                return result;

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Route("printRelFaturamento")]
        public HttpResponseMessage RelFaturamento(DateTime datainicio, DateTime datatermino, int? idprofissional, int? idpaciente, string situacao,string tipo)
        {
            try
            {
                if (datainicio > datatermino)
                {
                    throw new Exception("Periodo inválido");
                }

                var lista = _serviceRelatorio.RelFaturamento(datainicio, datatermino, idprofissional, idpaciente, situacao,tipo, base.GetUsuarioLogado().IdClinica);
                string profissional = "";
                if (idprofissional > 0)
                    profissional = _serviceCadastro.ObterFuncionarioById((int)idprofissional).Pessoa.Nome;
                else
                    profissional = "Todos";

                //vinculando dataset ao objeto relat
                var ds = new ReportDataSource();
                ds.Name = "dsdados"; ds.Value = lista.OrderBy(x => x.Data);

                var report = new LocalReport()
                {
                    ReportPath = AppDomain.CurrentDomain.BaseDirectory + "Report/RelatorioFaturamento.rdlc"
                };
                report.DataSources.Add(ds);
                report.Refresh();

                if (tipo == "C")
                {
                    tipo = "Convênio";
                }
                else
                {
                    tipo = "Particular";
                }

                ReportParameter[] parametros = new ReportParameter[] {
                    new ReportParameter("usuario",base.GetUsuarioLogado().Nome),
                    new ReportParameter("tipo",tipo),
                    new ReportParameter("profissional",profissional),
                    new ReportParameter("datainicio",datainicio.ToString("dd/MM/yyyy")),
                    new ReportParameter("datatermino",datatermino.ToString("dd/MM/yyyy"))
                 };

                report.SetParameters(parametros);

                //configurações da página ex: margin, top, left ...
                string deviceInfo =
                "<DeviceInfo>" +
                "<OutputFormat>PDF</OutputFormat>" +
                "<PageWidth>29.7cm</PageWidth>" +
                "<PageHeight>21cm</PageHeight>" +
                "<MarginTop>0.5cm</MarginTop>" +
                "<MarginLeft>0.5cm</MarginLeft>" +
                "<MarginRight>0.5cm</MarginRight>" +
                "<MarginBottom>0.5cm</MarginBottom>" +
                "</DeviceInfo>";

                string mimeType = "";
                string encoding = "";
                string filenameExtension = "";
                string[] streams = null;
                Microsoft.Reporting.WebForms.Warning[] warnings = null;
                byte[] bytes = report.Render("PDF", deviceInfo, out mimeType, out encoding, out filenameExtension, out streams, out warnings);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(bytes);
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);
                return result;

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [HttpGet]
        [Route("printRelAgendaMedica")]
        public HttpResponseMessage RelAgendaMedica(DateTime datainicio, DateTime datatermino, int? idprofissional, int? idpaciente,string situacao)
        {
            try
            {
                if (datainicio > datatermino)
                {
                    throw new Exception("Periodo inválido");
                }

                var lista = _serviceRelatorio.RelAgendaMedica(datainicio, datatermino, idprofissional, idpaciente, situacao, base.GetUsuarioLogado().IdClinica);
                string profissional = "";
                if (idprofissional > 0)
                    profissional = _serviceCadastro.ObterFuncionarioById((int)idprofissional).Pessoa.Nome;
                else
                    profissional = "Todos";

                //vinculando dataset ao objeto relat
                var ds = new ReportDataSource();
                ds.Name = "dsdados"; ds.Value = lista.OrderBy(x => x.Data);

                var report = new LocalReport()
                {
                    ReportPath = AppDomain.CurrentDomain.BaseDirectory + "Report/RelatorioAgenda.rdlc"
                };
                report.DataSources.Add(ds);
                report.Refresh();

                ReportParameter[] parametros = new ReportParameter[] {
                    new ReportParameter("usuario",base.GetUsuarioLogado().Nome),
                    new ReportParameter("profissional",profissional),
                    new ReportParameter("datainicio",datainicio.ToString("dd/MM/yyyy")),
                    new ReportParameter("datatermino",datatermino.ToString("dd/MM/yyyy"))
                 };
                
                report.SetParameters(parametros);

                //configurações da página ex: margin, top, left ...
                string deviceInfo =
                "<DeviceInfo>" +
                "<OutputFormat>PDF</OutputFormat>" +
                "<PageWidth>29.7cm</PageWidth>" +
                "<PageHeight>21cm</PageHeight>" +
                "<MarginTop>0.5cm</MarginTop>" +
                "<MarginLeft>0.5cm</MarginLeft>" +
                "<MarginRight>0.5cm</MarginRight>" +
                "<MarginBottom>0.5cm</MarginBottom>" +
                "</DeviceInfo>";

                string mimeType = "";
                string encoding = "";
                string filenameExtension = "";
                string[] streams = null;
                Microsoft.Reporting.WebForms.Warning[] warnings = null;
                byte[] bytes = report.Render("PDF", deviceInfo, out mimeType, out encoding, out filenameExtension, out streams, out warnings);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(bytes);
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);
                return result;

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("printRelAniversariantes")]
        public HttpResponseMessage RelAniversariantes(string mes)
        {
            try
            {
                var lista = _serviceRelatorio.RelAniversariantes(mes,base.GetUsuarioLogado().IdClinica);

                //vinculando dataset ao objeto relat
                var ds = new ReportDataSource();
                ds.Name = "dsdados"; ds.Value = lista.OrderBy(x => x.Nome);

                var report = new LocalReport();
                report.ReportPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Report/RelatorioAniversariantes.rdlc");
                report.DataSources.Add(ds);
                report.Refresh();

                // parametros
                ReportParameter[] parametros = new ReportParameter[] {
                new ReportParameter("usuario",base.GetUsuarioLogado().Nome)
                };

                report.SetParameters(parametros);

                //configurações da página ex: margin, top, left ...
                string deviceInfo =
                "<DeviceInfo>" +
                "<OutputFormat>PDF</OutputFormat>" +
                "<PageWidth>20cm</PageWidth>" +
                "<PageHeight>29.7cm</PageHeight>" +
                "<MarginTop>0.5cm</MarginTop>" +
                "<MarginLeft>0.5cm</MarginLeft>" +
                "<MarginRight>0.5cm</MarginRight>" +
                "<MarginBottom>0.5cm</MarginBottom>" +
                "</DeviceInfo>";

                string mimeType = "";
                string encoding = "";
                string filenameExtension = "";
                string[] streams = null;
                Microsoft.Reporting.WebForms.Warning[] warnings = null;
                byte[] bytes = report.Render("PDF", deviceInfo, out mimeType, out encoding, out filenameExtension, out streams, out warnings);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(bytes);
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);
                return result;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }
                
        [HttpGet]
        [Route("printRelOcupacoes")]
        public HttpResponseMessage RelOcupacoes()
        {
            try
            {
                var lista = _serviceRelatorio.RelOcupacao();

                //vinculando dataset ao objeto relat
                var ds = new ReportDataSource();
                ds.Name = "dsdados"; ds.Value = lista;

                var report = new LocalReport();
                report.ReportPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Report/RelatorioOcupacoes.rdlc");
                report.DataSources.Add(ds);
                report.Refresh();

                // parametros
                ReportParameter[] parametros = new ReportParameter[] {
                new ReportParameter("usuario",base.GetUsuarioLogado().Nome)
                };

                report.SetParameters(parametros);

                //configurações da página ex: margin, top, left ...
                string deviceInfo =
                "<DeviceInfo>" +
                "<OutputFormat>PDF</OutputFormat>" +
                "<PageWidth>20cm</PageWidth>" +
                "<PageHeight>29.7cm</PageHeight>" +
                "<MarginTop>0.5cm</MarginTop>" +
                "<MarginLeft>0.5cm</MarginLeft>" +
                "<MarginRight>0.5cm</MarginRight>" +
                "<MarginBottom>0.5cm</MarginBottom>" +
                "</DeviceInfo>";

                string mimeType = "";
                string encoding = "";
                string filenameExtension = "";
                string[] streams = null;
                Microsoft.Reporting.WebForms.Warning[] warnings = null;
                byte[] bytes = report.Render("PDF", deviceInfo, out mimeType, out encoding, out filenameExtension, out streams, out warnings);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(bytes);
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);
                return result;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        
        [HttpGet]
        [Route("printRelEspecialidades")]
        public HttpResponseMessage RelEspecialidades()
        {
            try
            {
                var lista = _serviceRelatorio.RelEspecialidade();

                //vinculando dataset ao objeto relat
                var ds = new ReportDataSource();
                ds.Name = "dsdados"; ds.Value = lista;

                var report = new LocalReport();
                report.ReportPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Report/RelatorioEspecialidades.rdlc");
                report.DataSources.Add(ds);
                report.Refresh();

                // parametros
                ReportParameter[] parametros = new ReportParameter[] {
                new ReportParameter("usuario",base.GetUsuarioLogado().Nome)
                };

                report.SetParameters(parametros);

                //configurações da página ex: margin, top, left ...
                string deviceInfo =
                "<DeviceInfo>" +
                "<OutputFormat>PDF</OutputFormat>" +
                "<PageWidth>20cm</PageWidth>" +
                "<PageHeight>29.7cm</PageHeight>" +
                "<MarginTop>0.5cm</MarginTop>" +
                "<MarginLeft>0.5cm</MarginLeft>" +
                "<MarginRight>0.5cm</MarginRight>" +
                "<MarginBottom>0.5cm</MarginBottom>" +
                "</DeviceInfo>";

                string mimeType = "";
                string encoding = "";
                string filenameExtension = "";
                string[] streams = null;
                Microsoft.Reporting.WebForms.Warning[] warnings = null;
                byte[] bytes = report.Render("PDF", deviceInfo, out mimeType, out encoding, out filenameExtension, out streams, out warnings);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(bytes);
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);
                return result;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("printRelProcedimentos")]
        public HttpResponseMessage RelProcedimentos()
        {
            try
            {
                var lista = _serviceRelatorio.RelProcedimentos();

                //vinculando dataset ao objeto relat
                var ds = new ReportDataSource();
                ds.Name = "dsdados"; ds.Value = lista;

                var report = new LocalReport();
                report.ReportPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Report/RelatorioProcedimentos.rdlc");
                report.DataSources.Add(ds);
                report.Refresh();

                // parametros
                ReportParameter[] parametros = new ReportParameter[] {
                new ReportParameter("usuario",base.GetUsuarioLogado().Nome)
                };

                report.SetParameters(parametros);

                //configurações da página ex: margin, top, left ...
                string deviceInfo =
                "<DeviceInfo>" +
                "<OutputFormat>PDF</OutputFormat>" +
                "<PageWidth>20cm</PageWidth>" +
                "<PageHeight>29.7cm</PageHeight>" +
                "<MarginTop>0.5cm</MarginTop>" +
                "<MarginLeft>0.5cm</MarginLeft>" +
                "<MarginRight>0.5cm</MarginRight>" +
                "<MarginBottom>0.5cm</MarginBottom>" +
                "</DeviceInfo>";

                string mimeType = "";
                string encoding = "";
                string filenameExtension = "";
                string[] streams = null;
                Microsoft.Reporting.WebForms.Warning[] warnings = null;
                byte[] bytes = report.Render("PDF", deviceInfo, out mimeType, out encoding, out filenameExtension, out streams, out warnings);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(bytes);
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);
                return result;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("printRelConvenios")]
        public HttpResponseMessage RelConvenios()
        {
            try
            {
                var lista = _serviceRelatorio.RelConvenio(base.GetUsuarioLogado().IdClinica);

                //vinculando dataset ao objeto relat
                var ds = new ReportDataSource();
                ds.Name = "dsdados"; ds.Value = lista;

                var report = new LocalReport();
                report.ReportPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Report/RelatorioConvenios.rdlc");
                report.DataSources.Add(ds);
                report.Refresh();

                // parametros
                ReportParameter[] parametros = new ReportParameter[] {
                new ReportParameter("usuario",base.GetUsuarioLogado().Nome)
                };

                report.SetParameters(parametros);

                //configurações da página ex: margin, top, left ...
                string deviceInfo =
                "<DeviceInfo>" +
                "<OutputFormat>PDF</OutputFormat>" +
                "<PageWidth>20cm</PageWidth>" +
                "<PageHeight>29.7cm</PageHeight>" +
                "<MarginTop>0.5cm</MarginTop>" +
                "<MarginLeft>0.5cm</MarginLeft>" +
                "<MarginRight>0.5cm</MarginRight>" +
                "<MarginBottom>0.5cm</MarginBottom>" +
                "</DeviceInfo>";

                string mimeType = "";
                string encoding = "";
                string filenameExtension = "";
                string[] streams = null;
                Microsoft.Reporting.WebForms.Warning[] warnings = null;
                byte[] bytes = report.Render("PDF", deviceInfo, out mimeType, out encoding, out filenameExtension, out streams, out warnings);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(bytes);
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);
                return result;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("printRelPacientes")]
        public HttpResponseMessage RelPacientes()
        {
            try
            {
                var lista = _serviceRelatorio.RelPacientes(base.GetUsuarioLogado().IdClinica);

                //vinculando dataset ao objeto relat
                var ds = new ReportDataSource();
                ds.Name = "dsdados"; ds.Value = lista;

                var report = new LocalReport()
                {
                    ReportPath = AppDomain.CurrentDomain.BaseDirectory + "Report/RelatorioPacientes.rdlc"
                };
                report.DataSources.Add(ds);
                report.Refresh();

                ReportParameter[] parametros = new ReportParameter[] {
                    new ReportParameter("usuario",base.GetUsuarioLogado().Nome)
                 };

                report.SetParameters(parametros);

                //configurações da página ex: margin, top, left ...
                string deviceInfo =
                "<DeviceInfo>" +
                "<OutputFormat>PDF</OutputFormat>" +
                "<PageWidth>29.7cm</PageWidth>" +
                "<PageHeight>21cm</PageHeight>" +
                "<MarginTop>0.5cm</MarginTop>" +
                "<MarginLeft>0.5cm</MarginLeft>" +
                "<MarginRight>0.5cm</MarginRight>" +
                "<MarginBottom>0.5cm</MarginBottom>" +
                "</DeviceInfo>";

                string mimeType = "";
                string encoding = "";
                string filenameExtension = "";
                string[] streams = null;
                Microsoft.Reporting.WebForms.Warning[] warnings = null;
                byte[] bytes = report.Render("PDF", deviceInfo, out mimeType, out encoding, out filenameExtension, out streams, out warnings);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(bytes);
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);
                return result;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("printRelFornecedores")]
        public HttpResponseMessage RelFornecedores()
        {
            try
            {
                var lista = _serviceRelatorio.RelFornecedor(base.GetUsuarioLogado().IdClinica);

                //vinculando dataset ao objeto relat
                var ds = new ReportDataSource();
                ds.Name = "dsdados"; ds.Value = lista;

                var report = new LocalReport()
                {
                    ReportPath = AppDomain.CurrentDomain.BaseDirectory + "Report/RelatorioFornecedores.rdlc"
                };
                report.DataSources.Add(ds);
                report.Refresh();

                ReportParameter[] parametros = new ReportParameter[] {
                    new ReportParameter("usuario",base.GetUsuarioLogado().Nome)
                 };

                report.SetParameters(parametros);

                //configurações da página ex: margin, top, left ...
                string deviceInfo =
                "<DeviceInfo>" +
                "<OutputFormat>PDF</OutputFormat>" +
                "<PageWidth>29.7cm</PageWidth>" +
                "<PageHeight>21cm</PageHeight>" +
                "<MarginTop>0.5cm</MarginTop>" +
                "<MarginLeft>0.5cm</MarginLeft>" +
                "<MarginRight>0.5cm</MarginRight>" +
                "<MarginBottom>0.5cm</MarginBottom>" +
                "</DeviceInfo>";

                string mimeType = "";
                string encoding = "";
                string filenameExtension = "";
                string[] streams = null;
                Microsoft.Reporting.WebForms.Warning[] warnings = null;
                byte[] bytes = report.Render("PDF", deviceInfo, out mimeType, out encoding, out filenameExtension, out streams, out warnings);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(bytes);
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);
                return result;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        
        [HttpGet]
        [Route("printRelCheques")]
        public HttpResponseMessage RelCheques(DateTime datainicio, DateTime datatermino, string situacao)
        {
            try
            {
                if (datainicio > datatermino)
                {
                    throw new Exception("Periodo inválido");
                }

                var lista = _serviceRelatorio.RelCheque(datainicio, datatermino, situacao, base.GetUsuarioLogado().IdClinica);

                //vinculando dataset ao objeto relat
                var ds = new ReportDataSource();
                ds.Name = "dsdados"; ds.Value = lista;

                var report = new LocalReport()
                {
                    ReportPath = AppDomain.CurrentDomain.BaseDirectory + "Report/RelatorioCheques.rdlc"
                };
                report.DataSources.Add(ds);
                report.Refresh();

                ReportParameter[] parametros = new ReportParameter[] {
                    new ReportParameter("usuario",base.GetUsuarioLogado().Nome),
                    new ReportParameter("datainicio",datainicio.ToString("dd/MM/yyyy")),
                    new ReportParameter("datatermino",datatermino.ToString("dd/MM/yyyy"))
                };

                report.SetParameters(parametros);

                //configurações da página ex: margin, top, left ...
                string deviceInfo =
                "<DeviceInfo>" +
                "<OutputFormat>PDF</OutputFormat>" +
                "<PageWidth>29.7cm</PageWidth>" +
                "<PageHeight>21cm</PageHeight>" +
                "<MarginTop>0.5cm</MarginTop>" +
                "<MarginLeft>0.5cm</MarginLeft>" +
                "<MarginRight>0.5cm</MarginRight>" +
                "<MarginBottom>0.5cm</MarginBottom>" +
                "</DeviceInfo>";

                string mimeType = "";
                string encoding = "";
                string filenameExtension = "";
                string[] streams = null;
                Microsoft.Reporting.WebForms.Warning[] warnings = null;
                byte[] bytes = report.Render("PDF", deviceInfo, out mimeType, out encoding, out filenameExtension, out streams, out warnings);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(bytes);
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);
                return result;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        
        [HttpGet]
        [Route("printRelFinanceiro")]
        public HttpResponseMessage RelFinanceiro(DateTime datainicio, DateTime datatermino, string tipo, string situacao, int idpessoa)
        {
            try
            {
                if (datainicio > datatermino)
                {
                    throw new Exception("Periodo inválido");
                }

                var lista = _serviceRelatorio.RelFinanceiro(datainicio, datatermino, tipo, situacao, idpessoa, base.GetUsuarioLogado().IdClinica);

                //vinculando dataset ao objeto relat
                var ds = new ReportDataSource();
                ds.Name = "dsdados"; ds.Value = lista;

                var report = new LocalReport()
                {
                    ReportPath = AppDomain.CurrentDomain.BaseDirectory + "Report/RelatorioFinanceiro.rdlc"
                };
                report.DataSources.Add(ds);
                report.Refresh();

                ReportParameter[] parametros = new ReportParameter[] {
                    new ReportParameter("usuario",base.GetUsuarioLogado().Nome),
                    new ReportParameter("tipo",tipo),
                    new ReportParameter("datainicio",datainicio.ToString("dd/MM/yyyy")),
                    new ReportParameter("datatermino",datatermino.ToString("dd/MM/yyyy"))
                };

                report.SetParameters(parametros);

                //configurações da página ex: margin, top, left ...
                string deviceInfo =
                "<DeviceInfo>" +
                "<OutputFormat>PDF</OutputFormat>" +
                "<PageWidth>29.7cm</PageWidth>" +
                "<PageHeight>21cm</PageHeight>" +
                "<MarginTop>0.5cm</MarginTop>" +
                "<MarginLeft>0.5cm</MarginLeft>" +
                "<MarginRight>0.5cm</MarginRight>" +
                "<MarginBottom>0.5cm</MarginBottom>" +
                "</DeviceInfo>";

                string mimeType = "";
                string encoding = "";
                string filenameExtension = "";
                string[] streams = null;
                Microsoft.Reporting.WebForms.Warning[] warnings = null;
                byte[] bytes = report.Render("PDF", deviceInfo, out mimeType, out encoding, out filenameExtension, out streams, out warnings);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(bytes);
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);
                return result;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("printRelQtdeProcedimentosRealizados")]
        public HttpResponseMessage RelQtdeProcedimentosRealizados(DateTime datainicio, DateTime datatermino)
        {
            try
            {
                if (datainicio > datatermino)
                {
                    throw new Exception("Periodo inválido");
                }

                var lista = _serviceRelatorio.ProcedimentosRealizados(datainicio, datatermino, base.GetUsuarioLogado().IdClinica);

                //vinculando dataset ao objeto relat
                var ds = new ReportDataSource();
                ds.Name = "dsdados"; ds.Value = lista;

                var report = new LocalReport()
                {
                    ReportPath = AppDomain.CurrentDomain.BaseDirectory + "Report/RelQtdeProcedimentosRealizados.rdlc"
                };
                report.DataSources.Add(ds);
                report.Refresh();

                ReportParameter[] parametros = new ReportParameter[] {
                    new ReportParameter("usuario",base.GetUsuarioLogado().Nome),
                    new ReportParameter("datainicio",datainicio.ToString("dd/MM/yyyy")),
                    new ReportParameter("datatermino",datatermino.ToString("dd/MM/yyyy"))
                };

                report.SetParameters(parametros);

                //configurações da página ex: margin, top, left ...
                string deviceInfo =
                "<DeviceInfo>" +
                "<OutputFormat>PDF</OutputFormat>" +
                "<PageWidth>20cm</PageWidth>" +
                "<PageHeight>29,7cm</PageHeight>" +
                "<MarginTop>0.5cm</MarginTop>" +
                "<MarginLeft>0.5cm</MarginLeft>" +
                "<MarginRight>0.5cm</MarginRight>" +
                "<MarginBottom>0.5cm</MarginBottom>" +
                "</DeviceInfo>";

                string mimeType = "";
                string encoding = "";
                string filenameExtension = "";
                string[] streams = null;
                Microsoft.Reporting.WebForms.Warning[] warnings = null;
                byte[] bytes = report.Render("PDF", deviceInfo, out mimeType, out encoding, out filenameExtension, out streams, out warnings);

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
                result.Content = new ByteArrayContent(bytes);
                result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(mimeType);
                return result;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}