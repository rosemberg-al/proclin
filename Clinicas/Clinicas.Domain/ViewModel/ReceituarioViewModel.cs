using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class ReceituarioViewModel
    {
        public int IdReceituario { get; set; }
        public int IdPaciente { get; set; }
        public int IdFuncionario { get; set; }
        public DateTime Data { get; set; }
        public string Situacao { get; set; }
        public string Descricao { get; set; }
        public string NomeFuncionario { get; set; }
        public string NomePaciente { get; set; }
        public List<ReceituarioMedicamentoViewModel> Medicamentos { get; set; }
        public ReceituarioViewModel()
        {
            Medicamentos = new List<ReceituarioMedicamentoViewModel>();
        }

    }
}