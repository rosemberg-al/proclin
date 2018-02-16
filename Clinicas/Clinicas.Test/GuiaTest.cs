using Clinicas.Domain.Tiss;
using Clinicas.Infrastructure.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinicas.Test
{
    [TestClass]
    public class GuiaTest
    {
        #region [ GUIA CONSULTA ]

        [TestMethod]
        public void DeveGerarumaGuiaConsulta()
        {

            var db = new ClinicasContext();

            var guia = new Guia();

            var dadosBeneficiario = new DadosBeneficiario("123456", "Plano B", DateTime.Now.AddMonths(12), "Renato Ayres de Oliveira", "123456");

            var cabecalhoGuia = new CabecalhoGuia("1234567", "0000001", DateTime.Now);
            var dadosContratado = new DadosContratado("PF", "12345656", "CID", "1234567", "RUA", "CAMPINAS 453", "123456", "", "BELO HORIZONTE", "MG", "66666", "30520540", "", "CRM", "123456777", "SP", "66666");
            var hipoteseDiagnostica = new HipoteseDiagnostica("A", "15 01 35", "", "", "", "", "");
            var dadosAtendimento = new DadosAtendimento(DateTime.Now,"123456","101012","1","1","Teste Observação",null,null);

            guia.GerarGuiaConsulta(cabecalhoGuia, dadosBeneficiario, dadosContratado, hipoteseDiagnostica, dadosAtendimento,db.Clinica.First());

            db.Guia.Add(guia);
            db.SaveChanges();
        }

        #endregion
    }
}
