using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Carteira
    {
        public int IdCarteira { get; private set; }
        public int IdPaciente { get; private set; }
        public int IdConvenio { get; private set; }
        public DateTime ValidadeCarteira { get; private set; }
        public string NumeroCarteira { get; private set; }
        public string Plano { get; private set; }
        public virtual Paciente Paciente { get; private set; }
        public virtual Convenio Convenio { get; private set; }

        public Carteira(string numero, string plano, DateTime data, Convenio convenio, Paciente paciente) {
            SetValidadeCarteira(data);
            SetNumeroCarteira(numero);
            SetPlano(plano);
            SetConvenio(convenio);
            SetPaciente(paciente);
        }

        private Carteira() { }

        private void SetValidadeCarteira(DateTime data)
        {
            if (data == DateTime.MinValue)
                throw new Exception("A data de validade da carteira nã pode ser vazia!");

            ValidadeCarteira = data;
        }

        private void SetPaciente(Paciente paciente)
        {
            if(paciente == null)
                throw new Exception("Não é possível cadastrar uma carteira sem paciente!");

            Paciente = paciente;
        }

        private void SetConvenio(Convenio convenio)
        {
            if (convenio == null)
                throw new Exception("Não é possível cadastrar uma carteira sem convênio!");

            Convenio = convenio;
        }

        private void SetPlano(string plano)
        {
           if(string.IsNullOrEmpty(plano))
                throw new Exception("Não é possível cadastrar uma carteira sem o nome do plano!");

            Plano = plano;
        }

        private void SetNumeroCarteira(string numero)
        {
            if (string.IsNullOrEmpty(numero))
                throw new Exception("Não é possível cadastrar uma carteira sem o número!");

            NumeroCarteira = numero;
        }

        
    }
}