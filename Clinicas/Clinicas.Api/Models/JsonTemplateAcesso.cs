using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Api.Models
{
    public class JsonTemplateAcesso
    {
        public JsonTemplateSubjectAcesso SUBJECT { get; set; }
        public JsonTemplateBodyAcesso BODY { get; set; }
    }

    public class JsonTemplateSubjectAcesso
    {
        public string DATA_HORA { get; set; }
    }

    public class JsonTemplateBodyAcesso
    {
        public string NOME { get; set; }
        public string LOGIN { get; set; }
        public string SENHA { get; set; }
    }
}