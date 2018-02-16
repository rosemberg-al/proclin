using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Convenio
    {
        public int IdConvenio { get; private set; }
        public Pessoa Pessoa { get; private set; }
        public Byte[] LogoGuia{ get; private set; }

        public string RegistroAns { get; private set; }

        public Convenio() { }

        public Convenio(Pessoa pessoa,string registroAns)
        {
            SetPessoa(pessoa);
            SetRegistroAns(registroAns);
        }

        public void SetRegistroAns(string registroAns)
        {
            this.RegistroAns = registroAns;
        }

        public void SetLogoGuia(Byte[] logoGuia)
        {
            this.LogoGuia = logoGuia;
        }

        public void SetPessoa(Pessoa pessoa)
        {
            this.Pessoa = pessoa;
        }
       
    }
}