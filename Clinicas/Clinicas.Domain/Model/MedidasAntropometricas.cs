using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class MedidasAntropometricas
    {
        public int IdMedida { get; private set; }
       
        public Decimal Peso { get; private set; }
        public Decimal Altura { get; private set; }
        public Decimal PerimetroCefalico { get; private set; }
        public DateTime Data { get; private set; }
        public Decimal Imc { get; private set; }

        public int IdPaciente { get; private set; }
        public virtual Paciente Paciente { get; private set; }

        public int IdFuncionario { get; private set; }
        public virtual Funcionario ProfissionalSaude { get; private set; }

        private MedidasAntropometricas() { }

        public MedidasAntropometricas(decimal peso, DateTime data, decimal altura, decimal perimetroCefalico, decimal imc, Paciente paciente, Funcionario profissionalSaude)
        {
            SetPeso(peso);
            SetAltura(altura);
            SetPerimetroCefalico(perimetroCefalico);
            SetImc(imc);
            SetData(data);
            SetPaciente(paciente);
            SetProfissionalSaude(profissionalSaude);

        }

        public void SetProfissionalSaude(Funcionario profissionalSaude)
        {
            if (profissionalSaude != null)
                ProfissionalSaude = profissionalSaude;
            else
                throw new Exception("Nenhum profissional de saúde encontrado");
        }

            public void SetPaciente(Paciente paciente)
        {
            if (paciente != null)
                Paciente = paciente;
            else
                throw new Exception("Nenhum paciente encontrado");
        }

        public void SetData(DateTime data)
        {
            if (data != null)
                Data = data;
        }

        public void SetImc(decimal imc)
        {
            if (imc > 0)
                Imc = imc;
        }

        public void SetPerimetroCefalico(decimal perimetroCefalico)
        {
            if (perimetroCefalico > 0)
                PerimetroCefalico = perimetroCefalico;
        }

        public void SetAltura(decimal altura)
        {
            if (altura > 0)
                Altura = altura;
        }

        public void SetPeso(decimal peso)
        {
            if (peso > 0)
                Peso = peso;
        }
    }
}