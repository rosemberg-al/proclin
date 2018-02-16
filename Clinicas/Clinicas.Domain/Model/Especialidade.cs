using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Especialidade
    {
        public int IdEspecialidade { get; private set; }
        public string NmEspecialidade { get; private set; }
        public string Situacao { get; private set; }
        public virtual ICollection<Procedimento> Procedimentos { get; set; }
        public virtual ICollection<Funcionario> Funcionarios { get; set; }
        public virtual ICollection<UnidadeAtendimento> UnidadeAtendimentos { get; set; }

        private Especialidade() {

            this.Procedimentos = new List<Procedimento>();
            this.Funcionarios = new List<Funcionario>();
            this.UnidadeAtendimentos = new List<UnidadeAtendimento>();
        }

        public Especialidade(string nome)
        {
            SetNomeEspecialidade(nome);
            SetSituacao("Ativo");
        }

        public void SetNomeEspecialidade(string nome)
        {
            if (!String.IsNullOrEmpty(nome))
                NmEspecialidade = nome;
        }

        public void SetSituacao(string situacao)
        {
            if (!String.IsNullOrEmpty(situacao))
                this.Situacao = situacao;
        }
    }
}