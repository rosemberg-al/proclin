using Clinicas.Domain.Model;
using Clinicas.Infrastructure.Base;
using Clinicas.Infrastructure.Context;
using Clinicas.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Validation;

namespace Clinicas.Infrastructure.Repository
{
    public class UsuarioRepository : RepositoryBase<ClinicasContext>, IUsuarioRepository
    {
        public UsuarioRepository(IUnitOfWork<ClinicasContext> unit)
            : base(unit)
        {
        }

        public ICollection<Usuario> ListarUsuarios()
        {
            return Context.Usuarios.ToList();
        }

        public Usuario ObterUsuarioPorLogin(string login)
        {
            return Context.Usuarios.Include(x=>x.Clinica).Include(x=>x.UnidadeAtendimento).Include(x=>x.GrupoUsuario).FirstOrDefault(x => x.Login == login);
        }

        public Usuario ObterUsuario(string login, string senha)
        {
            return Context.Usuarios.Include(x => x.UnidadeAtendimento).FirstOrDefault(x => x.Login == login && x.Senha == senha && x.Situacao == "Ativo");
        }

        public ICollection<Usuario> ListarUsuarios(int idclinica)
        {
            return Context.Usuarios.Include(x => x.GrupoUsuario).Include(x => x.Clinica).Where(x => x.IdClinica == idclinica).ToList();
        }

        public Usuario ObterUsuarioPorId(int idusuario)
        {
            return Context.Usuarios.Include(x => x.GrupoUsuario).Include(x => x.Clinica).Include(x => x.UnidadeAtendimento).First(x => x.IdUsuario == idusuario);
        }

        public Usuario SalvarUsuario(Usuario usuario)
        {
            try
            {
                if (usuario.IdUsuario > 0)
                {
                    Context.Entry(usuario).State = EntityState.Modified;
                }
                else
                {
                    Context.Usuarios.Add(usuario);
                }
                Context.SaveChanges();
                return usuario;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
           
        }

        public void DesativarUsuario(Usuario usuario)
        {
            usuario.Desativar();
            Context.Entry(usuario).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public void AlterarSenha(Usuario usuario, string novasenha)
        {
            usuario.AlterarSenha(novasenha);
            Context.Entry(usuario).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public GrupoUsuario ObterGrupoUsuarioAdministrador(string nome)
        {
            return Context.GrupoUsuario.First(x => x.Nome == nome);
        }
        public GrupoUsuario SalvarGrupoUsuario(GrupoUsuario grupo)
        {
            if (grupo.IdGrupoUsuario > 0)
            {
                Context.Entry(grupo).State = EntityState.Modified;
            }
            else
            {
                Context.GrupoUsuario.Add(grupo);
            }
            Context.SaveChanges();
            return grupo;
        }

        public void ExcluirUsuario(Usuario usuario)
        {
            usuario.Excluir();
            Context.Entry(usuario).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}