using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class HistoriaPregressaViewModel
    {
        public int IdHistoriaPregressa { get; set; }
        public int IdPaciente { get; set; }
        public string Paciente { get;  set; }
        public int IdFuncionario { get; set; }
        public string ProfissionalSaude { get; set; }
        public string Descricao { get; set; }
        public string Situacao { get; set; }
        public DateTime Data { get; set; }
    }
}