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

namespace Clinicas.Test
{
    [TestClass]
    public class DashboardTest
    {
        [TestMethod]
        public void QtdePacientesCadastrados()
        {
            var _serviceDashboard = new DashboardService(new DashboardRepository(new UnitOfWork<ClinicasContext>(new ClinicasContext())));
            using (var db = new ClinicasContext())
            {
                try
                {
                    int qtd = _serviceDashboard.QtdePacientes(1,1);

                    Assert.IsNotNull(qtd);
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
        
        //[TestMethod]
        //public void QtdeAtendimentosParticular()
        //{
        //    var _serviceDashboard = new DashboardService(new DashboardRepository(new UnitOfWork<ClinicasContext>(new ClinicasContext())));
        //    using (var db = new ClinicasContext())
        //    {
        //        try
        //        {
        //            int qtd = _serviceDashboard.QtdeAtendimentosParticular();

        //            Assert.IsNotNull(qtd);
        //        }
        //        catch (DbEntityValidationException ex)
        //        {
        //            // Retrieve the error messages as a list of strings.
        //            var errorMessages = ex.EntityValidationErrors
        //                    .SelectMany(x => x.ValidationErrors)
        //                    .Select(x => x.ErrorMessage);

        //            // Join the list to a single string.
        //            var fullErrorMessage = string.Join("; ", errorMessages);

        //            // Combine the original exception message with the new one.
        //            var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        //            // Throw a new DbEntityValidationException with the improved exception message.
        //            throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
        //        }
        //    }
        //}
    }
}
