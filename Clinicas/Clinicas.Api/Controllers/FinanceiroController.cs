using AutoMapper;
using Clinicas.Api.Models;
using Clinicas.Application.Services.Interfaces;
using Clinicas.Domain.Model;
using Clinicas.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Clinicas.Api.Controllers
{
    [RoutePrefix("financeiro")]
    [Authorize]
    public class FinanceiroController : BaseController
    {
        private readonly IFinanceiroService _financeiroService;
        private readonly ICadastroService _cadastroService;
        private readonly IUsuarioService _usuarioService;

        public FinanceiroController(IFinanceiroService financeiroService, ICadastroService cadastroService, IUsuarioService usuarioService) : base(usuarioService)
        {
            this._financeiroService = financeiroService;
            this._cadastroService = cadastroService;
            this._usuarioService = usuarioService;
        }


        #region [Financeiro]
        [HttpGet]
        [Route("excluirParcela")]
        public HttpResponseMessage ExcluirParcela(int idFinanceiro, int idParcela)
        {
            try
            {
                var parcela = _financeiroService.ObterFinanceiroParcelaPorId(idParcela);
                var financeiro = _financeiroService.ObterFinanceiroPorId(idFinanceiro);

                if (financeiro == null)
                    throw new Exception("Não foi possível recuperar dados do financeiro.");

                if (parcela == null)
                    throw new Exception("Não foi possível recuperar a parcela.");

                financeiro.ExcluirParcela(parcela, base.GetUsuarioLogado());

                _financeiroService.SalvarFinanceiro(financeiro);

                return Request.CreateResponse(HttpStatusCode.OK, idParcela);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("alterarParcela")]
        public HttpResponseMessage AlterarParcela(FinanceiroParcelaModel model)
        {
            try
            {
                var parcela = _financeiroService.ObterFinanceiroParcelaPorId(model.IdParcela);

                if (parcela == null)
                    throw new Exception("Não foi possível recuperar a parcela.");

                parcela.SetSituacao(model.Situacao);
                var plano = _financeiroService.ObterPlanodeContasPorId(model.IdPlanoConta ?? 0);
                parcela.SetPlanoConta(plano);

                var conta = _financeiroService.ObterContaPorId(model.IdConta);
                parcela.SetConta(conta);


                if (model.Situacao == "Baixado")
                {
                    parcela.SetTotalAcerto((decimal)model.TotalAcerto);
                    parcela.SetDataAcerto(model.DataAcerto ?? DateTime.MinValue);
                    var meiopagamento = _financeiroService.GetMeioPagamentoPorId(model.IdMeioPagamento);
                    parcela.SetMeioPagamento(meiopagamento);
                }
                else
                {
                    parcela.SetTotalAcerto(0);
                }
                parcela.SetUsuarioAlteracao(null);

                if (model.Situacao == "Baixado")
                    parcela.SetUsuarioBaixar(null);


                if (model.ValorDesconto > model.Valor)
                    throw new Exception(" O Valor do desconto não pode ser maior que o valor da parcela ");

                parcela.SetValorDesconto((decimal)model.ValorDesconto);
                parcela.SetValorAcrescimo((decimal)model.ValorAcrescimo);
                parcela.SetObservacao(model.Observacao);

                _financeiroService.AlterarParcela(parcela);


                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [System.Web.Http.HttpPost]
        [Route("salvarFinanceiro")]
        public HttpResponseMessage IncluirFinanceiro(FinanceiroModel model)
        {
            try
            {
                if (model.IdFinanceiro == 0)
                {
                    var pessoa = _cadastroService.ObterPessoaPorId(model.IdPessoa);
                    if (pessoa == null)
                        throw new Exception("Não foi possivel recuperar dados de pessoa");

                    string tipo = model.Tipo == "P" ? "Contas a Pagar" : "Contas a Receber";

                    var usuarioLogado = base.GetUsuarioLogado();
                    var financeiro = new Financeiro(pessoa, tipo, model.FormaPagamento, usuarioLogado.Clinica, usuarioLogado.UnidadeAtendimento);

                    var conta = _financeiroService.ObterContaPorId(model.IdConta);
                    var planoConta = _financeiroService.ObterPlanodeContasPorId(model.IdPlanoConta);

                    if (model.FormaPagamento == "A VISTA")
                    {
                        var parcela = new FinanceiroParcela(1,model.DataVencimento,(decimal)model.Total, DateTime.Now, conta, planoConta, null);
                        parcela.MeioPagamento = _financeiroService.ObterMeioPagamentoPorId(1);
                        parcela.Situacao = "Baixado";
                        parcela.UsuarioBaixa = base.GetUsuarioLogado();
                        parcela.TotalAcerto = model.Total;
                        parcela.DataAcerto = model.DataVencimento;
                        financeiro.AddParcela(parcela);
                    }
                    else // A PRAZO
                    {
                        if (model.Total != model.Parcelas.Sum(x => x.Valor))
                        {
                            throw new Exception(" O Total está diferente do valor das parcelas ");
                        }
                        foreach (var item in model.Parcelas)
                        {
                            financeiro.AddParcela(new FinanceiroParcela(item.NumeroParcela, item.DataVencimento, item.Valor, DateTime.Now, conta, planoConta, null));
                        }
                    }

                    _financeiroService.SalvarFinanceiro(financeiro);
                    return Request.CreateResponse(HttpStatusCode.OK, model);
                }
                return Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getTransferencias")]
        public List<TransferenciaContaModel> GetTransferencias()
        {
            var result = new List<TransferenciaContaModel>();
            var transferencias = _financeiroService.GetTransferencias();

            foreach (var item in transferencias)
            {
                result.Add(new TransferenciaContaModel
                {
                    Data = item.Data,
                    Descricao = item.Descricao,
                    IdContaDestino = item.IdContaDestino,
                    IdContaOrigem = item.IdContaOrigem,
                    IdTransferenciaConta = item.IdTransferenciaConta,
                    Valor = item.Valor,
                    ContaOrigem = new ContaModel { IdConta = item.ContaOrigem.IdConta, Nome = item.ContaOrigem.NmConta },
                    ContaDestino = new ContaModel { IdConta = item.ContaDestino.IdConta, Nome = item.ContaDestino.NmConta }
                });
            }

            return result;
        }

        [HttpGet]
        [Route("getTransferenciaById")]
        public TransferenciaContaModel GetTransferenciaById(int id)
        {
            var result = new TransferenciaContaModel();

            var item = _financeiroService.GetTransferenciaById(id);

            result.Data = item.Data;
            result.Descricao = item.Descricao;
            result.IdContaDestino = item.IdContaDestino;
            result.IdContaOrigem = item.IdContaOrigem;
            result.IdTransferenciaConta = item.IdTransferenciaConta;
            result.Valor = item.Valor;
            result.ContaOrigem = new ContaModel { IdConta = item.ContaOrigem.IdConta, Nome = item.ContaOrigem.NmConta };
            result.ContaDestino = new ContaModel { IdConta = item.ContaDestino.IdConta, Nome = item.ContaDestino.NmConta };
            return result;
        }

        [HttpGet]
        [Route("excluirTransferencia")]
        public void ExcluirTransferencia(int id)
        {
            var item = _financeiroService.GetTransferenciaById(id);
            if (item == null)
                throw new Exception("Não foi possível recupeerar a tranferência.");

            _financeiroService.ExcluirTransferencia(item);
        }

        [HttpPost]
        [Route("saveTransferencia")]
        public TransferenciaContaModel SalvarTransferencia(TransferenciaContaModel model)
        {
            if (model.IdTransferenciaConta == 0)
            {
                var contaOrigem = _financeiroService.ObterContaPorId(model.IdContaOrigem);
                var contaDestino = _financeiroService.ObterContaPorId(model.IdContaDestino);
                var transferencia = new TransferenciaConta(contaOrigem, contaDestino, model.Valor, DateTime.Now, model.Descricao);
                _financeiroService.SalvarTransferencia(transferencia);
            }
            else
            {
                var transferencia = _financeiroService.GetTransferenciaById(model.IdTransferenciaConta);

                transferencia.Descricao = model.Descricao;
                transferencia.ContaDestino = _financeiroService.ObterContaPorId(model.IdContaDestino);
                transferencia.ContaOrigem = _financeiroService.ObterContaPorId(model.IdContaOrigem);
                transferencia.Valor = model.Valor;

                _financeiroService.SalvarTransferencia(transferencia);
            }
            return model;
        }

        [HttpGet]
        [Route("getById")]
        public FinanceiroParcelaModel GetFinanceiroParcelaModeloById(int id)
        {
            var item = _financeiroService.ObterFinanceiroParcelaPorId(id);
            var result = new FinanceiroParcelaModel();

            if (item == null)
                throw new Exception("Não foi possivel recuperar dados da parcela.");


            result.DataInicio = item.DataInclusao;
            result.DataPagamento = item.DataBaixa;
            result.DataAcerto = item.DataAcerto;
            result.NumeroParcela = item.Numero;
            result.NomeCliente = item.Financeiro.Pessoa?.Nome;
            result.Situacao = item.Situacao;
            result.DataVencimento = item.DataVencimento;
            result.Valor = item.Valor;
            result.NomeCliente = item.Financeiro?.Pessoa?.Nome;
            result.PlanoConta = item.PlanoConta?.NmPlanoConta;
            result.IdParcela = item.IdParcela;
            result.TotalAcerto = item.TotalAcerto;
            result.IdFinanceiro = item.IdFinanceiro;
            result.TipoConta = item.Financeiro.Tipo;
            result.Conta = item.Conta.NmConta;
            result.IdConta = item.Conta.IdConta;
            result.IdMeioPagamento = item.MeioPagamento != null ? item.MeioPagamento.IdMeioPagamento : 0;
            result.DataAcerto = item.DataAcerto;
            result.ValorAcerto = item.TotalAcerto;
            result.ValorDesconto = item.ValorDesconto;
            result.ValorAcrescimo = item.ValorAcrescimo;
            result.IdPlanoConta = item.IdPlanoConta;
            result.Observacao = item.Observacao;
            return result;
        }

        [HttpGet]
        [Route("pesquisar")]
        public HttpResponseMessage PesquisarParcelas(string tipo, string descricao, string tipoConta)
        {
            try
            {
                var result = new List<FinanceiroParcelaModel>();
                var usuarioLogado = base.GetUsuarioLogado();

                var model = _financeiroService.ListarParcelasPesquisa(tipo, descricao, tipoConta, usuarioLogado.IdClinica,usuarioLogado.IdUnidadeAtendimento);

                foreach (var item in model)
                {
                    result.Add(new FinanceiroParcelaModel
                    {
                        DataInicio = item.DataInclusao,
                        DataPagamento = item.DataBaixa,
                        Numero = item.Numero,
                        DataAcerto = item.DataAcerto,
                        NumeroParcela = item.Numero,
                        NomeCliente = item.Financeiro.Pessoa?.Nome,
                        Situacao = item.Situacao,
                        DataVencimento = item.DataVencimento,
                        ValorAcerto = item.TotalAcerto,
                        Valor = item.Valor,
                        PlanoConta = item.PlanoConta?.NmPlanoConta,
                        IdParcela = item.IdParcela,
                        TotalAcerto = item.TotalAcerto,
                        IdFinanceiro = item.IdFinanceiro
                    });
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getContasApagarReceber")]
        public ICollection<FinanceiroParcelaModel> GetContasApagarReceber(string tipo)
        {
            var result = new List<FinanceiroParcelaModel>();
            var usuarioLogado = base.GetUsuarioLogado();

            if (tipo == "R")//contas a receber
            {
                
                var model = _financeiroService.ListarContasaReceber(usuarioLogado.IdClinica,usuarioLogado.IdUnidadeAtendimento);

                foreach (var item in model)
                {
                    result.Add(new FinanceiroParcelaModel
                    {
                        DataInicio = item.DataInclusao,
                        DataPagamento = item.DataBaixa,
                        Numero = item.Numero,
                        DataAcerto = item.DataAcerto,
                        NumeroParcela = item.Numero,
                        NomeCliente = item.Financeiro.Pessoa?.Nome,
                        Situacao = item.Situacao,
                        DataVencimento = item.DataVencimento,
                        ValorAcerto = item.TotalAcerto ?? 0,
                        Valor = item.Valor,
                        PlanoConta = item.PlanoConta?.NmPlanoConta,
                        IdParcela = item.IdParcela,
                        TotalAcerto = item.TotalAcerto ?? 0,
                        IdFinanceiro = item.IdFinanceiro
                    });
                }
            }
            else
            {
                var model = _financeiroService.ListarContasaPagar(usuarioLogado.IdClinica, usuarioLogado.IdUnidadeAtendimento);

                foreach (var item in model)
                {
                    result.Add(new FinanceiroParcelaModel
                    {
                        DataInicio = item.DataInclusao,
                        DataPagamento = item.DataBaixa,
                        DataAcerto = item.DataAcerto,
                        Numero = item.Numero,
                        NumeroParcela = item.Numero,
                        NomeCliente = item.Financeiro.Pessoa?.Nome,
                        Situacao = item.Situacao,
                        DataVencimento = item.DataVencimento,
                        Valor = item.Valor,
                        PlanoConta = item.PlanoConta?.NmPlanoConta,
                        IdParcela = item.IdParcela,
                        TotalAcerto = item.TotalAcerto,
                        IdFinanceiro = item.IdFinanceiro
                    });
                }
            }

            return result;
        }

        [HttpGet]
        [Route("getFinanceiroPorId")]
        public HttpResponseMessage GetFinanceiroPorId(int id)
        {
            var model = new FinanceiroModel();
            var consulta = _financeiroService.ObterFinanceiroPorId(id);

            // cabeçalho
            model.IdFinanceiro = consulta.IdFinanceiro;
            model.NomePessoa = consulta.Pessoa.Nome;
            model.IdPessoa = consulta.Pessoa.IdPessoa;
            model.FormaPagamento = consulta.FormaPagamento;
            model.TotalDesconto = consulta.Parcelas.Sum(x => x.ValorDesconto);
            model.Total = consulta.Total;
            model.TotalAcerto = consulta.Parcelas.Sum(x => x.TotalAcerto);
            model.TotalAcrescimo = consulta.Parcelas.Sum(x => x.ValorAcrescimo);
            model.Tipo = consulta.Tipo;
            model.FormaPagamento = consulta.FormaPagamento;
            // model.IdContrato = consulta.IdContrato;
            model.UsuarioInsercao = _usuarioService.ObterUsuarioPorId(base.GetUsuarioLogado().IdUsuario).Nome;
            model.DataInsercao = consulta.Parcelas.First().DataInclusao;
            // parecelas
            if (consulta.Parcelas != null)
            {
                var parcelas = new List<FinanceiroParcelaModel>();
                foreach (var item in consulta.Parcelas)
                {
                    parcelas.Add(new FinanceiroParcelaModel()
                    {
                        DataInicio = item.DataInclusao,
                        DataPagamento = item.DataBaixa,
                        Numero = item.Numero,
                        DataAcerto = item.DataAcerto,
                        NumeroParcela = item.Numero,
                        NomeCliente = item.Financeiro.Pessoa?.Nome,
                        Situacao = item.Situacao,
                        DataVencimento = item.DataVencimento,
                        ValorAcerto = item.TotalAcerto ?? 0,
                        Valor = item.Valor,
                        ValorDesconto = item.ValorDesconto ?? 0,
                        ValorAcrescimo = item.ValorAcrescimo ?? 0,
                        PlanoConta = item.PlanoConta?.NmPlanoConta,
                        IdParcela = item.IdParcela,
                        TotalAcerto = item.TotalAcerto ?? 0,
                        IdFinanceiro = item.IdFinanceiro
                    });
                }
                model.Parcelas = parcelas;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, " financeiro não possui parcelas ");
            }

            return Request.CreateResponse<FinanceiroModel>(HttpStatusCode.OK, model);
        }

        [HttpPost]
        [Route("gerarparcelas")]
        public IEnumerable<FinanceiroParcelaModel> GerarParcelasFinanceiro(FinanceiroParcelaModel model)
        {
            var parcelas = new List<FinanceiroParcelaModel>();

            int quantidadeParcelas = model.QuantidadeParcelas;
            DateTime DataVencimentoAdesao = model.DataVencimentoAdesao;
            DateTime DataVencimentoPrimeiraParcela = model.DataVencimentoPrimeiraParcela;
            Decimal valorParcela = model.Valor / quantidadeParcelas;
            DateTime? DataPagamento = null;

            for (int i = 1; i <= quantidadeParcelas; i++)
            {
                DataPagamento = null;
                if (model.IncluirParcelasPagas)
                {
                    DataPagamento = DataVencimentoPrimeiraParcela;
                }
                parcelas.Add(new FinanceiroParcelaModel()
                {
                    Valor = Math.Round(valorParcela, 2),
                    DataVencimento = DataVencimentoPrimeiraParcela,
                    DataPagamento = DataPagamento,
                    NumeroParcela = i
                });
                DataVencimentoPrimeiraParcela = DataVencimentoPrimeiraParcela.AddMonths(1);
            }
            var somadevalores = parcelas.Sum(x => x.Valor);
            decimal resto = 0;
            if (Convert.ToDecimal(somadevalores) - (Convert.ToDecimal(model.Valor)) != 0)
            {
                resto = Convert.ToDecimal(somadevalores) - Convert.ToDecimal(model.Valor);
                parcelas[0].Valor = parcelas[0].Valor - resto;
            }
            return parcelas;
        }

        #endregion

        #region [Cheques]

        [HttpGet]
        [Route("listarcheques")]
        public HttpResponseMessage ListarCheques()
        {
            try
            {
                var result = new List<ChequeViewModel>();

                var model = _financeiroService.ListarCheques(base.GetUsuarioLogado().IdClinica);
                if (model.Count > 0)
                {
                    Mapper.CreateMap<Cheque, ChequeViewModel>();
                    Mapper.CreateMap<Pessoa, PessoaViewModel>();
                    Mapper.CreateMap<Financeiro, FinanceiroViewModel>();
                    result = Mapper.Map<List<Cheque>, List<ChequeViewModel>>(model.ToList());
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("excluircheque")]
        public HttpResponseMessage ExcluirCheque(int id)
        {
            try
            {
                var cheque = _financeiroService.ObterChequePorId(id);
                if (cheque == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do cheque!");

                _financeiroService.ExcluirCheque(cheque);

                return Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getChequeById")]
        public HttpResponseMessage GetChequeById(int id)
        {
            try
            {
                var result = new ChequeViewModel();

                var model = _financeiroService.ObterChequePorId(id);
                if (model != null)
                {
                    Mapper.CreateMap<Cheque, ChequeViewModel>();
                    result = Mapper.Map<Cheque, ChequeViewModel>(model);

                    result.NomePessoa = model.Pessoa.Nome;
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



        [HttpGet]
        [Route("pesquisarCheques")]
        public HttpResponseMessage PesquisarCheques(string emitente, string banco)
        {
            try
            {
                var model = new List<ChequeViewModel>();
                var result = _financeiroService.PesquisarCheques(emitente, banco, base.GetUsuarioLogado().IdClinica);

                if (result.Count > 0)
                {
                    Mapper.CreateMap<Cheque, ChequeViewModel>();
                    Mapper.CreateMap<Pessoa, PessoaViewModel>();
                    Mapper.CreateMap<Financeiro, FinanceiroViewModel>();
                    model = Mapper.Map<List<Cheque>, List<ChequeViewModel>>(result.ToList());
                }
                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("salvarCheque")]
        public HttpResponseMessage SalvarCheque(ChequeViewModel model)
        {
            try
            {
                var clinica = _cadastroService.ObterClinicaById(base.GetUsuarioLogado().IdClinica);
                if (clinica == null)
                    throw new Exception("Não foi possivel recuperar dados da clínica!");

                if (model.IdCheque > 0)
                {
                    var cheque = _financeiroService.ObterChequePorId(model.IdCheque);
                    if (cheque == null)
                        throw new Exception("Não foi possivel recuperar dados do cheque!");

                    if (model.IdFinanceiro > 0)
                    {
                        var financeiro = _financeiroService.ObterFinanceiroPorId(Convert.ToInt32(model.IdFinanceiro));
                        if (financeiro == null)
                            throw new Exception("Não foi possivel recuperar dados de financeiro!");

                        cheque.SetFinanceiro(financeiro);
                    }

                    if (model.IdPessoa > 0)
                    {
                        var pessoa = _cadastroService.ObterPessoaPorId(Convert.ToInt32(model.IdPessoa));
                        if (pessoa == null)
                            throw new Exception("Não foi possivel recuperar dados de pessoa!");

                        cheque.SetPessoa(pessoa);
                    }

                    cheque.SetAgencia(model.Agencia);
                    cheque.SetBanco(model.Banco);
                    cheque.SetClinica(clinica);
                    cheque.SetValor(model.Valor);

                    if (model.BomPara != DateTime.MinValue)
                        cheque.SetBomPara(Convert.ToDateTime(model.BomPara));

                    cheque.SetConta(model.Conta);
                    cheque.SetEmitente(model.Emitente);
                    cheque.SetHistorico(model.Historico);
                    cheque.SetSituacao(model.Situacao);


                    _financeiroService.SalvarCheque(cheque);
                    return Request.CreateResponse(HttpStatusCode.OK, model);
                }
                else
                {
                    var cheque = new Cheque(model.Emitente, model.Banco, model.Agencia, model.Conta, model.Situacao,model.Valor, clinica);

                    if (model.IdFinanceiro > 0)
                    {
                        var financeiro = _financeiroService.ObterFinanceiroPorId(Convert.ToInt32(model.IdFinanceiro));
                        if (financeiro == null)
                            throw new Exception("Não foi possivel recuperar dados de financeiro!");

                        cheque.SetFinanceiro(financeiro);
                    }

                    if (model.IdPessoa > 0)
                    {
                        var pessoa = _cadastroService.ObterPessoaPorId(Convert.ToInt32(model.IdPessoa));
                        if (pessoa == null)
                            throw new Exception("Não foi possivel recuperar dados de pessoa!");

                        cheque.SetPessoa(pessoa);
                    }

                    if (model.BomPara != DateTime.MinValue)
                        cheque.SetBomPara(Convert.ToDateTime(model.BomPara));

                    cheque.SetHistorico(model.Historico);

                    _financeiroService.SalvarCheque(cheque);
                    return Request.CreateResponse(HttpStatusCode.OK, model);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        #endregion

        #region [Bancos]

        [HttpGet]
        [Route("listarbancos")]
        public HttpResponseMessage ListarBancos()
        {
            try
            {
                var model = new List<BancoViewModel>();
                var result = _financeiroService.ListarBancos();

                if (result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        model.Add(new BancoViewModel()
                        {
                            IdBanco = item.IdBanco,
                            NomeBanco = item.NomeBanco,
                            Codigo = item.Codigo

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

        //[HttpGet]
        //[Route("excluirbanco")]
        //public HttpResponseMessage ExcluirBanco(int id)
        //{
        //    try
        //    {
        //        var banco = _financeiroService.ObterBancoPorId(id);
        //        if (banco == null)
        //            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do banco!");

        //        _financeiroService.ExcluirBanco(banco);

        //        return Request.CreateResponse(HttpStatusCode.OK);

        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpGet]
        //[Route("getBancoById")]
        //public HttpResponseMessage GetBancoeById(int id)
        //{
        //    try
        //    {
        //        var result = new BancoViewModel();

        //        var model = _financeiroService.ObterBancoPorId(id);
        //        if (model != null)
        //        {
        //            Mapper.CreateMap<Banco, BancoViewModel>();
        //            result = Mapper.Map<Banco, BancoViewModel>(model);
        //        }

        //        return Request.CreateResponse(HttpStatusCode.OK, result);

        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}

        //[HttpPost]
        //[Route("salvarBanco")]
        //public HttpResponseMessage SalvarBanco(BancoViewModel model)
        //{
        //    try
        //    {
        //        if (model.IdBanco > 0)
        //        {
        //            var banco = _financeiroService.ObterBancoPorId(model.IdBanco);
        //            if (banco == null)
        //                throw new Exception("Não foi possivel recuperar dados do banco!");

        //            banco.NomeBanco = model.NomeBanco;

        //            _financeiroService.SalvarBanco(banco);
        //            return Request.CreateResponse(HttpStatusCode.OK, model);
        //        }
        //        else
        //        {
        //            var banco = new Banco();
        //            banco.NomeBanco = model.NomeBanco;

        //            _financeiroService.SalvarBanco(banco);
        //            return Request.CreateResponse(HttpStatusCode.OK, model);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
        //    }
        //}


        #endregion

        #region [Conta]

        [HttpGet]
        [Route("listarContas")]
        public HttpResponseMessage ListarContas()
        {
            try
            {
                var model = new List<ContaViewModel>();
                var result = _financeiroService.ListarContas(base.GetUsuarioLogado().IdClinica);

                foreach (var item in result)
                {
                    model.Add(new ContaViewModel()
                    {
                        IdConta = item.IdConta,
                        Nome = item.NmConta,
                        Saldo = item.Saldo,
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
        [Route("excluirConta")]
        public HttpResponseMessage ExcluirConta(int id)
        {
            try
            {
                var conta = _financeiroService.ObterContaPorId(id);
                if (conta == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados da conta");

                _financeiroService.ExcluirConta(conta);
                return Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getContaById")]
        public HttpResponseMessage GetContaById(int id)
        {
            try
            {
                var model = new ContaViewModel();
                var result = _financeiroService.ObterContaPorId(id);

                if (result == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados da conta");

                model.IdConta = result.IdConta;
                model.Nome = result.NmConta;
                model.Situacao = result.Situacao;

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("saveConta")]
        public HttpResponseMessage SalvarConta(ContaViewModel model)
        {
            try
            {
                if (model.IdConta > 0)
                {
                    var conta = _financeiroService.ObterContaPorId(model.IdConta);
                    if (conta == null)
                        throw new Exception("Não foi possivel recuperar dados da conta");

                    conta.SetNmConta(model.Nome);
                    conta.SetSituacao(model.Situacao);

                    _financeiroService.SalvarConta(conta);
                    return Request.CreateResponse(HttpStatusCode.OK, model);
                }
                else
                {
                    var conta = new Conta(model.Nome, base.GetUsuarioLogado().Clinica);

                    _financeiroService.SalvarConta(conta);
                    return Request.CreateResponse(HttpStatusCode.OK, model);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("pesquisarContas")]
        public HttpResponseMessage PesquisarContas(string nome, int? codigo)
        {
            try
            {
                var model = new List<ContaViewModel>();
                var result = _financeiroService.PesquisarContas(nome, codigo, base.GetUsuarioLogado().IdClinica);

                foreach (var item in result)
                {
                    model.Add(new ContaViewModel()
                    {
                        IdConta = item.IdConta,
                        Nome = item.NmConta,
                        Saldo = item.Saldo,
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

        #endregion

        #region [Plano de Conta]

        [HttpGet]
        [Route("listarPlanodeContas")]
        public HttpResponseMessage ListarPlanosdeConta()
        {
            try
            {
                var model = new List<PlanoContaViewModel>();
                var result = _financeiroService.ListarPlanoContas(base.GetUsuarioLogado().IdClinica);

                if (result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        model.Add(new PlanoContaViewModel()
                        {
                            IdPlanoConta = item.IdPlanoConta,
                            NmPlanoConta = item.NmPlanoConta,
                            Tipo = item.Tipo,
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
        [Route("excluirPlanoConta")]
        public HttpResponseMessage ExcluirPlanoConta(int id)
        {
            try
            {
                var pl = _financeiroService.ObterPlanodeContasPorId(id);
                if (pl == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do plano de conta");

               
                _financeiroService.ExcluirPlanoConta(pl);
                return Request.CreateResponse(HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getPlanoContaById")]
        public HttpResponseMessage GetPlanoContaById(int id)
        {
            try
            {
                var model = new PlanoContaViewModel();
                var result = _financeiroService.ObterPlanodeContasPorId(id);

                if (result == null)
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do plano de conta");

                model.IdPlanoConta = result.IdPlanoConta;
                model.NmPlanoConta = result.NmPlanoConta;
                model.Situacao = result.Situacao;
                model.Tipo = result.Tipo;
                model.Categoria = result.Categoria;
                model.Codigo = result.Codigo;

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("savePlanoConta")]
        public HttpResponseMessage SalvarPlanoConta(PlanoContaViewModel model)
        {
            try
            {
                if (model.IdPlanoConta > 0)
                {
                    var pl = _financeiroService.ObterPlanodeContasPorId(model.IdPlanoConta);
                    if (pl == null)
                        throw new Exception("Não foi possivel recuperar dados do plano de conta");

                    pl.SetNomePlanoConta(model.NmPlanoConta);
                    pl.SetSituacao(model.Situacao);
                    pl.SetTipo(model.Tipo);
                    pl.SetCategoria(model.Categoria);
                    pl.SetCodigo(model.Codigo);
                    pl.SetClinica(base.GetUsuarioLogado().Clinica);

                    _financeiroService.SalvarPlanoConta(pl);
                    return Request.CreateResponse(HttpStatusCode.OK, model);
                }
                else
                {
                    var pl = new PlanoConta(model.NmPlanoConta, model.Tipo, model.Categoria, model.Codigo, base.GetUsuarioLogado().Clinica);
                    _financeiroService.SalvarPlanoConta(pl);

                    return Request.CreateResponse(HttpStatusCode.OK, model);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("pesquisarPlanoContas")]
        public HttpResponseMessage PesquisarPlanoContas(string nome, int? codigo, string tipo)
        {
            try
            {
                var model = new List<PlanoContaViewModel>();
                var result = _financeiroService.PesquisarPlanoContas(nome, codigo, tipo, base.GetUsuarioLogado().IdClinica);

                foreach (var item in result)
                {
                    model.Add(new PlanoContaViewModel()
                    {
                        IdPlanoConta = item.IdPlanoConta,
                        NmPlanoConta = item.NmPlanoConta,
                        Categoria = item.Categoria,
                        Codigo = item.Codigo,
                        Tipo = item.Tipo,
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

        #endregion

        #region Meio de Pagamento
        
        [HttpGet]
        [Route("listarmeiospagamento")]
        public HttpResponseMessage ListarMeiosdePagamentos()
        {
            try
            {
                var result = new List<MeioPagamentoViewModel>();

                var model = _financeiroService.ListarMeiosdePagamentos();
                if (model.Count > 0)
                {
                    Mapper.CreateMap<MeioPagamento, MeioPagamentoViewModel>();
                    result = Mapper.Map<List<MeioPagamento>, List<MeioPagamentoViewModel>>(model.ToList());
                }

                return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        #endregion
    }
}