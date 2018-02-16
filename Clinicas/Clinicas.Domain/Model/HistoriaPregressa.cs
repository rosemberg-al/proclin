using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class HistoriaPregressa
    {
        public int IdHistoriaPregressa { get; private set; }
        public int IdPaciente { get; private set; }
        public string Descricao { get; private set; }
        public string Situacao { get; private set; }
        public DateTime Data { get; private set; }
        public virtual Paciente Paciente { get; private set; }

        public int IdFuncionario { get; private set; }
        public virtual Funcionario ProfissionalSaude { get; private set; }

        private HistoriaPregressa() { }

        public HistoriaPregressa(string descricao, Paciente paciente,Funcionario profissionalSaude)
        {
            SetDescricao(descricao);
            SetSituacao("Ativo");
            SetData(DateTime.Now);
            SetPaciente(paciente);
            SetProfissionalSaude(profissionalSaude);
        }

        public void SetPaciente(Paciente paciente)
        {
            if (paciente != null)
                Paciente = paciente;
            else
                throw new Exception("Nenhum paciente encontrado");
        }

        public void SetProfissionalSaude(Funcionario profissionalSaude)
        {
            if (profissionalSaude != null)
                this.ProfissionalSaude = profissionalSaude;
            else
                throw new Exception("Nenhum profissional de saúde encontrado");
        }

        public void SetData(DateTime data)
        {
            if (data != null)
                Data = data;
        }

        public void SetSituacao(string situacao)
        {
            if (!String.IsNullOrEmpty(situacao))
                Situacao = situacao;
        }

        public void SetDescricao(string descricao)
        {
            if (!String.IsNullOrEmpty(descricao))
                Descricao = descricao;
        }
    }
}