using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class TransferenciaContaMap : EntityTypeConfiguration<TransferenciaConta>
    {
        public TransferenciaContaMap()
        {
            this.HasKey(t => t.IdTransferenciaConta);

            // Properties
            this.Property(t => t.Descricao);

            this.HasRequired(t => t.ContaOrigem)
               .WithMany()
               .HasForeignKey(d => d.IdContaOrigem);

            this.HasRequired(t => t.ContaDestino)
                .WithMany()
                .HasForeignKey(d => d.IdContaDestino);
        }
    }
}