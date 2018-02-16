using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class VacinaViewModel
    {
        public int IdVacina { get; set; }
        public string Descricao { get; set; }
        public string Idade { get; set; }
        public string Doses { get; set; }
        public string Situacao { get; set; }
    }
}