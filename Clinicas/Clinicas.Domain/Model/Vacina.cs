using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Vacina
    {
        public int IdVacina { get; private set; }
        public string Descricao { get; private set; }
        public string Idade { get; private set; }
        public string Doses { get; private set; }
        public string Situacao { get; private set; }

        private Vacina() { }

        public Vacina(string descricao, string situacao)
        {
            SetDescricao(descricao);
            SetSituacao(situacao);
        }

        public void SetSituacao(string situacao)
        {
            if (!String.IsNullOrEmpty(situacao))
                Situacao = situacao;
        }

        public void SetDescricao(string descricao)
        {
            if (!String.IsNullOrEmpty(descricao))
                Descricao = descricao;
        }
    }
}