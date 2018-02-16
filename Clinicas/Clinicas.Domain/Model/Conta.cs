using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public partial class Conta
    {
        public Conta()
        {
            this.Financeiroes = new List<FinanceiroParcela>();
        }

        public int IdConta { get; private set; }
        public string NmConta { get; private set; }
        public decimal Saldo { get; private set; }
        public string Situacao { get; set; }
        
        public int IdClinica { get; set; }
        public Clinica Clinica { get; set; }

        public virtual ICollection<FinanceiroParcela> Financeiroes { get; set; }

        public Conta(string nome,Clinica clinica)
        {
            if (string.IsNullOrEmpty(nome))
            {
                throw new Exception("Campo Nome é Obrigatório ");
            }
            this.NmConta = nome;
            this.Saldo = 0;
            this.Situacao = "Ativo";
            SetClinica(clinica);
        }

        public void SetExcluido(string situacao)
        {
            this.Situacao = situacao;
        }

        public void SetNmConta(string nmconta)
        {
            this.NmConta = nmconta;
        }

        public void SetSaldo(decimal saldo)
        {
            this.Saldo = saldo;
        }

        public void SetSituacao(string situacao)
        {
            this.Situacao = situacao;
        }

        public void SetClinica(Clinica clinica)
        {
            if (clinica == null)
            {
                throw new Exception("Campo Clínica é Obrigatório");
            }
            this.Clinica = clinica;
        }



        
    }
}