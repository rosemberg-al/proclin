using Clinicas.Domain.DTO;
using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Application.Services.Interfaces
{
    public interface ISegurancaService
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