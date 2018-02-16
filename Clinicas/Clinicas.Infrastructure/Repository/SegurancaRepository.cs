using Clinicas.Domain.Model;
using Clinicas.Infrastructure.Base;
using Clinicas.Infrastructure.Context;
using Clinicas.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Clinicas.Domain.DTO;

namespace Clinicas.Infrastructure.Repository
{
    public class SegurancaRepository : RepositoryBase<ClinicasContext>, ISegurancaRepository
    {
        public SegurancaRepository(IUnitOfWork<ClinicasContext> unit)
            : base(unit)
        {
        }
        public ICollection<Funcionalidade> ListarFuncionalidades()
        {
            return Context.Funcionalidade.ToList();
        }

        public ICollection<Funcionalidade> ListarFuncionalidadesPorModulo(int idModulo)
        {
            return Context.Funcionalidade.Where(x => x.IdModulo == idModulo).ToList();
        }

        public ICollection<Modulo> ListarModulos()
        {
            return Context.Moduloes.ToList();
        }

        public Funcionalidade ObterFuncionalidadePorId(int idFuncionalidade)
        {
            return Context.Funcionalidade.FirstOrDefault(x => x.IdFuncionalidade == idFuncionalidade);
        }

        public Modulo ObterModuloPorId(int idModulo)
        {
            return Context.Moduloes.FirstOrDefault(x => x.IdModulo == idModulo);
        }

        public ICollection<ModulosModel> ObterFuncionalidadesPorGrupoUsuario(int idgrupoUsuario)
        {
            // erro entity não funciona linq 
            // db.GrupoUsuario.Include(x=>x.Funcionalidades).Include(a=>a.Funcionalidades.Select(k=>k.GrupoUsuarios)).ToList();
            var modulos = Context.Database.SqlQuery<ModulosModel>(" select DISTINCT IdModulo,	NmModulo,Icon from vw_menu_grupousuario where IdGrupoUsuario = '"+ idgrupoUsuario + "' ").ToList();
            var lista = new List<ModulosModel>();

            foreach (var item in modulos)
            {
                var func = Context.Database.SqlQuery<FuncionalidadeModel>(" select * from vw_menu_grupousuario where IdModulo = '" + item.IdModulo + "'   ").ToList();
                lista.Add(new ModulosModel()
                {
                    IdModulo = item.IdModulo,
                    NmModulo = item.NmModulo,
                    Icon = item.Icon,
                    Funcionalidades = func
                });
            }
            return lista;
        }

        public GrupoUsuario ObterGrupoUsuarioPorId(int idgrupousuario)
        {
            return Context.GrupoUsuario.Find(idgrupousuario);
        }

        public ICollection<GrupoUsuario> ListarGrupoUsuario()
        {
            return Context.GrupoUsuario.ToList();
        }
    }
}