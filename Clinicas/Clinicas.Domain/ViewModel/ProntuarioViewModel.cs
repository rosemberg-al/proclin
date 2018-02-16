using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class ProntuarioViewModel
    {
        public int CdPaciente { get;  set; }
        public int? IdDadosNascimento { get; set; }
        public string NmPaciente { get;  set; }
        public string Sexo { get;  set; }
        public DateTime? DtNascimento { get;  set; }
        public string CPF { get;  set; }
        public string RG { get;  set; }
        public string Mae { get;  set; }
        public string Pai { get;  set; }
        public string Cep { get;  set; }
        public string Logradouro { get;  set; }
        public string Nr { get;  set; }
        public string Bairro { get;  set; }
        public string Cidade { get;  set; }
        public string Estado { get;  set; }
        public string Complemento { get;  set; }
        public string Referencia { get;  set; }
        public string TelCelular { get;  set; }
        public string TelResidencial { get;  set; }
        public string Email { get;  set; }
        public string Observacao { get;  set; }
        public string Situacao { get;  set; }
        public string CartaoSus { get;  set; }
        public string Foto { get;  set; }
        public string IdadePaciente { get; set; }

    }
}