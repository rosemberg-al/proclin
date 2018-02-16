using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class LoteMap : EntityTypeConfiguration<Lote>
    {
        public LoteMap()
        {
            // Primary Key
            this.HasKey(t => t.IdLote);
            this.Property(t => t.Situacao);
            this.Property(t => t.Data);

            this.HasRequired(t => t.Convenio)
           .WithMany()
           .HasForeignKey(d => d.IdConvenio);

            this.HasRequired(t => t.Clinica)
           .WithMany()
           .HasForeignKey(d => d.IdClinica);

        }
    }
}