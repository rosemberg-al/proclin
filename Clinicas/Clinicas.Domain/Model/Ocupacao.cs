using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Ocupacao
    {
        public int IdOcupacao { get; private set; }
        public string NmOcupacao { get; private set; }
        public string Codigo { get; private set; }

        private Ocupacao() { }

        public Ocupacao(string nome, string codigo)
        {
            SetNomeOcupacao(nome);
            SetCodigoOcupacao(codigo);
        }

        public void SetCodigoOcupacao(string codigo)
        {
            if (!string.IsNullOrEmpty(codigo))
                Codigo = codigo;
        }

        public void SetNomeOcupacao(string nome)
        {
            if (!string.IsNullOrEmpty(nome))
                NmOcupacao = nome;
        }
    }
}