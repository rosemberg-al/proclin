using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class RegistroVacina
    {
        public int IdRegistroVacina { get; private set; }
        public int IdPaciente { get; private set; }
        public int IdVacina { get; private set; }
        public DateTime Data { get; private set; }
        public DateTime Hora { get; private set; }
        public string Lote { get; private set; }
        public string Dose { get; private set; }
        public virtual Paciente Paciente { get; private set; }
        public virtual Vacina Vacina { get; private set; }

        private RegistroVacina() { }

        public RegistroVacina(Paciente paciente, Vacina vacina, DateTime data, DateTime hora, string dose, string lote)
        {
            SetVacina(vacina);
            SetPaciente(paciente);
            SetDose(dose);
            SetLote(lote);
            SetHora(hora);
            SetData(data);
        }

        public void SetData(DateTime data)
        {
            if (data != null)
                Data = data;
        }

        public void SetHora(DateTime hora)
        {
            if (hora != null)
                Hora = hora;
        }

        public void SetLote(string lote)
        {
            if (!String.IsNullOrEmpty(lote))
                Lote = lote;
        }

        public void SetDose(string dose)
        {
            if (!String.IsNullOrEmpty(dose))
                Dose = dose;
        }

        public void SetPaciente(Paciente paciente)
        {
            if (paciente != null)
                Paciente = paciente;
        }

        public void SetVacina(Vacina vacina)
        {
            if (vacina != null)
                Vacina = vacina;
        }
    }
}