using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class BloqueioAgendaViewModel
    {
        public int IdBloqueio { get; set; }
        public int IdFuncionario { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public string Motivo { get; set; }
        public string Funcionario { get; set; }
    }
}