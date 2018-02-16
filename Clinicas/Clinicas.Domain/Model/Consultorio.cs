using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Consultorio
    {
        public int IdConsultorio { get; private set; }
        public string NmConsultorio { get; private set; }

        public int IdClinica { get; private set; }
        public virtual Clinica Clinica { get; private set; }

        public int IdUnidadeAtendimento { get; private set; }
        public virtual UnidadeAtendimento UnidadeAtendimento { get; private set; }


        public Consultorio() { }

        public Consultorio(string nmConsultorio, Clinica clinica, UnidadeAtendimento unidadeAtendimento)
        {
            SetNmConsultorio(nmConsultorio);
            SetUnidadeAtendimento(unidadeAtendimento);
            SetClinica(clinica);
        }

        public void SetClinica(Clinica clinica)
        {
            if (clinica != null)
                this.Clinica = clinica;
            else
                throw new Exception(" O Campo clínica é Obrigatório ");
        }

        public void SetUnidadeAtendimento(UnidadeAtendimento unidadeAtendimento)
        {
            if (unidadeAtendimento != null)
                this.UnidadeAtendimento = unidadeAtendimento;
            else
                throw new Exception(" O Campo Unidade de Atendimento é Obrigatório ");
        }

        public void SetNmConsultorio(string nmConsultorio)
        {
            if (!string.IsNullOrEmpty(nmConsultorio))
                this.NmConsultorio = nmConsultorio;
            else
                throw new Exception(" O Campo Nome do Consultório é Obrigatório ");
        }
    }

}