using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinicas.Infrastructure.Repository.Interfaces
{
    public interface IUsuarioRepository
    {
        Usuario ObterUsuarioPorId(int idUsuario);
        Usuario ObterUsuarioPorLogin(string idUsuario);
        Usuario ObterUsuario(string login, string senha);
        ICollection<Usuario> ListarUsuarios();
        ICollection<Usuario> ListarUsuarios(int idclinica);
        Usuario SalvarUsuario(Usuario usuario);
        void DesativarUsuario(Usuario usuario);
        void ExcluirUsuario(Usuario usuario);
        void AlterarSenha(Usuario usuario, string novasenha);
        GrupoUsuario ObterGrupoUsuarioAdministrador(string nome);
        GrupoUsuario SalvarGrupoUsuario(GrupoUsuario grupo);
    }
}
