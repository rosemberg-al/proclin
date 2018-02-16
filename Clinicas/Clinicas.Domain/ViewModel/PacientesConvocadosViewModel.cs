using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class PacientesConvocadosViewModel
    {
        public int IdAgenda { get; set; }
        public string NmProfissionalSaude { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }
        public string Consultorio { get; set; }
        public string NmPaciente { get; set;  }
    }
}