using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class TabelaConvenioViewModel
    {
        public int IdConvenio { get; set; }
        public int IdProcedimento { get; set; }
        public string NomeProcedimento { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorProfissional { get; set; }
        public string NomeConvenio { get; set; }
    }
}