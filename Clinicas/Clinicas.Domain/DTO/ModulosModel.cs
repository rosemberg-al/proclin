using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.DTO
{
    public class ModulosModel
    {
        public int IdModulo { get; set; }
        public string NmModulo { get; set; }
        public string Icon { get; set; }
        public ICollection<FuncionalidadeModel> Funcionalidades { get; set; }

        public ModulosModel()
        {
            this.Funcionalidades = new Collection<FuncionalidadeModel>();
        }
     }

    public class FuncionalidadeModel
    {
        public int IdFuncionalidade { get; set; }
        public string Nome { get; set; }
        public string Rota { get; set; }
    }

}