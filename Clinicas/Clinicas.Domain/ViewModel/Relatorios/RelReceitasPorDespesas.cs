using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.ViewModel.Relatorios
{
    public class RelReceitasPorDespesas
    {
        public List<DadosReceita> Receitas { get; set; }
        public List<DadosReceita> Despesas { get; set; }
    }

    public class DadosReceita
    {
        public decimal Valor { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
    }

}