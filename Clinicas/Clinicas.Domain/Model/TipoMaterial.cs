using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class TipoMaterial
    {
        public int IdTipoMaterial { get; private set; }
        public string Nome { get; private set; }
        public string Situacao { get; private set; }

        public int IdUnidadeAtendimento { get; private set; }
        public virtual UnidadeAtendimento UnidadeAtendimento { get; private set; }

        public TipoMaterial() { }
        public TipoMaterial(string nome,UnidadeAtendimento unidadeAtendimento)
        {
            SetNome(nome);
            SetUnidadeAtendimento(unidadeAtendimento);
            SetSituacao("Ativo");
        }

        public void SetNome(string nome)
        {
            if (!string.IsNullOrEmpty(nome))
                this.Nome = nome;
            else
                throw new Exception("Campo Nome é Obrigatório");
        }

        public void SetUnidadeAtendimento(UnidadeAtendimento unidadeAtendimento)
        {
            this.UnidadeAtendimento = unidadeAtendimento;
        }

        public void SetSituacao(string situacao)
        {
            this.Situacao = situacao;
        }

    }
}