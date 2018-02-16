using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class ModuloMap : EntityTypeConfiguration<Modulo>
    {
        public ModuloMap()
        {
            // Primary Key
            this.HasKey(t => t.IdModulo);

            // Properties
            this.Property(t => t.NmModulo)
                .HasMaxLength(60);

            this.Property(t => t.Icon)
               .HasMaxLength(60);


            /* this.HasMany(x => x.Funcionalidades)
               .WithMany(x => x.)
               .Map(map =>
               {
                   map.MapLeftKey("IdModulo");
                   map.MapRightKey("IdFuncionalidade");
                   map.ToTable("ModuloFuncionalidade");
               }); */
        }
    }
}