using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class TabelaPrecoViewModel
    {
        public int IdTabelaPreco { get; set; }
        public int IdClinica { get; set; }
        public int IdConvenio { get; set; }
        public string Nome { get; set; }
        public string Tipo { get; set; }
        public string Clinica { get; set; }
        public string Situacao { get; set; }
        public string Convenio { get; set; }
        public List<TabelaPrecoItensViewModel> Itens { get; set; }
    }
}