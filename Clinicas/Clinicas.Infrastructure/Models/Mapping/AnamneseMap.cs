using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class AnamneseMap : EntityTypeConfiguration<Anamnese>
    {
        public AnamneseMap()
        {

            // Primary Key
            this.HasKey(t => t.IdAnamnese);

            // Properties
            this.Property(t => t.Hma)
                .IsOptional()
                .HasMaxLength(4000);
            // Properties
            this.Property(t => t.CondutaMedica)
                .IsOptional()
                .HasMaxLength(4000);
            // Properties
            this.Property(t => t.Diagnostico)
                .IsOptional()
                .HasMaxLength(4000);

            this.HasRequired(t => t.Paciente)
                .WithMany(t => t.Anamneses)
                .HasForeignKey(d => d.IdPaciente);

            this.HasRequired(t => t.ProfissionalSaude)
                .WithMany()
            .HasForeignKey(d => d.IdFuncionario);
        }
    }
}