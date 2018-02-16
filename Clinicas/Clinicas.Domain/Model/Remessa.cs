using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Remessa
    {
        public int IdRemessa { get; set; }
        public string Nome { get; set; }
        public DateTime Data { get; set; }

        public int IdUsuario { get; set; }
        public Usuario Usuario { get; set; }

        public virtual ICollection<FinanceiroParcela> Parcelas { get; set; }

        public Remessa()
        {
            this.Parcelas = new Collection<FinanceiroParcela>();
        }
        public Remessa(string nome, Usuario usuario)
        {
            SetNome(nome);
            SetUsuario(usuario);
            Data = DateTime.Now;
        }

        public void SetNome(string nome)
        {
            if (string.IsNullOrEmpty(nome))
            {
                throw new Exception(" O campo nome deve ser obrigatório ");
            }
            this.Nome = nome;
        }
        public void SetUsuario(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new Exception(" O campo usuário deve ser obrigatório ");
            }
            this.Usuario = usuario;
        }

        public void AddTitulo(FinanceiroParcela parcela)
        {
            if (this.Parcelas == null)
                this.Parcelas = new Collection<FinanceiroParcela>();
            else if (this.Parcelas.Contains(parcela))
                return;
            this.Parcelas.Add(parcela);
        }
    }
}