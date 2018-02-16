using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class BuscaGuiaViewModel
    {
        public string Profissional { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string NomePaciente { get; set; }
        public string NumeroGuia { get; set; }
    }
}