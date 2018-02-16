using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class TabelaPrecoItensViewModel
    {
        public int IdTabelaPreco { get; set; }
        public int IdProcedimento { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorProfissional { get; set; }
        public ProcedimentoViewModel Procedimento { get; set; }
    }
}