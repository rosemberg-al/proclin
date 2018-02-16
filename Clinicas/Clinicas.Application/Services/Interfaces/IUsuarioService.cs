using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Application.Services.Interfaces
{
    public interface IUsuarioService
    {
        Usuario ObterUsuarioPorId(int idUsuario);
        Usuario ObterUsuarioLogin(string login);
        Usuario ObterUsuario(string login, string senha);
        ICollection<Usuario> ListarUsuarios(int idclinica);
        Usuario SalvarUsuario(Usuario usuario);
        void DesativarUsuario(Usuario usuario);
        void ExcluirUsuario(Usuario usuario);
        void AlterarSenha(Usuario usuario, string novasenha);
        GrupoUsuario ObterGrupoUsuarioAdministrador(string nome);
        GrupoUsuario SalvarGrupoUsuario(GrupoUsuario grupo);
    }
}