using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel.Relatorios
{
    public class RelProcedimento
    {
        public string Nome { get; set; }
        public int IdProcedimento { get; set; }
        public string Codigo { get; set; }
        public string Sexo { get; set; }
    }
}