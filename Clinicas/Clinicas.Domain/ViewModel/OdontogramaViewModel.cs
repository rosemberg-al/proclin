using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class OdontogramaViewModel
    {
        public int IdOdontograma { get;  set; }
        public DateTime DataInicio { get;  set; }
        public DateTime DataTermino { get;  set; }

        public int IdFuncionario { get;  set; }
        public string NmFuncionario { get;  set; }

        public int IdPaciente { get;  set; }
        public string NmPaciente { get;  set; }

        public string Situacao { get;  set; }
        public string Descricao { get;  set; }
        public int Dente { get; set; }
        public string Face { get; set; }
        public decimal Valor { get; set; }
    }
}