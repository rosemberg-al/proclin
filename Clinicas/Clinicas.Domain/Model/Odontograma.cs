using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Odontograma
    {
        public int IdOdontograma { get; private set;  }
        public DateTime DataInicio { get; private set; }
        public DateTime DataTermino { get; private set; }

        public int IdFuncionario { get; private set; }
        public virtual Funcionario Funcionario { get; private set; }

        public int IdPaciente { get; private set; }
        public virtual Paciente Paciente { get; private set; }

        public string Situacao { get; private set; }
        public string Descricao { get; private set; }
        public int Dente { get; private set; }
        public decimal Valor { get; private set; }
        public string Face { get; private set; }

        public Odontograma() { }
        
        public Odontograma(DateTime datainicio,DateTime datatermino, Funcionario funcionario, Paciente paciente, string situacao,string descricao,int dente,decimal valor,string face)
        {
            SetPeriodo(datainicio, datatermino);
            SetFuncionario(funcionario);
            SetSituacao(situacao);
            SetDescricao(descricao);
            SetDente(dente);
            SetPaciente(paciente);
            SetValor(valor);
            SetFace(face);
        }

        public void SetValor(decimal valor)
        {
            this.Valor = valor;
        }

        public void SetFace(string face)
        {
            if (string.IsNullOrEmpty(face))
                throw new Exception(" O Campo Face é Obrigatório ");
            else
                this.Face = face;
        }

        public void SetPaciente(Paciente paciente)
        {
            if (paciente != null)
                this.Paciente = paciente;
            else
                throw new Exception(" O Paciente é Obrigatório ");
        }

        public void SetDente(int dente)
        {
            this.Dente = dente;
        }

        public void SetDescricao(string descricao)
        {
            if (string.IsNullOrEmpty(descricao))
                throw new Exception(" O Campo descrição é Obrigatório ");
            else
                this.Descricao = descricao;
        }

        public void SetSituacao(string situacao)
        {
            if (string.IsNullOrEmpty(situacao))
                throw new Exception("O Campo Situação é Obrigatório ");
            else
                this.Situacao = situacao;
        }

        public void SetFuncionario(Funcionario funcionario)
        {
            if (funcionario != null)
                this.Funcionario = funcionario;
            else
                throw new Exception(" O Profissional de Saúde é Obrigatório ");
        }

        public void SetPeriodo(DateTime datainicio, DateTime datatermino)
        {
            this.DataInicio = datainicio;
            this.DataTermino = datatermino;

            if (datainicio > datatermino)
                throw new Exception(" Periódo inválido ");

            if ((this.DataInicio == null) || (this.DataTermino==null))
              throw new Exception(" Periodo do tratamento não definido ");
        }
    }
}