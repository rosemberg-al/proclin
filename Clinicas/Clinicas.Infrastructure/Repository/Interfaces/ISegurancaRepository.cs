using Clinicas.Domain.DTO;
using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinicas.Infrastructure.Repository.Interfaces
{
    public interface ISegurancaRepository
    {
        Funcionalidade ObterFuncionalidadePorId(int idFuncionalidade);
        ICollection<Funcionalidade> ListarFuncionalidades();
        ICollection<Funcionalidade> ListarFuncionalidadesPorModulo(int idModulo);
        Modulo ObterModuloPorId(int idModulo);
        ICollection<Modulo> ListarModulos();
        ICollection<ModulosModel> ObterFuncionalidadesPorGrupoUsuario(int idgrupoUsuario);
        GrupoUsuario ObterGrupoUsuarioPorId(int idgrupousuario);
        ICollection<GrupoUsuario> ListarGrupoUsuario();

    }
}
