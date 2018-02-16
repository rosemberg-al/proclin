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
    public class PacienteTest
    {
        [TestMethod]
        public void DeveCadastrarUmPaciente()
        {
            // var servicePaciente = new PacienteService(new PacienteRepository(new UnitOfWork<ClinicasContext>(new ClinicasContext())));
            using (var db = new ClinicasContext())
            {
                try
                {
                    var pessoa = new Pessoa("Renato", "","M", "PF", null, "", "", "", "", "", "", "", "", "", "", "", "", db.Estados.First(), db.Cidades.First(), "", "", "", "", "", db.Usuarios.First(), "", db.Clinica.First());
                    var paciente = new Paciente(pessoa, "", "Ativo");

                    db.Paciente.Add(paciente);
                    db.SaveChanges();
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
        public void DeveChamarumPaciente()
        {
           
        }











        //[TestMethod]
        //public void Telefone()
        //{
        //    using (var db = new ClinicasContext())
        //    {
        //        try
        //        {
        //            var especialidade = db.Especialidade.Include(x => x.Procedimentos).ToList();
        //            Assert.IsNotNull(especialidade);
        //        }
        //        catch (DbEntityValidationException ex)
        //        {
        //            Retrieve the error messages as a list of strings.
        //           var errorMessages = ex.EntityValidationErrors
        //                   .SelectMany(x => x.ValidationErrors)
        //                   .Select(x => x.ErrorMessage);

        //            Join the list to a single string.
        //            var fullErrorMessage = string.Join("; ", errorMessages);

        //            Combine the original exception message with the new one.
        //           var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        //            Throw a new DbEntityValidationException with the improved exception message.
        //            throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
        //        }
        //    }
        //}

    }
}
