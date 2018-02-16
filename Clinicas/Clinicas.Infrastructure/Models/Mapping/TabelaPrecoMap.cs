using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class TabelaPrecoMap : EntityTypeConfiguration<TabelaPreco>
    {
        public TabelaPrecoMap()
        {
            // Primary Key
            this.HasKey(t => t.IdTabelaPreco);

            this.Property(p => p.Nome).HasColumnName("NmTabelaPreco");

            // Properties
            this.Property(t => t.Tipo)
                .IsRequired();

            // Properties
            this.Property(t => t.Situacao)
                .IsRequired();

            this.HasOptional(t => t.Convenio)
              .WithMany()
              .HasForeignKey(d => d.IdConvenio);

            this.HasRequired(t => t.Clinica)
               .WithMany()
               .HasForeignKey(d => d.IdClinica);

        }
    }
}