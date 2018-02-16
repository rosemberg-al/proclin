using Clinicas.Domain.Tiss;
using Clinicas.Infrastructure.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Mail;
using System.Web.Hosting;

namespace Clinicas.Test
{
    [TestClass]
    public class EsqueceuSenhaTest
    {
        #region [ Email Esqueceu Senha ]

        [TestMethod]
        public void EnviarEmailEsqueceuSenha()
        {
            string body = string.Empty;
            //using streamreader for reading my htmltemplate   

            using (StreamReader reader = new StreamReader("C:\\TemplateEmail\\Agendamento.html"))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{NomeUsuario}", "Renato Ayres de Oliveira"); //replacing the required things  
            body = body.Replace("{Usuario}", "renatouai@gmail.com");
            body = body.Replace("{Senha}", "x150709");
            

            using (MailMessage mailMessage = new MailMessage())

            {

                mailMessage.From = new MailAddress("renato@genialsoft.com.br");

                mailMessage.Subject = "Recuperação de Senha";

                mailMessage.Body = body;

                mailMessage.IsBodyHtml = true;

                mailMessage.To.Add("renatouai@gmail.com");

                SmtpClient smtp = new SmtpClient();

                smtp.Host = "smtp.genialsoft.com.br"; //ConfigurationManager.AppSettings["Host"];

                smtp.EnableSsl = false; // Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);

                System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();

                NetworkCred.UserName = "renato@genialsoft.com.br"; // ConfigurationManager.AppSettings["UserName"]; //reading from web.config  

                NetworkCred.Password = "r15008956"; // ConfigurationManager.AppSettings["Password"]; //reading from web.config  

                smtp.UseDefaultCredentials = true;

                smtp.Credentials = NetworkCred;

                smtp.Port = 587; // int.Parse(ConfigurationManager.AppSettings["Port"]); //reading from web.config  

                smtp.Send(mailMessage);

            }
        }

        #endregion
    }
}
