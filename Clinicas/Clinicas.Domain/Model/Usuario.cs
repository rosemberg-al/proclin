using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Usuario
    {
        public int IdUsuario { get; private set; }
        public string Login { get; private set; }
        public string Email { get; private set; }
        public string Nome { get; private set; }
        public string Senha { get; private set; }
        public string Tipo { get; private set; }

        public string Situacao { get; private set; }
        public int IdGrupoUsuario { get; private set; }
        public GrupoUsuario GrupoUsuario { get; private set; }

        public int IdClinica { get; private set; }
        public Clinica Clinica { get; private set; }

        public int IdUnidadeAtendimento { get; private set; }
        public UnidadeAtendimento UnidadeAtendimento { get; private set; }

        public int? IdFuncionario { get; private set; }
        public Funcionario Funcionario { get; private set; }

        public string PrimeiroAcesso { get; private set; }

        public Usuario() {}

        public Usuario(string login,string email,string nome,GrupoUsuario grupoUsuario,Clinica clinica,UnidadeAtendimento unidadeAtendimento)
        {
            SetLogin(login);
            SetEmail(email);
            SetNome(nome);
            SetSenha("123456");
            SetSituacao("Ativo");
            SetGrupoUsuario(grupoUsuario);
            SetClinica(clinica);
            SetUnidadeAtendimento(unidadeAtendimento);
        }

        public void SetTipo(string tipo)
        {
            Tipo = tipo;
        }

        public void SetPrimeiroAcesso(string primeiroacesso)
        {
            PrimeiroAcesso = primeiroacesso;
        }

        public void SetClinica(Clinica clinica)
        {
            if (clinica != null)
                this.Clinica = clinica;
            else
                throw new Exception("Clínica não definida");
        }

        public void SetUnidadeAtendimento(UnidadeAtendimento unidade)
        {
            if (unidade != null)
                this.UnidadeAtendimento = unidade;
            else
                throw new Exception("Unidade de Atendimento não definida");
        }

        public void SetGrupoUsuario(GrupoUsuario grupoUsuario)
        {
            if (grupoUsuario!=null)
                this.GrupoUsuario = grupoUsuario;
            else
                throw new Exception("Grupo de Usuário não definido");
        }

        public void SetSenha(string senha)
        {
            if (!string.IsNullOrEmpty(senha))
                this.Senha = senha;
            else
                throw new Exception("Senha não definida");
        }

        public void SetSituacao(string situacao)
        {
            if (!string.IsNullOrEmpty(situacao))
                this.Situacao = situacao;
            else
                throw new Exception("Situação não definida");
        }

        public void SetNome(string nome)
        {
            if (!string.IsNullOrEmpty(nome))
                this.Nome = nome;
            else
                throw new Exception("Nome Obrigatório");
        }

        public void SetLogin(string login)
        {
            if (!string.IsNullOrEmpty(login))
                this.Login = login;
            else
                throw new Exception("Login Obrigatório");
        }

        public void SetEmail(string email)
        {
            if (!string.IsNullOrEmpty(email))
                this.Email = email;
            else
                throw new Exception("Email Obrigatório");
        }

        public void Desativar()
        {
            this.Situacao = "Inativo";
        }

        public void Ativar()
        {
            this.Situacao = "Ativo";
        }

        public void AlterarSenha(string novasenha)
        {
            if (!string.IsNullOrEmpty(novasenha))
                this.Senha = novasenha;
            else
                throw new Exception("Nova Senha Obrigatório");
        }

        public void Excluir()
        {
            this.Situacao = "Excluido";
        }

        public void ResetarSenha()
        {
            string SenhaCaracteresValidos = "abcdefghijklmnopqrstuvwxyz1234567890@#!?";
            //Aqui eu defino o número de caracteres que a senha terá
            int tamanho = 8;

            //Aqui pego o valor máximo de caracteres para gerar a senha
            int valormaximo = SenhaCaracteresValidos.Length;

            //Criamos um objeto do tipo randon
            Random random = new Random(DateTime.Now.Millisecond);

            //Criamos a string que montaremos a senha
            StringBuilder senha = new StringBuilder(tamanho);

            //Fazemos um for adicionando os caracteres a senha
            for (int i = 0; i < tamanho; i++)
                senha.Append(SenhaCaracteresValidos[random.Next(0, valormaximo)]);

            this.Senha = senha.ToString();
        }


        public string CriarSenhaNovoAcesso()
        {
            string SenhaCaracteresValidos = "abcdefghijklmnopqrstuvwxyz1234567890@#!?";
            //Aqui eu defino o número de caracteres que a senha terá
            int tamanho = 8;

            //Aqui pego o valor máximo de caracteres para gerar a senha
            int valormaximo = SenhaCaracteresValidos.Length;

            //Criamos um objeto do tipo randon
            Random random = new Random(DateTime.Now.Millisecond);

            //Criamos a string que montaremos a senha
            StringBuilder senha = new StringBuilder(tamanho);

            //Fazemos um for adicionando os caracteres a senha
            for (int i = 0; i < tamanho; i++)
                senha.Append(SenhaCaracteresValidos[random.Next(0, valormaximo)]);

           return senha.ToString();
        }
    }
}