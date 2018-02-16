using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Api.Models
{
    public class TransferenciaContaModel
    {
        public int IdTransferenciaConta { get; set; }
        public int IdContaDestino { get; set; }
        public int IdContaOrigem { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
        public ContaModel ContaOrigem { get; set; }
        public ContaModel ContaDestino { get; set; }
    }
}