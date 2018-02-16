using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel.Relatorios
{
    public class RelPlanoAgenda
    {
        public DateTime Data { get; set; }
        public string Hora { get; set; }
        public string Unidade { get; set; }
        public string Funcionario { get; set; }
        public string Dia { get; set; }
    }
}