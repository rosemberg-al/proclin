using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class TipoMaterialMap : EntityTypeConfiguration<TipoMaterial>
    {
        public TipoMaterialMap()
        {
            // Primary Key
            this.HasKey(t => t.IdTipoMaterial);
            this.Property(t => t.Nome);
            this.Property(t => t.Situacao);

            this.HasRequired(t => t.UnidadeAtendimento)
            .WithMany()
            .HasForeignKey(d => d.IdUnidadeAtendimento);
        }
    }
}