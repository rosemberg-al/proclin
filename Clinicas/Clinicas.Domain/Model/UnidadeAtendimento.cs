using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class UnidadeAtendimento
    {
        public int IdUnidadeAtendimento { get; private set; }
        public int IdClinica { get; private set; }
        public string Nome { get; private set; }
        public string CpfCnpj { get; private set; }
        public virtual Clinica Clinica { get; private set; }
        public string Logradouro { get; private set; }
        public string Numero { get; private set; }
        public string Bairro { get; private set; }
        public string Cep { get; private set; }
        public string Complemento { get; private set; }
        public int IdEstado { get; private set; }
        public int IdCidade { get; private set; }
        public virtual Estado Estado { get; private set; }
        public virtual Cidade Cidade { get; private set; }
        public string Email { get; private set; }
        public string Telefone1 { get; private set; }
        public string Telefone2 { get; private set; }
        public string Fax { get; private set; }
        public string Situacao { get; private set; }
        public virtual ICollection<Especialidade> Especialidades { get; private set; }

        private UnidadeAtendimento() {
            this.Especialidades = new List<Especialidade>();

        }

        // primeiro acesso
        public UnidadeAtendimento(string nome, string cpfcnpj, Clinica clinica,
           Estado estado, Cidade cidade, string email, string tel1,Especialidade especialidadePrincipal)
        {

            SetNome(nome);
            SetCpfCnpj(cpfcnpj);
            SetClinica(clinica);
            SetEstado(estado);
            SetCidade(cidade);
            SetEmail(email);
            SetTelefone1(tel1);
            SetSituacao("Ativo");
            AddEspecialidades(especialidadePrincipal);
        }


        public UnidadeAtendimento(string nome,string cpfcnpj, Clinica clinica, string cep,string logradouro,
            string bairro,string complemento,string numero, Estado estado,Cidade cidade,string email,string tel1,string tel2,string fax) {

            SetNome(nome);
            SetClinica(clinica);
            SetSituacao("Ativo");
            SetCpfCnpj(cpfcnpj);
            SetNumero(numero);
            SetEstado(estado);
            SetCidade(cidade);
            SetTelefone1(Telefone1);
            SetTelefone2(Telefone2);
            SetFax(fax);
            SetComplemento(complemento);
            SetLogradouro(logradouro);
            SetCep(cep);
        }

        public void AddEspecialidades(Especialidade especialidade)
        {
            if (Especialidades == null)
                Especialidades = new List<Especialidade>();

            Especialidades.Add(especialidade);
        }

        public void SetCpfCnpj(string cpfcnpj)
        {
            if (!string.IsNullOrEmpty(cpfcnpj))
                this.CpfCnpj = cpfcnpj;
            else
                throw new Exception("O CPF/CNPJ da Clínica é Obrigatório");
        }

        public void SetClinica(Clinica clinica)
        {
            if (clinica == null)
                throw new Exception("A clinica é obrigatória");

            Clinica = clinica;
        }

        public void SetSituacao(string situacao)
        {
            if (String.IsNullOrEmpty(situacao))
                throw new Exception("Situação Inválida");

            this.Situacao = situacao;
        }

        public void SetNome(string nome)
        {
            if (String.IsNullOrEmpty(nome))
                throw new Exception("O nome é obrigatório!");

            Nome = nome;
        }

        public void SetLogradouro(string logradouro)
        {
            if (string.IsNullOrEmpty(logradouro))
                throw new Exception("O campo logradouro é obrigatório!");

            Logradouro = logradouro;
        }

        public void SetNumero(string numero)
        {
            if (string.IsNullOrEmpty(numero))
                throw new Exception("O campo numero é obrigatório!");

            Numero = numero;
        }

        public void SetBairro(string bairro)
        {
            if (string.IsNullOrEmpty(bairro))
                throw new Exception("O campo bairro é obrigatório!");

            Bairro = bairro;
        }

        public void SetCep(string cep)
        {
            if (string.IsNullOrEmpty(cep))
                throw new Exception("O campo nome é obrigatório!");

            Cep = cep;
        }

        public void SetComplemento(string complemento)
        {
            if (!string.IsNullOrEmpty(complemento))
                Complemento = complemento;
        }

        public void SetEstado(Estado estado)
        {
            if (estado == null)
                throw new Exception("O campo estado é obrigatório!");

            Estado = estado;
        }

        public void SetCidade(Cidade cidade)
        {
            if (cidade == null)
                throw new Exception("O campo cidade é obrigatório!");

            Cidade = cidade;
        }

        public void SetEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new Exception("O campo email é obrigatório!");

            Email = email;
        }

        public void SetTelefone1(string tel1)
        {
            if (string.IsNullOrEmpty(tel1))
                throw new Exception("O campo telefone1 é obrigatório!");

            Telefone1 = tel1;
        }

        public void SetTelefone2(string tel2)
        {
            if (!string.IsNullOrEmpty(tel2))
                Telefone2 = tel2;
        }

        public void SetFax(string fax)
        {
            if (!string.IsNullOrEmpty(fax))
                Fax = fax;
        }
    }
}