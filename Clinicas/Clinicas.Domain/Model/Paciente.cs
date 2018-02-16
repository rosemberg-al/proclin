using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Paciente
    {
        public int IdPaciente { get;  set; }
        public Pessoa Pessoa { get; set; }

        public int? IdDadosNascimento { get; private set; }

        public string CartaoSus { get; private set; }
        public Byte[] Foto { get; private set; }
        public string Situacao { get; private set; }

       public string Idadae { get;  }

        public virtual DadosNascimento DadosNascimento { get; private set; }
        public virtual ICollection<Anamnese> Anamneses { get; private set; }
        public virtual ICollection<Atestado> Atestados { get; private set; }
        public virtual ICollection<RequisicaoExames> RequisicaoExames { get; private set; }
        public virtual ICollection<RegistroVacina> RegistroVacinas { get; private set; }
        public virtual ICollection<HistoriaPregressa> HistoriasPregressa { get; private set; }
        public virtual ICollection<Receituario> Receituarios { get; private set; }
        public virtual ICollection<Carteira> Carteiras { get; private set; }
        
        public Paciente()
        {
            Anamneses = new List<Anamnese>();
            Atestados = new List<Atestado>();
            RequisicaoExames = new List<RequisicaoExames>();
            RegistroVacinas = new List<RegistroVacina>();
            Receituarios = new List<Receituario>();
            HistoriasPregressa = new List<HistoriaPregressa>();
            Carteiras = new List<Carteira>();
        }

        public Paciente(Pessoa pessoa,string cartaosus, string situacao)
        {
            SetPessoa(pessoa);
            SetCartaoSus(cartaosus);
            SetSituacao(situacao);
        }

        public void SetPessoa(Pessoa pessoa)
        {
            this.Pessoa = pessoa;
        }

        public void SetFoto(Byte[] foto)
        {
            this.Foto = foto;
        }
        public void SetSituacao(string situacao)
        {
            this.Situacao = situacao;
        }

        public void SetCartaoSus(string cartaosus)
        {
            this.CartaoSus = cartaosus;
        }

        public void AddCarteiras(Carteira carteira)
        {
            if (Carteiras == null)
                Carteiras = new List<Carteira>();


            this.Carteiras.Add(carteira);
        }

        public void SetCarteiras(List<Carteira> carteiras)
        {
            if (Carteiras != null)
                Carteiras = new List<Carteira>();

            Carteiras = carteiras;
        }

        public void SetDadosNascimento(DadosNascimento dados)
        {
            if (dados != null)
                DadosNascimento = dados;
        }

        public string IdadeAtual()
        {
            if (this.Pessoa.DataNascimento != null)
            {
                int anos = DateTime.Now.Year - this.Pessoa.DataNascimento.Value.Year;

                if (DateTime.Now.Month < this.Pessoa.DataNascimento.Value.Month || (DateTime.Now.Month == this.Pessoa.DataNascimento.Value.Month && DateTime.Now.Day < this.Pessoa.DataNascimento.Value.Day))
                    anos--;

                return anos.ToString();
            }
            else
            {
                return null;
            }
        }
    }
}