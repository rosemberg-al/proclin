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
    [RoutePrefix("estoque")]
    [Authorize]
    public class EstoqueController : BaseController
    {
        private readonly IBuscaService _serviceBusca;
        private readonly IUsuarioService _usuarioService;
        private readonly ICadastroService _cadastroService;
        private readonly IEstoqueService _estoqueService;

        public EstoqueController(IBuscaService serviceBusca, IUsuarioService usuarioservice, ICadastroService cadastroService, IEstoqueService estoqueService) : base(usuarioservice)
        {
            _serviceBusca = serviceBusca;
            _usuarioService = usuarioservice;
            _cadastroService = cadastroService;
            _estoqueService = estoqueService;
        }

        [HttpGet]
        [Route("listaMateriais")]
        public HttpResponseMessage ListaMateriais()
        {
            try
            {
                var consulta = _estoqueService.ListaMateriais(base.GetUsuarioLogado().IdUnidadeAtendimento);
                var model = new List<MaterialViewModel>();

                foreach (var item in consulta)
                {
                    model.Add(new MaterialViewModel()
                    {
                        IdMaterial = item.IdMaterial,
                        Nome = item.Nome,
                        EstoqueAtual = item.EstoqueAtual,
                        EstoqueMinimo = item.EstoqueMinimo,
                        Situacao = item.Situacao
                    });
                }
                return Request.CreateResponse(HttpStatusCode.OK, model.OrderBy(x=>x.Nome));
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        
        [HttpGet]
        [Route("obterMaterialPorId")]
        public HttpResponseMessage ObterMaterialPorId(int id)
        {
            try
            {
                var consulta = _estoqueService.ObterMaterialPorId(id);
                var model = new MaterialViewModel();

                if (consulta != null)
                {
                    model.IdMaterial = consulta.IdMaterial;
                    model.Nome = consulta.Nome;
                    model.EstoqueMinimo = consulta.EstoqueMinimo;
                    model.Marca = consulta.Marca;
                    model.Modelo = consulta.Modelo;
                    model.Observacao = consulta.Observacao;
                    model.EstadoConservacao = consulta.EstadoConservacao;
                    model.Identificador = consulta.Identificador;
                    model.ValorVenda = consulta.ValorVenda;
                    model.ValorCompra = consulta.ValorCompra;
                    model.Situacao = consulta.Situacao;
                    model.EstoqueAtual = consulta.EstoqueAtual;
                    model.NmUnidadeAtendimento = consulta.UnidadeAtendimento.Nome;
                    model.NmTipoMaterial = consulta.TipoMaterial.Nome;
                    model.IdTipoMaterial = consulta.IdTipoMaterial;
                    model.IdUnidadeAtendimento = consulta.IdUnidadeAtendimento;
                }
                else
                {
                    throw new Exception(" Não foi possível obter dados do Material ");
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("excluirMaterial")]
        public HttpResponseMessage ExcluirMaterial(int id)
        {
            try
            {
                var consulta = _estoqueService.ObterMaterialPorId(id);
                if (consulta != null)
                    _estoqueService.ExcluirMaterial(consulta);
                else
                    throw new Exception("Não foi possível obter dados do material ");

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        
        [HttpPost]
        [Route("salvarMaterial")]
        public HttpResponseMessage SalvarMaterial(MaterialViewModel model)
        {
            try
            {
                if (model.IdMaterial > 0)
                {
                    var material = _estoqueService.ObterMaterialPorId(model.IdMaterial);
                    material.SetNome(model.Nome);
                    material.SetEstoqueMinimo(model.EstoqueMinimo);
                    material.SetMarca(model.Marca);
                    material.SetModelo(model.Modelo);
                    material.SetObservacao(model.Observacao);
                    material.SetEstadoConservacao(model.EstadoConservacao);
                    material.SetIdentificador(model.Identificador);
                    material.SetValorVenda(model.ValorVenda);
                    material.SetValorCompra(model.ValorCompra);
                    //material.SetEstoqueAtual(model.Nome);
                    material.SetTipoMaterial(_estoqueService.ObterTipoMaterialPorId(model.IdTipoMaterial));
                    material.SetUnidadeAtendimento(_cadastroService.ObterUnidadeAtendimentoPorId(model.IdUnidadeAtendimento));
                    _estoqueService.SalvarMaterial(material);
                }
                else
                {

                    var material = new Material(model.Nome, model.EstoqueMinimo, model.Marca, model.Modelo, model.Observacao, model.EstadoConservacao, model.Identificador, model.ValorVenda, model.ValorCompra, model.EstoqueAtual, _estoqueService.ObterTipoMaterialPorId(model.IdTipoMaterial), _cadastroService.ObterUnidadeAtendimentoPorId(base.GetUsuarioLogado().IdUnidadeAtendimento));
                    _estoqueService.SalvarMaterial(material);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("listaTipoMateriais")]
        public HttpResponseMessage ListaTipoMateriais()
        {
            try
            {
                var consulta = _estoqueService.ListaTipoMateriais(base.GetUsuarioLogado().IdUnidadeAtendimento);
                var model = new List<TipoMaterialViewModel>();

                foreach (var item in consulta)
                {
                    model.Add(new TipoMaterialViewModel()
                    {
                        IdTipoMaterial = item.IdTipoMaterial,
                        Nome = item.Nome,
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
        [Route("excluirTipoMaterial")]
        public HttpResponseMessage ExcluirTipoMaterial(int id)
        {
            try
            {
                var tipo = _estoqueService.ObterTipoMaterialPorId(id);
                if (tipo == null)
                    throw new Exception("Não foi possível obter daddos Tipo de Material");

                var consulta = _estoqueService.ListaMateriais(base.GetUsuarioLogado().IdUnidadeAtendimento).Where(x => x.IdTipoMaterial == id).ToList();
                if (consulta.Count > 0)
                    throw new Exception("Não foi possível realizar a exclusão. Existem materiais associados a este Tipo");

                _estoqueService.ExcluirTipoMaterial(tipo);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("obterTipoMaterialPorId")]
        public HttpResponseMessage ObterTipoMaterialPorId(int id)
        {
            try
            {
                var tipo = _estoqueService.ObterTipoMaterialPorId(id);
                var model = new TipoMaterialViewModel();
                model.IdTipoMaterial = tipo.IdTipoMaterial;
                model.Nome = tipo.Nome;
                model.Situacao = tipo.Situacao;

                return Request.CreateResponse(HttpStatusCode.OK,model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("salvarTipoMaterial")]
        public HttpResponseMessage SalvarTipoMaterial(TipoMaterialViewModel model)
        {
            try
            {
                if (model.IdTipoMaterial > 0)
                {
                    var tipo = _estoqueService.ObterTipoMaterialPorId(model.IdTipoMaterial);
                    tipo.SetNome(model.Nome);
                    tipo.SetSituacao(model.Situacao);
                    _estoqueService.SalvarTipoMaterial(tipo);
                }
                else
                {
                    var tipo = new TipoMaterial(model.Nome, base.GetUsuarioLogado().UnidadeAtendimento);
                    _estoqueService.SalvarTipoMaterial(tipo);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("salvarMovimentoEstoque")]
        public HttpResponseMessage SalvarMovimentoEstoque(MovimentoEstoqueViewModel model)
        {
            try
            {
                var material = _estoqueService.ObterMaterialPorId(model.IdMaterial);
                material.GerarMovimentoEstoque(model.Quantidade, model.Tipo, base.GetUsuarioLogado().UnidadeAtendimento);
                _estoqueService.SalvarMaterial(material);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Não UTILIZAR ESSA FUNCIONALIDADE 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("excluirMovimentoEstoque")]
        public HttpResponseMessage ExcluirMovimentoEstoque(int id)
        {
            try
            {
                var mov = _estoqueService.ObterMovimentoEstoquePorId(id);

                // salva material 
               /*  var mat = _estoqueService.ObterMovimentoEstoquePorId(id).Material;
                mat.GerarMovimentoEstoque(mov.Quantidade,mov.Tipo, base.GetUsuarioLogado().UnidadeAtendimento);
                _estoqueService.SalvarMaterial(mat); */

                // exclui movimento estoque
                _estoqueService.ExcluirMovimentoEstoque(_estoqueService.ObterMovimentoEstoquePorId(id));

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("listarMovimentoEstoque")]
        public HttpResponseMessage ListarMovimentoEstoque()
        {
            try
            {
                var lista = _estoqueService.ListarMovimentoEstoque(base.GetUsuarioLogado().IdUnidadeAtendimento);
                var model = new List<MovimentoEstoqueViewModel>();
                foreach(var item in lista)
                {
                    model.Add(new MovimentoEstoqueViewModel()
                    {
                        Data = item.Data,
                        Quantidade = item.Quantidade,
                        IdMaterial= item.IdMaterial,
                        Situacao = item.Situacao,
                        Nome = item.Material.Nome,
                        Tipo = item.Tipo,
                        IdMovimentoEstoque = item.IdMovimentoEstoque
                    });
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



    }
}