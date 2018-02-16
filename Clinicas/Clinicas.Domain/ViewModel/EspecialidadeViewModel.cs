using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class EspecialidadeViewModel
    {
        public int IdEspecialidade { get; set; }
        public string NmEspecialidade { get; set; }
        public ICollection<ProcedimentosViewModel> Procedimentos { get; set; }

        public EspecialidadeViewModel()
        {
            this.Procedimentos = new List<ProcedimentosViewModel>(); 
        }
    }
}