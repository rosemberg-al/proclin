using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class GrupoUsuarioMap : EntityTypeConfiguration<GrupoUsuario>
    {
        public GrupoUsuarioMap()
        {
            // Primary Key
            this.HasKey(t => t.IdGrupoUsuario);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(60);

            this.HasMany(s => s.Funcionalidades)
             .WithMany(c => c.GrupoUsuarios)
             .Map(cs =>
             {
                 cs.MapLeftKey("IdGrupoUsuario");
                 cs.MapRightKey("IdFuncionalidade");
                 cs.ToTable("GrupoFuncionalidade");
             });


        }
    }
}