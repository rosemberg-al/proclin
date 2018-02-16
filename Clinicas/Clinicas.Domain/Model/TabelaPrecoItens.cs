using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class TabelaPrecoItens
    {
        public int IdTabelaPreco { get; private set; }
        public int IdProcedimento { get; private set; }
        public decimal Valor { get; private set; }
        public decimal ValorProfissional { get; private set; }
        public virtual Procedimento Procedimento { get; private set; }
        public virtual TabelaPreco TabelaPreco { get; private set; }

        private TabelaPrecoItens() { }

        public TabelaPrecoItens(decimal valor, decimal valorprofissional, Procedimento procedimento, TabelaPreco tabela)
        {
            SetValor(valor);
            SetValorProfissional(valorprofissional);
            SetProcedimento(procedimento);
            SetTabela(tabela);
        }

        public void SetTabela(TabelaPreco tabela)
        {
            if (tabela == null)
                throw new Exception("A tabela é obrigatória!");

            TabelaPreco = tabela;
        }

        public void SetProcedimento(Procedimento procedimento)
        {
            if (procedimento == null)
                throw new Exception("O procedimento é obrigatório!");

            Procedimento = procedimento;
        }

        public void SetValorProfissional(decimal valorprofissional)
        {
            ValorProfissional = valorprofissional;
        }

        public void SetValor(decimal valor)
        {
            Valor = valor;
        }
    }
}