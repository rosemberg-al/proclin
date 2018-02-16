using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Medico
    {
        public int IdMedico { get; private set; }
        public string NomeMedico { get; private set; }
        public string Crm { get; private set; }
        public DateTime DataCadastro { get; private set; }

        private Medico() { }

        public Medico(string nome, string crm)
        {
            SetNomeMedico(nome);
            SetCrm(crm);
            SetDataCadastro(DateTime.Now);
        }

        public void SetDataCadastro(DateTime data)
        {
            if (data != null)
                DataCadastro = data;
        }

        public void SetCrm(string crm)
        {
            if (!String.IsNullOrEmpty(crm))
                Crm = crm;
        }

        public void SetNomeMedico(string nome)
        {
            if (!String.IsNullOrEmpty(nome))
                NomeMedico = nome;
        }
    }
}