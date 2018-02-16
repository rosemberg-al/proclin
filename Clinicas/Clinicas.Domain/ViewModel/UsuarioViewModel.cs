using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class UsuarioViewModel
    {
        public int IdUsuario { get; set; }
        public string Nome { get; set; }
        public int IdClinica { get; set; }
        public int IdFuncionario { get; set; }
        public int IdUnidadeAtendimento { get; set; }
        public string Senha { get; set; }
        public string NmClinica { get; set; }
        public string NmUnidadeAtendimento { get; set; }
        public int IdGrupoUsuario { get; set; }
        public string NmGrupoUsuario { get; set; }
        public string Login { get; set; }
        public string Situacao { get; set; }
        public string Email { get; set; }
        public bool PrimeiroAcesso { get; set; }
        public string Tipo { get; set; }
    }
}