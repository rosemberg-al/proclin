using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class ModeloProntuarioViewModel
    {
        public int IdModeloProntuario { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public string Situacao { get; set; }
        public string NomeModelo { get; set; }
    }
}