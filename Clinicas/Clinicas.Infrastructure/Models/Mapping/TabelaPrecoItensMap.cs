using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class TabelaPrecoItensMap : EntityTypeConfiguration<TabelaPrecoItens>
    {
        public TabelaPrecoItensMap()
        {
            // Primary Key
            this.HasKey(t => new { t.IdTabelaPreco, t.IdProcedimento });

            this.Property(p => p.Valor).IsRequired(); ;

            // Properties
            this.Property(t => t.ValorProfissional)
                .IsRequired();

        }
    }
}