using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class MedicoViewModel
    {
        public int IdMedico { get; set; }
        public string NomeMedico { get; set; }
        public string Crm { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}