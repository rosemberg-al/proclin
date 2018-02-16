using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class GuiaConsultaViewModel
    {
        public string RegistroANS { get; set; }
        public string NumeroGuia { get; set; }
        public int IdGuia { get; set; }
        public int IdBeneficiario { get; set; }
        public DateTime DataEmissaoGuia { get; set; }
        public string NumeroCarteira { get; set; }
        public string Plano { get; set; }
        public DateTime ValidadeCarteira { get; set; }
        public string Nome { get; set; }
        public string NumeroCartaoSus { get; set; }
        public string CodigoPrestador { get; set; }
        public string NomeProfissionalExecutante { get; set; }
        public string CodigoCnes { get; set; }
        public string TipoLogradouro { get; set; }
        public string Logradouro { get; set; }
        public string Municipio { get; set; }
        public string UFConselho { get; set; }
        public string CodigoIBGEMunicipio { get; set; }
        public string CEP { get; set; }
        public string NomeExecutor { get; set; }
        public string ConselhoProfissional { get; set; }
        public string NomeContratado { get; set; }
        public string NumeroConselho { get; set; }
        public string CodigoCBOs { get; set; }
        public string TipoDoenca { get; set; }
        public string TempoDoenca { get; set; }
        public string IndicacaoAcidente { get; set; }
        public string CID10 { get; set; }
        public string CID102 { get; set; }
        public string CID103 { get; set; }
        public string CID104 { get; set; }
        public DateTime DataAtendimento { get; set; }
        public string CodigoTabela { get; set; }
        public string CodigoProcedimento { get; set; }
        public string TipoConsulta { get; set; }
        public string TipoSaida { get; set; }
        public string Observacao { get; set; }
        public string TipoPessoa { get; set; }
        public string CodigoNaOperadora { get; set; }
        public string Cnpj { get; set; }
        public string Cpf { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string UfContratado { get; set; }
        public string ValorTempo { get; set; }
        public string Situacao { get; set; }

    }
}