using AutoMapper;
using Clinicas.Application.Services.Interfaces;
using Clinicas.Domain.Model;
using Clinicas.Domain.ViewModel;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.css;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;
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
    [RoutePrefix("prontuario")]
    [Authorize]
    public class ProntuarioController : BaseController
    {
        private readonly IProntuarioService _serviceProntuario;
        private readonly ICadastroService _serviceCadastro;
        private readonly IUsuarioService _usuarioService;
        public static string informacoes = "";
        public static string enderecoClinica = "";

        public ProntuarioController(IProntuarioService service, ICadastroService serviceCadastro, IUsuarioService usuarioService) : base(usuarioService)
        {
            _serviceProntuario = service;
            _serviceCadastro = serviceCadastro;
            _usuarioService = usuarioService;
        }

        [HttpGet]
        [Route("getUltimosAtendimentos")]
        public HttpResponseMessage UltimosAtendimentos(int id)
        {
            try
            {
                var consulta = _serviceProntuario.UltimosAtendimentos(id);
                var model = new List<UltimosAtendimentosViewModel>();
                foreach (var item in consulta)
                {
                    model.Add(new UltimosAtendimentosViewModel()
                    {
                        Data = item.Data,
                        Hora = item.Hora,
                        IdAgenda = item.IdAgenda,
                        Paciente = item.Paciente.Pessoa.Nome,
                        ProfissionalSaude = item.ProfissionalSaude.Pessoa.Nome,
                        Situacao = item.Situacao
                    });
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #region [Anamnese]
        [HttpGet]
        [Route("listarAnamneses")]
        public HttpResponseMessage ListarAnamnese(int id) // idpaciente
        {
            try
            {
                var consulta = _serviceProntuario.ListarAnamnese(id);
                var model = new List<AnamneseViewModel>();
                foreach (var item in consulta)
                {
                    model.Add(new AnamneseViewModel()
                    {
                        IdAnamnese = item.IdAnamnese,
                        Data = item.Data,
                        Diagnostico = item.Diagnostico,
                        Paciente = item.Paciente.Pessoa.Nome,
                        ProfissionalSaude = item.ProfissionalSaude.Pessoa.Nome,
                        Hma = item.Hma,
                        CondutaMedica = item.CondutaMedica,
                        Situacao = item.Situacao
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
        [Route("obterAnamnesePorId")]
        public HttpResponseMessage ObterAnamnesePorId(int id)
        {
            try
            {
                var item = _serviceProntuario.ObterAnamnesePorId(id);
                var model = new AnamneseViewModel();

                model.IdAnamnese = item.IdAnamnese;
                model.Data = item.Data;
                model.Diagnostico = item.Diagnostico;
                model.Paciente = item.Paciente.Pessoa.Nome;
                model.ProfissionalSaude = item.ProfissionalSaude.Pessoa.Nome;
                model.Hma = item.Hma;
                model.CondutaMedica = item.CondutaMedica;
                model.Situacao = item.Situacao;

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("salvarAnamnese")]
        public HttpResponseMessage SalvarAnamnese(AnamneseViewModel model)
        {
            try
            {
                if (model.IdAnamnese > 0)
                {
                    var anamnese = _serviceProntuario.ObterAnamnesePorId(model.IdAnamnese);
                    if (anamnese == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados da anamnese.");

                    anamnese.SetDiagnostico(model.Diagnostico);
                    anamnese.SetHma(model.Hma);
                    anamnese.SetSituacao(model.Situacao);
                    anamnese.SetCondutaMedica(model.CondutaMedica);

                    _serviceProntuario.SalvarAnamnese(anamnese);
                }
                else
                {
                    var paciente = _serviceCadastro.ObterPacienteById(model.IdPaciente);
                    if (paciente == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do paciente.");

                    var profissional = _serviceCadastro.ObterFuncionarioById(model.IdProfissionalSaude);
                    if (profissional == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do profissional de saúde");

                    var anamnese = new Anamnese(model.Hma, model.Diagnostico, model.CondutaMedica, paciente, profissional);
                    _serviceProntuario.SalvarAnamnese(anamnese);
                }

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("excluirAnamnese")]
        public HttpResponseMessage ExcluirAnamnese(int id)
        {
            try
            {
                _serviceProntuario.ObterAnamnesePorId(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion

        #region [Modelo Prontuário]
        [HttpGet]
        [Route("getModeloById")]
        public HttpResponseMessage ObterModeloProntuarioPorId(int id)
        {
            try
            {
                var model = new ModeloProntuarioViewModel();

                var modelo = _serviceProntuario.ObterModeloProntuarioPorId(id);

                if (modelo == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao recuperar dados do modelo.");

                Mapper.CreateMap<ModeloProntuario, ModeloProntuarioViewModel>();
                model = Mapper.Map<ModeloProntuario, ModeloProntuarioViewModel>(modelo);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("listarModeloProntuarioPorTipo")]
        public HttpResponseMessage ListarModeloProntuarioPorTipo(string tipo)
        {
            try
            {
                var model = new List<ModeloProntuarioViewModel>();
                var modelos = _serviceProntuario.ListarModeloProntuarioPorTipo(tipo);

                if (modelos.Count > 0)
                {

                    foreach (var item in modelos)
                    {
                        model.Add(new ModeloProntuarioViewModel()
                        {
                            Descricao = item.Descricao,
                            IdModeloProntuario = item.IdModeloProntuario,
                            Situacao = item.Situacao,
                            Tipo = item.Tipo,
                            NomeModelo = item.NomeModelo
                        });
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, model);
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getModelos")]
        public HttpResponseMessage ListarModelosProntuario()
        {
            try
            {
                var model = new List<ModeloProntuarioViewModel>();
                var modelos = _serviceProntuario.ListarModelosProntuario();

                if (modelos.Count > 0)
                {

                    foreach (var item in modelos)
                    {
                        model.Add(new ModeloProntuarioViewModel()
                        {
                            Descricao = item.Descricao,
                            IdModeloProntuario = item.IdModeloProntuario,
                            Situacao = item.Situacao,
                            Tipo = item.Tipo,
                            NomeModelo = item.NomeModelo
                        });
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, model);
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("saveModelo")]
        public HttpResponseMessage SalvarModeloProntuario(ModeloProntuarioViewModel model)
        {
            try
            {
                if (model.IdModeloProntuario > 0)
                {
                    var modelo = _serviceProntuario.ObterModeloProntuarioPorId(model.IdModeloProntuario);
                    if (modelo == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do modelo.");


                    modelo.SetDescricao(model.Descricao);
                    string situacao = model.Situacao == "Ativo" ? "Ativo" : "Inativo";
                    modelo.SetSituacao(situacao);
                    modelo.SetTipo(model.Tipo);
                    modelo.SetNomeModelo(model.NomeModelo);

                    _serviceProntuario.SalvarModeloProntuario(modelo);
                }
                else
                {
                    string situacao = model.Situacao == "Ativo" ? "Ativo" : "Inativo";
                    var modelo = new ModeloProntuario(model.Descricao, model.Tipo, situacao, model.NomeModelo);
                    _serviceProntuario.SalvarModeloProntuario(modelo);
                }

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("excluirModelo")]
        public HttpResponseMessage ExcluirModeloProntuario(int id)
        {
            try
            {
                if (id > 0)
                {
                    var modelo = _serviceProntuario.ObterModeloProntuarioPorId(id);
                    if (modelo == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do modelo.");

                    _serviceProntuario.ExcluirModelo(modelo);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do modelo.");
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("pesquisarModelos")]
        public HttpResponseMessage PesquisarModelos(string nome)
        {
            try
            {
                var model = new List<ModeloProntuarioViewModel>();
                var modelos = _serviceProntuario.PesquisarModelos(nome);

                if (modelos.Count > 0)
                {

                    foreach (var item in modelos)
                    {
                        model.Add(new ModeloProntuarioViewModel()
                        {
                            Descricao = item.Descricao,
                            IdModeloProntuario = item.IdModeloProntuario,
                            Situacao = item.Situacao,
                            Tipo = item.Tipo,
                            NomeModelo = item.NomeModelo
                        });
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, model);
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion

        #region [Historia Pregressa]

        [HttpGet]
        [Route("listarHistoriaPregressa")]
        public HttpResponseMessage ListarHistoriaPregressa(int id)
        {
            try
            {
                var model = new List<HistoriaPregressaViewModel>();
                var consulta = _serviceProntuario.ListarHistoriaPregressa(id);

                foreach (var item in consulta)
                {
                    model.Add(new HistoriaPregressaViewModel()
                    {
                        Descricao = item.Descricao,
                        IdHistoriaPregressa = item.IdHistoriaPregressa,
                        Data = item.Data,
                        IdPaciente = item.IdPaciente,
                        Paciente = item.Paciente.Pessoa.Nome,
                        ProfissionalSaude = item.ProfissionalSaude.Pessoa.Nome

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
        [Route("obterHistoriaPregressaPorIdPaciente")]
        public HttpResponseMessage ObterHistoriaPregressaPorIdPaciente(int id)
        {
            try
            {
                var model = new HistoriaPregressaViewModel();
                var item = _serviceProntuario.ObterHistoriaPregressaPorId(id);

                if (item == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao recuperar dados da história pregressa.");

                model.Descricao = item.Descricao;
                model.IdHistoriaPregressa = item.IdHistoriaPregressa;
                model.IdPaciente = item.IdPaciente;
                model.Paciente = item.Paciente.Pessoa.Nome;
                model.ProfissionalSaude = item.ProfissionalSaude.Pessoa.Nome;

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("salvarHistoriaPregressa")]
        public HttpResponseMessage SalvarHistoriaPregressa(HistoriaPregressaViewModel model)
        {
            try
            {
                if (model.IdHistoriaPregressa > 0)
                {
                    var historia = _serviceProntuario.ObterHistoriaPregressaPorId(model.IdHistoriaPregressa);
                    if (historia == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados da história pregressa.");


                    historia.SetDescricao(model.Descricao);
                    historia.SetSituacao(model.Situacao);

                    _serviceProntuario.SalvarHistoriaPregressa(historia);
                }
                else
                {
                    var paciente = _serviceCadastro.ObterPacienteById(model.IdPaciente);
                    if (paciente == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do paciente");

                    var profissionalsaude = _serviceCadastro.ObterFuncionarioById(model.IdFuncionario);
                    if (profissionalsaude == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do profissional de saúde");


                    var historia = new HistoriaPregressa(model.Descricao, paciente, profissionalsaude);
                    _serviceProntuario.SalvarHistoriaPregressa(historia);
                }

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("excluirHistoriaPregressa")]
        public HttpResponseMessage ExcluirHistoriaPregressa(int id)
        {
            try
            {
                _serviceProntuario.ExcluirHistoriaPregressa(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion

        #region [Cadastro de Vacinas]
        [HttpGet]
        [Route("getVacinaById")]
        public HttpResponseMessage ObterVacinaPorId(int id)
        {
            try
            {
                var model = new VacinaViewModel();

                var vacina = _serviceProntuario.ObterVacinaPorId(id);

                if (vacina == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao recuperar dados da vacina.");

                Mapper.CreateMap<Vacina, VacinaViewModel>();
                model = Mapper.Map<Vacina, VacinaViewModel>(vacina);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getVacinas")]
        public HttpResponseMessage ListarVacinas()
        {
            try
            {
                var model = new List<VacinaViewModel>();

                var vacinas = _serviceProntuario.ListarVacinas();

                if (vacinas.Count > 0)
                {
                    Mapper.CreateMap<Vacina, VacinaViewModel>();
                    model = Mapper.Map<List<Vacina>, List<VacinaViewModel>>(vacinas);
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        #endregion

        #region [Medidas Antropometricas]

        [HttpGet]
        [Route("listarMedidas")]
        public HttpResponseMessage ObterMedidasPorPaciente(int id)
        {
            try
            {
                var model = new List<MedidasAntropometricasViewModel>();
                var consulta = _serviceProntuario.ObterMedidasPorPaciente(id);

                foreach (var item in consulta)
                {
                    model.Add(new MedidasAntropometricasViewModel()
                    {
                        Altura = item.Altura,
                        Data = item.Data,
                        IdPaciente = item.IdPaciente,
                        IdMedida = item.IdMedida,
                        Imc = item.Imc,
                        PerimetroCefalico = item.PerimetroCefalico,
                        Peso = item.Peso
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
        [Route("excluirMedidas")]
        public HttpResponseMessage ExcluirMedidas(int id)
        {
            try
            {
                var medidas = _serviceProntuario.ObterMedidasPorId(id);
                _serviceProntuario.ExcluirMedidas(id);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("obterMedidas")]
        public HttpResponseMessage SalvarMedidaAntropometrica(int idmedida)
        {
            try
            {
                var consulta = _serviceProntuario.ObterMedidasPorId(idmedida);

                var model = new MedidasAntropometricasViewModel();
                model.Altura = consulta.Altura;
                model.Data = consulta.Data;
                model.PerimetroCefalico = consulta.PerimetroCefalico;
                model.Peso = consulta.Peso;
                model.IdMedida = consulta.IdMedida;
                model.IdPaciente = consulta.IdPaciente;
                model.IdProfissionalSaude = consulta.ProfissionalSaude.IdFuncionario;
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpPost]
        [Route("salvarMedidas")]
        public HttpResponseMessage SalvarMedidaAntropometrica(MedidasAntropometricasViewModel model)
        {
            try
            {
                if (model.IdMedida > 0)
                {
                    var medida = _serviceProntuario.ObterMedidasPorId(model.IdMedida);
                    if (medida == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados da medida antropmétrica.");


                    var paciente = _serviceCadastro.ObterPacienteById(model.IdPaciente);
                    if (paciente == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do paciente.");

                    var profissionalSaude = _serviceCadastro.ObterFuncionarioById(model.IdProfissionalSaude);
                    if (profissionalSaude == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do profissional de saúde.");

                    medida.SetAltura(model.Altura);
                    medida.SetImc(model.Imc);
                    medida.SetPeso(model.Peso);
                    medida.SetData(model.Data);
                    medida.SetPerimetroCefalico(model.PerimetroCefalico);
                    medida.SetPaciente(paciente);
                    medida.SetProfissionalSaude(profissionalSaude);

                    _serviceProntuario.SalvarMedidaAntropometrica(medida);
                }
                else
                {
                    var paciente = _serviceCadastro.ObterPacienteById(model.IdPaciente);
                    if (paciente == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do paciente.");

                    var profissionalSaude = _serviceCadastro.ObterFuncionarioById(model.IdProfissionalSaude);
                    if (profissionalSaude == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do profissional de saúde.");

                    var medida = new MedidasAntropometricas(model.Peso, model.Data, model.Altura, model.PerimetroCefalico, model.Imc, paciente, profissionalSaude);
                    _serviceProntuario.SalvarMedidaAntropometrica(medida);
                }

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #endregion

        #region [Atestado]

        [HttpGet]
        [Route("listarAtestados")]
        public HttpResponseMessage ListarAtestados(int id)
        {
            try
            {
                var model = new List<AtestadoViewModel>();
                var consulta = _serviceProntuario.ListarAtestadoByPaciente(id);

                foreach (var item in consulta)
                {
                    model.Add(new AtestadoViewModel()
                    {
                        Descricao = item.Descricao,
                        IdPaciente = item.IdPaciente,
                        Data = item.Data,
                        IdAtestado = item.IdAtestado,
                        IdFuncionario = item.IdFuncionario,
                        NomeFuncionario = item.Funcionario.Pessoa.Nome,
                        NomePaciente = item.Paciente.Pessoa.Nome,
                        Situacao = item.Situacao
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
        [Route("saveAtestado")]
        public HttpResponseMessage SalvarAtestado(AtestadoViewModel model)
        {
            try
            {
                if (model.IdAtestado > 0)
                {
                    var atestado = _serviceProntuario.GetAtestadoById(model.IdAtestado);
                    if (atestado == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do atestado.");

                    var paciente = _serviceCadastro.ObterPacienteById(model.IdPaciente);
                    if (paciente == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do paciente.");

                    var funcionario = _serviceCadastro.ObterFuncionarioById(model.IdFuncionario);
                    if (funcionario == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do funcionário.");

                    atestado.SetData(model.Data);
                    atestado.SetDescricao(model.Descricao);
                    atestado.SetSituacao("Ativo");
                    atestado.SetPaciente(paciente);
                    atestado.SetFuncionario(funcionario);

                    _serviceProntuario.SalvarAtestadoPaciente(atestado);
                }
                else
                {
                    var ppaciente = _serviceCadastro.ObterPacienteById(model.IdPaciente);
                    if (ppaciente == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do paciente.");

                    var funcionario = _serviceCadastro.ObterFuncionarioById(model.IdFuncionario);
                    if (funcionario == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do funcionário.");

                    var atestado = new Atestado(model.Descricao, ppaciente, funcionario);
                    _serviceProntuario.SalvarAtestadoPaciente(atestado);
                }

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public System.Drawing.Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);
            return returnImage;
        }


        [HttpGet]
        [Route("printAtestado")]
        public HttpResponseMessage ImprimirAtestado(int id)
        {
            BaseFont f_default = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            BaseFont f_bold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            BaseFont f_boldC = BaseFont.CreateFont(BaseFont.COURIER_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            var atestado = _serviceProntuario.GetAtestadoById(id);
            if (atestado == null)
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao recuperar dados do atestado.");

            var clinica = _serviceCadastro.ObterClinicaById(base.GetUsuarioLogado().IdClinica);
            var unidade = _serviceCadastro.ObterUnidadeAtendimentoPorId(base.GetUsuarioLogado().IdUnidadeAtendimento);
            if (clinica == null)
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao recuperar dados da clinica.");

            var profissional = _serviceCadastro.ObterFuncionarioById(atestado.IdFuncionario);

            informacoes = " Fones: " + unidade.Telefone1 + " - E-mail: " + unidade.Email + "";
            enderecoClinica = unidade.Logradouro + ", " + unidade.Numero + " B. " + unidade.Bairro + " - " + unidade.Cidade.Nome + " " + unidade.Estado.Nome;

            float hSum = 0;
            var pdf = new Document();
            var stream = new MemoryStream();

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(pdf, stream);
                writer.CloseStream = false;
                pdf.Open();
                PdfContentByte cb = writer.DirectContent;
                float topPos = pdf.PageSize.Height - pdf.TopMargin;
                float leftPos = pdf.LeftMargin;


                writer.PageEvent = new PDFFooter();

                float caixaLogoH = 80f;
                float caixaLogoW = 300f;

                var rect = new iTextSharp.text.Rectangle(leftPos, topPos - caixaLogoH - hSum, caixaLogoW, caixaLogoH);
                rect.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
                rect.BorderWidth = 0;
                rect.BorderColor = new BaseColor(0, 0, 0);
                cb.Rectangle(rect);
                cb.Stroke();

                string formal = "";

                if (profissional.Pessoa != null)
                {
                    formal = profissional.Pessoa.Sexo == "M" ? "Dr" : "Drª";
                }


                cb.BeginText();
                cb.SetFontAndSize(f_boldC, 12);
                cb.SetTextMatrix(leftPos + caixaLogoW + 20f, topPos - hSum - 15f);
                cb.ShowText(formal + " " + profissional.Pessoa.Nome.ToUpper());
                cb.EndText();


                caixaLogoH = 80f;
                caixaLogoW = 250f;

                var rect2 = new iTextSharp.text.Rectangle(leftPos, topPos - caixaLogoH - hSum, caixaLogoW, caixaLogoH);
                rect2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
                rect2.BorderWidth = 0;
                rect2.BorderColor = new BaseColor(0, 0, 0);


                var obj = new float();

                obj = 15f;
                if (clinica.Logo != null)
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(byteArrayToImage(clinica.Logo), System.Drawing.Imaging.ImageFormat.Jpeg);
                    logo.ScalePercent(60f);
                    logo.SetAbsolutePosition(leftPos + 10f, topPos - hSum - 60f);
                    pdf.Add(logo);
                }

                hSum += 110f;
                caixaLogoH = 480f;
                caixaLogoW = 600f;

                string html = "<ul><li class=\"headerdiv\">" + atestado.Descricao + "</li></ul>";

                String cssText = ".headerdiv {margin-top: 150px;} ul {list-style-type: none;} ";

                using (var cssMemoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(cssText)))
                {
                    using (var htmlMemoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(html)))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdf, htmlMemoryStream, cssMemoryStream);
                    }
                }

                pdf.Close();

                byte[] bytes = stream.ToArray();

                var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(bytes) };

                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline")
                {
                    FileName = "Atestado_" +
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
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro. " + ex.Message);
            }
        }


       

        [HttpGet]
        [Route("printAtestado2")]
        public HttpResponseMessage ImprimirAtestado2(int id)
        {
            var atestado = _serviceProntuario.GetAtestadoById(id);
            if (atestado == null)
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao recuperar dados do atestado.");



            //HtmlForm form = new HtmlForm();
            Document document = new Document(PageSize.A4, 30, 30, 30, 30);
            MemoryStream ms = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            var obj = new HTMLWorker(document);

            string corpo = atestado.Descricao;
            string appPath = AppDomain.CurrentDomain.BaseDirectory;

            var clinica = _serviceCadastro.ObterClinicaById(base.GetUsuarioLogado().IdClinica);
            var unidade = _serviceCadastro.ObterUnidadeAtendimentoPorId(base.GetUsuarioLogado().IdUnidadeAtendimento);
            if (clinica == null)
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao recuperar dados da clinica.");


            //MemoryStream msImagem = new MemoryStream(clinica.Logo, 0, clinica.Logo.Length);
            //msImagem.Write(clinica.Logo, 0, clinica.Logo.Length);
            //var returnImage = Image.FromStream(msImagem, true);//Exception occurs here
            //

            string html = " <html> " +
                            "  <body style=\"width: 600px; margin: 0 auto;\">  ";
            if (clinica.Logo != null)
            {
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(byteArrayToImage(clinica.Logo), System.Drawing.Imaging.ImageFormat.Jpeg);
                html = " <img alt='' src=\"" + logo + " /> <br><br>";
            }
            html = " " + corpo + "  " +
              "<br><br><p style='text-align: center; font-size: 10px;'>" + unidade.Logradouro + ", " + unidade.Numero + " B. " + unidade.Bairro + " - " + unidade.Cidade.Nome + " Fones: " + unidade.Telefone1 + "</p>" +
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
                FileName = "Atestado_" +
                           DateTime.Now.ToString("G")
                               .Replace(":", "")
                               .Replace("-", "")
                               .Replace("/", "")
                               .Replace(" ", "") + ".pdf"
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            return response;

        }

        [HttpGet]
        [Route("excluirAtestado")]
        public HttpResponseMessage ExcluirAtestado(int id) // idpaciente
        {
            try
            {
                _serviceProntuario.ExcluirAtestado(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion

        #region [Receituario]

        [HttpGet]
        [Route("listarReceituario")]
        public HttpResponseMessage ListarReceituario(int id) // idpaciente
        {
            try
            {
                var model = new List<ReceituarioViewModel>();
                var consulta = _serviceProntuario.ListarReceituariosByPaciente(id);

                foreach (var item in consulta)
                {
                    model.Add(new ReceituarioViewModel()
                    {
                        Data = item.Data,
                        NomeFuncionario = item.Funcionario.Pessoa.Nome,
                        IdReceituario = item.IdReceituario,
                        Situacao = item.Situacao
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
        [Route("saveReceituario")]
        public HttpResponseMessage SalvarReceituario(ReceituarioViewModel model)
        {
            try
            {
                if (model.IdReceituario > 0)
                {
                    var receituario = _serviceProntuario.ObterReceituarioPorId(model.IdReceituario);
                    if (receituario == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do receituário.");

                    var paciente = _serviceCadastro.ObterPacienteById(model.IdPaciente);
                    if (paciente == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do paciente.");

                    var funcionario = _serviceCadastro.ObterFuncionarioById(model.IdFuncionario);
                    if (funcionario == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do funcionário.");

                    receituario.SetData(model.Data);
                    receituario.SetDescricao(model.Descricao);
                    //receituario.SetSituacao(model.Situacao);
                    receituario.SetPaciente(paciente);
                    receituario.SetFuncionario(funcionario);

                    _serviceProntuario.SalvarReceituario(receituario);
                }
                else
                {
                    var ppaciente = _serviceCadastro.ObterPacienteById(model.IdPaciente);
                    if (ppaciente == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do paciente.");

                    var funcionario = _serviceCadastro.ObterFuncionarioById(model.IdFuncionario);
                    if (funcionario == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do funcionário.");

                    var receituario = new Receituario(model.Descricao, ppaciente, funcionario);

                    receituario.SetDescricao(model.Descricao);


                    var result = _serviceProntuario.SalvarReceituario(receituario);

                    foreach (var item in model.Medicamentos)
                    {
                        receituario.AddMedicamento(new ReceituarioMedicamento(result.IdReceituario, item.IdMedicamento, item.Nome, item.Posologia, item.Quantidade));
                    }
                    _serviceProntuario.SalvarReceituario(receituario);
                }

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("excluirReceituario")]
        public HttpResponseMessage ExcluirReceituario(int id) // idpaciente
        {
            try
            {
                _serviceProntuario.ExcluirReceituario(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getReceituarioById")]
        public HttpResponseMessage ObterReceituarioPorId(int id)
        {
            try
            {
                var model = new ReceituarioViewModel();

                var receituario = _serviceProntuario.ObterReceituarioPorId(id);

                if (receituario == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao recuperar dados do receituário.");

                model.IdFuncionario = receituario.IdFuncionario;
                model.IdPaciente = receituario.IdPaciente;
                model.IdReceituario = receituario.IdReceituario;
                model.NomeFuncionario = receituario.Funcionario == null ? "" : receituario.Funcionario.Pessoa.Nome;
                model.NomePaciente = receituario.Paciente == null ? "" : receituario.Paciente.Pessoa.Nome;
                model.Situacao = receituario.Situacao;
                model.Medicamentos = RetornaViewMedicamentos(receituario.IdReceituario);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private List<ReceituarioMedicamentoViewModel> RetornaViewMedicamentos(int id)
        {
            var model = new List<ReceituarioMedicamentoViewModel>();

            var medicamentos = _serviceProntuario.ObterMedicamentosReceituario(id);

            foreach (var item in medicamentos)
            {
                model.Add(new ReceituarioMedicamentoViewModel
                {
                    IdMedicamento = item.IdMedicamento,
                    IdReceituario = item.IdReceituario,
                    Nome = item.Nome,
                    Posologia = item.Posologia,
                    Quantidade = item.Quantidade
                });
            }

            return model;
        }

        #endregion


        #region [Odontograma]

        [HttpGet]
        [Route("printOrcamento")]
        public HttpResponseMessage ImprimirOrcamentoOrganograma(int id)
        {
            BaseFont f_default = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            BaseFont f_bold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            BaseFont f_boldC = BaseFont.CreateFont(BaseFont.COURIER_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            var odonto = _serviceProntuario.ListarOdontogramaPorIdPaciente(id).ToList();
            if (odonto.Count == 0)
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao recuperar dados do atestado.");

            var clinica = _serviceCadastro.ObterClinicaById(base.GetUsuarioLogado().IdClinica);
            var unidade = _serviceCadastro.ObterUnidadeAtendimentoPorId(base.GetUsuarioLogado().IdUnidadeAtendimento);
            if (clinica == null)
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao recuperar dados da clinica.");

            var profissional = _serviceCadastro.ObterFuncionarioById(odonto[0].IdFuncionario);

            informacoes = " Fones: " + unidade.Telefone1 + " - E-mail: " + unidade.Email + "";
            enderecoClinica = unidade.Logradouro + ", " + unidade.Numero + " B. " + unidade.Bairro + " - " + unidade.Cidade.Nome + " " + unidade.Estado.Nome;

            float hSum = 0;
            var pdf = new Document();
            //pdf.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
            var stream = new MemoryStream();

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(pdf, stream);
                writer.CloseStream = false;
               
                pdf.Open();
                PdfContentByte cb = writer.DirectContent;
                float topPos = pdf.PageSize.Height - pdf.TopMargin;
                float leftPos = pdf.LeftMargin;


                writer.PageEvent = new PDFFooter();

                float caixaLogoH = 80f;
                float caixaLogoW = 300f;

                var rect = new iTextSharp.text.Rectangle(leftPos, topPos - caixaLogoH - hSum, caixaLogoW, caixaLogoH);
                rect.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
                rect.BorderWidth = 0;
                rect.BorderColor = new BaseColor(0, 0, 0);
                cb.Rectangle(rect);
                cb.Stroke();

                string formal = "";

                if (profissional.Pessoa != null)
                {
                    formal = profissional.Pessoa.Sexo == "M" ? "Dr" : "Drª";
                }


                cb.BeginText();
                cb.SetFontAndSize(f_boldC, 12);
                cb.SetTextMatrix(leftPos + caixaLogoW + 20f, topPos - hSum - 15f);
                cb.ShowText(formal + " " + profissional.Pessoa.Nome.ToUpper());
                cb.EndText();

                cb.BeginText();
                cb.SetFontAndSize(f_boldC, 14);
                cb.SetTextMatrix(leftPos + caixaLogoW + 20f, topPos - hSum - 30f);
                cb.ShowText(odonto[0].Paciente.Pessoa.Nome.ToUpper());
                cb.EndText();

                cb.BeginText();
                cb.SetFontAndSize(f_boldC, 12);
                cb.SetTextMatrix(leftPos + caixaLogoW + 20f, topPos - hSum - 42f);
                cb.ShowText("Data de Nascimento: " + Convert.ToDateTime(odonto[0].Paciente.Pessoa.DataNascimento).ToShortDateString());
                cb.EndText();

                cb.BeginText();
                cb.SetFontAndSize(f_boldC, 12);
                cb.SetTextMatrix(leftPos + caixaLogoW + 20f, topPos - hSum - 52f);
                cb.ShowText("Mãe: " + odonto[0].Paciente.Pessoa.Mae);
                cb.EndText();




                caixaLogoH = 80f;
                caixaLogoW = 250f;

                var rect2 = new iTextSharp.text.Rectangle(leftPos, topPos - caixaLogoH - hSum, caixaLogoW, caixaLogoH);
                rect2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
                rect2.BorderWidth = 0;
                rect2.BorderColor = new BaseColor(0, 0, 0);


                var obj = new float();

                obj = 15f;
                if (clinica.Logo != null)
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(byteArrayToImage(clinica.Logo), System.Drawing.Imaging.ImageFormat.Jpeg);
                    logo.ScalePercent(60f);
                    logo.SetAbsolutePosition(leftPos + 10f, topPos - hSum - 60f);
                    pdf.Add(logo);
                }
                caixaLogoH = 480f;
                caixaLogoW = 600f;

                iTextSharp.text.Image logo2 = iTextSharp.text.Image.GetInstance(AppDomain.CurrentDomain.BaseDirectory + "\\imagens\\odontograma.png");
                logo2.ScalePercent(65f);
                logo2.SetAbsolutePosition(leftPos + 20f, topPos - hSum - 330f);
                pdf.Add(logo2);

                string tabela = @"<table class='tabler'> <tbody>
                                    <tr class='trtabela'>
                                    <th>Dente</th>
                                    <th>Face</th>
                                    <th>Inicio</th>
                                    <th>Término</th>
                                    <th>Descrição</th>
                                    <th>Valor</th>
                                    </tr> 
                                    ";
                decimal soma = 0;
                foreach (var item in odonto)
                {
                    tabela += "<tr>";
                    tabela += "<td>" + item.Dente + "</td>";
                    tabela += "<td>" + item.Face + "</td>";
                    tabela += "<td>" + item.DataInicio.ToShortDateString() + "</td>";
                    tabela += "<td>" + item.DataTermino.ToShortDateString() + "</td>";
                    tabela += "<td>" + item.Descricao + "</td>";
                    tabela += "<td>" + item.Valor + "</td>";
                    tabela += "</tr>";
                    soma = soma + Convert.ToDecimal(item.Valor);
                }
               

                tabela += "<tr><td colspan='5'>Total</td><td>" + soma + "</td></tr>";

                tabela += "</tbody></table>";

                string html = "<ul><li class=\"headerdiv\">" + tabela + "</li></ul>";

                String cssText = ".tabler {width: 100%;} .tabler,.tabler th,.tabler td {border: none;  text-align: left;  padding: 4px;} .headerdiv {margin-top: 450px;} ul {list-style-type: none;} th {background-color: #CCC;color: #000;}";

                using (var cssMemoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(cssText)))
                {
                    using (var htmlMemoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(html)))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdf, htmlMemoryStream, cssMemoryStream);
                    }
                }

                pdf.Close();

                byte[] bytes = stream.ToArray();

                var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(bytes) };

                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline")
                {
                    FileName = "Orcamento_" +
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
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro. " + ex.Message);
            }
        }

        [HttpGet]
        [Route("obterOdontogramaPorId")]
        public HttpResponseMessage ObterOdontogramaPorId(int id) // idodontograma
        {
            try
            {
                var consulta = new OdontogramaViewModel();
                var item = _serviceProntuario.ObterOdontogramaPorId(id);

                consulta.DataInicio = item.DataInicio;
                consulta.DataTermino = item.DataTermino;
                consulta.Descricao = item.Descricao;
                consulta.NmFuncionario = item.Funcionario.Pessoa.Nome;
                consulta.NmPaciente = item.Paciente.Pessoa.Nome;
                consulta.Dente = item.Dente;
                consulta.IdOdontograma = item.IdOdontograma;
                consulta.IdPaciente = item.IdPaciente;
                consulta.Situacao = item.Situacao;
                consulta.Face = item.Face;
                consulta.Valor = item.Valor;

                return Request.CreateResponse(HttpStatusCode.OK, item);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("listarOdontogramaPorIdPaciente")]
        public HttpResponseMessage ListarOdontogramaPorIdPaciente(int id) // idpaciente
        {
            try
            {
                var model = new List<OdontogramaViewModel>();
                var consulta = _serviceProntuario.ListarOdontogramaPorIdPaciente(id);

                foreach (var item in consulta)
                {
                    model.Add(new OdontogramaViewModel()
                    {
                        DataInicio = item.DataInicio,
                        DataTermino = item.DataTermino,
                        Descricao = item.Descricao,
                        NmFuncionario = item.Funcionario.Pessoa.Nome,
                        NmPaciente = item.Paciente.Pessoa.Nome,
                        Dente = item.Dente,
                        IdOdontograma = item.IdOdontograma,
                        IdPaciente = item.IdPaciente,
                        Face = item.Face,
                        Valor = item.Valor,
                        Situacao = item.Situacao
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
        [Route("salvarOdontograma")]
        public HttpResponseMessage SalvarOdontograma(OdontogramaViewModel model)
        {
            try
            {
                if (model.IdOdontograma > 0)
                {
                    var obj = _serviceProntuario.ObterOdontogramaPorId(model.IdOdontograma);

                    obj.SetPeriodo(model.DataInicio, model.DataTermino);
                    obj.SetDente(model.Dente);
                    obj.SetSituacao(model.Situacao);
                    obj.SetPaciente(_serviceCadastro.ObterPacienteById(model.IdPaciente));
                    obj.SetFuncionario(_serviceCadastro.ObterFuncionarioById(model.IdFuncionario));
                    obj.SetDescricao(model.Descricao);
                    obj.SetFace(model.Face);
                    obj.SetValor(model.Valor);

                    _serviceProntuario.SalvarOdontograma(obj);
                }
                else
                {
                    _serviceProntuario.SalvarOdontograma(new Odontograma(model.DataInicio, model.DataTermino, _serviceCadastro.ObterFuncionarioById(model.IdFuncionario), _serviceCadastro.ObterPacienteById(model.IdPaciente), model.Situacao, model.Descricao, model.Dente, model.Valor, model.Face));
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("excluirOdontograma")]
        public HttpResponseMessage ExcluirOdontograma(int id)
        {
            try
            {
                _serviceProntuario.ExcluirOdontograma(_serviceProntuario.ObterOdontogramaPorId(id));
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion

        //#region [Atestado]

        //[HttpGet]
        //[Route("printAtestado/{id:int}")]
        //public HttpResponseMessage ImprimirAtestado(int id)
        //{
        //    BaseFont f_default = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        //    BaseFont f_bold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        //    BaseFont f_boldC = BaseFont.CreateFont(BaseFont.COURIER_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

        //    var atestado = _service.GetAtestadoById(id);
        //    if (atestado == null)
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao recuperar dados do atestado.");


        //    //busca dados do profissional
        //    var profissional = _serviceFuncionario.ObterFuncionariPorId(atestado.IdFuncionario);
        //    var especialidades = _serviceFuncionario.ListarEspecialidadesFuncionario(atestado.IdFuncionario);
        //    //seta as informações do profissional
        //    informacoes = "Tels.: (31) 3441-6784 / " + profissional.Pessoa.Telefone1 + " - E-mail: " + profissional.Pessoa.Email + "";

        //    float hSum = 0;
        //    var pdf = new Document();
        //    var stream = new MemoryStream();



        //    try
        //    {
        //        PdfWriter writer = PdfWriter.GetInstance(pdf, stream);
        //        writer.CloseStream = false;
        //        pdf.Open();
        //        PdfContentByte cb = writer.DirectContent;
        //        float topPos = pdf.PageSize.Height - pdf.TopMargin;
        //        float leftPos = pdf.LeftMargin;


        //        writer.PageEvent = new PDFFooter();

        //        float caixaLogoH = 80f;
        //        float caixaLogoW = 500f;

        //        var rect = new iTextSharp.text.Rectangle(leftPos, topPos - caixaLogoH - hSum, caixaLogoW, caixaLogoH);
        //        rect.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
        //        rect.BorderWidth = 0;
        //        rect.BorderColor = new BaseColor(0, 0, 0);
        //        cb.Rectangle(rect);
        //        cb.Stroke();


        //        caixaLogoH = 80f;
        //        caixaLogoW = 250f;

        //        var rect2 = new iTextSharp.text.Rectangle(leftPos, topPos - caixaLogoH - hSum, caixaLogoW, caixaLogoH);
        //        rect2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
        //        rect2.BorderWidth = 0;
        //        rect2.BorderColor = new BaseColor(0, 0, 0);

        //        cb.Rectangle(rect2);
        //        cb.Stroke();

        //        cb.BeginText();
        //        cb.SetFontAndSize(f_boldC, 12);
        //        cb.SetTextMatrix(leftPos + caixaLogoW + 20f, topPos - hSum - 15f);
        //        cb.ShowText("Drª " + profissional.Pessoa.Nome.ToUpper());

        //        cb.SetFontAndSize(f_default, 9);
        //        cb.SetTextMatrix(leftPos + caixaLogoW + 20f, topPos - hSum - 30f);
        //        cb.ShowText(profissional.RegistroConselho);

        //        var obj = new float();

        //        obj = 15f;

        //        int i = 0;
        //        foreach (var item in especialidades)
        //        {
        //            if (i == 0)
        //            {
        //                if (especialidades.Count == 1)
        //                    cb.SetTextMatrix(leftPos + caixaLogoW + 20f, topPos - hSum - 45f);
        //                else
        //                    cb.SetTextMatrix(leftPos + caixaLogoW + 20f, topPos - hSum - 60f);

        //                cb.ShowText(item.NmEspecialidade.ToUpper());
        //                i++;
        //            }
        //            else
        //            {
        //                cb.SetTextMatrix(leftPos + caixaLogoW + 20f, topPos - hSum - 60f + obj);
        //                cb.ShowText(item.NmEspecialidade.ToUpper());
        //                i++;
        //                obj = obj + 15f;
        //            }
        //        }
        //        //cb.SetTextMatrix(leftPos + caixaLogoW + 20f, topPos - hSum - 60f);
        //        //cb.ShowText("Neonatologista");
        //        cb.EndText();

        //        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(AppDomain.CurrentDomain.BaseDirectory + "\\imagens\\Logomarca_CIP.jpg");
        //        logo.ScalePercent(60f);
        //        logo.SetAbsolutePosition(leftPos + 10f, topPos - hSum - 60f);
        //        pdf.Add(logo);

        //        hSum += 110f;
        //        caixaLogoH = 480f;
        //        caixaLogoW = 600f;

        //        //tive que fazer isso para dar uma margem no pdf
        //        string html = "<ul><li class=\"headerdiv\">" + atestado.Descricao + "</li></ul>";

        //        String cssText = ".headerdiv {margin-top: 150px;} ul {list-style-type: none;} ";

        //        using (var cssMemoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(cssText)))
        //        {
        //            using (var htmlMemoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(html)))
        //            {
        //                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdf, htmlMemoryStream, cssMemoryStream);
        //            }
        //        }

        //        pdf.Close();

        //        byte[] bytes = stream.ToArray();

        //        var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(bytes) };

        //        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline")
        //        {
        //            FileName = "CIP_Atestado_" +
        //                       DateTime.Now.ToString("G")
        //                           .Replace(":", "")
        //                           .Replace("-", "")
        //                           .Replace("/", "")
        //                           .Replace(" ", "") + ".pdf"
        //        };
        //        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro.");
        //    }

        //}

        public class PDFFooter : PdfPageEventHelper
        {
            // write on top of document
            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                base.OnOpenDocument(writer, document);
                PdfPTable tabFot = new PdfPTable(new float[] { 1F });
                tabFot.SpacingAfter = 10F;
                PdfPCell cell;
                tabFot.TotalWidth = 300F;
                cell = new PdfPCell(new Phrase(""));
                cell.BorderWidth = 0;
                tabFot.AddCell(cell);
                tabFot.WriteSelectedRows(0, -1, 150, document.Top, writer.DirectContent);
            }

            // write on start of each page
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                base.OnStartPage(writer, document);
            }

            // write on end of each page
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                BaseFont f_default = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                BaseFont f_bold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font fontH1 = new Font(Font.FontFamily.COURIER, 10, Font.NORMAL);

                base.OnEndPage(writer, document);
                PdfPTable tabFot = new PdfPTable(new float[] { 1F });
                tabFot.DefaultCell.Border = Rectangle.NO_BORDER;
                tabFot.DefaultCell.BorderWidth = 0;
                PdfPCell cell;
                tabFot.TotalWidth = 530F;
                cell = new PdfPCell(new Phrase(enderecoClinica, fontH1));
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.BorderWidth = 0;
                cell.BorderWidthTop = 1;
                tabFot.AddCell(cell);
                PdfPCell cell2;
                //cell2 = new PdfPCell(new Phrase("Tels.: (31) 3441-6784 / 999845236 - E-mail: symarapimentel@hotmail.com", fontH1));
                cell2 = new PdfPCell(new Phrase(informacoes, fontH1));


                cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2.BorderWidth = 0;
                tabFot.AddCell(cell2);
                tabFot.WriteSelectedRows(0, -20, 30, document.Bottom, writer.DirectContent);
            }

            //write on close of document
            public override void OnCloseDocument(PdfWriter writer, Document document)
            {
                base.OnCloseDocument(writer, document);
            }
        }
        public static void parseHTML(Document doc, string html)
        {
            HTMLWorker htmlWorker = new HTMLWorker(doc);
            htmlWorker.Parse(new StringReader(html));
        }

        [HttpGet]
        [Route("printReceituario")]
        public HttpResponseMessage ObterBytesReceituario(int id)
        {
            BaseFont f_default = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            BaseFont f_bold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            BaseFont f_boldC = BaseFont.CreateFont(BaseFont.COURIER_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

            var receita = _serviceProntuario.ObterReceituarioPorId(id);
            if (receita == null)
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao recuperar dados da receita.");

            var clinica = _serviceCadastro.ObterClinicaById(base.GetUsuarioLogado().IdClinica);
            var unidade = _serviceCadastro.ObterUnidadeAtendimentoPorId(base.GetUsuarioLogado().IdUnidadeAtendimento);
            if (clinica == null)
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao recuperar dados da clinica.");

            var profissional = _serviceCadastro.ObterFuncionarioById(receita.IdFuncionario);

            var especialidades = _serviceCadastro.ListarEspecialidadesPorProfissionalSaude(receita.IdFuncionario);

            informacoes = " Fones: " + unidade.Telefone1 + " - E-mail: " + unidade.Email + "";
            enderecoClinica = unidade.Logradouro + ", " + unidade.Numero + " B. " + unidade.Bairro + " - " + unidade.Cidade.Nome + " " + unidade.Estado.Nome;

            float hSum = 0;
            var pdf = new Document();
            var stream = new MemoryStream();

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(pdf, stream);
                writer.CloseStream = false;
                pdf.Open();
                PdfContentByte cb = writer.DirectContent;
                float topPos = pdf.PageSize.Height - pdf.TopMargin;
                float leftPos = pdf.LeftMargin;


                writer.PageEvent = new PDFFooter();

                float caixaLogoH = 80f;
                float caixaLogoW = 500f;

                var rect = new iTextSharp.text.Rectangle(leftPos, topPos - caixaLogoH - hSum, caixaLogoW, caixaLogoH);
                rect.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
                rect.BorderWidth = 0;
                rect.BorderColor = new BaseColor(0, 0, 0);
                cb.Rectangle(rect);
                cb.Stroke();


                caixaLogoH = 80f;
                caixaLogoW = 250f;

                var rect2 = new iTextSharp.text.Rectangle(leftPos, topPos - caixaLogoH - hSum, caixaLogoW, caixaLogoH);
                rect2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
                rect2.BorderWidth = 0;
                rect2.BorderColor = new BaseColor(0, 0, 0);

                cb.Rectangle(rect2);
                cb.Stroke();
                float caixaHeaderH = 20f;

                string conselho = "";

                if (!String.IsNullOrEmpty(profissional.RegistroConselho))
                    conselho = profissional.RegistroConselho;

                string formal = "";

                if (profissional.Pessoa != null)
                {
                    formal = profissional.Pessoa.Sexo == "M" ? "Dr" : "Drª";
                }

                hSum += caixaHeaderH;
                cb.BeginText();
                cb.SetFontAndSize(f_boldC, 12);
                cb.SetTextMatrix(leftPos + caixaLogoW + 20f, topPos - hSum - 15f);
                cb.ShowText(formal + " " + profissional.Pessoa.Nome.ToUpper());
                cb.EndText();
                cb.BeginText();
                cb.SetFontAndSize(f_default, 9);
                cb.SetTextMatrix(leftPos + caixaLogoW + 20f, topPos - hSum - 30f);
                cb.ShowText(conselho);
                cb.EndText();

                var obj = new float();
                var obj2 = new float();

                obj = 15f;

                obj2 = 45f;

                //int i = 0;
                foreach (var item in especialidades)
                {

                    cb.BeginText();
                    cb.SetFontAndSize(f_default, 9);
                    cb.SetTextMatrix(leftPos + caixaLogoW + 20f, topPos - hSum - obj2);
                    cb.ShowText(item.NmEspecialidade.ToUpper());
                    cb.EndText();

                    obj2 += 15f;

                    //if (i == 0)
                    //{
                    //    if (especialidades.Count == 1)
                    //        cb.SetTextMatrix(leftPos + caixaLogoW + 20f, topPos - hSum - 45f);
                    //    else
                    //        cb.SetTextMatrix(leftPos + caixaLogoW + 20f, topPos - hSum - 60f);

                    //    cb.ShowText(item.NmEspecialidade.ToUpper());
                    //    i++;
                    //}
                    //else
                    //{
                    //    cb.SetTextMatrix(leftPos + caixaLogoW + 20f, topPos - hSum - 60f + obj);
                    //    cb.ShowText(item.NmEspecialidade.ToUpper());
                    //    i++;
                    //    obj = obj + 15f;
                    //}
                }
                //cb.EndText();

                if (clinica.Logo != null)
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(byteArrayToImage(clinica.Logo), System.Drawing.Imaging.ImageFormat.Jpeg);
                    logo.ScalePercent(60f);
                    logo.SetAbsolutePosition(leftPos + 10f, topPos - hSum - 60f);
                    pdf.Add(logo);
                }

                //var medicamentos = _serviceProntuario.ObterMedicamentosReceituario(receita.IdReceituario);
                //hSum += 40f;
                //caixaLogoH = 60f;
                //caixaLogoW = 10f;
                //int j = 1;
                //foreach (var item in medicamentos)
                //{
                //    cb.BeginText();
                //    cb.SetFontAndSize(f_default, 9);
                //    cb.SetTextMatrix(leftPos + caixaLogoW + 20f, topPos - hSum - obj2);
                //    cb.ShowText(j.ToString() + ")   " + item.Nome + "    -    " + item.Quantidade.ToString() + " unidade(s)");
                //    cb.EndText();
                //    obj2 += 15f;
                //    cb.BeginText();
                //    cb.SetFontAndSize(f_default, 9);
                //    cb.SetTextMatrix(leftPos + caixaLogoW + 20f, topPos - hSum - obj2);
                //    cb.ShowText("       " + item.Posologia);
                //    cb.EndText();
                //    obj2 += 15f;
                //    j++;
                //}

                hSum += 110f;
                caixaLogoH = 480f;
                caixaLogoW = 600f;


                //pego a string e faço um replace no tamanho das divs

                string desc = receita.Descricao.Replace("85%", "60%").Replace("15%", "40%");


                string html = "<ul><li class=\"headerdiv\">" + desc + "</li></ul>";

                String cssText = ".headerdiv {margin-top: 230px;} ul {list-style-type: none;} ";

                using (var cssMemoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(cssText)))
                {
                    using (var htmlMemoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(html)))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdf, htmlMemoryStream, cssMemoryStream);
                    }
                }



                pdf.Close();

                byte[] bytes = stream.ToArray();

                var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(bytes) };

                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline")
                {
                    FileName = "Receita_" +
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
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro. " + ex.Message);
            }
        }

        [HttpGet]
        [Route("printReceituario/{id:int}")]
        public HttpResponseMessage ImprimirReceita(int id)
        {
            var receita = _serviceProntuario.ObterReceituarioPorId(id);
            if (receita == null)
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao recuperar dados da receita.");

            //HtmlForm form = new HtmlForm();
            Document document = new Document(PageSize.A4, 20, 20, 20, 20);
            MemoryStream ms = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, ms);
            var obj = new HTMLWorker(document);

            string corpo = receita.Descricao;
            string appPath = AppDomain.CurrentDomain.BaseDirectory;

            string html = " <html> " +
                            "  <body style=\"width: 600px; margin: 0 auto;\">  " +
                                    " <table style=\"width:100%;\">" +
                                         " <tr>" +
                                            " <td style=\"width:49%; text-align:left\">" +
                                                "  <img alt='' src=\"" + appPath + "/imagens/logomarca_atestado.jpg\"/>  " +
                                            " </td>" +
                                            " <td style=\"width:50%; text-align:left\">" +
                                                " <p style=\"font-family: cursive; font-size: 16px;line-height: 1;\">Dr(a) Marina Rabelo Benfica</p> " +
                                                 " <p>CRMMG - 45784</p> " +
                                                " <p>Pediatra</p>   " +
                                                " <p>Neonatologista</p>    " +
                                            " </td>" +
                                        " </tr>" +
                                    " </table>" +
                                     " <br> " +
                                    " " + corpo + "  " +
                                    "<br><br><p style='text-align: center; font-size: 10px;'>Av. Coronel José Dias Bicalho, 659 - B. São José / Pampulha - BH - Fones: (31) 2527-0670 e 3441-6784</p>" +
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
                FileName = "CIP_Receita_" +
                           DateTime.Now.ToString("G")
                               .Replace(":", "")
                               .Replace("-", "")
                               .Replace("/", "")
                               .Replace(" ", "") + ".pdf"
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

            return response;

        }

        #region [Hospital]
        [HttpGet]
        [Route("getHospitalById")]
        public HttpResponseMessage ObterHospitalPorId(int id)
        {
            try
            {
                var model = new HospitalViewModel();

                var hospital = _serviceProntuario.ObterHospitalPorId(id);

                if (hospital == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao recuperar dados do hospital.");

                Mapper.CreateMap<Hospital, HospitalViewModel>();
                model = Mapper.Map<Hospital, HospitalViewModel>(hospital);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("listarHospitaisPorNome")]
        public HttpResponseMessage ListarHospitaisPorNome(string nome)
        {
            try
            {
                var model = new List<HospitalViewModel>();

                var hospitais = _serviceProntuario.ListarHospitaisPorNome(nome);

                if (hospitais.Count > 0)
                {
                    Mapper.CreateMap<Hospital, HospitalViewModel>();
                    model = Mapper.Map<List<Hospital>, List<HospitalViewModel>>(hospitais);
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getHospitais")]
        public HttpResponseMessage ObterHospitais()
        {
            try
            {
                var model = new List<HospitalViewModel>();
                var hospitais = _serviceProntuario.ObterHospitais();

                foreach (var x in hospitais)
                {
                    model.Add(new HospitalViewModel()
                    {
                        IdHospital = x.IdHospital,
                        Nome = x.Nome
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
        [Route("saveHospital")]
        public HttpResponseMessage SalvarHospital(HospitalViewModel model)
        {
            try
            {
                if (model.IdHospital > 0)
                {
                    var hospital = _serviceProntuario.ObterHospitalPorId(model.IdHospital);
                    if (hospital == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do hospital.");


                    hospital.SetNome(model.Nome);

                    _serviceProntuario.SalvarHospital(hospital);
                }
                else
                {
                    var hospital = new Hospital(model.Nome);
                    _serviceProntuario.SalvarHospital(hospital);
                }

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion

        //[HttpGet]
        //[Route("getAtestadoById")]
        //public HttpResponseMessage ObterAtestadoPorId(int id)
        //{
        //    try
        //    {
        //        var model = new AtestadoViewModel();

        //        var atestado = _service.GetAtestadoById(id);

        //        if (atestado == null)
        //            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao recuperar dados do atestado.");

        //        Mapper.CreateMap<Atestado, AtestadoViewModel>();
        //        Mapper.CreateMap<Pessoa, PacienteViewModel>();
        //        Mapper.CreateMap<Funcionario, FuncionarioViewModel>();
        //        model = Mapper.Map<Atestado, AtestadoViewModel>(atestado);

        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpGet]
        //[Route("getAtestadosByPaciente")]
        //public HttpResponseMessage ListarAtestadoByPaciente(int id)
        //{
        //    try
        //    {
        //        var model = new List<AtestadoViewModel>();

        //        var atestado = _service.ListarAtestadoByPaciente(id);

        //        if (atestado.Count > 0)
        //        {
        //            Mapper.CreateMap<Atestado, AtestadoViewModel>()
        //                .ForMember(x => x.NomeFuncionario, y => y.MapFrom(z => z.Funcionario.Pessoa.Nome))
        //                .ForMember(x => x.NomePaciente, y => y.MapFrom(z => z.Paciente.Pessoa.Nome));
        //            model = Mapper.Map<List<Atestado>, List<AtestadoViewModel>>(atestado);

        //        }
        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}


        //[HttpGet]
        //[Route("getModelosAtestados")]
        //public HttpResponseMessage ListarModelosAtestadosAtivos()
        //{
        //    try
        //    {
        //        var model = new List<ModeloProntuarioViewModel>();

        //        var modelos = _service.ListarModelosAtestadosAtivos();

        //        if (modelos.Count > 0)
        //        {
        //            Mapper.CreateMap<ModeloProntuario, ModeloProntuarioViewModel>();
        //            model = Mapper.Map<List<ModeloProntuario>, List<ModeloProntuarioViewModel>>(modelos);
        //        }
        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}


        //[HttpPost]
        //[Route("saveAtestado")]
        //public HttpResponseMessage SalvarAtestado(AtestadoViewModel model)
        //{
        //    try
        //    {
        //        if (model.IdAtestado > 0)
        //        {
        //            var atestado = _service.GetAtestadoById(model.IdAtestado);
        //            if (atestado == null)
        //                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do atestado.");

        //            var paciente = _servicePaciente.ObterPacientePorId(model.IdPaciente);
        //            if (paciente == null)
        //                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do paciente.");

        //            var funcionario = _serviceFuncionario.ObterFuncionariPorId(model.IdFuncionario);
        //            if (funcionario == null)
        //                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do funcionário.");

        //            atestado.SetData(model.Data);
        //            atestado.SetDescricao(model.Descricao);
        //            string situacao = model.Situacao == "A" ? "Ativo" : "Excluido";
        //            atestado.SetSituacao(situacao);
        //            atestado.SetPaciente(paciente);
        //            atestado.SetFuncionario(funcionario);

        //            _service.SalvarAtestadoPaciente(atestado);
        //        }
        //        else
        //        {
        //            var ppaciente = _servicePaciente.ObterPacientePorId(model.IdPaciente);
        //            if (ppaciente == null)
        //                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do paciente.");

        //            var funcionario = _serviceFuncionario.ObterFuncionariPorId(model.IdFuncionario);
        //            if (funcionario == null)
        //                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do funcionário.");

        //            var atestado = new Atestado(model.Descricao, ppaciente, funcionario);
        //            _service.SalvarAtestadoPaciente(atestado);
        //        }

        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}
        //#endregion






        //#region [Medidas Antropometricas]

        //[HttpGet]
        //[Route("excluirMedidas")]
        //public HttpResponseMessage ExcluirMedidas(int id)
        //{
        //    try
        //    {
        //        var medidas = _service.ObterMedidasPorId(id);
        //        _service.ExcluirMedidas(id);

        //        return Request.CreateResponse(HttpStatusCode.OK);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}


        //[HttpGet]
        //[Route("getMedidasById")]
        //public HttpResponseMessage ObterMedidasPorId(int id)
        //{
        //    try
        //    {
        //        var model = new MedidasAntropometricasViewModel();

        //        var medidas = _service.ObterMedidasPorId(id);

        //        if (medidas == null)
        //            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao recuperar dados da medida antropométrica.");

        //        Mapper.CreateMap<MedidasAntropometricas, MedidasAntropometricasViewModel>();
        //        model = Mapper.Map<MedidasAntropometricas, MedidasAntropometricasViewModel>(medidas);

        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpGet]
        //[Route("getMedidasByPaciente")]
        //public HttpResponseMessage ObterMedidasPorPaciente(int id)
        //{
        //    try
        //    {
        //        var model = new List<MedidasAntropometricasViewModel>();

        //        var medidas = _service.ObterMedidasPorPaciente(id);

        //        if (medidas.Count > 0)
        //        {
        //            Mapper.CreateMap<MedidasAntropometricas, MedidasAntropometricasViewModel>();
        //            model = Mapper.Map<List<MedidasAntropometricas>, List<MedidasAntropometricasViewModel>>(medidas);
        //        }
        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}


        //[HttpPost]
        //[Route("saveMedidas")]
        //public HttpResponseMessage SalvarMedidaAntropometrica(MedidasAntropometricasViewModel model)
        //{
        //    try
        //    {
        //        if (model.IdMedida > 0)
        //        {
        //            var medida = _service.ObterMedidasPorId(model.IdMedida);
        //            if (medida == null)
        //                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados da medida antropmétrica.");


        //            medida.SetAltura(model.Altura);
        //            medida.SetImc(model.Imc);
        //            medida.SetPeso(model.Peso);
        //            medida.SetData(model.Data);
        //            medida.SetPerimetroCefalico(model.PerimetroCefalico);

        //            _service.SalvarMedidaAntropometrica(medida);
        //        }
        //        else
        //        {
        //            var paciente = _servicePaciente.ObterPacientePorId(model.IdPaciente);
        //            if (paciente == null)
        //                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do paciente.");

        //            var medida = new MedidasAntropometricas(model.Peso, model.Data, model.Altura, model.PerimetroCefalico, model.Imc, paciente);
        //            _service.SalvarMedidaAntropometrica(medida);
        //        }

        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //#endregion

        //#region [Receituário]
        //[HttpGet]
        //[Route("getReceituarioById")]
        //public HttpResponseMessage ObterReceituarioPorId(int id)
        //{
        //    try
        //    {
        //        var model = new ReceituarioViewModel();

        //        var receituario = _service.ObterReceituarioPorId(id);

        //        if (receituario == null)
        //            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao recuperar dados do receituário.");

        //        Mapper.CreateMap<Receituario, ReceituarioViewModel>();
        //        Mapper.CreateMap<Pessoa, PacienteViewModel>();
        //        Mapper.CreateMap<Funcionario, FuncionarioViewModel>();
        //        model = Mapper.Map<Receituario, ReceituarioViewModel>(receituario);

        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpGet]
        //[Route("getReceituarioByIdPaciente")]
        //public HttpResponseMessage ListarReceituariosByPaciente(int id)
        //{
        //    try
        //    {
        //        var model = new List<ReceituarioViewModel>();

        //        var receituario = _service.ListarReceituariosByPaciente(id);

        //        if (receituario.Count > 0)
        //        {
        //            Mapper.CreateMap<Receituario, ReceituarioViewModel>()
        //                .ForMember(x => x.NomeFuncionario, y => y.MapFrom(z => z.Funcionario.Pessoa.Nome))
        //                .ForMember(x => x.NomePaciente, y => y.MapFrom(z => z.Paciente.Pessoa.Nome));
        //            model = Mapper.Map<List<Receituario>, List<ReceituarioViewModel>>(receituario);

        //        }
        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}



        //#endregion

        //#region [Cadastro de Vacinas]
        //[HttpGet]
        //[Route("getVacinaById")]
        //public HttpResponseMessage ObterVacinaPorId(int id)
        //{
        //    try
        //    {
        //        var model = new VacinaViewModel();

        //        var vacina = _serviceProntuario.ObterVacinaPorId(id);

        //        if (vacina == null)
        //            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao recuperar dados da vacina.");

        //        Mapper.CreateMap<Vacina, VacinaViewModel>();
        //        model = Mapper.Map<Vacina, VacinaViewModel>(vacina);

        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpGet]
        //[Route("getVacinas")]
        //public HttpResponseMessage ListarVacinas()
        //{
        //    try
        //    {
        //        var model = new List<VacinaViewModel>();

        //        var vacinas = _serviceProntuario.ListarVacinas();

        //        if (vacinas.Count > 0)
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
        //[Route("saveVacina")]
        //public HttpResponseMessage Salvarvacina(VacinaViewModel model)
        //{
        //    try
        //    {
        //        if (model.IdVacina > 0)
        //        {
        //            var vacina = _serviceProntuario.ObterVacinaPorId(model.IdVacina);
        //            if (vacina == null)
        //                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados da vacina.");


        //            vacina.SetDescricao(model.Descricao);
        //            string situacao = model.Situacao == "A" ? "Ativo" : "Inativo";
        //            vacina.SetSituacao(situacao);

        //            _serviceProntuario.SalvarVacina(vacina);
        //        }
        //        else
        //        {
        //            string situacao = model.Situacao == "A" ? "Ativo" : "Inativo";
        //            var vacina = new Vacina(model.Descricao, situacao);
        //            _serviceProntuario.SalvarVacina(vacina);
        //        }

        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //#endregion

        //#region [Requisições de Exames]

        //[HttpGet]
        //[Route("printRequisicao/{id:int}")]
        //public HttpResponseMessage ImprimirPedidoExame(int id)
        //{
        //    BaseFont f_default = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        //    BaseFont f_bold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        //    BaseFont f_boldC = BaseFont.CreateFont(BaseFont.COURIER_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
        //    Font fontH1 = new Font(Font.FontFamily.COURIER, 10, Font.NORMAL);
        //    Font fontHMedico = new Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL);
        //    Font fontDiagnostico = new Font(Font.FontFamily.COURIER, 10, Font.BOLD);

        //    var exame = _service.ObterRequisicaoExameById(id);
        //    if (exame == null)
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao recuperar dados da requisição de exames.");


        //    //busca dados do profissional
        //    var profissional = _serviceFuncionario.ObterFuncionariPorId(exame.IdMedico);

        //    float hSum = 0;
        //    var pdf = new Document();
        //    pdf.SetPageSize(PageSize.A5.Rotate());
        //    var stream = new MemoryStream();

        //    try
        //    {
        //        PdfWriter writer = PdfWriter.GetInstance(pdf, stream);
        //        writer.CloseStream = false;
        //        pdf.Open();
        //        PdfContentByte cb = writer.DirectContent;
        //        float topPos = pdf.PageSize.Height - pdf.TopMargin;
        //        float leftPos = pdf.LeftMargin;


        //        float caixaLogoH = 60f;
        //        float caixaLogoW = 300f;

        //        var rect = new iTextSharp.text.Rectangle(leftPos, topPos - caixaLogoH - hSum, caixaLogoW, caixaLogoH);
        //        rect.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
        //        rect.BorderWidth = 0;
        //        rect.BorderColor = new BaseColor(0, 0, 0);
        //        cb.Rectangle(rect);
        //        cb.Stroke();


        //        caixaLogoH = 60f;
        //        caixaLogoW = 150f;


        //        var rect2 = new iTextSharp.text.Rectangle(leftPos, topPos - caixaLogoH - hSum, caixaLogoW, caixaLogoH);
        //        rect2.Border = iTextSharp.text.Rectangle.LEFT_BORDER | iTextSharp.text.Rectangle.RIGHT_BORDER;
        //        rect2.BorderWidth = 0;
        //        rect2.BorderColor = new BaseColor(0, 0, 0);

        //        cb.Rectangle(rect2);
        //        cb.Stroke();

        //        cb.BeginText();
        //        cb.SetFontAndSize(f_boldC, 18);
        //        cb.SetTextMatrix(leftPos + caixaLogoW + 40f, topPos - hSum - 25f);
        //        cb.ShowText("CLÍNICA INFANTIL PAMPULHA");
        //        cb.EndText();

        //        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(AppDomain.CurrentDomain.BaseDirectory + "\\imagens\\Logomarca_CIP.jpg");
        //        logo.ScalePercent(40f);
        //        logo.SetAbsolutePosition(leftPos + 10f, topPos - hSum - 40f);
        //        pdf.Add(logo);


        //        PdfPTable tabFot = new PdfPTable(new float[] { 1F });
        //        tabFot.DefaultCell.Border = Rectangle.NO_BORDER;
        //        tabFot.DefaultCell.BorderWidth = 0;
        //        PdfPCell cell;
        //        tabFot.TotalWidth = 540f;
        //        cell = new PdfPCell();
        //        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell.BorderWidth = 0;
        //        cell.BorderWidthTop = 1;
        //        tabFot.AddCell(cell);
        //        PdfPCell cell2 = new PdfPCell();

        //        cell2.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cell2.BorderWidth = 0;
        //        tabFot.AddCell(cell2);
        //        tabFot.WriteSelectedRows(0, -20, 30, pdf.Top - 60f, writer.DirectContent);


        //        hSum += 10f;
        //        //cb.SetTextMatrix(pdf.LeftMargin + 2f, topPos - hSum);

        //        cb.BeginText();
        //        cb.SetFontAndSize(f_boldC, 14);
        //        cb.SetTextMatrix(leftPos + 110f, topPos - hSum - 80f);
        //        cb.ShowText("REQUISIÇÃO DE EXAMES COMPLEMENTARES");
        //        cb.EndText();

        //        cb.BeginText();
        //        cb.SetFontAndSize(f_boldC, 10);
        //        cb.SetTextMatrix(leftPos + 20f, topPos - hSum - 100f);

        //        if (exame.Tipo == "Urgente")
        //            cb.ShowText("Urgente [ X ]   Rotina [  ]   Controle [  ]   Classe: " + exame.Classe);
        //        else if (exame.Tipo == "Rotina")
        //            cb.ShowText("Urgente [  ]   Rotina [ X ]   Controle [  ]   Classe: " + exame.Classe);
        //        else if (exame.Tipo == "Controle")
        //            cb.ShowText("Urgente [  ]   Rotina [  ]   Controle [ X ]   Classe: " + exame.Classe);

        //        cb.EndText();

        //        cb.BeginText();
        //        cb.SetFontAndSize(f_boldC, 10);
        //        cb.SetTextMatrix(leftPos + 20f, topPos - hSum - 120f);
        //        cb.ShowText("Nome: " + exame.Paciente.Pessoa.Nome.ToUpper());
        //        cb.EndText();



        //        cb.BeginText();
        //        cb.SetFontAndSize(f_boldC, 10);
        //        cb.SetTextMatrix(leftPos + 20f, topPos - hSum - 140f);
        //        cb.ShowText("Natureza do Exame: " + exame.NaturezaExame);
        //        cb.EndText();

        //        cb.BeginText();
        //        cb.SetFontAndSize(f_boldC, 10);
        //        cb.SetTextMatrix(leftPos + 20f, topPos - hSum - 160f);
        //        cb.ShowText("Data: " + exame.Data.ToShortDateString());
        //        cb.EndText();

        //        cb.BeginText();
        //        cb.SetFontAndSize(f_boldC, 10);
        //        cb.SetTextMatrix(leftPos + 20f, topPos - hSum - 180f);
        //        cb.ShowText("Diagnóstico Clínico: ");
        //        cb.EndText();

        //        var p = new Paragraph(exame.DiagnosticoClinico, fontDiagnostico) { SpacingBefore = 190f };
        //        p.Alignment = Element.ALIGN_JUSTIFIED;
        //        p.IndentationLeft = 80;
        //        pdf.Add(p);



        //        PdfPTable tabmedico = new PdfPTable(new float[] { 1F });
        //        tabmedico.DefaultCell.Border = Rectangle.NO_BORDER;
        //        tabmedico.DefaultCell.BorderWidth = 0;
        //        PdfPCell cellMedico1;
        //        tabmedico.TotalWidth = 300F;
        //        cellMedico1 = new PdfPCell(new Phrase("Médico Requisitante CRM MG", fontDiagnostico));
        //        cellMedico1.HorizontalAlignment = Element.ALIGN_CENTER;
        //        cellMedico1.BorderWidth = 0;
        //        cellMedico1.BorderWidthTop = 1;
        //        tabmedico.AddCell(cellMedico1);
        //        tabmedico.WriteSelectedRows(0, -20, 250, pdf.Bottom, writer.DirectContent);


        //        pdf.Close();

        //        byte[] bytes = stream.ToArray();

        //        var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(bytes) };

        //        response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("inline")
        //        {
        //            FileName = "CIP_Atestado_" +
        //                       DateTime.Now.ToString("G")
        //                           .Replace(":", "")
        //                           .Replace("-", "")
        //                           .Replace("/", "")
        //                           .Replace(" ", "") + ".pdf"
        //        };
        //        response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");

        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro.");
        //    }

        //}

        //[HttpGet]
        //[Route("getRequisicaoById")]
        //public HttpResponseMessage ObterRequisicaoExameById(int id)
        //{
        //    try
        //    {
        //        var model = new RequisicaoExamesViewModel();

        //        var requisicao = _service.ObterRequisicaoExameById(id);

        //        if (requisicao == null)
        //            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Erro ao recuperar dados da requisição de exames.");

        //        Mapper.CreateMap<RequisicaoExames, RequisicaoExamesViewModel>();
        //        model = Mapper.Map<RequisicaoExames, RequisicaoExamesViewModel>(requisicao);

        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpGet]
        //[Route("getRequisicaoByPaciente")]
        //public HttpResponseMessage ListarRequisicoesByPaciente(int id)
        //{
        //    try
        //    {
        //        var model = new List<RequisicaoExamesViewModel>();

        //        var requisicoes = _service.ListarRequisicoesByPaciente(id);

        //        if (requisicoes.Count > 0)
        //        {
        //            Mapper.CreateMap<RequisicaoExames, RequisicaoExamesViewModel>()
        //                .ForMember(x => x.MedicoSolicitante, y => y.MapFrom(z => z.Medico.NomeMedico))
        //                .ForMember(x => x.NomePaciente, y => y.MapFrom(z => z.Paciente.Pessoa.Nome));
        //            model = Mapper.Map<List<RequisicaoExames>, List<RequisicaoExamesViewModel>>(requisicoes);

        //        }
        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpPost]
        //[Route("saveRequisicaoExame")]
        //public HttpResponseMessage SalvarRequisicaoExame(RequisicaoExamesViewModel model)
        //{
        //    try
        //    {
        //        if (model.IdRequisicao > 0)
        //        {
        //            var requisicao = _service.ObterRequisicaoExameById(model.IdRequisicao);
        //            if (requisicao == null)
        //                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados da requisição de exames.");

        //            var medico = _serviceFuncionario.ObterMedicoPorId(model.IdMedico);
        //            if (medico == null)
        //                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do médico.");

        //            requisicao.SetClasse(model.Classe);
        //            requisicao.SetMedico(medico);
        //            requisicao.SetDiagnosticoClinico(model.DiagnosticoClinico);
        //            requisicao.SetMaterial(model.Material);
        //            requisicao.SetNaturezaExame(model.NaturezaExame);
        //            requisicao.SetTipo(model.Tipo);

        //            _service.SalvarRequisicaoExame(requisicao);
        //        }
        //        else
        //        {
        //            var ppaciente = _servicePaciente.ObterPacientePorId(model.IdPaciente);
        //            if (ppaciente == null)
        //                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do paciente.");

        //            var medico = _serviceFuncionario.ObterMedicoPorId(model.IdMedico);
        //            if (medico == null)
        //                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do médico.");

        //            var requisicao = new RequisicaoExames(ppaciente, medico, model.Tipo, model.Classe, model.NaturezaExame, model.Material, model.DiagnosticoClinico);
        //            _service.SalvarRequisicaoExame(requisicao);
        //        }

        //        return Request.CreateResponse(HttpStatusCode.OK, model);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}
        //#endregion
    }
}