using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class CarteiraViewModel
    {
        public int IdCarteira { get; set; }
        public int IdPaciente { get; set; }
        public int IdConvenio { get; set; }
        public DateTime ValidadeCarteira { get; set; }
        public string NumeroCarteira { get; set; }
        public string Plano { get; set; }
        public string Paciente { get; set; }
        public string Convenio { get; set; }
        public string RegistroAns { get; set; }
    }
}