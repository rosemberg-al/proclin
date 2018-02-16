using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Pessoa
    {
        public Pessoa() {  }

        public int IdPessoa { get; set; }
        public string Nome { get; set; } // ou Fantasia
        public string RazaoSocial { get; set; }
        public string Tipo { get; set; }
        public string Sexo { get; set; }

        public System.DateTime? DataNascimento { get; set; }
        public string CpfCnpj { get; set; }
        public string RG { get; set; }
        public string IE { get; set; }
        public string Situacao { get; set; }
        public string Profissao { get; set; }
        public string Mae { get; set; }
        public string Pai { get; set; }
        public string Conjuge { get; set; }

        public string Telefone1 { get; set; }
        public string Telefone2 { get; set; }
        public string Site { get; set; }
        public string Email { get; set; }
        public string Observacoes { get; set; }

        public int IdClinica { get; set; }
        public Clinica Clinica { get; set; }


        public virtual ICollection<Financeiro> Financeiroes { get; set; }

        #region [ ENDERECO ]
        public string TipoEndereco { get; set; }
        public string Cep { get; set; }
        public int IdEstado { get; set; }
        public virtual Estado Estado { get; set; }
        public int IdCidade { get; set; }
        public virtual Cidade Cidade { get; set; }
        public string Bairro { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Referencia { get; set; }
        #endregion

        #region [ AUDITORIA ]
        public DateTime DataInclusao { get; set; }
        public int IdUsuarioInclusao { get; set; }
        public Usuario UsuarioInclusao { get; set; }

        public DateTime? DataAlteracao { get; set; }
        public int? IdUsuarioAlteracao { get; set; }
        public Usuario UsuarioAlteracao { get; set; }

        public DateTime? DataExclusao { get; set; }
        public int? IdUsuarioExclusao { get; set; }
        public Usuario UsuarioExclusao { get; set; }
        #endregion

        public Pessoa(string nome, string razaoSocial,string sexo, string tipo, DateTime? dataNascimento, string cpfcnpj,
            string rg, string ie, string profissao, string mae, string pai, string email, string site, string telefone1, string telefone2, string observacoes,
            string cep, Estado estado, Cidade cidade, string bairro, string logradouro, string numero, string complemento, string referencia, Usuario usuarioInclusao, string conjuge,Clinica clinica)
        {
            SetNome(nome);
            SetRazaoSocial(razaoSocial);
            SetTipo(tipo);
            SetDataNascimento(dataNascimento);
            SetCpfCnpj(cpfcnpj);
            SetRg(rg);
            SetIe(ie);
            SetSituacao("Ativo");
            SetProfissao(profissao);
            SetMae(mae);
            SetPai(pai);
            SetEmail(email);
            SetTelefone1(telefone1);
            SetTelefone2(telefone2);
            SetSite(site);
            SetObservacoes(observacoes);
            SetInclusao(usuarioInclusao);
            SetConjugue(conjuge);
            SetEndereco(cep, estado, cidade, bairro, logradouro, numero, complemento, referencia);
            SetSexo(sexo);
            SetClinica(clinica);

            if (tipo == "PF")
                ValidaPessoaPF();
            else if(tipo=="PJ")
                ValidaPessoaPJ();
            else
                throw new Exception("Tipo Pessoa não definido");
           
            if (this.Estado == null)
                throw new Exception("O Campo Estado é Obrigatório ");
            if (this.Cidade == null)
                throw new Exception("O Campo Cidade é Obrigatório ");

            if (this.Clinica == null)
            {
                throw new Exception("Clínica não definida ");
            }

        }

        public Pessoa(string nome, string sexo, string tipo, DateTime? dataNascimento, string cpfcnpj,
            string mae, string pai, Usuario usuarioInclusao, Clinica clinica)
        {
            SetNome(nome);
            SetTipo(tipo);
            SetDataNascimento(dataNascimento);
            SetCpfCnpj(cpfcnpj);
            SetSituacao("Ativo");
            SetMae(mae);
            SetPai(pai);
            SetInclusao(usuarioInclusao);
            SetSexo(sexo);
            SetClinica(clinica);
        }

        public void ValidaPessoaPF()
        {
            if (string.IsNullOrEmpty(Nome))
                throw new Exception("O Campo Nome é Obrigatório ");

            if (string.IsNullOrEmpty(Sexo))
                throw new Exception("O Campo Sexo é Obrigatório ");

            if (DataNascimento == null)
                throw new Exception("O Campo Data Nascimento é Obrigatório ");

            if (string.IsNullOrEmpty(CpfCnpj))
                throw new Exception("O Campo CPF é Obrigatório ");

            // pj
            this.RazaoSocial = null;
            this.IE = null;            
        }

        public void ValidaPessoaPJ()
        {
            if (string.IsNullOrEmpty(Nome))
                throw new Exception("O Campo Nome Fantasia é Obrigatório ");

            if (string.IsNullOrEmpty(RazaoSocial))
                throw new Exception("O Campo Razão Social é Obrigatório ");

            if (string.IsNullOrEmpty(CpfCnpj))
                throw new Exception("O Campo CNPJ é Obrigatório ");

            // pf
            this.DataNascimento = null;
            this.Sexo = null;
        }

        public void SetEndereco(string cep, Estado estado, Cidade cidade, string bairro, string logradouro, string numero, string complemento, string referencia)
        {
            this.Cep = cep;
            this.Estado = estado;
            this.Cidade = cidade;
            this.Bairro = bairro;
            this.Logradouro = logradouro;
            this.Numero = numero;
            this.Complemento = complemento;
            this.Referencia = referencia;
        }

        public void SetInclusao(Usuario usuario)
        {
            this.UsuarioInclusao = usuario;
            this.DataInclusao = DateTime.Now;
        }

        

        public void SetAlteracao(Usuario usuario)
        {
            this.UsuarioAlteracao = usuario;
            this.DataAlteracao = DateTime.Now;
        }

        public void SetExclusao(Usuario usuario)
        {
            this.UsuarioExclusao = usuario;
            this.DataExclusao = DateTime.Now;
        }

        public void SetObservacoes(string observacoes)
        {
            this.Observacoes = observacoes;
        }

        public void SetSite(string site)
        {
            this.Site = site;
        }

        public void SetTelefone1(string telefone1)
        {
            this.Telefone1 = telefone1;
        }

        public void SetTelefone2(string telefone2)
        {
            this.Telefone2 = telefone2;
        }

        public void SetEmail(string email)
        {
            this.Email = email;
        }

        public void SetPai(string pai)
        {
            this.Pai = pai;
        }

        public void SetMae(string mae)
        {
            this.Mae = mae;
        }

        public void SetProfissao(string profissao)
        {
            this.Profissao = profissao;
        }

        public void SetSituacao(string situacao)
        {
            this.Situacao = situacao;
        }

        public void SetIe(string ie)
        {
            this.IE = ie;
        }

        public void SetRg(string rg)
        {
            this.RG = rg;
        }

        public void SetSexo(string sexo)
        {
            this.Sexo = sexo;
        }

        public void SetConjugue(string conjugue)
        {
            this.Conjuge = conjugue;
        }

        public void SetCpfCnpj(string cpfcnpj)
        {
            this.CpfCnpj = cpfcnpj;
        }

        public void SetDataNascimento(DateTime? dataNascimento)
        {
            this.DataNascimento = dataNascimento;
        }

        public void SetTipo(string tipo)
        {
            this.Tipo = tipo;
        }

        public void SetRazaoSocial(string razaoSocial)
        {
            this.RazaoSocial = razaoSocial;
        }

        public void SetNome(string nome)
        {
            this.Nome = nome;
        }

        public void SetClinica(Clinica clinica)
        {
            this.Clinica = clinica;
        }
    }
}