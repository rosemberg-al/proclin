using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class MovimentoEstoque
    {
        public int IdMovimentoEstoque { get; private set; }
        public DateTime Data { get; private set; }
        public int Quantidade { get; private set; }
        public string Tipo { get; private set; }
        public string Situacao { get; private set; }
        public int IdUnidadeAtendimento { get; private set; }
        public virtual UnidadeAtendimento UnidadeAtendimento { get; private set; }
        public int IdMaterial { get; private set; }
        public virtual Material Material { get; private set; }

        public MovimentoEstoque() { }

        public MovimentoEstoque(DateTime data,string tipo,int quantidade, UnidadeAtendimento unidadeAtendimento)
        {
            SetData(data);
            SetTipo(tipo);
            SetQuantidade(quantidade);
            SetUnidadeAtendimento(unidadeAtendimento);
            SetSituacao("Ativo");

        }

        public void SetSituacao(string situacao)
        {
            this.Situacao = situacao;
        }

        public void SetData(DateTime data)
        {
            this.Data = data;
        }

        public void SetTipo(string tipo)
        {
            this.Tipo = tipo;
        }

        public void SetQuantidade(int quantidade)
        {
            this.Quantidade = quantidade;
        }

        public void SetUnidadeAtendimento(UnidadeAtendimento unidadeAtendimento)
        {
            this.UnidadeAtendimento = unidadeAtendimento;
        }

        public void SetMaterial(Material material)
        {
            this.Material = material;
        }
    }
}