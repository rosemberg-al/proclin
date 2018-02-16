using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class TabelaPreco
    {
        public int IdTabelaPreco { get; private set; }
        public int IdClinica { get; private set; }
        public int? IdConvenio { get; private set; }
        public string Nome { get; private set; }
        public string Tipo { get; private set; }
        public string Situacao { get; private set; }
        public virtual Clinica Clinica { get; private set; }
        public virtual Convenio Convenio { get; private set; }
        public virtual List<TabelaPrecoItens> Itens { get; private set; }

        private TabelaPreco() { }

        public TabelaPreco(string nome, string tipo, Clinica clinica)
        {
            SetNome(nome);
            SetTipo(tipo);
            SetClinica(clinica);
            SetSituacao("Ativo");
        }
        public void AddIten(TabelaPrecoItens item)
        {
            if (Itens == null)
                Itens = new List<TabelaPrecoItens>();

            Itens.Add(item);
        }

        public void SetConvenio(Convenio convenio)
        {
            if(convenio == null)
                throw new Exception("O Convênio é obrigatório");

            Convenio = convenio;
        }

        public void SetClinica(Clinica clinica)
        {
            if (clinica == null)
                throw new Exception("A clinica é obrigatória");

            Clinica = clinica;
        }

        public void SetTipo(string tipo)
        {
            if(String.IsNullOrEmpty(tipo))
                throw new Exception("O Tipo é obrigatório");

            Tipo = tipo;
        }

        public void SetNome(string nome)
        {
            if (String.IsNullOrEmpty(nome))
                throw new Exception("O Nome é obrigatório");

            Nome = nome;
        }

        public void SetSituacao(string situacao)
        {
            if (string.IsNullOrEmpty(situacao)) 
                throw new Exception("Situação é obrigatório");

            this.Situacao = situacao;
        }
    }
}