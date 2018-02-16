using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class AtestadoViewModel
    {
        public int IdAtestado { get; set; }
        public int IdPaciente { get; set; }
        public int IdFuncionario { get; set; }
        public string Descricao { get; set; }
        public string Situacao { get; set; }
        public DateTime Data { get; set; }
        public string NomePaciente { get; set; }
        public string NomeFuncionario { get; set; }
    }
}