using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Anamnese
    {
        public int IdAnamnese { get; private set; }
        public DateTime Data { get; private set; }
       
        public string Hma { get; private set; }
        public string Diagnostico { get; private set; }
        public string CondutaMedica { get; private set; }
        public string Situacao { get; private set; }

        public int IdPaciente { get; private set; }
        public virtual Paciente Paciente { get; private set; }

        public int IdFuncionario { get; private set; }
        public Funcionario ProfissionalSaude { get; private set; }
        
        private Anamnese()
        {

        }

        public Anamnese(string hma, string diagnostico, string conduta, Paciente paciente,Funcionario profissionalSaude)
        {
            SetData(DateTime.Now);
            SetSituacao("Ativo");
            SetHma(hma);
            SetDiagnostico(diagnostico);
            SetCondutaMedica(conduta);
            SetPaciente(paciente);
            SetProfissionalSaude(profissionalSaude);
        }

        public void SetCondutaMedica(string conduta)
        {
            if (!String.IsNullOrEmpty(conduta))
                CondutaMedica = conduta;
        }

        public void SetDiagnostico(string diagnostico)
        {
            if (!String.IsNullOrEmpty(diagnostico))
                Diagnostico = diagnostico;
        }

        public void SetHma(string hma)
        {
            if (!String.IsNullOrEmpty(hma))
                Hma = hma;
        }

        public void SetSituacao(string situacao)
        {
            if (!String.IsNullOrEmpty(situacao))
                Situacao = situacao;
        }

        public void SetData(DateTime data)
        {
            if (data != null)
                Data = data;
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
                throw new Exception("Nenhum Profissional de Saúde encontrado");
        }
    }
}