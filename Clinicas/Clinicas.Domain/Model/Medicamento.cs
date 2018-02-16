using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Medicamento
    {
        public int IdMedicamento { get; set; }
        public string Nome { get; set; }
        public string Fabricante { get; set; }
        public string RegistroMS { get; set; }
        public string PrincipioAtivo { get; set; }
        public string Indicacao { get; set; }
        public string Posologia { get; set; }
        public string ContraIndicacao { get; set; }
        public decimal Preco { get; set; }
    }
}