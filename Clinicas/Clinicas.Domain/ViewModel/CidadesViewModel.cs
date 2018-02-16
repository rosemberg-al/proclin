using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class CidadesViewModel
    {
        public int Id { get; set; }
        public int IdEstado { get; set; }
        public string Uf { get; set; }
        public string Nome { get; set; }
    }
}