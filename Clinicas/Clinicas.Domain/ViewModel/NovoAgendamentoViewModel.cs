using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class NovoAgendamentoViewModel
    {
        public int idagenda { get; set; }
        public int idpaciente { get; set; }
        public int idUnidadeAtendimento { get; set; }

        public int idprofissional { get; set; }
        public int idespecialidade { get; set; }
        public int idprocedimento { get; set; }
        public int? idconvenio { get; set; }
        public string tipo { get; set; }
        public string observacoes { get; set; }
        public string solicitante { get; set; }
        public decimal valor { get; set; }
        public decimal valorProfissional { get; set; }
        public int idtipoatendimento { get; set; }
    }
}