using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class ModeloProntuario
    {
        public int IdModeloProntuario { get; private set; }
        public string Descricao { get; private set; }
        public string NomeModelo { get; private set; }
        public string Tipo { get; private set; }
        public string Situacao { get; private set; }

        private ModeloProntuario() { }

        public ModeloProntuario(string descricao, string tipo, string situacao, string nomeModelo)
        {
            SetDescricao(descricao);
            SetSituacao(situacao);
            SetTipo(tipo);
            SetNomeModelo(nomeModelo);
        }

        public void SetTipo(string tipo)
        {
            if (!String.IsNullOrEmpty(tipo))
                Tipo = tipo;
        }

        public void SetNomeModelo(string nomeModelo)
        {
            if (!String.IsNullOrEmpty(nomeModelo))
                NomeModelo = nomeModelo;
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