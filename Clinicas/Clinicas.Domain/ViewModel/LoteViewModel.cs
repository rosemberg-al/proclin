using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class LoteViewModel
    {
        public int IdLote { get; set; }
        public int IdClinica { get; set; }
        public int IdConvenio { get; set; }
        public string NomeConvenio { get; set; }
        public DateTime Data { get; set; }
        public string Situacao { get; set; }
    }

}