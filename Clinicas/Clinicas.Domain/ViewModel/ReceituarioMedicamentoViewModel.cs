using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class ReceituarioMedicamentoViewModel
    {
        public int IdReceituario { get; set; }
        public int IdMedicamento { get; set; }
        public string Nome { get; set; }
        public string Posologia { get; set; }
        public int Quantidade { get; set; }
    }
}