using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Api.Models
{
    public class ContaModel
    {
        public int IdConta { get; set; }
        public string Nome { get; set; }
        public decimal Saldo { get; set; }
    }
}