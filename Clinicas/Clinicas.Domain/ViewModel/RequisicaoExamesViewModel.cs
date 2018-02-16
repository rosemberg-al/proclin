using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class RequisicaoExamesViewModel
    {
        public int IdRequisicao { get; set; }
        public int IdPaciente { get; set; }
        public string DiagnosticoClinico { get; set; }
        public string Material { get; set; }
        public string NaturezaExame { get; set; }
        public DateTime Data { get; set; }
        public int IdMedico { get; set; }
        public string Classe { get; set; }
        public string Tipo { get; set; }
        public string MedicoSolicitante { get; set; }
        public string NomePaciente { get; set; }
    }
}