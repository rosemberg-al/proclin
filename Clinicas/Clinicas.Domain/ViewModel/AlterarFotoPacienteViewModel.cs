using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class AlterarFotoPacienteViewModel
    {
        public Byte[] Foto { get; set; }
        public int IdPaciente { get; set; }
    }
}