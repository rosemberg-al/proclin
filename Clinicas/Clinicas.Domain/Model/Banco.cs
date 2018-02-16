using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public partial class Banco
    {
        public int IdBanco { get; set; }
        public string NomeBanco { get; set; }
        public string Codigo { get; set; }
    }
}