using AutoMapper;
using Clinicas.Application.Services.Interfaces;
using Clinicas.Domain.Model;
using Clinicas.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Web.Http;
using System.Drawing.Imaging;
using System.Drawing;

namespace Clinicas.Api.Controllers
{
    [RoutePrefix("cadastros")]
    [Authorize]
    public class CadastroController : BaseController
    {
        private readonly ICadastroService _service;
        private readonly IUsuarioService _usuarioService;

        public CadastroController(ICadastroService service, IUsuarioService usuarioService) : base(usuarioService)
        {
            _service = service;
            _usuarioService = usuarioService;
        }

        #region [Cadastro de Especialidades]
        [HttpGet]
        [Route("getEspecialidadeById")]
        public HttpResponseMessage ObterEspecialidadeById(int id)
        {
            try
            {
                var model = new EspecialidadeViewModel();

                var result = _service.GetEspecialidadeById(id);

                if (result != null)
                {
                    Mapper.CreateMap<Especialidade, EspecialidadeViewModel>();
                    model = Mapper.Map<Especialidade, EspecialidadeViewModel>(result);

                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados da especialidade.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getEspecialidadePorNome")]
        public HttpResponseMessage ListarEspecialidadesPorNome(string nome)
        {
            try
            {
                var model = new List<EspecialidadeViewModel>();

                var result = _service.ListarEspecialidadesPorNome(nome);

                if (result != null)
                {
                    Mapper.CreateMap<Especialidade, EspecialidadeViewModel>();
                    model = Mapper.Map<List<Especialidade>, List<EspecialidadeViewModel>>(result);

                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados da especialidade.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getAllEspecialidades")]
        public HttpResponseMessage ListarEspecialidades()
        {
            try
            {
                var model = new List<EspecialidadeViewModel>();
                var result = _service.ListarEspecialidades();

                if (result != null)
                {
                    /* Mapper.CreateMap<Especialidade, EspecialidadeViewModel>();
                    model = Mapper.Map<List<Especialidade>, List<EspecialidadeViewModel>>(result); */
                    foreach (var item in result)
                    {
                        var procedimento = new List<ProcedimentosViewModel>();
                        if (item.Procedimentos.Count > 0)
                        {
                            foreach (var x in item.Procedimentos)
                            {
                                procedimento.Add(new ProcedimentosViewModel()
                                {
                                    IdProcedimento = x.IdProcedimento,
                                    Codigo = x.Codigo,
                                    NmProcedimento = x.NomeProcedimento
                                });
                            }
                        }
                        model.Add(new EspecialidadeViewModel()
                        {
                            IdEspecialidade = item.IdEspecialidade,
                            NmEspecialidade = item.NmEspecialidade,
                            Procedimentos = procedimento
                        });
                    }

                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados da especialidade.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("saveEspecialidade")]
        public HttpResponseMessage SalvarEspecialidade(EspecialidadeViewModel model)
        {
            try
            {
                if (model.IdEspecialidade > 0)
                {
                    var especialidade = _service.GetEspecialidadeById(model.IdEspecialidade);
                    if (especialidade == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados da especialidade.");

                    especialidade.SetNomeEspecialidade(model.NmEspecialidade.ToUpper());

                    _service.SalvarEspecialidade(especialidade);
                }
                else
                {
                    var especialidade = new Especialidade(model.NmEspecialidade.ToUpper());
                    _service.SalvarEspecialidade(especialidade);
                }

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("excluirEspecialidade")]
        public HttpResponseMessage ExcluirEspecialidade(int id)
        {
            try
            {
                var especialidade = _service.GetEspecialidadeById(id);
                if (especialidade == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados da especialidade.");
                else
                    _service.ExcluirEspecialidade(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion

        #region [Cadastro de Procedimentos]
        [HttpGet]
        [Route("getProcedimentoById")]
        public HttpResponseMessage ObterProcedimentoById(int id)
        {
            try
            {
                var model = new ProcedimentoViewModel();

                var result = _service.ObterProcedimentoById(id);

                if (result != null)
                {
                    Mapper.CreateMap<Procedimento, ProcedimentoViewModel>();
                    model = Mapper.Map<Procedimento, ProcedimentoViewModel>(result);

                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do procedimento.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getProcedimentosPorNome")]
        public HttpResponseMessage ListarProcedimentosPorNome(string nome)
        {
            try
            {
                var model = new List<ProcedimentoViewModel>();

                var result = _service.ListarProcedimentosPorNome(nome);

                if (result != null)
                {
                    Mapper.CreateMap<Procedimento, ProcedimentoViewModel>();
                    model = Mapper.Map<List<Procedimento>, List<ProcedimentoViewModel>>(result);

                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do procedimento.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getProcedimentosPorNomeOuCodigo")]
        public HttpResponseMessage ListarProcedimentosPorNomeOuCodigo(string search)
        {
            try
            {
                var model = new List<ProcedimentoViewModel>();

                var result = _service.ListarProcedimentosPorNomeOuCodigo(search);

                if (result != null)
                {
                    Mapper.CreateMap<Procedimento, ProcedimentoViewModel>();
                    model = Mapper.Map<List<Procedimento>, List<ProcedimentoViewModel>>(result);

                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do procedimento.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getAllProcedimentos")]
        public HttpResponseMessage ListarProcedimentos()
        {
            try
            {
                var model = new List<ProcedimentoViewModel>();
                var result = _service.ListarProcedimentos();

                if (result != null)
                {
                    foreach (var item in result)
                    {

                        #region especialidades
                        var especialidades = new List<EspecialidadeViewModel>();
                        if (item.Especialidades != null)
                        {
                            foreach (var x in item.Especialidades)
                            {
                                especialidades.Add(new EspecialidadeViewModel()
                                {
                                    IdEspecialidade = x.IdEspecialidade,
                                    NmEspecialidade = x.NmEspecialidade
                                });
                            }
                        }
                        #endregion

                        model.Add(new ProcedimentoViewModel()
                        {
                            Codigo = item.Codigo,
                            NomeProcedimento = item.NomeProcedimento,
                            IdProcedimento = item.IdProcedimento,
                            Especialidades = especialidades
                        });
                    }
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do procedimento.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public string RetonaSexoBanco(string sexo)
        {
            switch (sexo)
            {
                case "A":
                    return "Ambos";
                case "F":
                    return "Feminino";
                case "M":
                    return "Masculino";
                default:
                    return "Ambos";
            }
        }

        [HttpPost]
        [Route("saveProcedimento")]
        public HttpResponseMessage SalvarProcedimento(ProcedimentoViewModel model)
        {
            try
            {
                if (model.IdProcedimento > 0)
                {
                    var procedimento = _service.ObterProcedimentoById(model.IdProcedimento);
                    if (procedimento == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do procedimento.");

                    procedimento.SetCodigoSus(model.Codigo);
                    procedimento.SetNomeProcediemnto(model.NomeProcedimento.ToUpper());
                    procedimento.SetOdontologico(model.Odontologico);
                    procedimento.SetPreparo(model.Preparo);
                    procedimento.SetSexo(RetonaSexoBanco(model.Sexo));
                    procedimento.SetValor(model.Valor);
                    procedimento.SetValorProfissional(model.ValorProfissional);

                    _service.SalvarProcedimento(procedimento);
                }
                else
                {
                    var procedimento = new Procedimento(model.NomeProcedimento.ToUpper(), model.Valor, model.ValorProfissional, RetonaSexoBanco(model.Sexo), model.Codigo, model.Odontologico, model.Preparo);
                    _service.SalvarProcedimento(procedimento);
                }

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        #endregion

        #region [Cadastro de convenios]
        [HttpGet]
        [Route("getConvenioById")]
        public HttpResponseMessage ObterConvenioById(int id)
        {
            try
            {
                var model = new ConvenioViewModel();
                var result = _service.ObterConvenioById(id);

                if (result != null)
                {
                    // dados convenio
                    model.IdConvenio = result.Pessoa.IdPessoa;
                    model.Tipo = result.Pessoa.Tipo;
                    model.Nome = result.Pessoa.Nome;
                    model.RazaoSocial = result.Pessoa.RazaoSocial;
                    model.NomeFantasia = result.Pessoa.Nome;
                    model.DataNascimento = result.Pessoa.DataNascimento;
                    model.CPF = result.Pessoa.CpfCnpj;
                    model.CNPJ = result.Pessoa.CpfCnpj;
                    model.Mae = result.Pessoa.Mae;
                    model.Pai = result.Pessoa.Pai;
                    model.Observacao = result.Pessoa.Observacoes;
                    model.InscricaoEstadual = result.Pessoa.IE;
                    model.Observacao = result.Pessoa.Observacoes;
                    model.Sexo = result.Pessoa.Sexo;
                    model.Situacao = result.Pessoa.Situacao;
                    model.RegistroAns = result.RegistroAns;
                    model.LogoGuia = result.LogoGuia;
                    // endereço
                    model.Cep = result.Pessoa.Cep;
                    model.Logradouro = result.Pessoa.Logradouro;
                    model.Numero = result.Pessoa.Numero;
                    model.Bairro = result.Pessoa.Bairro;
                    model.EstadoSelecionado = result.Pessoa.IdEstado;
                    model.CidadeSelecionada = result.Pessoa.IdCidade;
                    model.Complemento = result.Pessoa.Complemento;
                    model.Referencia = result.Pessoa.Referencia;

                    // contatos 
                    model.Telefone1 = result.Pessoa.Telefone1;
                    model.Telefone2 = result.Pessoa.Telefone2;
                    model.Email = result.Pessoa.Email;
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do fornecedor.");
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("pesquisarConvenios")]
        public HttpResponseMessage PesquisarConvenios(string nome, int? idConvenio)
        {
            try
            {
                var model = new List<ConvenioViewModel>();
                var result = _service.PesquisarConvenios(nome, idConvenio, base.GetUsuarioLogado().IdClinica);

                if (result != null)
                {
                    foreach (var item in result)
                    {
                        model.Add(new ConvenioViewModel()
                        {
                            IdConvenio = item.Pessoa.IdPessoa,
                            Nome = item.Pessoa.Nome,
                            Tipo = item.Pessoa.Tipo,
                            Telefone1 = item.Pessoa.Telefone1,
                            Telefone2 = item.Pessoa.Telefone2,
                            Email = item.Pessoa.Email,
                            Situacao = item.Pessoa.Situacao
                        });
                    }
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do convenio.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("listarConvenios")]
        public HttpResponseMessage ListarConvenios()
        {
            try
            {
                var model = new List<ConvenioViewModel>();
                var result = _service.ListarConvenios(base.GetUsuarioLogado().IdClinica);

                if (result != null)
                {
                    foreach (var item in result)
                        model.Add(new ConvenioViewModel()
                        {
                            IdConvenio = item.Pessoa.IdPessoa,
                            RegistroAns = item.RegistroAns,
                            Nome = item.Pessoa.Nome,
                            Tipo = item.Pessoa.Tipo,
                            Telefone1 = item.Pessoa.Telefone1,
                            Telefone2 = item.Pessoa.Telefone2,
                            Email = item.Pessoa.Email,
                            Situacao = item.Pessoa.Situacao
                        });
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do convenio.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("saveConvenio")]
        public HttpResponseMessage SalvarConvenio(ConvenioViewModel model)
        {
            try
            {
                if (model.IdConvenio > 0)
                {
                    var convenio = _service.ObterConvenioById(model.IdConvenio);
                    if (convenio == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do convenio.");

                    #region dados convenio 

                    if (model.Tipo == "PF")
                    {
                        convenio.Pessoa.SetNome(model.Nome.ToUpper());
                        convenio.Pessoa.SetDataNascimento(model.DataNascimento);
                        convenio.Pessoa.SetCpfCnpj(model.CPF);
                        convenio.Pessoa.SetSexo(model.Sexo);
                        convenio.Pessoa.SetMae(model.Mae);
                        convenio.Pessoa.SetPai(model.Pai);
                        convenio.Pessoa.SetDataNascimento(model.DataNascimento);
                        convenio.Pessoa.SetObservacoes(model.Observacao);
                    }
                    else
                    {
                        convenio.Pessoa.SetCpfCnpj(model.CNPJ);
                        convenio.Pessoa.SetRazaoSocial(model.RazaoSocial);
                        convenio.Pessoa.SetIe(model.InscricaoEstadual);
                        convenio.Pessoa.SetNome(model.NomeFantasia);
                        convenio.Pessoa.SetObservacoes(model.Observacao);
                        convenio.SetRegistroAns(model.RegistroAns);
                    }
                    convenio.Pessoa.SetTipo(model.Tipo);
                    convenio.Pessoa.SetSituacao(model.Situacao);
                    convenio.Pessoa.SetAlteracao(base.GetUsuarioLogado());
                    #endregion

                    #region dados endereço
                    var estado = _service.ObterEstadoPorId(model.EstadoSelecionado);
                    if (estado == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do estado.");

                    var cidade = _service.ObterCidadePorId(model.CidadeSelecionada);
                    if (estado == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados da cidade.");

                    convenio.Pessoa.SetEndereco(model.Cep, estado, cidade, model.Bairro, model.Logradouro, model.Numero, model.Complemento, model.Referencia);
                    #endregion

                    #region contatos
                    convenio.Pessoa.SetTelefone1(model.Telefone1);
                    convenio.Pessoa.SetTelefone2(model.Telefone2);
                    convenio.Pessoa.SetEmail(model.Email);
                    #endregion

                    _service.SalvarConvenio(convenio);
                }
                else
                {
                    if (model.Tipo == "PF")
                    {
                        model.CpfCnpj = model.CPF;

                    }
                    else if (model.Tipo == "PJ")
                    {
                        model.CpfCnpj = model.CNPJ;
                        model.Nome = model.NomeFantasia;
                    }

                    //TODO: criar dois construtores 
                    var pessoa = new Pessoa(model.Nome, model.RazaoSocial, model.Sexo, model.Tipo, model.DataNascimento, model.CpfCnpj, model.Rg, model.InscricaoEstadual, model.Profissao, model.Mae, model.Pai, model.Email,
                        model.Site, model.Telefone1, model.Telefone2, model.Observacao, model.Cep, _service.ObterEstadoPorId(model.EstadoSelecionado), _service.ObterCidadePorId(model.CidadeSelecionada), model.Bairro, model.Logradouro, model.Numero, model.Complemento, model.Referencia, base.GetUsuarioLogado(), model.Conjuge, base.GetUsuarioLogado().Clinica);
                    var convenio = new Convenio(pessoa, model.RegistroAns);
                    var novo = _service.SalvarConvenio(convenio);
                    model.IdConvenio = novo.IdConvenio;
                }

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("excluirConvenioById")]
        public HttpResponseMessage ExcluirConvenio(int id)
        {
            try
            {
                var model = new ConvenioViewModel();
                var result = _service.ObterConvenioById(id);

                if (result != null)
                {
                    result.Pessoa.SetExclusao(base.GetUsuarioLogado());
                    _service.ExcluirConvenio(result);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do convenio.");
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        public bool ThumbnailCallback()
        {
            return false;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("uploadthumbNail")]
        public async Task<string> CadastrarLogoGuia(int idconveio)
        {

            var convenio = _service.ObterConvenioById(idconveio);
            if (convenio == null)
                throw new Exception("Não foi possível recuperar dados do convênio!");

            string nomeArquivo = "";
            string diretorioRaiz = AppDomain.CurrentDomain.BaseDirectory + "imagens\\";
            var streamProvider = new MultipartMemoryStreamProvider();

            try
            {
                await Request.Content.ReadAsMultipartAsync(streamProvider);

                var fileStream = await streamProvider.Contents[0].ReadAsByteArrayAsync();


                if (fileStream != null && fileStream.Length > 0)
                {
                    var complemento = DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond;
                    //extrair o tipo do arquivo 
                    var extensao =
                        Path.GetExtension(streamProvider.Contents[0].Headers.ContentDisposition.FileName.Replace("\"", ""));
                    nomeArquivo = convenio.Pessoa.Nome.ToLower() + complemento + extensao;

                    File.WriteAllBytes(diretorioRaiz + nomeArquivo, fileStream);

                    System.Drawing.Image fullSizeImg = System.Drawing.Image.FromFile(diretorioRaiz + nomeArquivo);

                    System.Drawing.Image.GetThumbnailImageAbort dummyCallBack = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);

                    System.Drawing.Image thumbNailImg = fullSizeImg.GetThumbnailImage(140, 51, dummyCallBack, IntPtr.Zero);

                    String MyString = convenio.Pessoa.Nome.ToLower() + complemento + "_thumb.png";

                    //Save the thumbnail in Png format. You may change it to a diff format with the ImageFormat property
                    thumbNailImg.Save(diretorioRaiz + MyString, ImageFormat.Png);
                    byte[] imagem = imageToByteArray(thumbNailImg);

                    convenio.SetLogoGuia(imagem);

                    thumbNailImg.Dispose();

                    _service.SalvarConvenio(convenio);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return "";
        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        #endregion

        #region [Cadastro de CID]
        [HttpGet]
        [Route("getCidByCodigo")]
        public HttpResponseMessage ObterCidByCodigo(string codigo)
        {
            try
            {
                var model = new CidViewModel();

                var result = _service.GetCidByCodigo(codigo);

                if (result != null)
                {
                    Mapper.CreateMap<Cid, CidViewModel>();
                    model = Mapper.Map<Cid, CidViewModel>(result);

                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do CID.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getCidPorNome")]
        public HttpResponseMessage ListarCidsPorNome(string nome)
        {
            try
            {
                var model = new List<CidViewModel>();

                var result = _service.ListarCidsPorNome(nome);

                if (result != null)
                {
                    Mapper.CreateMap<Cid, CidViewModel>();
                    model = Mapper.Map<List<Cid>, List<CidViewModel>>(result);

                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do CID.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getAllCid")]
        public HttpResponseMessage ListarCids()
        {
            try
            {
                var model = new List<CidViewModel>();

                var result = _service.ListarCids();

                if (result != null)
                {
                    Mapper.CreateMap<Cid, CidViewModel>();
                    model = Mapper.Map<List<Cid>, List<CidViewModel>>(result);

                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do CID.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("saveCid")]
        public HttpResponseMessage SalvarCid(CidViewModel model)
        {
            try
            {
                var cid = _service.GetCidByCodigo(model.Codigo);
                if (cid != null)
                {

                    if (string.IsNullOrEmpty(model.CamposIrradiados))
                        model.CamposIrradiados = "0000";

                    cid.SetAgravo(model.Agravo);
                    cid.SetCamposIrradiados(model.CamposIrradiados);
                    cid.SetCodigo(model.Codigo.ToUpper());
                    cid.SetDescricao(model.Descricao.ToUpper());
                    cid.SetEstadio(model.Estadio);
                    cid.SetSexoOcorrencia(model.SexoOcorrencia);


                    _service.SalvarCid(cid, true);
                }
                else
                {
                    if (string.IsNullOrEmpty(model.CamposIrradiados))
                        model.CamposIrradiados = "0000";

                    var novocid = new Cid(model.Codigo.ToUpper(), model.Descricao.ToUpper(), model.Agravo, model.SexoOcorrencia, model.Estadio, model.CamposIrradiados);
                    _service.SalvarCid(novocid, false);
                }

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion

        #region [Cadastro de Ocupações]
        [HttpGet]
        [Route("getOcupacaoById")]
        public HttpResponseMessage GetOcupacaoById(int id)
        {
            try
            {
                var model = new OcupacaoViewModel();

                var result = _service.GetOcupacaoById(id);

                if (result != null)
                {
                    Mapper.CreateMap<Ocupacao, OcupacaoViewModel>();
                    model = Mapper.Map<Ocupacao, OcupacaoViewModel>(result);

                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados da ocupação.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getOcupacoesPorNome")]
        public HttpResponseMessage ListarOcupacoesPorNome(string nome)
        {
            try
            {
                var model = new List<OcupacaoViewModel>();

                var result = _service.ListarOcupacoesPorNome(nome);

                if (result != null)
                {
                    Mapper.CreateMap<Ocupacao, OcupacaoViewModel>();
                    model = Mapper.Map<List<Ocupacao>, List<OcupacaoViewModel>>(result);

                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados da ocupação.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getAllOcupcaoes")]
        public HttpResponseMessage ListarOcupacoes()
        {
            try
            {
                var model = new List<OcupacaoViewModel>();

                var result = _service.ListarOcupacoes();

                if (result != null)
                {
                    Mapper.CreateMap<Ocupacao, OcupacaoViewModel>();
                    model = Mapper.Map<List<Ocupacao>, List<OcupacaoViewModel>>(result);

                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados da ocupação.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("saveOcupacao")]
        public HttpResponseMessage SalvarOcupacao(OcupacaoViewModel model)
        {
            try
            {
                if (model.IdOcupacao > 0)
                {
                    var ocupacao = _service.GetOcupacaoById(model.IdOcupacao);
                    if (ocupacao == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados da ocupação.");

                    ocupacao.SetCodigoOcupacao(model.Codigo);
                    ocupacao.SetNomeOcupacao(model.NmOcupacao);

                    _service.SalvarOcupacao(ocupacao);
                }
                else
                {
                    var ocupacao = new Ocupacao(model.NmOcupacao, model.Codigo);
                    _service.SalvarOcupacao(ocupacao);
                }

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #endregion

        #region [Cadastro de fornecedores]

        [HttpGet]
        [Route("getFornecedorById")]
        public HttpResponseMessage ObterFornecedorById(int id)
        {
            try
            {
                var model = new FornecedorViewModel();
                var result = _service.ObterFornecedorById(id);

                if (result != null)
                {
                    // dados fornecedor
                    model.IdFornecedor = result.Pessoa.IdPessoa;
                    model.Tipo = result.Pessoa.Tipo;
                    model.Nome = result.Pessoa.Nome;
                    model.RazaoSocial = result.Pessoa.RazaoSocial;
                    model.NomeFantasia = result.Pessoa.Nome;
                    model.DataNascimento = result.Pessoa.DataNascimento;
                    model.CPF = result.Pessoa.CpfCnpj;
                    model.CNPJ = result.Pessoa.CpfCnpj;
                    model.Mae = result.Pessoa.Mae;
                    model.Pai = result.Pessoa.Pai;
                    model.Observacao = result.Pessoa.Observacoes;
                    model.InscricaoEstadual = result.Pessoa.IE;
                    model.Observacao = result.Pessoa.Observacoes;
                    model.Sexo = result.Pessoa.Sexo;
                    model.Situacao = result.Pessoa.Situacao;


                    // endereço
                    model.Cep = result.Pessoa.Cep;
                    model.Logradouro = result.Pessoa.Logradouro;
                    model.Numero = result.Pessoa.Numero;
                    model.Bairro = result.Pessoa.Bairro;
                    model.EstadoSelecionado = result.Pessoa.IdEstado;
                    model.CidadeSelecionada = result.Pessoa.IdCidade;
                    model.Complemento = result.Pessoa.Complemento;
                    model.Referencia = result.Pessoa.Referencia;

                    // contatos 
                    model.Telefone1 = result.Pessoa.Telefone1;
                    model.Telefone2 = result.Pessoa.Telefone2;
                    model.Email = result.Pessoa.Email;
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do fornecedor.");
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("pesquisarFornecedores")]
        public HttpResponseMessage PesquisarFornecedores(string nome, int? idFornecedor)
        {
            try
            {
                var model = new List<FornecedorViewModel>();

                var result = _service.PesquisarFornecedores(nome, idFornecedor, base.GetUsuarioLogado().IdClinica);

                if (result != null)
                {
                    foreach (var item in result)
                    {
                        model.Add(new FornecedorViewModel()
                        {
                            IdFornecedor = item.Pessoa.IdPessoa,
                            Nome = item.Pessoa.Nome,
                            Tipo = item.Pessoa.Tipo,
                            Telefone1 = item.Pessoa.Telefone1,
                            Telefone2 = item.Pessoa.Telefone2,
                            Email = item.Pessoa.Email,
                            Situacao = item.Pessoa.Situacao
                        });
                    }
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do fornecedor.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("listarFornecedores")]
        public HttpResponseMessage ListarFornecedores()
        {
            try
            {
                var model = new List<FornecedorViewModel>();
                var result = _service.ListarFornecedores(base.GetUsuarioLogado().IdClinica);

                if (result != null)
                {
                    foreach (var item in result)
                        model.Add(new FornecedorViewModel()
                        {
                            IdFornecedor = item.Pessoa.IdPessoa,
                            Nome = item.Pessoa.Nome,
                            Tipo = item.Pessoa.Tipo,
                            Telefone1 = item.Pessoa.Telefone1,
                            Telefone2 = item.Pessoa.Telefone2,
                            Email = item.Pessoa.Email,
                            Situacao = item.Pessoa.Situacao
                        });
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do fornecedor.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("saveFornecedor")]
        public HttpResponseMessage SalvarFornecedor(FornecedorViewModel model)
        {
            try
            {
                if (model.IdFornecedor > 0)
                {
                    var fornecedor = _service.ObterFornecedorById(model.IdFornecedor);
                    if (fornecedor == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do fornecedor.");

                    #region dados fornecedor 

                    if (model.Tipo == "PF")
                    {
                        fornecedor.Pessoa.SetNome(model.Nome.ToUpper());
                        fornecedor.Pessoa.SetDataNascimento(model.DataNascimento);
                        fornecedor.Pessoa.SetCpfCnpj(model.CPF);
                        fornecedor.Pessoa.SetSexo(model.Sexo);
                        fornecedor.Pessoa.SetMae(model.Mae);
                        fornecedor.Pessoa.SetPai(model.Pai);
                        fornecedor.Pessoa.SetDataNascimento(model.DataNascimento);
                        fornecedor.Pessoa.SetObservacoes(model.Observacao);
                    }
                    else
                    {
                        fornecedor.Pessoa.SetCpfCnpj(model.CNPJ);
                        fornecedor.Pessoa.SetRazaoSocial(model.RazaoSocial);
                        fornecedor.Pessoa.SetIe(model.InscricaoEstadual);
                        fornecedor.Pessoa.SetNome(model.NomeFantasia);
                        fornecedor.Pessoa.SetObservacoes(model.Observacao);
                    }
                    fornecedor.Pessoa.SetTipo(model.Tipo);
                    fornecedor.Pessoa.SetSituacao(model.Situacao);
                    fornecedor.Pessoa.SetAlteracao(base.GetUsuarioLogado());
                    #endregion

                    #region dados endereço
                    var estado = _service.ObterEstadoPorId(model.EstadoSelecionado);
                    if (estado == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do estado.");

                    var cidade = _service.ObterCidadePorId(model.CidadeSelecionada);
                    if (estado == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados da cidade.");

                    fornecedor.Pessoa.SetEndereco(model.Cep, estado, cidade, model.Bairro, model.Logradouro, model.Numero, model.Complemento, model.Referencia);
                    #endregion

                    #region contatos
                    fornecedor.Pessoa.SetTelefone1(model.Telefone1);
                    fornecedor.Pessoa.SetTelefone2(model.Telefone2);
                    fornecedor.Pessoa.SetEmail(model.Email);
                    #endregion

                    _service.SalvarFornecedor(fornecedor);
                }
                else
                {
                    if (model.Tipo == "PF")
                    {
                        model.CpfCnpj = model.CPF;

                    }
                    else if (model.Tipo == "PJ")
                    {
                        model.CpfCnpj = model.CNPJ;
                        model.Nome = model.NomeFantasia;
                    }


                    //TODO: criar dois construtores 
                    var pessoa = new Pessoa(model.Nome, model.RazaoSocial, model.Sexo, model.Tipo, model.DataNascimento, model.CpfCnpj, model.Rg, model.InscricaoEstadual, model.Profissao, model.Mae, model.Pai, model.Email,
                        model.Site, model.Telefone1, model.Telefone2, model.Observacao, model.Cep, _service.ObterEstadoPorId(model.EstadoSelecionado), _service.ObterCidadePorId(model.CidadeSelecionada), model.Bairro, model.Logradouro, model.Numero, model.Complemento, model.Referencia, base.GetUsuarioLogado(), model.Conjuge, base.GetUsuarioLogado().Clinica);
                    var fornecedor = new Fornecedor(pessoa);
                    _service.SalvarFornecedor(fornecedor);
                }

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("excluirFornecedorById")]
        public HttpResponseMessage ExcluirFornecedor(int id)
        {
            try
            {
                var model = new FornecedorViewModel();
                var result = _service.ObterFornecedorById(id);

                if (result != null)
                {
                    result.Pessoa.SetExclusao(base.GetUsuarioLogado());
                    _service.ExcluirFornecedor(result);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do fornecedor.");
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion

        #region [Cadastro de funcionarios - Profissional de Saúde ]
        [HttpGet]
        [Route("getFuncionarioById")]
        public HttpResponseMessage ObterFuncionarioById(int id)
        {
            try
            {
                var model = new FuncionarioViewModel();
                var result = _service.ObterFuncionarioById(id);

                if (result != null)
                {
                    // dados funcionario
                    model.IdFuncionario = result.Pessoa.IdPessoa;
                    model.Tipo = result.Pessoa.Tipo;
                    model.Nome = result.Pessoa.Nome;
                    model.RazaoSocial = result.Pessoa.RazaoSocial;
                    model.NomeFantasia = result.Pessoa.Nome;
                    model.DataNascimento = result.Pessoa.DataNascimento;
                    model.CPF = result.Pessoa.CpfCnpj;
                    model.CNPJ = result.Pessoa.CpfCnpj;
                    model.Mae = result.Pessoa.Mae;
                    model.Pai = result.Pessoa.Pai;
                    model.Observacao = result.Pessoa.Observacoes;
                    model.InscricaoEstadual = result.Pessoa.IE;
                    model.Observacao = result.Pessoa.Observacoes;
                    model.Sexo = result.Pessoa.Sexo;
                    model.Situacao = result.Pessoa.Situacao;
                    model.TipoFuncionario = result.Tipo;

                    // endereço
                    model.Cep = result.Pessoa.Cep;
                    model.Logradouro = result.Pessoa.Logradouro;
                    model.Numero = result.Pessoa.Numero;
                    model.Bairro = result.Pessoa.Bairro;
                    model.EstadoSelecionado = result.Pessoa.IdEstado;
                    model.CidadeSelecionada = result.Pessoa.IdCidade;
                    model.Complemento = result.Pessoa.Complemento;
                    model.Referencia = result.Pessoa.Referencia;

                    // contatos 
                    model.Telefone1 = result.Pessoa.Telefone1;
                    model.Telefone2 = result.Pessoa.Telefone2;
                    model.Email = result.Pessoa.Email;
                    model.Especialidades = RetornaEspecialidadesModel(result.Especialidades.ToList());
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do funcionario.");
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public List<EspecialidadeViewModel> RetornaEspecialidadesModel(List<Especialidade> especialidades)
        {
            var model = new List<EspecialidadeViewModel>();
            foreach (var item in especialidades)
            {
                model.Add(new EspecialidadeViewModel
                {
                    IdEspecialidade = item.IdEspecialidade,
                    NmEspecialidade = item.NmEspecialidade
                });
            }
            return model;
        }

        [HttpGet]
        [Route("pesquisarFuncionarios")]
        public HttpResponseMessage PesquisarFuncionarios(string nome, int? idFuncionario)
        {
            try
            {
                var model = new List<FuncionarioViewModel>();
                var result = _service.PesquisarFuncionarios(nome, idFuncionario, base.GetUsuarioLogado().IdClinica);

                if (result != null)
                {
                    foreach (var item in result)
                    {
                        model.Add(new FuncionarioViewModel()
                        {
                            IdFuncionario = item.Pessoa.IdPessoa,
                            Nome = item.Pessoa.Nome,
                            Tipo = item.Pessoa.Tipo,
                            Telefone1 = item.Pessoa.Telefone1,
                            Telefone2 = item.Pessoa.Telefone2,
                            Email = item.Pessoa.Email,
                            Situacao = item.Pessoa.Situacao
                        });
                    }
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do funcionario.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("listarFuncionarios")]
        public HttpResponseMessage ListarFuncionarios()
        {
            try
            {
                var model = new List<FuncionarioViewModel>();
                var result = _service.ListarFuncionarios(base.GetUsuarioLogado().IdClinica);

                if (result != null)
                {
                    foreach (var item in result)
                        model.Add(new FuncionarioViewModel()
                        {
                            IdFuncionario = item.Pessoa.IdPessoa,
                            Nome = item.Pessoa.Nome,
                            Tipo = item.Tipo,
                            Telefone1 = item.Pessoa.Telefone1,
                            Telefone2 = item.Pessoa.Telefone2,
                            Email = item.Pessoa.Email,
                            Situacao = item.Pessoa.Situacao
                        });
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do funcionario.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("saveFuncionario")]
        public HttpResponseMessage SalvarFuncionario(FuncionarioViewModel model)
        {
            try
            {
                if (model.IdFuncionario > 0)
                {
                    var funcionario = _service.ObterFuncionarioById(model.IdFuncionario);
                    if (funcionario == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do funcionario.");

                    if (model.FinalizarPrimeiroAcesso)
                    {
                        #region [Especialidades]
                        if (model.Especialidades.Count > 0)
                            _service.ExcluirEspecialidades(funcionario.Especialidades.ToList(), funcionario.IdFuncionario);

                        foreach (var item in model.Especialidades)
                        {
                            var espec = _service.GetEspecialidadeById(item.IdEspecialidade);
                            if (espec != null)
                            {
                                funcionario.AddEspecialidades(espec);
                            }
                        }
                        #endregion

                        _service.SalvarFuncionario(funcionario);

                        var usuario = _usuarioService.ObterUsuarioPorId(base.GetUsuarioLogado().IdUsuario);
                        usuario.SetPrimeiroAcesso("N");
                        _usuarioService.SalvarUsuario(usuario);
                    }
                    else
                    {

                        #region dados fornecedor 

                        if (model.Tipo == "PF")
                        {
                            funcionario.Pessoa.SetNome(model.Nome.ToUpper());
                            funcionario.Pessoa.SetDataNascimento(model.DataNascimento);
                            funcionario.Pessoa.SetCpfCnpj(model.CPF);
                            funcionario.Pessoa.SetSexo(model.Sexo);
                            funcionario.Pessoa.SetMae(model.Mae);
                            funcionario.Pessoa.SetPai(model.Pai);
                            funcionario.Pessoa.SetDataNascimento(model.DataNascimento);
                            funcionario.Pessoa.SetObservacoes(model.Observacao);
                            funcionario.SetTipo(model.TipoFuncionario);
                        }
                        else
                        {
                            funcionario.Pessoa.SetCpfCnpj(model.CNPJ);
                            funcionario.Pessoa.SetRazaoSocial(model.RazaoSocial);
                            funcionario.Pessoa.SetIe(model.InscricaoEstadual);
                            funcionario.Pessoa.SetNome(model.NomeFantasia);
                            funcionario.Pessoa.SetObservacoes(model.Observacao);
                            funcionario.SetTipo(model.TipoFuncionario);
                        }
                        funcionario.Pessoa.SetTipo(model.Tipo);
                        funcionario.Pessoa.SetSituacao(model.Situacao);
                        funcionario.Pessoa.SetAlteracao(base.GetUsuarioLogado());
                        #endregion

                        #region dados endereço
                        var estado = _service.ObterEstadoPorId(model.EstadoSelecionado);
                        if (estado == null)
                            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do estado.");

                        var cidade = _service.ObterCidadePorId(model.CidadeSelecionada);
                        if (estado == null)
                            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados da cidade.");

                        funcionario.Pessoa.SetEndereco(model.Cep, estado, cidade, model.Bairro, model.Logradouro, model.Numero, model.Complemento, model.Referencia);
                        #endregion

                        #region contatos
                        funcionario.Pessoa.SetTelefone1(model.Telefone1);
                        funcionario.Pessoa.SetTelefone2(model.Telefone2);
                        funcionario.Pessoa.SetEmail(model.Email);
                        #endregion

                        #region [Especialidades]
                        if (model.Especialidades.Count > 0)
                            _service.ExcluirEspecialidades(funcionario.Especialidades.ToList(), funcionario.IdFuncionario);

                        foreach (var item in model.Especialidades)
                        {
                            var espec = _service.GetEspecialidadeById(item.IdEspecialidade);
                            if (espec != null)
                            {
                                funcionario.AddEspecialidades(espec);
                            }
                        }
                        #endregion

                        _service.SalvarFuncionario(funcionario);

                    }
                }
                else
                {
                    if (model.Tipo == "PF")
                    {
                        model.CpfCnpj = model.CPF;

                    }
                    else if (model.Tipo == "PJ")
                    {
                        model.CpfCnpj = model.CNPJ;
                        model.Nome = model.NomeFantasia;
                    }

                    if (model.PrimeiroAcesso)
                    {
                        //TODO: criar dois construtores 
                        var pessoa = new Pessoa(model.Nome, model.Sexo, model.Tipo, model.DataNascimento, model.CpfCnpj, model.Mae, model.Pai, base.GetUsuarioLogado(), base.GetUsuarioLogado().Clinica);
                        var funcionario = new Funcionario(pessoa, model.TipoFuncionario);

                        var result = _service.SalvarFuncionario(funcionario);

                        model.IdFuncionario = result.IdFuncionario;
                    }
                    else
                    {

                        //TODO: criar dois construtores 
                        var pessoa = new Pessoa(model.Nome, model.RazaoSocial, model.Sexo, model.Tipo, model.DataNascimento, model.CpfCnpj, model.Rg, model.InscricaoEstadual, model.Profissao, model.Mae, model.Pai, model.Email,
                            model.Site, model.Telefone1, model.Telefone2, model.Observacao, model.Cep, _service.ObterEstadoPorId(model.EstadoSelecionado), _service.ObterCidadePorId(model.CidadeSelecionada), model.Bairro, model.Logradouro, model.Numero, model.Complemento, model.Referencia, base.GetUsuarioLogado(), model.Conjuge, base.GetUsuarioLogado().Clinica);
                        var funcionario = new Funcionario(pessoa, model.TipoFuncionario);

                        #region [Especialidades]
                        foreach (var item in model.Especialidades)
                        {
                            var espec = _service.GetEspecialidadeById(item.IdEspecialidade);
                            if (espec != null)
                            {
                                funcionario.AddEspecialidades(espec);
                            }
                        }
                        #endregion

                        _service.SalvarFuncionario(funcionario);
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
        [Route("excluirFuncionarioById")]
        public HttpResponseMessage ExcluirFuncionario(int id)
        {
            try
            {
                var model = new FuncionarioViewModel();
                var result = _service.ObterFuncionarioById(id);

                if (result != null)
                {
                    result.Pessoa.SetExclusao(base.GetUsuarioLogado());
                    _service.ExcluirFuncionario(result);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do funcionario.");
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("listarProfissionaisSaude")]
        public HttpResponseMessage ListarProfissionaisSaude()
        {
            try
            {
                var model = new List<FuncionarioViewModel>();
                var result = _service.ListarProfissionaisSaude(base.GetUsuarioLogado().IdClinica);


                if (result != null)
                {
                    foreach (var item in result)

                        model.Add(new FuncionarioViewModel()
                        {
                            IdFuncionario = item.Pessoa.IdPessoa,
                            Nome = item.Pessoa.Nome,
                            Sexo = item.Pessoa.Sexo,
                            Tipo = item.Tipo,
                            QtdeAgendaDisponiveis = _service.QuantidadeAgendasDisponiveis(item.IdFuncionario),
                            Telefone1 = item.Pessoa.Telefone1,
                            Telefone2 = item.Pessoa.Telefone2,
                            Email = item.Pessoa.Email,
                            Especialidades = item.Especialidades.Select(a => new EspecialidadeViewModel()
                            {
                                IdEspecialidade = a.IdEspecialidade,
                                NmEspecialidade = a.NmEspecialidade
                            }).ToList(),
                            Situacao = item.Pessoa.Situacao
                        });
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do funcionario.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("listarEspecialidadesPorProfissionalSaude")]
        public HttpResponseMessage ListarEspecialidadesPorProfissionalSaude(int idprofissional)
        {
            try
            {
                var model = new List<EspecialidadeViewModel>();
                var result = _service.ListarEspecialidadesPorProfissionalSaude(idprofissional);

                if (result != null)
                {
                    foreach (var item in result)
                        model.Add(new EspecialidadeViewModel()
                        {
                            IdEspecialidade = item.IdEspecialidade,
                            NmEspecialidade = item.NmEspecialidade
                        });
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados especialidade.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("listarProcedimentosPorEspecialidade")]
        public HttpResponseMessage ListarProcedimentosPorEspecialidade(int idespecialidade)
        {
            try
            {
                var model = new List<ProcedimentosViewModel>();
                var result = _service.ListarProcedimentosPorEspecialidade(idespecialidade);

                if (result != null)
                {
                    foreach (var item in result)
                        model.Add(new ProcedimentosViewModel()
                        {
                            IdProcedimento = item.IdProcedimento,
                            NmProcedimento = item.NmProcedimento
                        });
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados procedimento.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        #endregion

        #region [Cadastro de pacientes]
        [HttpGet]
        [Route("getPacienteById")]
        public HttpResponseMessage ObterPacienteById(int id)
        {
            try
            {
                var model = new PacienteViewModel();
                var result = _service.ObterPacienteById(id);

                if (result != null)
                {
                    // dados paciente
                    model.IdPaciente = result.Pessoa.IdPessoa;
                    model.Tipo = result.Pessoa.Tipo;
                    model.Nome = result.Pessoa.Nome;
                    model.RazaoSocial = result.Pessoa.RazaoSocial;
                    model.NomeFantasia = result.Pessoa.Nome;
                    model.DataNascimento = result.Pessoa.DataNascimento;
                    model.CPF = result.Pessoa.CpfCnpj;
                    model.CartaoSus = result.CartaoSus;
                    model.CNPJ = result.Pessoa.CpfCnpj;
                    model.Mae = result.Pessoa.Mae;
                    model.Pai = result.Pessoa.Pai;
                    model.Observacao = result.Pessoa.Observacoes;
                    model.InscricaoEstadual = result.Pessoa.IE;
                    model.Observacao = result.Pessoa.Observacoes;
                    model.Sexo = result.Pessoa.Sexo;
                    model.Situacao = result.Pessoa.Situacao;
                    model.Idade = result.IdadeAtual();
                    model.Foto = result.Foto;
                    // endereço
                    model.Cep = result.Pessoa.Cep;
                    model.Logradouro = result.Pessoa.Logradouro;
                    model.Numero = result.Pessoa.Numero;
                    model.Bairro = result.Pessoa.Bairro;
                    model.EstadoSelecionado = result.Pessoa.IdEstado;
                    model.CidadeSelecionada = result.Pessoa.IdCidade;
                    model.Complemento = result.Pessoa.Complemento;
                    model.Referencia = result.Pessoa.Referencia;

                    // contatos 
                    model.Telefone1 = result.Pessoa.Telefone1;
                    model.Telefone2 = result.Pessoa.Telefone2;
                    model.Email = result.Pessoa.Email;

                    // convenios
                    if (result.Carteiras.Count > 0)
                    {
                        foreach (var item in result.Carteiras)
                            model.Carteiras.Add(new CarteiraViewModel()
                            {
                                IdCarteira = item.IdCarteira,
                                IdConvenio = item.IdConvenio,
                                IdPaciente = item.IdPaciente,
                                Convenio = item.Convenio.Pessoa.Nome,
                                NumeroCarteira = item.NumeroCarteira,
                                ValidadeCarteira = item.ValidadeCarteira,
                                Plano = item.Plano
                            });
                    }
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do paciente.");
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("pesquisarPacientes")]
        public HttpResponseMessage PesquisarPacientes(string nome, int? idPaciente)
        {
            try
            {
                var model = new List<PacienteViewModel>();
                var result = _service.PesquisarPacientes(nome, idPaciente, base.GetUsuarioLogado().IdClinica);

                if (result != null)
                {
                    foreach (var item in result)
                    {
                        model.Add(new PacienteViewModel()
                        {
                            IdPaciente = item.Pessoa.IdPessoa,
                            Nome = item.Pessoa.Nome,
                            Tipo = item.Pessoa.Tipo,
                            Telefone1 = item.Pessoa.Telefone1,
                            Telefone2 = item.Pessoa.Telefone2,
                            Email = item.Pessoa.Email,
                            Situacao = item.Pessoa.Situacao
                        });
                    }
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do paciente.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("listarPacientes")]
        public HttpResponseMessage ListarPacientes()
        {
            try
            {
                var model = new List<PacienteViewModel>();
                var result = _service.ListarPacientes(base.GetUsuarioLogado().IdClinica);

                if (result != null)
                {
                    foreach (var item in result)
                        model.Add(new PacienteViewModel()
                        {
                            IdPaciente = item.Pessoa.IdPessoa,
                            Nome = item.Pessoa.Nome,
                            Tipo = item.Pessoa.Tipo,
                            Telefone1 = item.Pessoa.Telefone1,
                            Telefone2 = item.Pessoa.Telefone2,
                            Sexo = item.Pessoa.Sexo,
                            Email = item.Pessoa.Email,
                            Situacao = item.Pessoa.Situacao
                        });
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do paciente.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("savePaciente")]
        public HttpResponseMessage SalvarPaciente(PacienteViewModel model)
        {
            try
            {
                if (model.IdPaciente > 0)
                {
                    var paciente = _service.ObterPacienteById(model.IdPaciente);
                    if (paciente == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do paciente.");

                    #region dados paciente 

                    if (model.Tipo == "PF")
                    {
                        paciente.Pessoa.SetNome(model.Nome.ToUpper());
                        paciente.Pessoa.SetDataNascimento(model.DataNascimento);
                        paciente.Pessoa.SetCpfCnpj(model.CPF);
                        paciente.Pessoa.SetSexo(model.Sexo);
                        paciente.Pessoa.SetMae(model.Mae);
                        paciente.Pessoa.SetPai(model.Pai);
                        paciente.Pessoa.SetDataNascimento(model.DataNascimento);
                        paciente.Pessoa.SetObservacoes(model.Observacao);
                        paciente.SetCartaoSus(model.CartaoSus);
                    }

                    paciente.Pessoa.SetTipo(model.Tipo);
                    paciente.Pessoa.SetSituacao(model.Situacao);
                    paciente.Pessoa.SetAlteracao(base.GetUsuarioLogado());
                    #endregion

                    #region dados endereço
                    var estado = _service.ObterEstadoPorId(model.EstadoSelecionado);
                    if (estado == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do estado.");

                    var cidade = _service.ObterCidadePorId(model.CidadeSelecionada);
                    if (cidade == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados da cidade.");

                    paciente.Pessoa.SetEndereco(model.Cep, estado, cidade, model.Bairro, model.Logradouro, model.Numero, model.Complemento, model.Referencia);
                    #endregion

                    #region contatos
                    paciente.Pessoa.SetTelefone1(model.Telefone1);
                    paciente.Pessoa.SetTelefone2(model.Telefone2);
                    paciente.Pessoa.SetEmail(model.Email);
                    #endregion

                    #region convenios
                    if (paciente.Carteiras.Count > 0)
                    {
                        _service.ExcluirCarteiraPaciente(paciente.Carteiras.ToList());
                    }


                    foreach (var item in model.Carteiras)
                    {
                        var convenio = _service.ObterConvenioById(item.IdConvenio);
                        if (convenio == null)
                            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do convênio.");

                        var cart = new Carteira(item.NumeroCarteira, item.Plano, item.ValidadeCarteira, convenio, paciente);
                        paciente.AddCarteiras(cart);
                    }
                    #endregion

                    if (model.Foto != null)
                    {
                        paciente.SetFoto(model.Foto);
                    }
                    _service.SalvarPaciente(paciente);
                }
                else
                {
                    if (model.Tipo == "PF")
                    {
                        model.CpfCnpj = model.CPF;

                    }
                    else if (model.Tipo == "PJ")
                    {
                        model.CpfCnpj = model.CNPJ;
                        model.Nome = model.NomeFantasia;
                    }

                    //TODO: criar dois construtores 
                    var pessoa = new Pessoa(model.Nome, model.RazaoSocial, model.Sexo, model.Tipo, model.DataNascimento, model.CpfCnpj, model.Rg, model.InscricaoEstadual, model.Profissao, model.Mae, model.Pai, model.Email,
                        model.Site, model.Telefone1, model.Telefone2, model.Observacao, model.Cep, _service.ObterEstadoPorId(model.EstadoSelecionado), _service.ObterCidadePorId(model.CidadeSelecionada), model.Bairro, model.Logradouro, model.Numero, model.Complemento, model.Referencia, base.GetUsuarioLogado(), model.Conjuge, base.GetUsuarioLogado().Clinica);
                    var paciente = new Paciente(pessoa, model.CartaoSus, "");

                    /*  if (model.Foto.Count() > 0)
                         paciente.SetFoto(model.Foto); */
                    if (model.Foto != null)
                    {
                        paciente.SetFoto(model.Foto);
                    }

                    #region convenios
                    foreach (var item in model.Carteiras)
                    {
                        var convenio = _service.ObterConvenioById(item.IdConvenio);
                        if (convenio == null)
                            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do convênio.");

                        var cart = new Carteira(item.NumeroCarteira, item.Plano, item.ValidadeCarteira, convenio, paciente);
                        paciente.AddCarteiras(cart);
                    }

                    #endregion
                    paciente.Pessoa.IdClinica = base.GetUsuarioLogado().IdClinica;
                    _service.SalvarPaciente(paciente);
                }

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("excluirPacienteById")]
        public HttpResponseMessage ExcluirPaciente(int id)
        {
            try
            {
                var model = new PacienteViewModel();
                var result = _service.ObterPacienteById(id);

                if (result != null)
                {
                    result.Pessoa.SetExclusao(base.GetUsuarioLogado());
                    _service.ExcluirPaciente(result);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do paciente.");
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion

        #region [Clinica]

        [AllowAnonymous]
        [HttpPost]
        [Route("uploadlogoclinica")]
        public async Task<Byte[]> CadastrarLogoClinica(int id)
        {

            var clinica = _service.ObterClinicaById(id);
            if (clinica == null)
                throw new Exception("Não foi possível recuperar dados da clinica!");

            string nomeArquivo = "";
            string diretorioRaiz = AppDomain.CurrentDomain.BaseDirectory + "imagens\\";
            var streamProvider = new MultipartMemoryStreamProvider();

            try
            {
                await Request.Content.ReadAsMultipartAsync(streamProvider);

                var fileStream = await streamProvider.Contents[0].ReadAsByteArrayAsync();


                if (fileStream != null && fileStream.Length > 0)
                {
                    var complemento = DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond;
                    //extrair o tipo do arquivo 
                    var extensao =
                        Path.GetExtension(streamProvider.Contents[0].Headers.ContentDisposition.FileName.Replace("\"", ""));
                    nomeArquivo = clinica.Nome.ToLower() + complemento + extensao;

                    File.WriteAllBytes(diretorioRaiz + nomeArquivo, fileStream);

                    System.Drawing.Image fullSizeImg = System.Drawing.Image.FromFile(diretorioRaiz + nomeArquivo);

                    System.Drawing.Image.GetThumbnailImageAbort dummyCallBack = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);

                    System.Drawing.Image thumbNailImg = fullSizeImg.GetThumbnailImage(240, 151, dummyCallBack, IntPtr.Zero);

                    String MyString = clinica.Nome.Trim().ToLower() + complemento + "_thumb.png";

                    //Save the thumbnail in Png format. You may change it to a diff format with the ImageFormat property
                    thumbNailImg.Save(diretorioRaiz + MyString, ImageFormat.Png);
                    byte[] imagem = imageToByteArray(thumbNailImg);

                    clinica.SetLogo(imagem);

                    thumbNailImg.Dispose();

                    _service.SalvarClinica(clinica);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return clinica.Logo;
        }

        [HttpPost]
        [Route("saveClinica")]
        public HttpResponseMessage SalvarClinica(ClinicaViewModel model)
        {
            try
            {
                var clinica = _service.ObterClinicaById(model.IdClinica);
                if (clinica == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados da clinica.");

                if (model.PrimeiroAcesso)
                {
                    /* #region dados endereço
                    var estado = _service.ObterEstadoPorId(model.IdEstado);
                    if (estado == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do estado.");

                    var cidade = _service.ObterCidadePorId(model.IdCidade);
                    if (cidade == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados da cidade.");

                    clinica.SetBairro(model.Bairro);
                    clinica.SetCep(model.Cep);
                    clinica.SetCidade(cidade);
                    clinica.SetComplemento(model.Complemento);
                    clinica.SetEstado(estado);
                    clinica.SetLogradouro(model.Logradouro);
                    clinica.SetNumero(model.Numero);
                    clinica.SetSituacao("Ativo");
                    #endregion */
                }
                else
                {
                    //#region dados endereço
                    //var estado = _service.ObterEstadoPorId(model.IdEstado);
                    //if (estado == null)
                    //    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do estado.");

                    //var cidade = _service.ObterCidadePorId(model.IdCidade);
                    //if (cidade == null)
                    //    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados da cidade.");

                    //clinica.SetBairro(model.Bairro);
                    //clinica.SetCep(model.Cep);
                    //clinica.SetCidade(cidade);
                    //clinica.SetComplemento(model.Complemento);
                    //clinica.SetEmail(model.Email);
                    //clinica.SetEstado(estado);
                    //clinica.SetFax(model.Fax);
                    //clinica.SetLogradouro(model.Logradouro);
                    //clinica.SetNome(model.Nome);
                    //clinica.SetCpfCnpj(model.CpfCnpj);
                    //clinica.SetNumero(model.Numero);
                    //clinica.SetSituacao("Ativo");
                    //clinica.SetTelefone1(model.Telefone1);
                    //clinica.SetTelefone2(model.Telefone2);

                    ////if (model.Unidades.Count > 0)
                    ////{
                    ////    //exclui as unidades
                    ////    foreach (var item in model.Unidades)
                    ////    {
                    ////        if (clinica.Unidades != null)
                    ////        {
                    ////            var existe = clinica.Unidades.Where(x => x.Nome.ToUpper() == item.Nome.ToUpper()).FirstOrDefault();
                    ////            if (existe == null)
                    ////                clinica.AddUnidade(new UnidadeAtendimento(item.Nome, clinica));
                    ////        }
                    ////        else
                    ////            clinica.AddUnidade(new UnidadeAtendimento(item.Nome, clinica));
                    ////    }
                    ////}

                    //#endregion

                    if (model.Logo != null)
                    {
                        if (model.Logo.Count() > 0)
                            clinica.SetLogo(model.Logo);
                    }
                }
                _service.SalvarClinica(clinica);
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("addUnidade")]
        public HttpResponseMessage IncluirUnidadeClinica(UnidadeAtendimentoViewModel model)
        {
            try
            {
                var clinica = _service.ObterClinicaById(base.GetUsuarioLogado().IdClinica);
                if (clinica == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados da clinica.");

                #region dados endereço
                var estado = _service.ObterEstadoPorId(model.IdEstado);
                if (estado == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do estado.");

                var cidade = _service.ObterCidadePorId(model.IdCidade);
                if (cidade == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados da cidade.");

                var unidade = clinica.Unidades.Where(x => x.IdUnidadeAtendimento == model.IdUnidadeAtendimento).FirstOrDefault();
                if (unidade == null)
                {
                    var uni = new UnidadeAtendimento(model.Nome, model.CpfCnpj, clinica, model.Cep, model.Logradouro, model.Bairro, model.Complemento, model.Numero, estado, cidade, model.Email, model.Telefone1, model.Telefone2, model.Fax);

                    if (model.Especialidades.Count > 0)
                    {

                        foreach (var item in model.Especialidades)
                        {
                            uni.AddEspecialidades(_service.GetEspecialidadeById(item.IdEspecialidade));
                        }
                    }
                    clinica.AddUnidade(uni);
                }
                else
                {
                    unidade.SetBairro(model.Bairro);
                    unidade.SetCpfCnpj(model.CpfCnpj);
                    unidade.SetCep(model.Cep);
                    unidade.SetCidade(cidade);
                    unidade.SetComplemento(model.Complemento);
                    unidade.SetEmail(model.Email);
                    unidade.SetEstado(estado);
                    unidade.SetFax(model.Fax);
                    unidade.SetLogradouro(model.Logradouro);
                    unidade.SetNome(model.Nome);
                    unidade.SetNumero(model.Numero);
                    unidade.SetTelefone1(model.Telefone1);
                    unidade.SetTelefone2(model.Telefone2);

                    _service.RemoverEspecialidadeUnidadeAtendimento(model.IdUnidadeAtendimento);
                    unidade.Especialidades.Clear();
                    if (model.Especialidades.Count > 0)
                    {
                        foreach (var item in model.Especialidades)
                        {
                            unidade.AddEspecialidades(_service.GetEspecialidadeById(item.IdEspecialidade));
                        }
                    }

                }
                #endregion

                _service.SalvarClinica(clinica);
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("listarUnidadesAtendimento")]
        public HttpResponseMessage ListarUnidadesAtendimento()
        {
            try
            {
                var model = new List<UnidadeAtendimentoViewModel>();
                var clinica = _service.ObterClinicaById(base.GetUsuarioLogado().IdClinica);


                if (clinica == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados da clinica.");
                if (clinica.Unidades != null)
                {
                    foreach (var item in clinica.Unidades.Where(x => x.Situacao != "Excluido"))
                    {
                        model.Add(new UnidadeAtendimentoViewModel
                        {
                            Bairro = item.Bairro,
                            Cep = item.Cep,
                            IdCidade = item.IdCidade,
                            Complemento = item.Complemento,
                            Email = item.Email,
                            Fax = item.Fax,
                            IdClinica = item.IdClinica,
                            IdEstado = item.IdEstado,
                            IdUnidadeAtendimento = item.IdUnidadeAtendimento,
                            Logradouro = item.Logradouro,
                            Nome = item.Nome,
                            Numero = item.Numero,
                            Telefone1 = item.Telefone1,
                            Telefone2 = item.Telefone2,
                            Situacao = item.Situacao
                        });
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
        [Route("getDadosUnidade")]
        public HttpResponseMessage ObterDadosUnidadeAtendimento(int id)
        {
            try
            {
                var model = new UnidadeAtendimentoViewModel();
                var clinica = _service.ObterClinicaById(base.GetUsuarioLogado().IdClinica);

                if (clinica == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados da clinica.");

                var unidade = clinica.Unidades.Where(x => x.IdUnidadeAtendimento == id).FirstOrDefault();

                if (unidade == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados da unidade de atendimento.");

                model.IdClinica = unidade.IdClinica;
                model.IdEstado = unidade.IdEstado;
                model.Logradouro = unidade.Logradouro;
                model.Nome = unidade.Nome;
                model.Numero = unidade.Numero;
                model.Telefone1 = unidade.Telefone1;
                model.Telefone2 = unidade.Telefone2;
                model.Bairro = unidade.Bairro;
                model.Cep = unidade.Cep;
                model.Complemento = unidade.Complemento;
                model.Email = unidade.Email;
                model.IdCidade = unidade.IdCidade;
                model.Fax = unidade.Fax;
                model.Clinica = clinica.Nome.ToUpper();
                model.IdUnidadeAtendimento = unidade.IdUnidadeAtendimento;
                model.CpfCnpj = unidade.CpfCnpj;

                if (unidade.Especialidades != null)
                {
                    foreach (var item in unidade.Especialidades)
                    {
                        model.Especialidades.Add(new EspecialidadeViewModel()
                        {
                            IdEspecialidade = item.IdEspecialidade,
                            NmEspecialidade = item.NmEspecialidade
                        });
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
        [Route("excluirUnidade")]
        public HttpResponseMessage ExcluirUnidadeAtendimento(int id)
        {
            try
            {
                var clinica = _service.ObterClinicaById(base.GetUsuarioLogado().IdClinica);

                if (clinica == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados da clinica.");

                var unidade = clinica.Unidades.Where(x => x.IdUnidadeAtendimento == id).FirstOrDefault();

                if (unidade == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados da unidade de atendimento.");


                _service.ExcluirUnidadeAtendimento(unidade);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getDadosClinica")]
        public HttpResponseMessage ObterDadosClinica()
        {
            try
            {
                var model = new ClinicaViewModel();
                var result = _service.ObterClinicaById(base.GetUsuarioLogado().IdClinica);

                if (result == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados da clinica.");

                model.IdClinica = result.IdClinica;
                model.Logo = result.Logo;
                model.Nome = result.Nome;
                model.Situacao = result.Situacao;
                model.Unidades = RetornaUnidadesClinica(result.Unidades.ToList());

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public List<UnidadeAtendimentoViewModel> RetornaUnidadesClinica(List<UnidadeAtendimento> unidades)
        {
            var lista = new List<UnidadeAtendimentoViewModel>();
            if (unidades != null)
            {
                foreach (var item in unidades.Where(x => x.Situacao != "Excluido"))
                {
                    lista.Add(new UnidadeAtendimentoViewModel { IdUnidadeAtendimento = item.IdUnidadeAtendimento, Nome = item.Nome });
                }
            }

            return lista;
        }


        #endregion

        #region [Tabela de Preço]

        [HttpGet]
        [Route("getTabelaById")]
        public HttpResponseMessage ObterTabelaById(int id)
        {
            try
            {
                var model = new TabelaPrecoViewModel();

                var result = _service.ObterTabelaById(id);

                if (result != null)
                {
                    model.Clinica = result.Clinica.Nome;
                    model.Convenio = result.Convenio == null ? "Particular" : result.Convenio.Pessoa.Nome;
                    model.IdClinica = result.IdClinica;
                    model.IdConvenio = result.IdConvenio ?? 0;
                    model.IdTabelaPreco = result.IdTabelaPreco;
                    model.Situacao = result.Situacao;
                    model.Nome = result.Nome;
                    model.Tipo = result.Tipo;
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados da Tabela de preço.");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("saveTabela")]
        public HttpResponseMessage SalvarTabelaPreco(TabelaPrecoViewModel model)
        {
            try
            {
                if (model.IdTabelaPreco > 0)
                {
                    var tabela = _service.ObterTabelaById(model.IdTabelaPreco);

                    tabela.SetNome(model.Nome);
                    tabela.SetTipo(model.Tipo);

                    _service.SalvarTabelaPreco(tabela);
                }
                else
                {
                    var clinica = _service.ObterClinicaById(base.GetUsuarioLogado().IdClinica);
                    if (clinica == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados da clinica.");


                    var nova = new TabelaPreco(model.Nome.ToUpper(), model.Tipo, clinica);

                    if (model.IdConvenio > 0)
                    {
                        var convenio = _service.ObterConvenioById(model.IdConvenio);
                        if (convenio == null)
                            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do convênio.");

                        nova.SetConvenio(convenio);
                    }


                    _service.SalvarTabelaPreco(nova);
                }

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getProcedimentoTabela")]
        public HttpResponseMessage GetProcedimentosByTabela(int idproc, int id)
        {
            try
            {
                var model = new TabelaPrecoItensViewModel();

                var item = _service.GetProcedimentosByTabela(idproc, id);
                if (item != null)
                {
                    model.IdProcedimento = item.IdProcedimento;
                    model.IdTabelaPreco = item.IdTabelaPreco;
                    model.Valor = item.Valor;
                    model.ValorProfissional = item.ValorProfissional;
                    model.Procedimento = ConvertProcedimentoParaViewModel(item.Procedimento);
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getAllTabelasPorConvenioAtivas")]
        public HttpResponseMessage GetAllTabelasPorConvenioAtivas(string tipo, int id)
        {
            try
            {
                var model = new List<TabelaPrecoViewModel>();

                var result = _service.ListarTabelasPrecoPorConvenioAtivas(tipo, id, base.GetUsuarioLogado().IdClinica);

                foreach (var item in result)
                {
                    model.Add(new TabelaPrecoViewModel
                    {
                        Clinica = item.Clinica.Nome,
                        Convenio = item.Convenio == null ? "Particular" : item.Convenio.Pessoa.Nome,
                        IdClinica = item.IdClinica,
                        IdConvenio = item.IdConvenio ?? 0,
                        IdTabelaPreco = item.IdTabelaPreco,
                        Situacao = item.Situacao,
                        Nome = item.Nome,
                        Tipo = item.Tipo
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
        [Route("getAllTabelasPorConvenio")]
        public HttpResponseMessage GetAllTabelasPorConvenio(string tipo, int id)
        {
            try
            {
                var model = new List<TabelaPrecoViewModel>();

                var result = _service.ListarTabelasPrecoPorConvenio(tipo, id, base.GetUsuarioLogado().IdClinica);

                foreach (var item in result)
                {
                    model.Add(new TabelaPrecoViewModel
                    {
                        Clinica = item.Clinica.Nome,
                        Convenio = item.Convenio == null ? "Particular" : item.Convenio.Pessoa.Nome,
                        IdClinica = item.IdClinica,
                        IdConvenio = item.IdConvenio ?? 0,
                        IdTabelaPreco = item.IdTabelaPreco,
                        Situacao = item.Situacao,
                        Nome = item.Nome,
                        Tipo = item.Tipo
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
        [Route("listarTabelas")]
        public HttpResponseMessage ListarTabelas()
        {
            try
            {
                var model = new List<TabelaPrecoViewModel>();
                var result = _service.ListarTabelasPreco(base.GetUsuarioLogado().IdClinica);

                foreach (var item in result)
                {
                    model.Add(new TabelaPrecoViewModel
                    {
                        Clinica = item.Clinica.Nome,
                        Convenio = item.Convenio == null ? "Particular" : item.Convenio.Pessoa.Nome,
                        IdClinica = item.IdClinica,
                        IdConvenio = item.IdConvenio ?? 0,
                        IdTabelaPreco = item.IdTabelaPreco,
                        Situacao = item.Situacao,
                        Nome = item.Nome,
                        Tipo = item.Tipo
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
        [Route("excluirTabela")]
        public HttpResponseMessage ExcluirTabelaPreco(int id)
        {
            try
            {
                var tabela = _service.ObterTabelaById(id);
                _service.ExcluirTabelaPreco(tabela);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getprocedimentosByTabela")]
        public HttpResponseMessage GetprocedimentosByTabela(int id)
        {
            try
            {
                var model = new List<TabelaPrecoItensViewModel>();
                var result = _service.ListarItensTabelasPreco(id);

                foreach (var item in result)
                {
                    model.Add(new TabelaPrecoItensViewModel
                    {
                        IdProcedimento = item.IdProcedimento,
                        IdTabelaPreco = item.IdTabelaPreco,
                        Valor = item.Valor,
                        ValorProfissional = item.ValorProfissional,
                        //TabelaPreco = ConvertTabelaPrecoParaViewModel(item.TabelaPreco),
                        Procedimento = ConvertProcedimentoParaViewModel(item.Procedimento)
                    });
                }

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public ProcedimentoViewModel ConvertProcedimentoParaViewModel(Procedimento procedimento)
        {
            return new ProcedimentoViewModel
            {
                Codigo = procedimento.Codigo,
                IdProcedimento = procedimento.IdProcedimento,
                NomeProcedimento = procedimento.NomeProcedimento
            };
        }

        [HttpPost]
        [Route("saveprocedimentostabela")]
        public HttpResponseMessage SalvarItensTabelaPreco(TabelaPrecoViewModel model)
        {
            try
            {
                var tabela = _service.ObterTabelaById(model.IdTabelaPreco);

                if (tabela == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados da tabela de preço.");

                if (tabela.Itens == null)
                {
                    foreach (var item in model.Itens)
                    {
                        var proc = _service.ObterProcedimentoById(item.IdProcedimento);
                        if (proc == null)
                            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do procedimento.");

                        var itemTab = new TabelaPrecoItens(item.Valor, item.ValorProfissional, proc, tabela);
                        tabela.AddIten(itemTab);
                    }
                }
                else
                {
                    _service.ExcluirItensTabela(tabela.Itens, model.IdTabelaPreco);

                    foreach (var item in model.Itens)
                    {
                        var proc = _service.ObterProcedimentoById(item.IdProcedimento);
                        if (proc == null)
                            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível obter dados do procedimento.");

                        var itemTab = new TabelaPrecoItens(item.Valor, item.ValorProfissional, proc, tabela);
                        tabela.AddIten(itemTab);
                    }
                }

                _service.SalvarTabelaPreco(tabela);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        public TabelaPrecoViewModel ConvertTabelaPrecoParaViewModel(TabelaPreco tabela)
        {
            return new TabelaPrecoViewModel
            {
                Clinica = tabela.Clinica.Nome,
                Convenio = tabela.Convenio == null ? "Particular" : tabela.Convenio.Pessoa.Nome,
                IdClinica = tabela.IdClinica,
                IdConvenio = tabela.IdConvenio ?? 0,
                Nome = tabela.Nome,
                IdTabelaPreco = tabela.IdTabelaPreco,
                Tipo = tabela.Tipo
            };
        }
        #endregion

        #region [Consultorio]

        [HttpGet]
        [Route("obterConsultorioPorId")]
        public HttpResponseMessage ObterConsultorioPorId(int id)
        {
            try
            {
                var model = new ConsultorioViewModel();
                var result = _service.ObterConsultorioPorId(id);

                if (result != null)
                {
                    model.NmClinica = result.Clinica.Nome;
                    model.IdConsultorio = result.IdConsultorio;
                    model.NmConsultorio = result.NmConsultorio;
                    model.IdClinica = result.IdClinica;
                    model.IdUnidadeAtendimento = result.IdUnidadeAtendimento;
                }
                else
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do consultório");

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("excluirConsultorio")]
        public HttpResponseMessage ExcluirConsultorio(int id)
        {
            try
            {
                if (_service.ObterConsultorioPorId(id) == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do consultório");
                else
                    _service.ExcluirConsultorio(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("salvarConsultorio")]
        public HttpResponseMessage SalvarConsultorio(ConsultorioViewModel model)
        {
            try
            {

                if (model.IdConsultorio > 0)
                {
                    var consultorio = _service.ObterConsultorioPorId(model.IdConsultorio);
                    if (consultorio == null)
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do consultório");
                    else
                        consultorio.SetNmConsultorio(model.NmConsultorio);
                    var unidadeAtendimento = _service.ObterClinicaById(base.GetUsuarioLogado().IdClinica).Unidades.First(x => x.IdUnidadeAtendimento == model.IdUnidadeAtendimento);
                    consultorio.SetUnidadeAtendimento(unidadeAtendimento);
                    _service.SalvarConsultorio(consultorio);
                }
                else
                {

                    var unidadeAtendimento = _service.ObterClinicaById(base.GetUsuarioLogado().IdClinica).Unidades.First(x => x.IdUnidadeAtendimento == model.IdUnidadeAtendimento);
                    var consultorio = new Consultorio(model.NmConsultorio, base.GetUsuarioLogado().Clinica, unidadeAtendimento);
                    _service.SalvarConsultorio(consultorio);
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("listarConsultorios")]
        public HttpResponseMessage ListarConsultorios()
        {
            try
            {
                var model = new List<ConsultorioViewModel>();
                var result = _service.ListarConsultorios(base.GetUsuarioLogado().IdClinica);

                foreach (var item in result)
                {
                    model.Add(new ConsultorioViewModel
                    {
                        NmConsultorio = item.NmConsultorio,
                        IdConsultorio = item.IdConsultorio,
                        NmClinica = item.Clinica.Nome,
                        NmUnidadeAtendimento = item.UnidadeAtendimento.Nome
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

        #region [Busca Pessoa]

        [HttpGet]
        [Route("getPessoaPorNome")]
        public HttpResponseMessage ObterPessoaPorNome(string nome)
        {
            try
            {
                var model = new List<PessoaViewModel>();
                var result = _service.ListarPessoaPorNome(nome, base.GetUsuarioLogado().IdClinica);

                foreach (var item in result)
                {
                    model.Add(new PessoaViewModel
                    {
                        Email = item.Email,
                        CpfCnpj = item.CpfCnpj,
                        IdCidade = item.IdCidade,
                        IdPessoa = item.IdPessoa,
                        Mae = item.Mae,
                        Nome = item.Nome,
                        Situacao = item.Situacao,
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