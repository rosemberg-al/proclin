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
    public class EstoqueTest
    {
        [TestMethod]
        public void EstoqueSaida()
        {

            using (var db = new ClinicasContext())
            {
                var material = db.Material.Find(5);
                material.GerarMovimentoEstoque(10, "Saida", db.Unidades.Find(5));

                db.Entry(material).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        
        
    }
}
