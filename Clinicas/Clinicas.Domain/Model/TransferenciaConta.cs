using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class TransferenciaConta
    {
        public int IdTransferenciaConta { get; set; }
        public int IdContaDestino { get; set; }
        public int IdContaOrigem { get; set; }
        public virtual Conta ContaOrigem { get; set; }
        public virtual Conta ContaDestino { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }

        public TransferenciaConta() { }

        public TransferenciaConta(Conta contaOrigem, Conta contaDestino, decimal valor, DateTime data, string descricao)
        {
            ContaOrigem = contaOrigem;
            ContaDestino = contaDestino;
            Valor = valor;
            Data = data;
            Descricao = descricao;
        }
    }
}