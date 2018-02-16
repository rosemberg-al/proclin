using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class RelAgendaMedica
    {
        public int IdAgenda { get; set; }
        public int IdProfissional { get; set; }
        public string ProfissionalSaude { get; set; }
        public string NmProcedimento { get; set; }
        public string Paciente { get; set; }
        public DateTime Data { get; set; }
        public string Hora { get; set; }
        public string Tipo { get; set; }
        public string TipoAtendimento { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorProfissional { get; set; }
        public string Situacao { get; set; }
    }
}