using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class ChequeViewModel
    {
        public int IdCheque { get; set; }
        public DateTime DataInclusao { get; set; }
        public string Emitente { get; set; }
        public string Banco { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public decimal Valor { get; set; }
        public DateTime BomPara { get; set; }
        public string Situacao { get; set; }
        public Nullable<int> IdFinanceiro { get; set; }
        public int IdPessoa { get; set; }
        public string Historico { get; set; }
        public string NomePessoa { get; set; }
    }
}