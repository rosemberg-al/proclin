using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class FinanceiroViewModel
    {
        public int IdFinanceiro { get; set; }
        public decimal? TotalDesconto { get; set; }
        public decimal? Total { get; set; }
        public decimal? TotalAcerto { get; set; }
        public int IdPessoa { get; set; }
        public string Tipo { get; set; }
        public string FormaPagamento { get; set; }
    }
}