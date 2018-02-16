using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class PessoaViewModel
    {
        public int IdPessoa { get; set; }
        public string Nome { get; set; } // ou Fantasia
        public string RazaoSocial { get; set; }
        public string Tipo { get; set; }
        public string Sexo { get; set; }

        public System.DateTime? DataNascimento { get; set; }
        public string CpfCnpj { get; set; }
        public string RG { get; set; }
        public string IE { get; set; }
        public string Situacao { get; set; }
        public string Profissao { get; set; }
        public string Mae { get; set; }
        public string Pai { get; set; }
        public string Conjuge { get; set; }

        public string Telefone1 { get; set; }
        public string Telefone2 { get; set; }
        public string Site { get; set; }
        public string Email { get; set; }
        public string Observacoes { get; set; }

        #region [ ENDERECO ]
        public string TipoEndereco { get; set; }
        public string Cep { get; set; }
        public int IdEstado { get; set; }
        public string Estado { get; set; }
        public int IdCidade { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Referencia { get; set; }
        #endregion

    }
}