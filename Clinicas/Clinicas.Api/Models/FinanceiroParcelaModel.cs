using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Api.Models
{
    public class FinanceiroParcelaModel
    {
        public decimal Valor { get; set; }
        public string Observacao { get; set; }
        public decimal? ValorAcrescimo { get; set; }
        public int Numero { get; set; }
        public int? IdPlanoConta { get; set; }
        public decimal? ValorDesconto { get; set; }
        public string TipoConta { get; set; }
        public int IdMeioPagamento { get; set; }
        public int IdConta { get; set; }
        public string Conta { get; set; }
        public DateTime DataVencimentoPrimeiraParcela { get; set; }
        public DateTime? DataPagamento { get; set; }
        public bool IncluirParcelasPagas { get; set; }
        public int QuantidadeParcelas { get; set; }
        public int IdParcela { get; set; }
        public decimal ValorAdesao { get; set; }
        public decimal? ValorAcerto { get; set; }
        public decimal? TotalAcerto { get; set; }
        public DateTime DataVencimentoAdesao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataAcerto { get; set; }
        public DateTime DataTermino { get; set; }
        public DateTime DataVencimento { get; set; }
        public int NumeroParcela { get; set; }
        public string NomeCliente { get; set; }
        public string PlanoConta { get; set; }
        public string Situacao { get; set; }
        public int IdFinanceiro { get; set; }

        public string Inclusao { get; set; }
        public string Exclusao { get; set; }
        public string Alteracao { get; set; }
    }
}