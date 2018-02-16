using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class ProcedimentosConvenioViewModel
    {
        public int IdProcedimento { get; set; }
        public string NomeProcedimento { get; set; }
        public string Convenio { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorProfissional { get; set; }
        public decimal Diferenca { get; set; }
    }
}