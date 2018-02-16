using Clinicas.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Clinicas.Domain.Model;
using Clinicas.Infrastructure.Repository.Interfaces;

namespace Clinicas.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioService(IUsuarioRepository rp)
        {
            _usuarioRepository = rp;
        }

        public ICollection<Usuario> ListarUsuarios(int idclinica)
        {
            return _usuarioRepository.ListarUsuarios(idclinica);
        }

        public Usuario ObterUsuarioLogin(string login)
        {
            return _usuarioRepository.ObterUsuarioPorLogin(login);
        }

        public Usuario ObterUsuario(string login, string senha)
        {
            return _usuarioRepository.ObterUsuario(login, senha);
        }

        public void AlterarSenha(Usuario usuario, string novasenha)
        {
            _usuarioRepository.AlterarSenha(usuario, novasenha);
        }
        public GrupoUsuario ObterGrupoUsuarioAdministrador(string nome) {
            return _usuarioRepository.ObterGrupoUsuarioAdministrador(nome);
        }
        public GrupoUsuario SalvarGrupoUsuario(GrupoUsuario grupo)
        {
            return _usuarioRepository.SalvarGrupoUsuario(grupo);
        }

        public void DesativarUsuario(Usuario usuario)
        {
            _usuarioRepository.DesativarUsuario(usuario);
        }

        public void ExcluirUsuario(Usuario usuario)
        {
            _usuarioRepository.ExcluirUsuario(usuario);
        }

        public Usuario ObterUsuarioPorId(int idusuario)
        {
            return _usuarioRepository.ObterUsuarioPorId(idusuario);
        }

        public Usuario SalvarUsuario(Usuario usuario)
        {
            return _usuarioRepository.SalvarUsuario(usuario);
        }
    }
}