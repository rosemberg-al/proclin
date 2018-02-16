using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Fornecedor
    {
        public int IdFornecedor { get; private set; }
        public Pessoa Pessoa { get; private set; }

        private Fornecedor() { }

        public Fornecedor(Pessoa pessoa)
        {
            SetPessoa(pessoa);
        }
        public void SetPessoa(Pessoa pessoa)
        {
            this.Pessoa = pessoa;
        }
    }
}