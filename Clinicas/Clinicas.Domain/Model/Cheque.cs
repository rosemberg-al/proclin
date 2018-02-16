using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Cheque
    {
        public int IdCheque { get; private set; }
        public int IdClinica { get; private set; }
        public Nullable<System.DateTime> DataInclusao { get; private set; }
        public string Emitente { get; private set; }
        public string Banco { get; private set; }
        public string Agencia { get; private set; }
        public string Conta { get; private set; }
        public decimal Valor { get; private set; }
        public Nullable<System.DateTime> BomPara { get; private set; }
        public string Situacao { get; private set; }
        public Nullable<int> IdFinanceiro { get; private set; }
        public Nullable<int> IdPessoa { get; private set; }
        public string Historico { get; private set; }
        public virtual Pessoa Pessoa { get; private set; }
        public virtual Financeiro Financeiro { get; private set; }
        public virtual Clinica Clinica { get; private set; }

        private Cheque() { }
        public Cheque(string emitente, string banco, string agencia, string conta, string situacao,decimal valor, Clinica clinica)
        {
            SetEmitente(emitente);
            SetBanco(banco);
            SetAgencia(agencia);
            SetConta(conta);
            SetSituacao(situacao);
            DataInclusao = DateTime.Now;
            SetClinica(clinica);
            SetValor(valor);
        }

        public void SetClinica(Clinica clinica)
        {
            if (clinica != null)
                Clinica = clinica;
        }

        public void SetValor(decimal valor)
        {
            this.Valor = valor;
        }
        public void SetPessoa(Pessoa pessoa)
        {
            if (pessoa != null)
                Pessoa = pessoa;
        }

        public void SetFinanceiro(Financeiro financeiro)
        {
            if (financeiro != null)
                Financeiro = financeiro;
        }

        public void SetBomPara(DateTime bompara)
        {
            if (bompara != DateTime.MinValue)
                BomPara = bompara;
        }

        public void SetHistorico(string historico)
        {
            if (!string.IsNullOrEmpty(historico))
                Historico = historico;
        }

        public void SetSituacao(string situacao)
        {
            if (!string.IsNullOrEmpty(situacao))
                Situacao = situacao;
        }

        public void SetConta(string conta)
        {
            if (!string.IsNullOrEmpty(conta))
                Conta = conta;
        }

        public void SetAgencia(string agencia)
        {
            if (!string.IsNullOrEmpty(agencia))
                Agencia = agencia;
        }

        public void SetBanco(string banco)
        {
            if (!string.IsNullOrEmpty(banco))
                Banco = banco;
        }

        public void SetEmitente(string emitente)
        {
            if(!string.IsNullOrEmpty(emitente))
                Emitente = emitente;
        }
    }
}