using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class ConvenioViewModel
    {
        public int IdConvenio { get; set; }

        public string Tipo { get; set; }
        public string Nome { get; set; }
        public string Sexo { get; set; }
        public string Pai { get; set; }
        public string Mae { get; set; }
        public string Profissao { get; set; }
        public string Site { get; set; }
        public string Conjuge { get; set; }
        public string CpfCnpj { get; set; }
        public string RegistroAns { get; set; }
        public Byte[] LogoGuia { get; set; }

        public DateTime? DataNascimento { get; set; }
        public string CPF { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string CNPJ { get; set; }
        public string Rg { get; set; }
        public string InscricaoEstadual { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        public int CidadeSelecionada { get; set; }
        public int EstadoSelecionado { get; set; }
        public string Complemento { get; set; }
        public string Referencia { get; set; }
        public string Telefone1 { get; set; }
        public string Telefone2 { get; set; }
        public string Email { get; set; }
        public string Observacao { get; set; }
        public string Situacao { get; set; }
    }
}