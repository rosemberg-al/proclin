using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel
{
    public class GuiasLoteViewModel
    {
        public int IdLote { get; set; }
        public List<int> Guias { get; set; }

        public GuiasLoteViewModel()
        {
            Guias = new List<int>();
        }
    }
}