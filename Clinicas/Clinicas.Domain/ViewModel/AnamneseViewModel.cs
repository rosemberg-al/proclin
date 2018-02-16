using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class AnamneseViewModel
    {
        public int IdAnamnese { get; set; }
        public DateTime Data { get; set; }
        public int IdPaciente { get; set; }
        public string Paciente { get; set; }
        public int IdProfissionalSaude { get; set; }
        public string ProfissionalSaude { get; set; }
        public string Hma { get; set; }
        public string Diagnostico { get; set; }
        public string CondutaMedica { get; set; }
        public string Situacao { get; set; }
    }
}