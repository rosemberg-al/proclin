using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class ModeloProntuarioMap : EntityTypeConfiguration<ModeloProntuario>
    {
        public ModeloProntuarioMap()
        {

            // Primary Key
            this.HasKey(t => t.IdModeloProntuario);

            // Properties
            this.Property(t => t.Descricao)
                .IsRequired()
                .HasMaxLength(4000);

            this.Property(t => t.NomeModelo)
               .IsRequired()
               .HasMaxLength(100);

            // Properties
            this.Property(t => t.Tipo)
                .IsRequired();
            // Properties
            this.Property(t => t.Situacao)
                .IsRequired();
        }
    }
}