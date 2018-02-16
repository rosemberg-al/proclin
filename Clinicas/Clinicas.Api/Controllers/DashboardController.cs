using Clinicas.Application.Services.Interfaces;
using Clinicas.Domain.DTO;
using Clinicas.Domain.ViewModel.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Clinicas.Api.Controllers
{
    [Authorize]
    [RoutePrefix("dashboard")]
    public class DashboardController : BaseController
    {
        private readonly IPacienteService _service;
        private readonly IDashboardService _Dservice;
        private readonly IUsuarioService _usuarioService;

        public DashboardController(IPacienteService service, IDashboardService Dservice, IUsuarioService usuarioService) : base(usuarioService)
        {
            _service = service;
            _Dservice = Dservice;
            _usuarioService = usuarioService;
        }

        [HttpGet]
        [Route("getTotaisfinanceiro")]
        public HttpResponseMessage TotalaPagar()
        {
            var model = new FinanceiroDTO();

            try
            {
                var usuarioLogado = base.GetUsuarioLogado();
                model.TotalaPagar = _Dservice.TotalPagar(usuarioLogado.IdClinica, usuarioLogado.IdUnidadeAtendimento);
                model.TotalaReceber = _Dservice.TotalReceber(usuarioLogado.IdClinica, usuarioLogado.IdUnidadeAtendimento);
                model.TotalaContasPagas = _Dservice.TotalaContasPagas(usuarioLogado.IdClinica, usuarioLogado.IdUnidadeAtendimento);
                model.TotalContasRecebidas = _Dservice.TotalContasRecebidas(usuarioLogado.IdClinica, usuarioLogado.IdUnidadeAtendimento);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getdespesas")]
        public HttpResponseMessage DespesasPorCategoria()
        {
            var model = new List<RelDespesasCategoria>();

            try
            {
                var usuarioLogado = base.GetUsuarioLogado();
                model = _Dservice.DespesasPorCategoria(usuarioLogado.IdClinica,usuarioLogado.IdUnidadeAtendimento);

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("getResceitaxDespesas")]
        public HttpResponseMessage ReceitasPorDespesas()
        {
            var result = new RelReceitasPorDespesas
            {
                Despesas = new List<DadosReceita>(),
                Receitas = new List<DadosReceita>()
            };
            try
            {
                var usuarioLogado = base.GetUsuarioLogado();
                var model = _Dservice.ReceitasPorDespesas(usuarioLogado.IdClinica, usuarioLogado.IdUnidadeAtendimento);
               
                //adiciono os meses que não teve faturamento
                for (int i = 0; i <= 11; i++)
                {
                    if (!model.Despesas.Where(p => p.Mes == i).Any())
                        result.Despesas.Add(new DadosReceita
                        {
                            Ano = DateTime.Now.Year,
                            Mes = i,
                            Valor = 0
                        });
                    if (!model.Receitas.Where(p => p.Mes == i).Any())
                        model.Receitas.Add(new DadosReceita
                        {
                            Ano = DateTime.Now.Year,
                            Mes = i,
                            Valor = 0
                        });
                }

                //ordena os meses
                result.Despesas.AddRange(model.Despesas);
                result.Receitas.AddRange(model.Receitas);

                result.Despesas.OrderBy(x => x.Mes).ToList();
                result.Receitas.OrderBy(x => x.Mes).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }


        [HttpGet]
        [Route("qtdeAtendimentosParticular")]
        public HttpResponseMessage QtdeAtendimentosParticular()
        {
            var model = new FinanceiroDTO();

            try
            {
                // return Request.CreateResponse(HttpStatusCode.OK, model);
                /* else
                   return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do funcionário."); */

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("qtdeAtendimentosConvenio")]
        public HttpResponseMessage QtdeAtendimentosConvenio()
        {
            var model = new FinanceiroDTO();

            try
            {
                // return Request.CreateResponse(HttpStatusCode.OK, model);
                /* else
                   return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do funcionário."); */

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("qtdePacientes")]
        public HttpResponseMessage QtdePacientes()
        {
            var model = new FinanceiroDTO();

            try
            {
                // return Request.CreateResponse(HttpStatusCode.OK, model);
                /* else
                   return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do funcionário."); */

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);

            }
        }


        /// <summary>
        /// Atendimentos Marcados 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("qtdeAgendamentos")]
        public HttpResponseMessage QtdeAgendamentos()
        {
            var model = new FinanceiroDTO();

            try
            {
                // return Request.CreateResponse(HttpStatusCode.OK, model);
                /* else
                   return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do funcionário."); */

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);

            }
        }

        /// <summary>
        /// Resumo de atendimentos por mês (ano atual)
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("resumoAtendimentosPorMes")]
        public HttpResponseMessage ResumoAtendimentosPorMes()
        {
            var model = new FinanceiroDTO();

            try
            {
                //  return Request.CreateResponse(HttpStatusCode.OK, model);
                /* else
                   return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do funcionário."); */

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);

            }
        }


        /// <summary>
        /// Relação de pacientes marcados 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getAgendamentoDia")]
        public HttpResponseMessage getAgendamentoDia()
        {
            var model = new FinanceiroDTO();

            try
            {
                // return Request.CreateResponse(HttpStatusCode.OK, model);
                /* else
                   return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Não foi possível recuperar dados do funcionário."); */

                return Request.CreateResponse(HttpStatusCode.OK, model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);

            }
        }


    }
}