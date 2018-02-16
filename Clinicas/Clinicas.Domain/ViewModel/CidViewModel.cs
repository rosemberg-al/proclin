using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class CidViewModel
    {
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string Agravo { get; set; }
        public string SexoOcorrencia { get; set; }
        public string Estadio { get; set; }
        public string CamposIrradiados { get; set; }
    }
}