using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class RegistroVacinaViewModel
    {
        public int IdRegistroVacina { get; set; }
        public int IdVacina { get; set; }
        public int IdPaciente { get; set; }
        public DateTime Data { get; set; }
        public DateTime Hora { get; set; }
        public string Lote { get; set; }
        public string HoraVacina { get; set; }
        public string Dose { get; set; }
        public VacinaViewModel Vacina { get; set; }
    }
}