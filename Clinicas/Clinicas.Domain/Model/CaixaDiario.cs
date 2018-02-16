using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class CaixaDiario
    {
        public decimal SaldoAbertura { get; set; }
        public decimal SaldoFehamento { get; set; }
        public string SituacaoCaixa { get; set; }
        public DateTime Data { get; set; }
    }

    public class MovimentoCaixa
    {
        public virtual CaixaDiario CaixaDiario { get; set; }
        public string Historico { get; set; }
        public Conta Conta { get; set; }
        public decimal Entrada { get; set; }
        public Financeiro Financeiro { get; set; }
        public decimal Saida { get; set; }
    }
}