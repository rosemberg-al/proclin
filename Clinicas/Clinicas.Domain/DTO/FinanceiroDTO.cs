using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinicas.Domain.DTO
{
    public class FinanceiroDTO
    {
        public decimal TotalaPagar { get; set; }
        public decimal TotalaReceber { get; set; }
        public decimal TotalContasRecebidas { get; set; }
        public decimal TotalaContasPagas { get; set; }
    }
}