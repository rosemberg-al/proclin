using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Api.Models
{
    public class JsonTemplateSenha
    {
        public JsonTemplateSubjectSenha SUBJECT { get; set; }
        public JsonTemplateBodySenha BODY { get; set; }
    }

    public class JsonTemplateSubjectSenha
    {
        public string PROTOCOLO { get; set; }
    }

    public class JsonTemplateBodySenha
    {
        public string USUARIO { get; set; }
        public string NOVA_SENHA { get; set; }
    }
   
}