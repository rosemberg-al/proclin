using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class ProcedimentoViewModel
    {
        public int IdProcedimento { get; set; }
        public string NomeProcedimento { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorProfissional { get; set; }
        public string Sexo { get; set; }
        public int Codigo { get; set; }
        public string Odontologico { get; set; }
        public string Preparo { get; set; }
        public ICollection<EspecialidadeViewModel> Especialidades { get; set; }

        public ProcedimentoViewModel()
        {
            this.Especialidades = new List<EspecialidadeViewModel>();
        }
    }
}