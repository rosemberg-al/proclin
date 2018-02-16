using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class ProcedimentoMap : EntityTypeConfiguration<Procedimento>
    {
       public ProcedimentoMap()
        {
            // Primary Key
            this.HasKey(t => t.IdProcedimento);

            this.Property(p => p.IdProcedimento).IsRequired();
            this.Property(p => p.NomeProcedimento).IsRequired().HasMaxLength(60).HasColumnName("NmProcedimento");
            this.Property(p => p.Sexo).IsRequired().HasMaxLength(30);
            this.Property(p => p.Codigo).IsRequired();
            this.Property(p => p.Odontologico).IsRequired();
            this.Property(p => p.Preparo).IsOptional();

            this.HasMany(s => s.Especialidades)
              .WithMany(s => s.Procedimentos).Map(s =>
              {
                  s.MapLeftKey("IdProcedimento");
                  s.MapRightKey("IdEspecialidade");
                  s.ToTable("EspecialidadeProcedimento");
              });
        }
    }
}