using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class FinanceiroParcela
    {
        public int IdParcela { get; set; }
        public int Numero { get; set; }
        public int IdFinanceiro { get; set; }
        public int? IdConta { get; set; }
        public int? IdPlanoConta { get; set; }
        public int? IdMeioPagamento { get; set; }
        public virtual MeioPagamento MeioPagamento { get; set; }
        public virtual Financeiro Financeiro { get; set; }
        public DateTime DataVencimento { get; set; }
        public virtual PlanoConta PlanoConta { get; set; }
        public virtual Conta Conta { get; set; }
        public decimal Valor { get; set; }
        public string Situacao { get; set; }
        public DateTime? DataAcerto { get; set; }
        public int? DiasAtrazo { get; set; }
        public decimal? ValorJuros { get; set; }
        public decimal? ValorMora { get; set; }
        public decimal? ValorAcrescimo { get; set; }
        public decimal? ValorDesconto { get; set; }
        public decimal? TotalAcerto { get; set; }

        public DateTime DataInclusao { get; set; }
        public int IdUsuarioInclusao { get; set; }
        public Usuario UsuarioInclusao { get; set; }

        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public Usuario UsuarioAlteracao { get; set; }

        public DateTime? DataExclusao { get; set; }
        public int? IdUsuarioExclusao { get; set; }
        public Usuario UsuarioExclusao { get; set; }

        public DateTime? DataBaixa { get; set; }
        public int? IdUsuarioBaixa { get; set; }
        public Usuario UsuarioBaixa { get; set; }

        public string Observacao { get; set; }
        public string NossoNumero { get; set; }

        public int? IdRemessa { get; set; }
        public Remessa Remessa { get; set; }

        protected FinanceiroParcela() { }

        /// <summary>
        /// inclui parcela em aberto
        /// </summary>
        public FinanceiroParcela(
            int numero,
            DateTime dataVencimento,
            decimal valor,
            DateTime dataInclusao,
            string observacao,
            Conta conta,
            PlanoConta planoConta,
            Usuario usuarioInclusao)
        {
            this.DataVencimento = dataVencimento;
            this.Valor = valor;
            this.DataInclusao = dataInclusao;
            this.Observacao = observacao;
            this.Numero = numero;
            this.Situacao = "Aberto";
            this.Conta = conta;
            this.PlanoConta = planoConta;
            this.UsuarioInclusao = usuarioInclusao;
            this.DataInclusao = DateTime.Now;

        }

        public FinanceiroParcela(
           int numero,
           DateTime dataVencimento,
           decimal valor,
           DateTime dataInclusao,
           Conta conta,
           PlanoConta planoConta,
           Usuario usuarioInclusao)
        {
            this.DataVencimento = dataVencimento;
            this.Valor = valor;
            this.DataInclusao = dataInclusao;
            this.Numero = numero;
            this.Situacao = "Aberto";
            this.Conta = conta;
            this.PlanoConta = planoConta;
            this.UsuarioInclusao = usuarioInclusao;
            this.DataInclusao = DateTime.Now;

        }

        /// <summary>
        /// inclui parcela baixada
        /// </summary>
        public FinanceiroParcela(
            int numero,
            DateTime dataVencimento,
            decimal valor,
            DateTime dataInclusao,
            DateTime dataAcerto,
            decimal valorAcerto,
            string observacao,
            string nossoNumero,
            Conta conta,
            PlanoConta planoConta,
            MeioPagamento meiopagamento,
            Usuario usuarioInclusao,
            Usuario usuarioBaixa)
        {
            this.DataVencimento = dataVencimento;
            this.Valor = valor;
            this.DataInclusao = dataInclusao;
            this.Observacao = observacao;
            this.NossoNumero = nossoNumero;
            this.Numero = numero;
            this.Situacao = "Baixado";
            this.Conta = conta;
            this.PlanoConta = planoConta;
            this.UsuarioInclusao = usuarioInclusao;
            this.DataInclusao = DateTime.Now;
            this.MeioPagamento = meiopagamento;
            this.TotalAcerto = valorAcerto;


            this.DataAcerto = dataAcerto;
            this.UsuarioBaixa = UsuarioInclusao;
            this.DataBaixa = DateTime.Now;
        }

        public void SetConta(Conta conta)
        {
            if (conta != null)
                this.Conta = conta;
        }

        public void SetPlanoConta(PlanoConta plano)
        {
            if (plano != null)
                this.PlanoConta = plano;
        }

        public void SetSituacao(string situacao)
        {
            this.Situacao = situacao;
        }
        public void SetUsuarioExclusao(Usuario usuario)
        {
            this.UsuarioExclusao = usuario;
            this.DataExclusao = DateTime.Now;
        }

        public void SetUsuarioAlteracao(Usuario usuario)
        {
            this.UsuarioAlteracao = usuario;
            this.DataAlteracao = DateTime.Now;
        }

        public void SetUsuarioBaixar(Usuario usuario)
        {
            this.UsuarioBaixa = usuario;
            this.DataBaixa = DateTime.Now;
        }
        public void SetTotalAcerto(decimal totalAcerto)
        {
            this.TotalAcerto = totalAcerto;
        }

        public void SetObservacao(string observacao)
        {
            if (!string.IsNullOrEmpty(observacao))
                Observacao = observacao;
        }
        public void SetDataAcerto(DateTime dataAcerto)
        {
            if (dataAcerto != DateTime.MinValue)
                this.DataAcerto = dataAcerto;
        }
        public void SetMeioPagamento(MeioPagamento meiopagamento)
        {
            this.MeioPagamento = meiopagamento;
        }
        public void CriarTitulo()
        {
            this.NossoNumero = this.IdParcela.ToString().PadLeft(13, '0');
        }

        public void SetValorDesconto(decimal valorDesconto)
        {
            if (valorDesconto > 0)
                this.ValorDesconto = valorDesconto;
        }

        public void SetValorAcrescimo(decimal valorAcrescimo)
        {
            if (valorAcrescimo > 0)
                this.ValorAcrescimo = valorAcrescimo;
        }
    }
}