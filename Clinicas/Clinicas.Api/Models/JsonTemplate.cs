using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Api.Models
{
    public class JsonTemplate
    {
        public JsonTemplateSubject SUBJECT { get; set; }
        public JsonTemplateBody BODY { get; set; }
    }

    public class JsonTemplateSubject
    {
        public string PROTOCOLO { get; set; }
    }

    public class JsonTemplateBody
    {
        public string PACIENTE { get; set; }
        public string CLINICA { get; set; }
        public string DATA_HORA { get; set; }
        public string TELEFONE { get; set; }
        public string PROTOCOLO { get; set; }
    }

    public class JsonTemplateItemRow
    {
        public JsonTemplateItemRow()
        {
            this.ROW = new List<string>();
        }
        public List<string> ROW { get; set; }
    }
}