using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class TipoAtendimento
    {
        public int IdTipoAtendimento { get; set; }
        public string NmTipoAtendimento { get; set; }
        public string Descricao { get; set; }
    }
}