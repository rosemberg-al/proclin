using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class AgendaProcedimentoViewModel
    {
        public string Data { get; set; }
        public string Observacao { get; set; }
        public string Solicitante { get; set; }
        public string Hora { get; set; }
        public string Tipo { get; set; }
        public int IdPaciente { get; set; }
        public int IdTipoAtendimento { get; set; }
        public int IdProfissional { get; set; }
        public int IdAgenda { get; set; }
        public int IdProcedimento { get; set; }
        public int IdConvenio { get; set; }
        public int IdEspecialidade { get; set; }
        public int IdUnidadeAtendimento { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorProfissional { get; set; }
        public bool Avulsa { get; set; }
    }
}