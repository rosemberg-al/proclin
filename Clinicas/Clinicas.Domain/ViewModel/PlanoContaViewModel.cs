using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class PlanoContaViewModel
    {
        public int IdPlanoConta { get; set; }
        public string NmPlanoConta { get; set; }
        public string Situacao { get; set; }
        public string Tipo { get; set; }
        public string Categoria { get; set; }
        public string Codigo { get; set; }
        public int IdClinica { get;  set; }


    }
}