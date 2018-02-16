using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class UltimosAtendimentosViewModel
    {
        public int IdAgenda { get; set; }
        public string Paciente { get; set; }
        public string ProfissionalSaude { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }
        public string Situacao { get; set; }
    }
}