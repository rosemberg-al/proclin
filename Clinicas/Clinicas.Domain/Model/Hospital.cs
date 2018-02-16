using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Hospital
    {
        public int IdHospital { get; private set; }
        public string Nome { get; private set; }

        private Hospital() { }
        public Hospital(string nome)
        {
            SetNome(nome);
        }
        public void SetNome(string nome)
        {
            if (!String.IsNullOrEmpty(nome))
                Nome = nome;
        }
    }
}