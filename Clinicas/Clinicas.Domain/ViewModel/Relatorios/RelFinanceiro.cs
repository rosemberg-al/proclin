using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel.Relatorios
{
    public class RelFinanceiro
    {
        public int IdFinanceiro { get; set; }
        public string Nome { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal ValorVencimento { get; set; }
        public DateTime? DataAcerto { get; set; }
        public decimal? ValorAcerto { get; set; }
        public string Situacao { get; set; }

    }
}