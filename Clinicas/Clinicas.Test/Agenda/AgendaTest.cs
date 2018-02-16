using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Clinicas.Infrastructure.Context;
using Clinicas.Domain.DTO;
using System.Collections.Generic;
using System.Linq;
using Clinicas.Domain.Model;
using System.Data.Entity.Validation;
using System.Data;

namespace Clinicas.Test
{
    [TestClass]
    public class AgendaTest
    {
        [TestMethod]
        public void LiberarAgenda()
        {
            var db = new ClinicasContext();

            var agenda = new Agenda();

            agenda.Aguardando(DateTime.Now, new TimeSpan(08,00,00), db.Usuarios.Find(1),db.Funcionario.First(x=>x.Tipo== "Profissional de Saúde"),db.Clinica.First(), "teste");

            db.Agenda.Add(agenda);
            db.SaveChanges();

            /* var guia = new AgendaDTO();

            var especialidade = "";
            var profissional = "";

            DateTime data = DateTime.Now;
            DateTime TempoInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 08, 00, 00);
            DateTime TempoTermino = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 18, 00, 00);

            DateTime IntervaloInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 00, 00);
            DateTime IntervaloTermino = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 00, 00);

            double intervaloMinutos = 30;

            List<DateTime> arrHorario = new List<DateTime>();
            while (TempoInicio <= TempoTermino)
            {
                arrHorario.Add(TempoInicio);
                TempoInicio = TempoInicio.AddHours(0).AddMinutes(intervaloMinutos).AddSeconds(0);
            }

            List<DateTime> arrIntervalo = new List<DateTime>();
            while (IntervaloInicio < IntervaloTermino)
            {
                arrIntervalo.Add(IntervaloInicio);
                IntervaloInicio = IntervaloInicio.AddHours(0).AddMinutes(intervaloMinutos).AddSeconds(0);
            }
            var list3 = arrHorario.Where(x => !arrIntervalo.Contains(x)).ToList();

            db.SaveChanges(); */
        }

        //[TestMethod]
        //public void SalvarAguardando()
        //{
        //    try
        //    {
        //        var db = new ClinicasContext();
        //        var agenda = new Agenda();
        //        var itens = new List<AgendaProcedimento>();
        //        var funcionario = db.Funcionario.Find(553);
        //        itens.Add(new AgendaProcedimento(funcionario));
        //        agenda.Aguardando(DateTime.Now, new TimeSpan(08, 00, 00), db.Usuarios.Find(1), "observacao",  itens);

        //        db.Agenda.Add(agenda);
        //        db.SaveChanges();
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        // Retrieve the error messages as a list of strings.
        //        var errorMessages = ex.EntityValidationErrors
        //                .SelectMany(x => x.ValidationErrors)
        //                .Select(x => x.ErrorMessage);

        //        // Join the list to a single string.
        //        var fullErrorMessage = string.Join("; ", errorMessages);

        //        // Combine the original exception message with the new one.
        //        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

        //        // Throw a new DbEntityValidationException with the improved exception message.
        //        throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
        //    }
        //}

        [TestMethod]
        public void SalvarMarcado()
        {
            try
            {
                var db = new ClinicasContext();
                var agenda = db.Agenda.ToList();
                foreach(var item in agenda)
                {
                    item.Marcado(db.Usuarios.First(), db.Paciente.First(), db.Funcionario.First(), db.Especialidade.First(), db.Procedimento.First(), db.Convenio.First(), "C", "observação", "blá blá ", 0, 0, db.TipoAtendimento.First());
                }
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

        [TestMethod]
        public void SalvarCancealdo()
        {
            try
            {
                var db = new ClinicasContext();
                var agenda = db.Agenda.Include("Itens").First(x => x.IdAgenda == 4);
                agenda.CancelarAtendimento(db.Usuarios.Find(1));
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

        [TestMethod]
        public void SalvarRealizado()
        {
            try
            {
                var db = new ClinicasContext();
                var agenda = db.Agenda.Include("Itens").First(x => x.IdAgenda == 4);
                agenda.ConfirmarAtendimento(db.Usuarios.Find(1));
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
}
