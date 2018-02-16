using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class MedidasAntropometricasViewModel
    {
        public int IdMedida { get; set; }
        public int IdPaciente { get; set; }
        public int IdProfissionalSaude { get; set; }
        public Decimal Peso { get; set; }
        public Decimal Altura { get; set; }
        public Decimal PerimetroCefalico { get; set; }
        public DateTime Data { get; set; }
        public Decimal Imc { get; set; }
    }
}