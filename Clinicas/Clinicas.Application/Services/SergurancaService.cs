using Clinicas.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clinicas.Domain.Model;
using Clinicas.Infrastructure.Repository.Interfaces;
using Clinicas.Domain.DTO;

namespace Clinicas.Application.Services
{
    public class SegurancaService : ISegurancaService
    {
        private readonly ISegurancaRepository _repository;

        public SegurancaService(ISegurancaRepository rp)
        {
            _repository = rp;
        }
        
        public ICollection<Funcionalidade> ListarFuncionalidades()
        {
            return _repository.ListarFuncionalidades();
        }

        public ICollection<Funcionalidade> ListarFuncionalidadesPorModulo(int idModulo)
        {
            return _repository.ListarFuncionalidadesPorModulo(idModulo);
        }

        public ICollection<Modulo> ListarModulos()
        {
            return _repository.ListarModulos();
        }

        public Funcionalidade ObterFuncionalidadePorId(int idFuncionalidade)
        {
            return _repository.ObterFuncionalidadePorId(idFuncionalidade);
        }

        public ICollection<ModulosModel> ObterFuncionalidadesPorGrupoUsuario(int idgrupoUsuario)
        {
            return _repository.ObterFuncionalidadesPorGrupoUsuario(idgrupoUsuario);
        }
        
        public Modulo ObterModuloPorId(int idModulo)
        {
            return _repository.ObterModuloPorId(idModulo);
        }

        public GrupoUsuario ObterGrupoUsuarioPorId(int idgrupousuario)
        {
            return _repository.ObterGrupoUsuarioPorId(idgrupousuario);
        }

        public ICollection<GrupoUsuario> ListarGrupoUsuario()
        {
            return _repository.ListarGrupoUsuario();
        }
    }
}