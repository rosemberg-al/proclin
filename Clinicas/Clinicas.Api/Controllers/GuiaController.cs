using AutoMapper;
using Clinicas.Api.Extensions;
using Clinicas.Application.Services.Interfaces;
using Clinicas.Domain.Model;
using Clinicas.Domain.Tiss;
using Clinicas.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;

namespace Clinicas.Api.Controllers
{
    [RoutePrefix("guia")]
    [Authorize]
    public class GuiaController : BaseController
    {
        private readonly IGuiaService _service;
        private readonly IUsuarioService _usuarioService;
        private readonly ICadastroService _serviceCadastro;

        public GuiaController(IGuiaService service, IUsuarioService usuarioService, ICadastroService serviceCadastro) : base(usuarioService)
        {
            _service = service;
            _usuarioService = usuarioService;
            _serviceCadastro = serviceCadastro;
        }

        [HttpPost]
        [Route("getguiasbuscaavancada")]
        public HttpResponseMessage ListraGuiasConsultasBuscaAvancada(BuscaGuiaViewModel model)
        {
            try
            {
                var lista = new List<GuiaConsultaViewModel>();
                var result = _service.ListarGuiasBuscaAvancada(model);

                foreach (var item in result)
                {
                    lista.Add(new GuiaConsultaViewModel
                    {
                        DataEmissaoGuia = item.CabecalhoGuia.DataEmissaoGuia,
                        Nome = item.DadosBeneficiario.Nome,
                        NomeProfissionalExecutante = item.DadosContratado.NomeProfissionalExecutante,
                        NumeroCarteira = item.DadosBeneficiario.NumeroCarteira,
                        NumeroGuia = item.CabecalhoGuia.NumeroGuia,
                        IdGuia = item.IdGuia
                    });
                }

                return Request.CreateResponse(HttpStatusCode.OK, lista);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Route("pesquisar")]
        public HttpResponseMessage ListraGuiasConsultasBuscaAvancada(int? idguia, string nome)
        {
            try
            {
                var lista = new List<GuiaConsultaViewModel>();
                var result = _service.Pesqusiar(idguia, nome);

                foreach (var item in result)
                {
                    lista.Add(new GuiaConsultaViewModel
                    {
                        DataEmissaoGuia = item.CabecalhoGuia.DataEmissaoGuia,
                        Nome = item.DadosBeneficiario.Nome,
                        NomeProfissionalExecutante = item.DadosContratado.NomeProfissionalExecutante,
                        NumeroCarteira = item.DadosBeneficiario.NumeroCarteira,
                        NumeroGuia = item.CabecalhoGuia.NumeroGuia,
                        IdGuia = item.IdGuia
                    });
                }

                return Request.CreateResponse(HttpStatusCode.OK, lista);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getGuiaById")]
        public HttpResponseMessage ObterGuiaPorId(int id)
        {
            try
            {
                var guia = new GuiaConsultaViewModel();
                var result = _service.ObterGuiaPorId(id);

                if (result == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar a guia solicitada!");

                guia.CEP = result.DadosContratado.CEP;
                guia.CID10 = result.HipoteseDiagnostica.CID10;
                guia.CID102 = result.HipoteseDiagnostica.CID102;
                guia.CID103 = result.HipoteseDiagnostica.CID103;
                guia.CID104 = result.HipoteseDiagnostica.CID104;
                guia.CodigoNaOperadora = result.DadosContratado.CodigoNaOperadora;
                guia.CodigoCBOs = result.DadosContratado.CodigoCBOs;
                guia.CodigoCnes = result.DadosContratado.CodigoCnes;
                guia.CodigoIBGEMunicipio = result.DadosContratado.CodigoIBGEMunicipio;
                guia.CodigoPrestador = result.DadosContratado.CodigoNaOperadora;
                guia.CodigoProcedimento = result.DadosAtendimento.CodigoProcedimento;
                guia.CodigoTabela = result.DadosAtendimento.CodigoTabela;
                guia.Complemento = result.DadosContratado.Complemento;
                guia.ConselhoProfissional = result.DadosContratado.ConselhoProfissional;
                guia.DataAtendimento = result.DadosAtendimento.DataAtendimento;
                guia.DataEmissaoGuia = result.CabecalhoGuia.DataEmissaoGuia;
                guia.IdGuia = result.IdGuia;
                guia.IndicacaoAcidente = result.HipoteseDiagnostica.IndicacaoAcidente;
                guia.Logradouro = result.DadosContratado.Logradouro;
                guia.Municipio = result.DadosContratado.Municipio;
                guia.Nome = result.DadosBeneficiario.Nome;
                guia.NomeContratado = result.DadosContratado.NomeContratado;
                guia.NomeExecutor = result.DadosContratado.NomeProfissionalExecutante;
                guia.NomeProfissionalExecutante = result.DadosContratado.NomeProfissionalExecutante;
                guia.Numero = result.DadosContratado.Numero;
                guia.NumeroCartaoSus = result.DadosBeneficiario.NumeroCartaoSus;
                guia.NumeroCarteira = result.DadosBeneficiario.NumeroCarteira;
                guia.NumeroConselho = result.DadosContratado.NumeroConselho;
                guia.NumeroGuia = result.CabecalhoGuia.NumeroGuia;
                guia.Observacao = result.DadosAtendimento.Observacao;
                guia.Plano = result.DadosBeneficiario.Plano;
                guia.RegistroANS = result.CabecalhoGuia.RegistroANS;
                guia.TempoDoenca = result.HipoteseDiagnostica.TempoDoenca;
                guia.TipoConsulta = result.DadosAtendimento.TipoConsulta;
                guia.TipoSaida = result.DadosAtendimento.TipoSaida;
                guia.TipoDoenca = result.HipoteseDiagnostica.TipoDoenca;
                guia.TipoLogradouro = result.DadosContratado.TipoLogradouro;
                guia.TipoPessoa = result.DadosContratado.TipoPessoa;
                guia.UFConselho = result.DadosContratado.UFConselho;
                guia.UfContratado = result.DadosContratado.UFContratado;
                guia.ValidadeCarteira = result.DadosBeneficiario.ValidadeCarteira;
                guia.ValorTempo = result.HipoteseDiagnostica.TempoDoenca;
                guia.IdBeneficiario = result.IdBeneficiario;


                return Request.CreateResponse(HttpStatusCode.OK, guia);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getguiasconsultas")]
        public HttpResponseMessage ListraGuiasConsultas()
        {
            try
            {
                var model = new List<GuiaConsultaViewModel>();
                var result = _service.ListarGuiasPorTipo("Consulta", base.GetUsuarioLogado().IdClinica);
                foreach (var item in result)
                {
                    model.Add(new GuiaConsultaViewModel
                    {
                        DataEmissaoGuia = item.CabecalhoGuia.DataEmissaoGuia,
                        Nome = item.DadosBeneficiario.Nome,
                        NomeProfissionalExecutante = item.DadosContratado.NomeProfissionalExecutante,
                        NumeroCarteira = item.DadosBeneficiario.NumeroCarteira,
                        NumeroGuia = item.CabecalhoGuia.NumeroGuia,
                        Situacao = item.Situacao,
                        IdGuia = item.IdGuia
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
        [Route("getguiasspsadt")]
        public HttpResponseMessage ListraGuiasSpSadt()
        {
            try
            {
                var model = new List<GuiaConsultaViewModel>();
                var result = _service.ListarGuiasPorTipo("SPSADT", base.GetUsuarioLogado().IdClinica);
                foreach (var item in result)
                {
                    model.Add(new GuiaConsultaViewModel
                    {
                        DataEmissaoGuia = item.CabecalhoGuia.DataEmissaoGuia,
                        Nome = item.DadosBeneficiario.Nome,
                        NomeProfissionalExecutante = item.DadosContratado.NomeProfissionalExecutante,
                        NumeroCarteira = item.DadosBeneficiario.NumeroCarteira,
                        NumeroGuia = item.CabecalhoGuia.NumeroGuia,
                        Situacao = item.Situacao,
                        IdGuia = item.IdGuia
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
        [Route("saveGuiaConsulta")]
        public HttpResponseMessage SalvarGuiaConsulta(GuiaConsultaViewModel model)
        {
            try
            {
                string tempodoenca = string.Empty;

                if (!string.IsNullOrEmpty(model.TempoDoenca))//concateno os campos valor e tempo de doença
                    tempodoenca = model.ValorTempo + " " + model.TempoDoenca;

                if (model.IdGuia > 0)
                {
                    var guia = _service.ObterGuiaPorId(model.IdGuia);

                    if (guia == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "A guia selecionada não existe!");

                    if (guia.CabecalhoGuia != null)
                    {
                        guia.CabecalhoGuia.RegistroANS = model.RegistroANS;
                        guia.CabecalhoGuia.DataEmissaoGuia = model.DataEmissaoGuia;
                        guia.CabecalhoGuia.NumeroGuia = model.NumeroGuia;
                        guia.IdBeneficiario = model.IdBeneficiario;
                    }
                    if (guia.DadosAtendimento != null)
                    {
                        guia.DadosAtendimento.CodigoProcedimento = model.CodigoProcedimento;
                        guia.DadosAtendimento.CodigoTabela = model.CodigoTabela;
                        guia.DadosAtendimento.DataAtendimento = model.DataAtendimento;
                        guia.DadosAtendimento.Observacao = model.Observacao;
                        guia.DadosAtendimento.TipoConsulta = model.TipoConsulta;
                        guia.DadosAtendimento.TipoSaida = model.TipoSaida;
                    }
                    if (guia.DadosBeneficiario != null)
                    {
                        guia.DadosBeneficiario.Nome = model.Nome;
                        guia.DadosBeneficiario.NumeroCartaoSus = model.NumeroCartaoSus;
                        guia.DadosBeneficiario.NumeroCarteira = model.NumeroCarteira;
                        guia.DadosBeneficiario.Plano = model.Plano;
                        guia.DadosBeneficiario.ValidadeCarteira = model.ValidadeCarteira;
                    }
                    if (guia.DadosContratado != null)
                    {
                        guia.DadosContratado.CEP = model.CEP;
                        guia.DadosContratado.CodigoCBOs = model.CodigoCBOs;
                        guia.DadosContratado.CodigoCnes = model.CodigoCnes;
                        guia.DadosContratado.CodigoIBGEMunicipio = model.CodigoIBGEMunicipio;
                        guia.DadosContratado.CodigoNaOperadora = model.CodigoNaOperadora;
                        guia.DadosContratado.Complemento = model.Complemento;
                        guia.DadosContratado.ConselhoProfissional = model.ConselhoProfissional;
                        guia.DadosContratado.Logradouro = model.Logradouro;
                        guia.DadosContratado.Municipio = model.Municipio;
                        guia.DadosContratado.NomeContratado = model.NomeContratado;
                        guia.DadosContratado.NomeProfissionalExecutante = model.NomeProfissionalExecutante;
                        guia.DadosContratado.Numero = model.Numero;
                        guia.DadosContratado.NumeroConselho = model.NumeroConselho;
                        guia.DadosContratado.TipoLogradouro = model.TipoLogradouro;
                        guia.DadosContratado.TipoPessoa = model.TipoPessoa;
                        guia.DadosContratado.UFConselho = model.UFConselho;
                        guia.DadosContratado.UFContratado = model.UfContratado;
                    }
                    if (guia.HipoteseDiagnostica != null)
                    {
                        guia.HipoteseDiagnostica.CID10 = model.CID10;
                        guia.HipoteseDiagnostica.CID102 = model.CID102;
                        guia.HipoteseDiagnostica.CID103 = model.CID103;
                        guia.HipoteseDiagnostica.CID104 = model.CID104;
                        guia.HipoteseDiagnostica.IndicacaoAcidente = model.IndicacaoAcidente;
                        guia.HipoteseDiagnostica.TempoDoenca = tempodoenca;
                        guia.HipoteseDiagnostica.TipoDoenca = model.TipoDoenca;
                    }
                    _service.Salvar(guia);
                }
                else
                {
                    var guia = new Guia();
                    guia.IdBeneficiario = model.IdBeneficiario;

                    var cabecalho = new CabecalhoGuia(model.RegistroANS, "123456", model.DataEmissaoGuia);
                    var dadosbeneficiario = new DadosBeneficiario(model.NumeroCarteira, model.Plano, model.ValidadeCarteira, model.Nome, model.NumeroCartaoSus);
                    var dadoscontratado = new DadosContratado(model.TipoPessoa, model.CodigoNaOperadora, model.NomeContratado, model.CodigoCnes, model.TipoLogradouro, model.Logradouro,
                        model.Numero, model.Complemento, model.Municipio, model.UfContratado, model.CodigoIBGEMunicipio, model.CEP, model.NomeProfissionalExecutante, model.ConselhoProfissional,
                        model.NumeroConselho, model.UFConselho, model.CodigoCBOs);
                    var hipotese = new HipoteseDiagnostica(model.TipoDoenca, tempodoenca, model.IndicacaoAcidente, model.CID10, model.CID102, model.CID103, model.CID104);
                    var dadosatendimento = new DadosAtendimento(model.DataAtendimento, model.CodigoTabela, model.CodigoProcedimento, model.TipoConsulta, model.TipoSaida,
                        model.Observacao, null, null);

                    guia.GerarGuiaConsulta(cabecalho, dadosbeneficiario, dadoscontratado, hipotese, dadosatendimento, base.GetUsuarioLogado().Clinica);

                    _service.Salvar(guia);
                }

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("printconsulta")]
        public HttpResponseMessage GuiaPdf(int idGuia)
        {
            try
            {
                var fileBytes = _service.GetGuiaConsultaPdf(idGuia);
                var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(fileBytes) };

                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline")
                {
                    FileName = "GuiaConsulta_" + idGuia + "_" +
                               DateTime.Now.ToString("G")
                                   .Replace(":", "")
                                   .Replace("-", "")
                                   .Replace("/", "")
                                   .Replace(" ", "") + ".pdf"
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

                return response;

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [HttpGet]
        [Route("printspsadt/{idGuia:int}")]
        public HttpResponseMessage GuiaSpSadtPdf(int idGuia)
        {
            try
            {
                var fileBytes = _service.GetGuiaSpSadtPdf(idGuia);
                var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(fileBytes) };

                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline")
                {
                    FileName = "GuiaConsulta_" + idGuia + "_" +
                               DateTime.Now.ToString("G")
                                   .Replace(":", "")
                                   .Replace("-", "")
                                   .Replace("/", "")
                                   .Replace(" ", "") + ".pdf"
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

                return response;

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }


        [HttpGet]
        [Route("cancelar")]
        public HttpResponseMessage CancelarGuia(int idguia)
        {
            try
            {
                _service.CancelarGuia(idguia);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #region [Lotes guias]
        [HttpPost]
        [Route("salvarlote")]
        public HttpResponseMessage SalvarLote(LoteViewModel model)
        {
            try
            {
                var convenio = _serviceCadastro.ObterConvenioById(model.IdConvenio);
                if (convenio == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possivel recuperar dados do convenio!");

                var clinica = _serviceCadastro.ObterClinicaById(base.GetUsuarioLogado().IdClinica);
                if (clinica == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possivel recuperar dados da clinica!");


                var lote = new Lote(model.Situacao, convenio, clinica);
                var novo = _service.SalvarLote(lote);

                model.IdLote = novo.IdLote;

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [HttpGet]
        [Route("excluirlote")]
        public HttpResponseMessage ExcluirLote(int idlote)
        {
            try
            {
                var lote = _service.ObterLotePorId(idlote);
                if (lote == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possivel recuperar dados do lote!");

                _service.ExcluirLote(lote);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [HttpGet]
        [Route("listarlotes")]
        public HttpResponseMessage ListarLotes()
        {
            try
            {
                var lista = new List<LoteViewModel>();
                var model = _service.ListarLotes(base.GetUsuarioLogado().IdClinica);

                foreach (var item in model)
                {
                    lista.Add(new LoteViewModel
                    {
                        Data = item.Data,
                        IdClinica = item.IdClinica,
                        IdConvenio = item.IdConvenio,
                        IdLote = item.IdLote,
                        NomeConvenio = item.Convenio.Pessoa.Nome,
                        Situacao = item.Situacao
                    });
                }

                return Request.CreateResponse(HttpStatusCode.OK, lista);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("obterloteporid")]
        public HttpResponseMessage ObterLotePorId(int idLote)
        {
            try
            {
                var lote = _service.ObterLotePorId(idLote);
                if (lote == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possivel recuperar dados do lote!");

                var model = new LoteViewModel
                {
                    Data = lote.Data,
                    IdClinica = lote.IdClinica,
                    IdConvenio = lote.IdConvenio,
                    IdLote = lote.IdLote,
                    NomeConvenio = lote.Convenio.Pessoa.Nome,
                    Situacao = lote.Situacao
                };
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("listarlotesporconvenio")]
        public HttpResponseMessage ListarLotesPorConvenio(int idConvenio)
        {
            try
            {
                var lista = new List<LoteViewModel>();
                var model = _service.ListarLotesPorConvenio(base.GetUsuarioLogado().IdClinica, idConvenio);
                foreach (var item in model)
                {
                    lista.Add(new LoteViewModel
                    {
                        Data = item.Data,
                        IdClinica = item.IdClinica,
                        IdConvenio = item.IdConvenio,
                        IdLote = item.IdLote,
                        NomeConvenio = item.Convenio.Pessoa.Nome,
                        Situacao = item.Situacao
                    });
                }
                return Request.CreateResponse(HttpStatusCode.OK, lista);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        //[HttpGet]
        //[Route("xmlLoteConsulta")]
        public HttpResponseMessage GetXmlLote(int idLote)
        {
            var lote = _service.ObterLotePorId(idLote);
            if (lote == null)
                throw new Exception("Lote não encontrado.");

            HttpResponseMessage result = null;
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                ConformanceLevel = ConformanceLevel.Fragment,
                CloseOutput = false,
                Indent = true
            };

            using (var stream = new MemoryStream())
            {
                using (var writer = XmlWriter.Create(stream, settings))
                {

                    //escreve o elmento raiz
                    writer.WriteStartElement("ans", "mensagemTISS");
                    writer.WriteAttributeString("xmlns", "ans", "http://www.ans.gov.br/padroes/tiss/schemas");
                    writer.WriteAttributeString("xmlns", "xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    writer.WriteAttributeString("xsi", "schemaLocation", "http://www.ans.gov.br/padroes/tiss/schemas http://www.ans.gov.br/padroes/tiss/schemas/tissV2_02_01.xsd");

                    #region[identificacaoTransacao]
                    writer.WriteStartElement("ans", "cabecalho");//inicio do cabeçalho
                                                                 //identificacaoTransacao
                    writer.WriteStartElement("ans", "identificacaoTransacao");//inicio do identificacaoTransacao
                    writer.WriteElementString("ans", "tipoTransacao", "ENVIO_LOTE_GUIAS");
                    writer.WriteElementString("ans", "sequencialTransacao", lote.IdLote.ToString());
                    writer.WriteElementString("ans", "dataRegistroTransacao", DateTime.Now.ToString("YYYY-MM-DD"));
                    writer.WriteElementString("ans", "horaRegistroTransacao", DateTime.Now.ToLongTimeString());
                    writer.WriteEndElement();//fim do identificacaoTransacao
                                             //origem
                    writer.WriteStartElement("ans", "origem");//inicio do origem
                    writer.WriteStartElement("ans", "codigoPrestadorNaOperadora");//inicio do codigoPrestadorNaOperadora
                    writer.WriteElementString("ans", "CNPJ", lote.Clinica.Nome);
                    writer.WriteEndElement();//fim do codigoPrestadorNaOperadora
                    writer.WriteEndElement();//fim do origem
                                             //destino
                    writer.WriteStartElement("ans", "destino");//inicio do destino
                    writer.WriteElementString("ans", "registroANS", lote.Convenio.RegistroAns);
                    writer.WriteEndElement();//fim do destino
                    //versão padrão
                    writer.WriteStartElement("ans", "versaoPadrao", "3.00.02");//inicio do destino
                    writer.WriteEndElement();//fim do versão padrão
                    writer.WriteEndElement();//fim do cabeçalho
                    #endregion


                    writer.WriteStartElement("ans", "prestadorParaOperadora");//inicio do prestadorParaOperadora
                    writer.WriteStartElement("ans", "loteGuias");//inicio do loteGuias

                    writer.WriteElementString("ans", "numeroLote", lote.IdLote.ToString());

                    writer.WriteStartElement("ans", "guias");//inicio do guias
                    writer.WriteStartElement("ans", "guiaFaturamento");//inicio do guiaFaturamento



                    foreach (var item in lote.Guias)
                    {
                        writer.WriteStartElement("ans", "guiaConsulta");//inicio do guiaConsulta

                        writer.WriteStartElement("ans", "identificacaoGuia");//inicio do identificacaoGuia
                        writer.WriteStartElement("ans", "identificacaoFontePagadora");//inicio do identificacaoFontePagadora
                        writer.WriteElementString("ans", "registroANS", lote.Convenio.RegistroAns);
                        writer.WriteEndElement();//fim do identificacaoFontePagadora


                        writer.WriteElementString("ans", "dataEmissaoGuia", item.DadosAtendimento.DataAtendimento.ToString("YYYY-MM-DD"));//inicio da dataEmissaoGuiaa
                        writer.WriteElementString("ans", "numeroGuiaPrestador", item.CabecalhoGuia.NumeroGuia.ToString());//inicio do numeroGuiaPrestador
                        writer.WriteElementString("ans", "numeroGuiaOperadora", item.CabecalhoGuia.NumeroGuia.ToString());//inicio do numeroGuiaOperadora
                        writer.WriteEndElement();//fim do identificacaoGuia


                        writer.WriteStartElement("ans", "beneficiario");//inicio do beneficiario
                        writer.WriteElementString("ans", "numeroCarteira", item.DadosBeneficiario.NumeroCarteira.ToString());//inicio do numeroCarteira
                        writer.WriteElementString("ans", "nomeBeneficiario", item.DadosBeneficiario.Nome);//inicio do nomeBeneficiario
                        writer.WriteElementString("ans", "nomePlano", item.DadosBeneficiario.Plano);//inicio do nomePlano
                        writer.WriteEndElement();//fim do beneficiario

                        writer.WriteStartElement("ans", "dadosContratado");//inicio do dadosContratado
                        writer.WriteStartElement("ans", "identificacao");//inicio do identificacao
                        writer.WriteElementString("ans", "CNPJ", item.DadosContratado.CodigoNaOperadora);
                        writer.WriteEndElement();//fim do identificacao
                        writer.WriteElementString("ans", "nomeContratado", item.DadosContratado.NomeContratado.ToUpper());//inicio do nomeContratado
                        writer.WriteStartElement("ans", "enderecoContratado");//inicio do enderecoContratado
                        writer.WriteElementString("ans", "tipoLogradouro", item.DadosContratado.TipoLogradouro);
                        writer.WriteElementString("ans", "logradouro", item.DadosContratado.Logradouro.ToUpper());
                        writer.WriteElementString("ans", "numero", item.DadosContratado.Numero);
                        writer.WriteElementString("ans", "complemento", item.DadosContratado.Complemento);
                        writer.WriteElementString("ans", "codigoIBGEMunicipio", item.DadosContratado.CodigoIBGEMunicipio);
                        writer.WriteElementString("ans", "municipio", item.DadosContratado.Municipio);
                        writer.WriteElementString("ans", "codigoUF", item.DadosContratado.UFContratado);
                        writer.WriteElementString("ans", "cep", item.DadosContratado.CEP);
                        writer.WriteEndElement();//fim do enderecoContratado
                        writer.WriteElementString("ans", "numeroCNES", item.DadosContratado.CodigoCnes);
                        writer.WriteEndElement();//fim do dadosContratado



                        writer.WriteStartElement("ans", "profissionalExecutante");//inicio do profissionalExecutante
                        writer.WriteElementString("ans", "nomeProfissional", item.DadosContratado.NomeProfissionalExecutante);//inicio do nomeProfissional
                        writer.WriteStartElement("ans", "conselhoProfissional");//inicio do conselhoProfissional
                        writer.WriteElementString("ans", "siglaConselho", item.DadosContratado.ConselhoProfissional);
                        writer.WriteElementString("ans", "numeroConselho", item.DadosContratado.NumeroConselho);
                        writer.WriteElementString("ans", "ufConselho", item.DadosContratado.UFConselho);
                        writer.WriteEndElement();//fim do conselhoProfissional
                        writer.WriteElementString("ans", "cbos", item.DadosContratado.CodigoCBOs);//inicio do cbos
                        writer.WriteEndElement();//fim do profissionalExecutante

                        writer.WriteStartElement("ans", "dadosAtendimento");//inicio do dadosAtendimento
                        writer.WriteElementString("ans", "dataAtendimento", item.DadosAtendimento.DataAtendimento.ToString("YYYY-MM-DD"));//inicio do dataAtendimento

                        writer.WriteStartElement("ans", "procedimento");//inicio do procedimento
                        writer.WriteElementString("ans", "codigoTabela", item.DadosAtendimento.CodigoTabela);//inicio do codigoTabela
                        writer.WriteElementString("ans", "codigoProcedimento", item.DadosAtendimento.CodigoProcedimento);//inicio do codigoProcedimento
                        writer.WriteEndElement();//fim do procedimento


                        writer.WriteElementString("ans", "tipoConsulta", item.DadosAtendimento.TipoConsulta);//inicio do tipoConsulta
                        writer.WriteElementString("ans", "tipoSaida", item.DadosAtendimento.TipoSaida);//inicio do tipoSaida

                        writer.WriteEndElement();//fim do dadosAtendimento

                        writer.WriteElementString("ans", "observacao", item.DadosAtendimento.Observacao);//inicio do observacao

                        writer.WriteEndElement();//fim do guiaConsulta
                    }


                    writer.WriteEndElement();//fim do guiaFaturamento
                    writer.WriteEndElement();//fim do guias
                    writer.WriteEndElement();//fim do loteGuias
                    writer.WriteEndElement();//fim do prestadorParaOperadora


                    writer.WriteStartElement("ans", "epilogo");//inicio do epilogo
                    writer.WriteElementString("ans", "hash", "54sda64d5sa465das45da64d65sa4d65s4a54d");//inicio do codigoTabela
                    writer.WriteEndElement();//fim do epilogo

                    // encerra o elemento raiz
                    writer.WriteEndElement();//ans:mensagemTISS
                    writer.Close();
                    result = Request.CreateXMLResponse(stream);
                }
            }

            return result;
        }

        [HttpGet]
        [Route("xmlLoteConsulta")]
        public HttpResponseMessage xml(int idLote)
        {
            var lote = _service.ObterLotePorId(idLote);
            if (lote == null)
                throw new Exception("Lote não encontrado.");

            HttpResponseMessage result = null;
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                ConformanceLevel = ConformanceLevel.Fragment,
                CloseOutput = false,
                Indent = true
            };

            string dadosxml = string.Empty;

            StringBuilder sbXML = new StringBuilder();
            sbXML.AppendLine("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>");
            sbXML.AppendLine("<ans:mensagemTISS xmlns:ans=\"http://www.ans.gov.br/padroes/tiss/schemas\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.ans.gov.br/padroes/tiss/schemas http://www.ans.gov.br/padroes/tiss/schemas/tissV3_01_00.xsd\">");
            sbXML.AppendLine("  <ans:cabecalho>");
            sbXML.AppendLine("      <ans:identificacaoTransacao>");
            sbXML.AppendLine("          <ans:tipoTransacao>ENVIO_LOTE_GUIAS</ans:tipoTransacao>");
            sbXML.AppendLine(String.Format("                <ans:sequencialTransacao>{0}</ans:sequencialTransacao>", lote.IdLote.ToString()));
            dadosxml += lote.IdLote.ToString();
            string datageracao = DateTime.Now.ToString("yyyy-MM-dd");
            string horageracao = DateTime.Now.ToString("HH:mm:ss");
            dadosxml += datageracao;
            dadosxml += horageracao;
            sbXML.AppendLine(String.Format("                <ans:dataRegistroTransacao>{0}</ans:dataRegistroTransacao>", datageracao));
            sbXML.AppendLine(String.Format("                <ans:horaRegistroTransacao>{0}</ans:horaRegistroTransacao>", horageracao));
            sbXML.AppendLine("      </ans:identificacaoTransacao>");
            sbXML.AppendLine("      <ans:origem>");
            sbXML.AppendLine("          <ans:identificacaoPrestador>");
            sbXML.AppendLine(String.Format("                <ans:codigoPrestadorNaOperadora>{0}</ans:codigoPrestadorNaOperadora>", lote.Clinica.Nome));
            dadosxml += lote.Clinica.Nome;
            sbXML.AppendLine("          </ans:identificacaoPrestador>");
            sbXML.AppendLine("      </ans:origem>");
            sbXML.AppendLine("      <ans:destino>");
            sbXML.AppendLine(String.Format("            <ans:registroANS>{0}</ans:registroANS>", lote.Convenio.RegistroAns));
            dadosxml += lote.Convenio.RegistroAns;
            sbXML.AppendLine("      </ans:destino>");
            sbXML.AppendLine("      <ans:versaoPadrao>3.02.00</ans:versaoPadrao>");
            dadosxml += "3.02.00";
            sbXML.AppendLine("  </ans:cabecalho>");
            sbXML.AppendLine("  <ans:prestadorParaOperadora>");
            sbXML.AppendLine("      <ans:loteGuias>");
            sbXML.AppendLine(String.Format("            <ans:numeroLote>{0}</ans:numeroLote>", lote.IdLote.ToString()));
            dadosxml += lote.IdLote.ToString();
            sbXML.AppendLine("          <ans:guiasTISS>");
            //Guias
            foreach (var item in lote.Guias)
            {
                sbXML.AppendLine("  <ans:guiaConsulta>");


                sbXML.AppendLine("      <ans:identificacaoGuia>");
                sbXML.AppendLine("      <ans:identificacaoFontePagadora>");
                sbXML.AppendLine(String.Format("            <ans:registroANS>{0}</ans:registroANS>", lote.Convenio.RegistroAns));
                dadosxml += lote.Convenio.RegistroAns;
                sbXML.AppendLine("      </ans:identificacaoFontePagadora>");
                sbXML.AppendLine(String.Format("            <ans:dataEmissaoGuia>{0}</ans:dataEmissaoGuia>", item.DadosAtendimento.DataAtendimento.ToString("YYYY-MM-DD")));
                sbXML.AppendLine(String.Format("            <ans:numeroGuiaPrestador>{0}</ans:numeroGuiaPrestador>", item.CabecalhoGuia.NumeroGuia.ToString()));
                sbXML.AppendLine(String.Format("            <ans:numeroGuiaOperadora>{0}</ans:numeroGuiaOperadora>", item.CabecalhoGuia.NumeroGuia.ToString()));
                sbXML.AppendLine("      </ans:identificacaoGuia>");
                dadosxml += item.DadosAtendimento.DataAtendimento.ToString("YYYY-MM-DD");
                dadosxml += item.CabecalhoGuia.NumeroGuia.ToString();
                dadosxml += item.CabecalhoGuia.NumeroGuia.ToString();

                sbXML.AppendLine("      <ans:beneficiario>");
                sbXML.AppendLine(String.Format("            <ans:numeroCarteira>{0}</ans:numeroCarteira>", item.DadosBeneficiario.NumeroCarteira.ToString()));
                sbXML.AppendLine(String.Format("            <ans:nomeBeneficiario>{0}</ans:nomeBeneficiario>", item.DadosBeneficiario.Nome));
                sbXML.AppendLine(String.Format("            <ans:nomePlano>{0}</ans:nomePlano>", item.DadosBeneficiario.Plano));
                sbXML.AppendLine("      </ans:beneficiario>");

                dadosxml += item.DadosBeneficiario.NumeroCarteira.ToString();
                dadosxml += item.DadosBeneficiario.Nome;
                dadosxml += item.DadosBeneficiario.Plano;

                sbXML.AppendLine("      <ans:dadosContratado>");
                sbXML.AppendLine("      <ans:identificacao>");
                sbXML.AppendLine(String.Format("            <ans:CNPJ>{0}</ans:CNPJ>", item.DadosContratado.CodigoNaOperadora));
                dadosxml += item.DadosContratado.CodigoNaOperadora;
                sbXML.AppendLine("      </ans:identificacao>");
                sbXML.AppendLine(String.Format("            <ans:nomeContratado>{0}</ans:nomeContratado>", item.DadosContratado.NomeContratado.ToUpper()));
                dadosxml += item.DadosContratado.NomeContratado.ToUpper();
                sbXML.AppendLine("      <ans:enderecoContratado>");
                sbXML.AppendLine(String.Format("            <ans:tipoLogradouro>{0}</ans:tipoLogradouro>", item.DadosContratado.TipoLogradouro));
                sbXML.AppendLine(String.Format("            <ans:logradouro>{0}</ans:logradouro>", item.DadosContratado.Logradouro.ToUpper()));
                sbXML.AppendLine(String.Format("            <ans:numero>{0}</ans:numero>", item.DadosContratado.Numero));
                sbXML.AppendLine(String.Format("            <ans:complemento>{0}</ans:complemento>", item.DadosContratado.Complemento));
                sbXML.AppendLine(String.Format("            <ans:codigoIBGEMunicipio>{0}</ans:codigoIBGEMunicipio>", item.DadosContratado.CodigoIBGEMunicipio));
                sbXML.AppendLine(String.Format("            <ans:municipio>{0}</ans:municipio>", item.DadosContratado.Municipio));
                sbXML.AppendLine(String.Format("            <ans:codigoUF>{0}</ans:codigoUF>", item.DadosContratado.UFContratado));
                sbXML.AppendLine(String.Format("            <ans:cep>{0}</ans:cep>", item.DadosContratado.CEP));
                sbXML.AppendLine("      </ans:enderecoContratado>");
                sbXML.AppendLine(String.Format("            <ans:numeroCNES>{0}</ans:numeroCNES>", item.DadosContratado.CodigoCnes));
                sbXML.AppendLine("      </ans:dadosContratado>");

                dadosxml += item.DadosContratado.TipoLogradouro;
                dadosxml += item.DadosContratado.Logradouro.ToUpper();
                dadosxml += item.DadosContratado.Numero;
                dadosxml += item.DadosContratado.Complemento;
                dadosxml += item.DadosContratado.CodigoIBGEMunicipio;
                dadosxml += item.DadosContratado.Municipio;
                dadosxml += item.DadosContratado.UFContratado;
                dadosxml += item.DadosContratado.CEP;
                dadosxml += item.DadosContratado.CodigoCnes;

                sbXML.AppendLine("      <ans:profissionalExecutante>");
                sbXML.AppendLine(String.Format("            <ans:nomeProfissional>{0}</ans:nomeProfissional>", item.DadosContratado.NomeProfissionalExecutante.ToUpper()));
                sbXML.AppendLine("      <ans:conselhoProfissional>");
                sbXML.AppendLine(String.Format("            <ans:siglaConselho>{0}</ans:siglaConselho>", item.DadosContratado.ConselhoProfissional));
                sbXML.AppendLine(String.Format("            <ans:numeroConselho>{0}</ans:numeroConselho>", item.DadosContratado.NumeroConselho));
                sbXML.AppendLine(String.Format("            <ans:ufConselho>{0}</ans:ufConselho>", item.DadosContratado.UFConselho));
                sbXML.AppendLine("      </ans:conselhoProfissional>");
                sbXML.AppendLine(String.Format("            <ans:cbos>{0}</ans:cbos>", item.DadosContratado.CodigoCBOs));
                sbXML.AppendLine("      </ans:profissionalExecutante>");

                dadosxml += item.DadosContratado.NomeProfissionalExecutante.ToUpper();
                dadosxml += item.DadosContratado.ConselhoProfissional;
                dadosxml += item.DadosContratado.NumeroConselho;
                dadosxml += item.DadosContratado.UFConselho;
                dadosxml += item.DadosContratado.CodigoCBOs;


                sbXML.AppendLine("      <ans:dadosAtendimento>");
                sbXML.AppendLine(String.Format("            <ans:dataAtendimento>{0}</ans:dataAtendimento>", item.DadosAtendimento.DataAtendimento.ToString("YYYY-MM-DD")));
                sbXML.AppendLine("      <ans:procedimento>");
                sbXML.AppendLine(String.Format("            <ans:codigoTabela>{0}</ans:codigoTabela>", item.DadosAtendimento.CodigoTabela));
                sbXML.AppendLine(String.Format("            <ans:codigoProcedimento>{0}</ans:codigoProcedimento>", item.DadosAtendimento.CodigoProcedimento));
                sbXML.AppendLine("      </ans:procedimento>");
                sbXML.AppendLine(String.Format("            <ans:tipoConsulta>{0}</ans:tipoConsulta>", item.DadosAtendimento.TipoConsulta));
                sbXML.AppendLine(String.Format("            <ans:tipoSaida>{0}</ans:tipoSaida>", item.DadosAtendimento.TipoSaida));
                sbXML.AppendLine("      </ans:dadosAtendimento>");

                dadosxml += item.DadosAtendimento.DataAtendimento.ToString("YYYY-MM-DD");
                dadosxml += item.DadosAtendimento.CodigoTabela;
                dadosxml += item.DadosAtendimento.CodigoProcedimento;
                dadosxml += item.DadosAtendimento.TipoConsulta;
                dadosxml += item.DadosAtendimento.TipoSaida;

                if (!string.IsNullOrEmpty(item.DadosAtendimento.Observacao))
                {
                    sbXML.AppendLine(String.Format("            <ans:observacao>{0}</ans:observacao>", item.DadosAtendimento.Observacao));
                    dadosxml += item.DadosAtendimento.Observacao;
                }

                sbXML.AppendLine("  </ans:guiaConsulta>");
            }

            sbXML.AppendLine("          </ans:guiasTISS>");
            sbXML.AppendLine("      </ans:loteGuias>");
            sbXML.AppendLine("  </ans:prestadorParaOperadora>");
            sbXML.AppendLine("      <ans:epilogo>");
            sbXML.AppendLine(String.Format("            <ans:hash>{0}</ans:hash>", GetMd5Hash(dadosxml)));
            sbXML.AppendLine("      </ans:epilogo>");
            sbXML.AppendLine("</ans:mensagemTISS>");

            byte[] bytesFromBuilder = Encoding.UTF8.GetBytes(sbXML.ToString());

            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(bytesFromBuilder) };

            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline")
            {
                FileName = "lote_guias_" + idLote + "_" +
                           DateTime.Now.ToString("G")
                               .Replace(":", "")
                               .Replace("-", "")
                               .Replace("/", "")
                               .Replace(" ", "") + ".xml"
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");

            return response;
        }

        public string GetMd5Hash(string input)
        {
            MD5 md5Hash = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        public string ObterHashAns(byte[] arquivo)
        {
            XDocument xDocument = null;
            bool result = false;
            var iso88591Encoding = Encoding.GetEncoding("ISO-8859-1");

            using (var streamReader = new MemoryStream(arquivo, false))
            {
                xDocument = XDocument.Load(streamReader);
                var root = xDocument.Root;
                var ans = root.Name.Namespace;

                var currentHash = (from epilogo in root.Elements(ans + "epilogo")
                                   from hash in epilogo.Elements(ans + "hash")
                                   select hash).First();

                var lote = (from op in root.Elements(ans + "prestadorParaOperadora")
                            from loteGuias in op.Elements(ans + "loteGuias")
                            from idLote in loteGuias.Elements(ans + "numeroLote")
                            select idLote).First();


                var concatText = string.Concat(from element in root.Elements().Where(i => i.Name.LocalName != "epilogo")
                                               select element.Value);

                string calculatedHash = string.Empty;
                using (var md5Hash = System.Security.Cryptography.MD5.Create())
                {
                    var bytes = md5Hash.ComputeHash(iso88591Encoding.GetBytes(concatText));
                    calculatedHash = string.Concat(bytes.Select(b => b.ToString("x2").ToUpper()));
                }

                return calculatedHash.ToUpper();
            }
        }

        public bool ValidadorHashAns(byte[] arquivo)
        {
            XDocument xDocument = null;
            bool result = false;
            var iso88591Encoding = Encoding.GetEncoding("ISO-8859-1");

            using (var streamReader = new MemoryStream(arquivo, false))
            {
                xDocument = XDocument.Load(streamReader);
                var root = xDocument.Root;
                var ans = root.Name.Namespace;

                var currentHash = (from epilogo in root.Elements(ans + "epilogo")
                                   from hash in epilogo.Elements(ans + "hash")
                                   select hash).First();

                var lote = (from op in root.Elements(ans + "prestadorParaOperadora")
                            from loteGuias in op.Elements(ans + "loteGuias")
                            from idLote in loteGuias.Elements(ans + "numeroLote")
                            select idLote).First();


                var concatText = string.Concat(from element in root.Elements().Where(i => i.Name.LocalName != "epilogo")
                                               select element.Value);

                string calculatedHash = string.Empty;
                using (var md5Hash = System.Security.Cryptography.MD5.Create())
                {
                    var bytes = md5Hash.ComputeHash(iso88591Encoding.GetBytes(concatText));
                    calculatedHash = string.Concat(bytes.Select(b => b.ToString("x2").ToUpper()));
                }

                if (currentHash.Value.ToUpper().Equals(calculatedHash))
                    result = true;

            }

            return result;
        }

        // Verify a hash against a string.
        public bool VerifyMd5Hash(string input, string hash)
        {
            MD5 md5Hash = MD5.Create();
            // Hash the input.
            string hashOfInput = GetMd5Hash(input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        [Route("addguiaslote")]
        public HttpResponseMessage AdicionarGuiasLote(GuiasLoteViewModel model)
        {
            try
            {
                var lote = _service.ObterLotePorId(model.IdLote);
                if (lote == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possivel recuperar dados do lote!");

                foreach (var item in model.Guias)
                {
                    var guia = _service.ObterGuiaPorId(item);
                    lote.AddGuia(guia);
                }
                lote.SetSituacao("Faturado");

                _service.SalvarLote(lote);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("gerarlote")]
        public HttpResponseMessage GerarLote(int idLote)
        {
            try
            {
                var lote = _service.ObterLotePorId(idLote);
                if (lote == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possivel recuperar dados do lote!");


                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Route("listarguiaslote")]
        public HttpResponseMessage ListarGuiasLote(int idConvenio, string tipo)
        {
            try
            {
                var model = new List<ListaGuiasLoteViewModel>();

                var guias = _service.ListarGuiasPorConvenio(idConvenio, base.GetUsuarioLogado().IdClinica, tipo);

                foreach (var item in guias)
                {
                    model.Add(new ListaGuiasLoteViewModel
                    {
                        Convenio = item.Convenio.Pessoa.Nome,
                        Data = item.DadosAtendimento.DataAtendimento,
                        IdGuia = item.IdGuia,
                        Paciente = item.DadosBeneficiario.Nome,
                        Situacao = item.Situacao,
                        Selected = false
                    });
                }

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #endregion

    }
}