using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Tiss
{
    public class Guia
    {
        public int IdGuia { get; set; }
        public int IdBeneficiario { get; set; }
        public int IdConvenio { get; set; }
        public int? IdLote { get; set; }

        public CabecalhoGuia CabecalhoGuia { get; private set; }
        public DadosBeneficiario DadosBeneficiario { get; private set; }
        public DadosContratado DadosContratado { get; private set; }
        public DadosAtendimento DadosAtendimento { get; private set; }
        public HipoteseDiagnostica HipoteseDiagnostica { get; private set; }
        public int IdClinica { get; private set; }
        public Clinica Clinica { get; private set; }
        public Convenio Convenio { get; private set; }
        public virtual Lote Lote { get; private set; }

        public string TipoGuia { get; set; }
        public string Situacao { get; set; }
        public Guia() { }

        public void GerarGuiaConsulta(CabecalhoGuia cabecalhoGuia, DadosBeneficiario dadosBeneficiario, DadosContratado dadosContratado, HipoteseDiagnostica hipoteseDiagnostica, DadosAtendimento dadosAtendimento,Clinica clinica)
        {
            if (cabecalhoGuia == null)
                throw new Exception("Obrigatório preenchimento do cabeçalho da guia ");
            else
                this.CabecalhoGuia = cabecalhoGuia;

            if (dadosBeneficiario == null)
                throw new Exception("Dados Beneficiários é Obrigatório ");
            else
                this.DadosBeneficiario = dadosBeneficiario;

            if (dadosContratado == null)
                throw new Exception("Dados Contratato é Obrigatório ");
            else
                this.DadosContratado = dadosContratado;

            this.HipoteseDiagnostica = hipoteseDiagnostica;

            if (dadosAtendimento == null)
                throw new Exception("Dados Atendimento é Obrigatório ");
            else
                this.DadosAtendimento = dadosAtendimento;

            this.TipoGuia = "Consulta";
            this.Situacao = "Emitida";
            this.Clinica = clinica;
        }

        public void GerarGuiaSPSADT(DadosAutorizacao dadosautorizaacao, DadosBeneficiario dadosBeneficiario, DadosContratado dadosContratado)
        {

            this.Situacao = "Emitida"; 
        }

        public void GerarGuiaSolicitacaoInternacao()
        {

        }

        public void GerarGuiaResumoInternacao()
        {

        }

        public void GerarGuiaHonorarioIndividual()
        {

        }

        public void SetClinica(Clinica clinica)
        {
            this.Clinica = clinica;
        }

        public void Cancelar()
        {
            this.Situacao = "Cancelada";
        }
    }

    public class CabecalhoGuia
    {
        public string RegistroANS { get; set; }
        public string NumeroGuia { get; set; }
        public DateTime DataEmissaoGuia { get; set; }

        public CabecalhoGuia() { }

        public CabecalhoGuia(string registroans, string numeroguia, DateTime dataEmissao)
        {
            if (String.IsNullOrEmpty(registroans))
                throw new Exception("O Campo Registro ANS é Obrigatório ");
            else
                this.RegistroANS = registroans;

            if (String.IsNullOrEmpty(numeroguia))
                throw new Exception("O Campo Número da Guia é Obrigatório ");
            else
                this.NumeroGuia = numeroguia;

            if (DataEmissaoGuia == null)
                throw new Exception("O Campo Data da Emissão é Obrigatório ");
            else
                this.DataEmissaoGuia = dataEmissao;
        }
    }

    public class DadosBeneficiario
    {
        public DadosBeneficiario() { }

        public string NumeroCarteira { get; set; }
        public string Plano { get; set; }
        public DateTime ValidadeCarteira { get; set; }
        public string Nome { get; set; }
        public string NumeroCartaoSus { get; set; }

        public DadosBeneficiario(string numeroCarteira, string plano, DateTime validadeCarteira, string nome, string numeroCartaoSus)
        {
            if (String.IsNullOrEmpty(numeroCarteira))
                throw new Exception("O Campo Número da Carteira é Obrigatório ");
            else
                this.NumeroCarteira = numeroCarteira;

            if (String.IsNullOrEmpty(plano))
                throw new Exception("O Campo Plano é Obrigatório ");
            else
                this.Plano = plano;

            this.ValidadeCarteira = validadeCarteira;

            if (String.IsNullOrEmpty(nome))
                throw new Exception("O Campo Nome é Obrigatório ");
            else
                this.Nome = nome;

            this.NumeroCartaoSus = numeroCartaoSus;
        }
    }

    public class DadosContratado
    {
        public DadosContratado() { }
        public string TipoPessoa { get; set; }
        public string CodigoNaOperadora { get; set; }
        public string NomeContratado { get; set; }
        public string CodigoCnes { get; set; }
        public string TipoLogradouro { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Municipio { get; set; }
        public string UFContratado { get; set; }
        public string CodigoIBGEMunicipio { get; set; }
        public string CEP { get; set; }
        public string NomeProfissionalExecutante { get; set; }
        public string ConselhoProfissional { get; set; }
        public string NumeroConselho { get; set; }
        public string UFConselho { get; set; }
        public string CodigoCBOs { get; set; }

        public DadosContratado(string tipoPessoa, string codigonaOperadora, string nomeContratado, string codigoCnes, string tipoLogradouro, string logradouro, string numero, string complemento,
            string municipio, string ufContratado, string codigoIBGEMunicipio, string cep, string nomeProfissionalExecutante, string conselhoProfissional,
            string numeroConselho, string ufConselho, string codigoCbos)
        {
            if (String.IsNullOrEmpty(codigonaOperadora))
                throw new Exception("O Código na Operadora / CNPJ / CPF é Obrigatório ");
            else
                this.CodigoNaOperadora = codigonaOperadora;

            if (String.IsNullOrEmpty(nomeContratado))
                throw new Exception("O Nome do contratado é Obrigatório ");
            else
                this.NomeContratado = nomeContratado;

            this.NomeContratado = nomeContratado;
            this.CodigoCnes = codigoCnes;
            this.TipoLogradouro = tipoLogradouro;
            this.Logradouro = logradouro;
            this.Numero = numero;
            this.Complemento = complemento;
            this.Municipio = municipio;
            this.UFContratado = ufContratado;
            this.CodigoIBGEMunicipio = codigoIBGEMunicipio;
            this.CEP = cep;
            this.TipoPessoa = tipoPessoa;
            this.NomeProfissionalExecutante = nomeProfissionalExecutante;
            this.CodigoCBOs = codigoCbos;

            if (String.IsNullOrEmpty(conselhoProfissional))
                throw new Exception("O Campo Conselho Profissional é Obrigatório ");
            else
                this.ConselhoProfissional = conselhoProfissional;

            if (String.IsNullOrEmpty(numeroConselho))
                throw new Exception("O Campo Conselho Profissional é Obrigatório ");
            else
                this.NumeroConselho = numeroConselho;

            if (String.IsNullOrEmpty(ufConselho))
                throw new Exception("O Campo UF é Obrigatório ");
            else
                this.UFConselho = ufConselho;

            if (tipoPessoa == "PJ" && string.IsNullOrEmpty(this.NomeProfissionalExecutante))
                throw new Exception(" Para pessoas jurídicas o profissional Executante é Obrigatório ");
        }
    }

    public class HipoteseDiagnostica
    {
        public HipoteseDiagnostica() { }
        public string TipoDoenca { get; set; }
        public string TempoDoenca { get; set; }
        public string IndicacaoAcidente { get; set; }
        public string CID10 { get; set; }
        public string CID102 { get; set; }
        public string CID103 { get; set; }
        public string CID104 { get; set; }

        public HipoteseDiagnostica(string tipoDoenca, string tempoDoenca, string indicacaoAcidente, string cid10, string cid102, string cid103, string cid104)
        {
            this.TipoDoenca = tipoDoenca;
            this.TempoDoenca = tempoDoenca;
            this.IndicacaoAcidente = indicacaoAcidente;
            this.CID10 = cid10;
            this.CID102 = cid102;
            this.CID103 = cid103;
            this.CID104 = cid104;
        }
    }

    public class DadosAtendimento
    {
        public DadosAtendimento() { }
        public DateTime DataAtendimento { get; set; }
        public string CodigoTabela { get; set; }
        public string CodigoProcedimento { get; set; }
        public string TipoConsulta { get; set; }
        public string TipoSaida { get; set; }
        public string Observacao { get; set; }
        public DateTime? DataAssinaturaMedico { get; set; }
        public DateTime? DataAssinaturaBeneficiario { get; set; }

        public DadosAtendimento(DateTime dataAtendimento, string codigoTabela, string codigoProcedimento, string tipoConsulta, string tipoSaida, string observacao, DateTime? dataAssinaturaMedica, DateTime? dataAssinaturaBeneficiario)
        {
            if (dataAtendimento == DateTime.MinValue)
                throw new Exception("O Campo Data de Atendimento é Obrigatório ");
            else
                this.DataAtendimento = dataAtendimento;

            if (String.IsNullOrEmpty(codigoTabela))
                throw new Exception("O Campo Código da Tabela é Obrigatório ");
            else
                this.CodigoTabela = codigoTabela;

            if (String.IsNullOrEmpty(codigoProcedimento))
                throw new Exception("O Campo Código do Procedimento é Obrigatório ");
            else
                this.CodigoProcedimento = codigoProcedimento;

            if (String.IsNullOrEmpty(tipoConsulta))
                throw new Exception("O Campo Tipo de Consulta é Obrigatório ");
            else
                this.TipoConsulta = tipoConsulta;

            if (String.IsNullOrEmpty(tipoSaida))
                throw new Exception("O Campo Tipo de Saída é Obrigatório ");
            else
                this.TipoSaida = tipoSaida;

            this.Observacao = observacao;
            this.DataAssinaturaMedico = dataAssinaturaMedica;
            this.DataAssinaturaBeneficiario = dataAssinaturaBeneficiario;
        }
    }

    public class DadosAutorizacao
    {
        public DadosAutorizacao() { }
        public CabecalhoGuia CabecalhoGuia { get; set; }
        public string NumeroGuiaPrincipal { get; set; }
        public DateTime DataAutorizacao { get; set; }
        public string Senha { get; set; }
        public Boolean PacienteInternado { get; set; }
        public DateTime ValidadeSenha { get; set; }

        public DadosAutorizacao(CabecalhoGuia cabecalhoGuia, bool pacienteInternado, string numeroGuiaPrincipal, DateTime dataAutorizacao, string senha, DateTime validadeSenha)
        {
            this.CabecalhoGuia = cabecalhoGuia;
            this.PacienteInternado = pacienteInternado;
            this.NumeroGuiaPrincipal = numeroGuiaPrincipal;
            this.NumeroGuiaPrincipal = numeroGuiaPrincipal;
            this.DataAutorizacao = dataAutorizacao;
            this.Senha = senha;
            this.ValidadeSenha = validadeSenha;

            // validações
            if (cabecalhoGuia != null)
                throw new Exception("Cabeçalho Guia Obrigatório ");

            if ((pacienteInternado) && (String.IsNullOrEmpty(numeroGuiaPrincipal)))
            {
                throw new Exception("O Campo Número Guia Principal é  Obrigatório quando se tratar de solicitação de SADT em paciente internado ");
            }
        }

    }

    public class DadosSolicitacaoProcedimentosExamesSolicitados
    {
        public DadosSolicitacaoProcedimentosExamesSolicitados() { }
        public DateTime DataHoraSolicitacao { get; set; }
        public string CaraterSolicitacao { get; set; }
        public string CID10 { get; set; }
        public string IndicacaoClinica { get; set; }
        public string CodigoTabela { get; set; }
        public string CodigoProcedimento { get; set; }
        public string DescricaoProcedimento { get; set; }
        public int QuantidadeSolicitada { get; set; }
        public int QuantidadeAutorizada { get; set; }

        public DadosSolicitacaoProcedimentosExamesSolicitados(DateTime dataHoraSolicitacao, string caraterSolicitacao, string cid10, string indicacaoClinica,
            string codigoTabela, string codigoProcedimento, string descricaoProcedimento, int quantidadeSolicitada, int quantidadeAutorizada)
        {
            this.DataHoraSolicitacao = dataHoraSolicitacao;
            this.CaraterSolicitacao = caraterSolicitacao;
            this.CID10 = cid10;
            this.IndicacaoClinica = indicacaoClinica;
            this.CodigoTabela = codigoTabela;
            this.CodigoProcedimento = codigoProcedimento;
            this.DescricaoProcedimento = descricaoProcedimento;
            this.QuantidadeSolicitada = quantidadeSolicitada;
            this.QuantidadeAutorizada = QuantidadeAutorizada;




        }

    }

   
}