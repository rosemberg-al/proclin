using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class CidadesMap : EntityTypeConfiguration<Cidade>
    {
        public CidadesMap()
        {
            // Primary Key
            this.HasKey(t => t.IdCidade);

            // Properties
            this.Property(t => t.Nome)
                .HasMaxLength(100).IsRequired();

            this.Property(t => t.Ibge).IsOptional();
        }
    }
}