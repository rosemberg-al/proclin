using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class GerarAgendaViewModel
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int IdProfissional { get; set; }
        public int IdUnidadeAtendimento { get; set; }
        public List<AgendaMedica> AgendaMedica { get; set; }
        public GerarAgendaViewModel()
        {
            AgendaMedica = new List<AgendaMedica>();
        }
    }

    public class AgendaMedica
    {
        public int IntervaloMinutos { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraTermino { get; set; }
        public string Dia { get; set; }
        public TimeSpan? IntervaloInicio { get; set; }
        public TimeSpan? IntervaloTermino { get; set; }
    }
}