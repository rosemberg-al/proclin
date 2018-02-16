using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class LiberarAgendaViewModel
    {
        public DateTime DataInicio { get; set; }
        public DateTime DataTermino { get; set; }
        public string HoraInicio { get; set; }
        public string HoraTermino { get; set; }
        public string IntervaloInicio { get; set; }
        public string IntervaloTermino { get; set; }
        public double IntervaloMinutos { get; set; }
        public int IdProfissional { get; set; }
        public bool PossuiIntervalo { get; set; }
        public DiaSemana DiaSemana { get; set; }

        public Usuario Usuario { get; set; }
        public int IdClinica { get; set; }
    }
    public class DiaSemana {
                
        public bool Domingo { get; set; }
        public bool Segunda { get; set; }
        public bool Terca { get; set; }
        public bool Quarta { get; set; }
        public bool Quinta { get; set; }
        public bool Sexta { get; set; }
        public bool Sabado { get; set; }

    }
}