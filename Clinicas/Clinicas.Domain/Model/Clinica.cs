using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Clinica
    {
        public int IdClinica { get; private set; }
        public string Nome { get; private set; }
        public string Situacao { get; private set; }
        public Byte[] Logo { get; private set; }
        public DateTime DataInclusao { get; private set; }
        public virtual ICollection<UnidadeAtendimento> Unidades { get; private set; }

        public Clinica() {
            this.Unidades = new List<UnidadeAtendimento>();
        }

        public Clinica(string nome) {
            SetNome(nome);
            SetSituacao("Ativo");
            this.DataInclusao = DateTime.Now;
        }
       
        public void SetDataInclusao()
        {
            DataInclusao = DateTime.Now;
        }

        public void SetNome(string nome)
        {
            if (string.IsNullOrEmpty(nome))
                throw new Exception("O campo nome é obrigatório!");
            Nome = nome;
        }

        public void SetSituacao(string situacao)
        {
            if (string.IsNullOrEmpty(situacao))
                throw new Exception("O Campo Situação é obrigatório!");

            Situacao = situacao;
        }

        public void AddUnidade(UnidadeAtendimento unidade)
        {
            if (Unidades == null)
                Unidades = new List<UnidadeAtendimento>();

            Unidades.Add(unidade);
        }

        public void RemoveUnidade(UnidadeAtendimento unidade)
        {
            if (Unidades != null)
            {
                this.Unidades.Remove(unidade);
            }
        }

        public void SetLogo(Byte[] logo)
        {
            if (logo.Count() > 0)
                Logo = logo;
        }
    }
}