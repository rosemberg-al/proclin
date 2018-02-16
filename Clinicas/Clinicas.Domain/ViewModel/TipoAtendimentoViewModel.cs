using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class TipoAtendimentoViewModel
    {
        public int CdTipoAtendimento { get; set; }
        public string NmTipoAtendimento { get; set; }
        public string Descricao { get; set; }
    }
}