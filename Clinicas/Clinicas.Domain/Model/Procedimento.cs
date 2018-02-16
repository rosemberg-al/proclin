using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Procedimento
    {
        public int IdProcedimento { get; private set; }
        public string NomeProcedimento { get; private set; }
        public decimal Valor { get; private set; }
        public decimal ValorProfissional { get; private set; }
        public string Sexo { get; private set; }
        public int Codigo { get; private set; }
        public string Odontologico { get; private set; }
        public string Preparo { get; private set; }
        public virtual ICollection<Especialidade> Especialidades { get; set; }

        private Procedimento() {
            this.Especialidades = new List<Especialidade>();
        }

        public Procedimento(string nome, decimal valor, decimal valorProfissional, string sexo, int codigoSus, string odonto, string preparo)
        {
            SetNomeProcediemnto(nome);
            SetValor(valor);
            SetValorProfissional(valorProfissional);
            SetSexo(sexo);
            SetCodigoSus(codigoSus);
            SetOdontologico(odonto);
            SetPreparo(preparo);
        }

        public void SetPreparo(string preparo)
        {
            if (!string.IsNullOrEmpty(preparo))
                Preparo = preparo;
        }

        public void SetOdontologico(string odonto)
        {
            if (!string.IsNullOrEmpty(odonto))
                Odontologico = odonto;
        }

        public void SetCodigoSus(int codigo)
        {
            if (codigo > 0)
                Codigo = codigo;
        }

        public void SetSexo(string sexo)
        {
            if (!string.IsNullOrEmpty(sexo))
                Sexo = sexo;
        }

        public void SetValorProfissional(decimal valorProfissional)
        {
            if (valorProfissional > 0)
                ValorProfissional = valorProfissional;
        }

        public void SetValor(decimal valor)
        {
            if (valor > 0)
                Valor = valor;
        }
        public void SetNomeProcediemnto(string nome)
        {
            if (!string.IsNullOrEmpty(nome))
                NomeProcedimento = nome;
        }
    }
}