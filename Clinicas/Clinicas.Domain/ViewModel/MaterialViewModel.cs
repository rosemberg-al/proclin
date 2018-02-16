using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class MaterialViewModel
    {
        public int IdMaterial { get; set; }
        public string Nome { get; set; }
        public int EstoqueMinimo { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Observacao { get; set; }
        public string EstadoConservacao { get; set; }
        public string Identificador { get; set; }
        public decimal ValorVenda { get; set; }
        public decimal ValorCompra { get; set; }
        public string Situacao { get; set; }
        public int EstoqueAtual { get; set; }

        public int IdTipoMaterial { get; set; }
        public string NmTipoMaterial { get; set; }

        public int IdUnidadeAtendimento { get; set; }
        public string NmUnidadeAtendimento { get; set; }
    }
    public class TipoMaterialViewModel
    {
        public int IdTipoMaterial { get; set; }
        public string Nome { get; set; }
        public string Situacao { get; set; }
    }

    public class MovimentoEstoqueViewModel
    {
        public int IdMovimentoEstoque { get; set; }
        public DateTime Data { get; set; }
        public string Tipo { get; set; }
        public int Quantidade { get; set; }
        public string Situacao { get; set; }
        public int IdMaterial { get; set; }
        public string Nome { get; set; }
    }
}