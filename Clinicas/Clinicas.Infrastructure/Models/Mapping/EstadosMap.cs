using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class EstadosMap : EntityTypeConfiguration<Estado>
    {
        public EstadosMap()
        {
            // Primary Key
            this.HasKey(t => t.IdEstado);

            // Properties
            this.Property(t => t.Nome)
                .HasMaxLength(60);

            // Table & Column Mappings
            this.Property(t => t.IdEstado).HasColumnName("IdEstado");
            this.Property(t => t.Nome).HasColumnName("Nome");
        }
    }
}