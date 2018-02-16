using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class BloqueioAgenda
    {
        public int IdBloqueio { get; private set; }
        public int IdFuncionario { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime DataFim { get; private set; }
        public string Motivo { get; private set; }
        public int IdClinica { get; private set; }
        public virtual Funcionario Funcionario { get; private set; }
        public virtual Clinica Clinica { get; private set; }

        private BloqueioAgenda() { }
        public BloqueioAgenda(Funcionario funcionario, Clinica clinica, DateTime dataInicio, DateTime dataFim, string motivo)
        {
            SetFuncionario(funcionario);
            SetMotivo(motivo);
            SetDataInicio(dataInicio);
            SetDataFim(dataFim);
            SetClinica(clinica);
        }

        public void SetClinica(Clinica clinica)
        {
            if (clinica == null)
                throw new Exception("O campo clinica é obrigatório!");

            Clinica = clinica;
        }

        public void SetDataFim(DateTime dataFim)
        {
            if (dataFim == DateTime.MinValue)
                throw new Exception("O campo data fim é obrigatório!");

            DataFim = dataFim;
        }

        public void SetDataInicio(DateTime dataInicio)
        {
            if(dataInicio == DateTime.MinValue)
                throw new Exception("O campo data inicio é obrigatório!");

            DataInicio = dataInicio;
        }

        public void SetMotivo(string motivo)
        {
            if (string.IsNullOrEmpty(motivo))
                throw new Exception("O campo motivo é obrigatório!");

            Motivo = motivo;

        }

        public void SetFuncionario(Funcionario funcionario)
        {
            if (funcionario == null)
                throw new Exception("O campo funcionario é obrigatório!");

            Funcionario = funcionario;

        }
    }
}