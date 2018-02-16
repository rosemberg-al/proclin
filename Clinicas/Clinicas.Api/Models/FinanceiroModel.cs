using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Api.Models
{
    public class FinanceiroModel
    {
        public int IdFinanceiro { get; set; }
        public int IdMeioPagamento { get; set; }
        public decimal? TotalDesconto { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalAcerto { get; set; }
        public int IdPessoa { get; set; }
        public string NomePessoa { get; set; }
        //public PessoaModel Pessoa { get; set; }
        public string Tipo { get; set; }
        public string FormaPagamento { get; set; }
        public List<FinanceiroParcelaModel> Parcelas { get; set; }
        public int? IdContrato { get; set; }
        public int IdPlanoConta { get; set; }
        public int IdConta { get; set; }

        public string UsuarioInsercao { get; set; }
        public DateTime DataInsercao { get; set; }

        public DateTime DataVencimento { get; set; }

        public decimal? TotalAcrescimo { get; set; }

        public FinanceiroModel()
        {
            Parcelas = new List<FinanceiroParcelaModel>();
        }
    }
}