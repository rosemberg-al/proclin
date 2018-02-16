using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Clinicas.Infrastructure.Context;
using Clinicas.Domain.DTO;
using System.Collections.Generic;
using System.Linq;
using Clinicas.Application.Services;
using Clinicas.Infrastructure.Repository;
using Clinicas.Infrastructure.Base;
using Clinicas.Domain.Model;
using System.Data.Entity.Validation;
using System.Data.Entity;

namespace Clinicas.Test
{
    [TestClass]
    public class UsuarioTest
    {
        [TestMethod]
        public void Login()
        {
            var _service = new UsuarioService(new UsuarioRepository(new UnitOfWork<ClinicasContext>(new ClinicasContext())));
            using (var db = new ClinicasContext())
            {
                try
                {
                    var usuario = _service.ObterUsuario("renatouai@gmail.com", "150709");
                    Assert.IsNull(usuario);
                }
                catch (DbEntityValidationException ex)
                {
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
            }
        }


        [TestMethod]
        public void RetornaFuncionalidadesUsuario()
        {
            //  var _service = new UsuarioService(new UsuarioRepository(new UnitOfWork<ClinicasContext>(new ClinicasContext())));
            using (var db = new ClinicasContext())
            {
                try
                {
                    var modulos = db.Database.SqlQuery<ModulosModel>(" select * from vw_menu_grupousuario where IdGrupoUsuario = '1' ").ToList();

                    var lista = new List<ModulosModel>();

                    foreach(var item in modulos)
                    {
                        var func = db.Database.SqlQuery<FuncionalidadeModel>(" select * from vw_menu_grupousuario where IdModulo = '"+item.IdModulo+"'   ").ToList();

                        lista.Add(new ModulosModel()
                        {
                            IdModulo = item.IdModulo,
                            NmModulo = item.NmModulo,
                            Icon = item.Icon,
                            Funcionalidades = func
                        });
                    }


                    Assert.IsNotNull(lista);
                }
                catch (DbEntityValidationException ex)
                {
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
            }
        }



      
    }
}
