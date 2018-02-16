using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class UnidadeAtendimentoViewModel
    {
        public int IdUnidadeAtendimento { get; set; }
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public int IdClinica { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Complemento { get; set; }
        public int IdEstado { get; set; }
        public int IdCidade { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Email { get; set; }
        public string Telefone1 { get; set; }
        public string Telefone2 { get; set; }
        public string Fax { get; set; }
        public string Clinica { get; set; }
        public string Situacao { get; set; }

        public ICollection<EspecialidadeViewModel> Especialidades { get; set; }

        public UnidadeAtendimentoViewModel()
        {
            this.Especialidades = new List<EspecialidadeViewModel>();
        }
    }
}