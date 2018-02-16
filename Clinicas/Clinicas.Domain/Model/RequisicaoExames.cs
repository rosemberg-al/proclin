using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class RequisicaoExames
    {
        public int IdRequisicao { get; private set; }
        public int IdPaciente { get; private set; }
        public string DiagnosticoClinico { get; private set; }
        public string Material { get; private set; }
        public string NaturezaExame { get; private set; }
        public DateTime Data { get; private set; }
        public int IdMedico { get; private set; }
        public string Classe { get; private set; }
        public string Tipo { get; private set; }
        public virtual Paciente Paciente { get; private set; }
        public virtual Medico Medico { get; private set; }

        private RequisicaoExames() { }

        public RequisicaoExames(Paciente paciente, Medico medico, string tipo, string classe, string natureza, string material, string diagnostico)
        {
            SetDiagnosticoClinico(diagnostico);
            SetPaciente(paciente);
            SetMaterial(material);
            SetNaturezaExame(natureza);
            SetData(DateTime.Now);
            SetMedico(medico);
            SetClasse(classe);
            SetTipo(tipo);
        }

        public void SetMedico(Medico medico)
        {
            if (medico != null)
                Medico = medico;
        }

        public void SetTipo(string tipo)
        {
            if (!String.IsNullOrEmpty(tipo))
                Tipo = tipo;
        }

        public void SetClasse(string classe)
        {
            if (!String.IsNullOrEmpty(classe))
                Classe = classe;
        }

        public void SetData(DateTime data)
        {
            if (data != null)
                Data = data;
        }

        public void SetNaturezaExame(string natureza)
        {
            if (!String.IsNullOrEmpty(natureza))
                NaturezaExame = natureza;
        }

        public void SetMaterial(string material)
        {
            if (!String.IsNullOrEmpty(material))
                Material = material;
        }

        public void SetPaciente(Paciente paciente)
        {
            if (paciente != null)
                Paciente = paciente;
        }

        public void SetDiagnosticoClinico(string diagnostico)
        {
            if (!String.IsNullOrEmpty(diagnostico))
                DiagnosticoClinico = diagnostico;
        }
    }
}