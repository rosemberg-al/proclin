using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Cidade
    {
        public int IdCidade { get; set; }
        public string Nome { get; set; }
        public int? Ibge { get; set; }
        public int IdEstado { get; set; }
        public virtual Estado Estado { get; set; }


        public Cidade() { }
    }
}