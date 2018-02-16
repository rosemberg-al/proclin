using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class DadosNascimentoViewModel
    {
        public int IdDadosNascimento { get; set; }
        public Decimal PesoNascimento { get; set; }
        public Decimal AlturaNascimento { get; set; }
        public Decimal PerimetroCraniano { get; set; }
        public Decimal PerimetroAbdominal { get; set; }
        public string IdadeGestacional { get; set; }
        public int IdHospital { get; set; }
        public DateTime DataAlta { get; set; }
        public Decimal PesoAlta { get; set; }
        public string TipoParto { get; set; }
        public string TipoSanguineoRN { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime HoraNascimento { get; set; }
        public string AssistiuRecemNascidoRN { get; set; }
        public string AleitamentoMaternoPrimeiraHoraVida { get; set; }
        public string TipoSanguineoMae { get; set; }
        public string TestePezinho { get; set; }
        public DateTime DataTestePezinho { get; set; }
        public string TesteReflexoVermelho { get; set; }
        public string Condutas { get; set; }
    }
}