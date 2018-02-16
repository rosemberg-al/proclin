using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class ListaGuiasLoteViewModel
    {
        public int IdGuia { get; set; }
        public string Paciente { get; set; }
        public string Situacao { get; set; }
        public string Convenio { get; set; }
        public DateTime Data { get; set; }
        public bool Selected { get; set; }
    }
}