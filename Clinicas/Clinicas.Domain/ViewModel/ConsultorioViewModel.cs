using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class ConsultorioViewModel
    {
        public int IdConsultorio { get; set; }
        public string NmConsultorio { get; set; }
        public int IdClinica { get; set; }
        public string NmClinica { get; set; }
        public string NmUnidadeAtendimento { get; set; }
        public int IdUnidadeAtendimento { get; set; }

    }
}