using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class BloqueioAgendaAvisoViewModel
    {
        public bool Aceito { get; set; }
        public List<PacientesMarcados> PacientesMarcados { get; set; }

        public BloqueioAgendaAvisoViewModel()
        {
            PacientesMarcados = new List<ViewModel.PacientesMarcados>();
        }
    }

    public class PacientesMarcados
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Data { get; set; }
    }
}