using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class FuncionalidadeMap : EntityTypeConfiguration<Funcionalidade>
    {
        public FuncionalidadeMap()
        {
            // Primary Key
            this.HasKey(t => t.IdFuncionalidade);

            // Properties
            this.Property(t => t.Nome)
                .HasMaxLength(60);

            this.Property(t => t.Controller)
             .HasMaxLength(60);

            this.HasRequired(t => t.Modulo)
             .WithMany(t => t.Funcionalidades)
             .HasForeignKey(d => d.IdModulo);
        }
    }
}