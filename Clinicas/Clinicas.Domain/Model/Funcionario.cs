using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Funcionario
    {
        public int IdFuncionario { get; private set; }
        public Pessoa Pessoa { get; private set; }

        public string Tipo { get; private set; }
        public string RegistroConselho { get; private set; }

        public virtual ICollection<Especialidade> Especialidades { get; private set; }

        public Funcionario() {
            this.Especialidades = new List<Especialidade>();
        }

        public void AddEspecialidades(Especialidade especialidade)
        {
            if (Especialidades == null)
                Especialidades = new List<Especialidade>();

            Especialidades.Add(especialidade);
        }

        public Funcionario(Pessoa pessoa,string tipo)
        {
            SetPessoa(pessoa);
            SetTipo(tipo);
        }
        public void SetPessoa(Pessoa pessoa)
        {
            this.Pessoa = pessoa;
        }
        public void SetTipo(string tipo)
        {
            this.Tipo = tipo;
        }
        public void SetRegistroConselho(string registroConselho)
        {
            this.RegistroConselho = registroConselho;
        }
    }
}