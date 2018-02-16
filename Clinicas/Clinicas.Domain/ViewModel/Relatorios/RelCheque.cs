using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel.Relatorios
{
    public class RelCheque
    {
        public string Emitente { get; set; }
        public string Banco { get; set; }
        public DateTime BomPara { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public decimal Valor { get;  set; }
        public string Situacao { get; set; }
        public string IdCheque { get; set; }
    }
}