using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.Model
{
    public class Funcionalidade
    {
        public int IdFuncionalidade { get; set; }
        public string Nome { get; set; }
        public string Controller { get; set; }

        public int IdModulo { get; set; }
        public virtual Modulo Modulo { get; set; }
        public virtual ICollection<GrupoUsuario> GrupoUsuarios { get; set; }

        public Funcionalidade()
        {
            this.GrupoUsuarios = new Collection<GrupoUsuario>();
        }
    }
}