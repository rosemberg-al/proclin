using Clinicas.Domain.Tiss;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Lote
    {
        public int IdLote { get; private set; }
        public DateTime Data { get; private set; }
        public int IdConvenio { get; private set; }
        public int IdClinica { get; private set; }
        public string Situacao { get; private set; }
        public Convenio Convenio { get; private set; }
        public Clinica Clinica { get; private set; }
        public List<Guia> Guias { get; private set; }

        private Lote() { }

        public Lote(string situacao, Convenio convenio, Clinica clinica)
        {
            SetSituacao(situacao);
            SetConvenio(convenio);
            SetClinica(clinica);
            Data = DateTime.Now;
        }

        public void AddGuia(Guia guia)
        {
            if (Guias == null)
                Guias = new List<Guia>();

            Guias.Add(guia);
        }

        public void SetClinica(Clinica clinica)
        {
            if (clinica != null)
                Clinica = clinica;
        }

        public void SetConvenio(Convenio convenio)
        {
            if (convenio != null)
                Convenio = convenio;
        }

        public void SetSituacao(string situacao)
        {
            if (!String.IsNullOrEmpty(situacao))
                Situacao = situacao;
        }
    }
}