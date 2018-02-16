using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class AgendaMedicaViewModel
    {
        public int IdAgenda { get; set; }
        public int IdProfissional { get; set; }
        public string Paciente { get; set; }
        public int IdPaciente { get; set; }
        public string Data { get; set; }
        public string Hora { get; set; }
        public string Situacao { get; set; }
    }
}

