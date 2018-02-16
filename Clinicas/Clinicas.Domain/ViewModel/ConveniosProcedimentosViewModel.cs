using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class ConveniosProcedimentosViewModel
    {
        public decimal Valor { get; set; }
        public decimal ValorProfissional { get; set; }
        public int CdProcedimento { get; set; }
        public string NmConvenio { get; set; }
        public int CdConvenio { get; set; }
    }
}