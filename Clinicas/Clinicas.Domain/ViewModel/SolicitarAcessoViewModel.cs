using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class SolicitarAcessoViewModel
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cnpj { get; set; }
        public string Telefone { get; set; }
        public int IdCidade { get; set; }
        public int IdEstado { get; set; }
        public int IdEspecialidade { get; set;  }

    }
}