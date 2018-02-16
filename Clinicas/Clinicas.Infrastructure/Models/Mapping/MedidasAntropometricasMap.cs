using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class MedidasAntropometricasMap : EntityTypeConfiguration<MedidasAntropometricas>
    {
        public MedidasAntropometricasMap()
        {

            // Primary Key
            this.HasKey(t => t.IdMedida);

            this.Property(t => t.Altura)
                .IsRequired();

            this.Property(t => t.Data)
                .IsRequired();

            this.Property(t => t.Imc)
                .IsRequired();

            this.Property(t => t.PerimetroCefalico)
                .IsRequired();

            this.Property(t => t.Peso)
               .IsRequired();

            this.HasRequired(t => t.Paciente)
                .WithMany()
                .HasForeignKey(d => d.IdPaciente);

            this.HasRequired(t => t.ProfissionalSaude)
                .WithMany()
            .HasForeignKey(d => d.IdFuncionario);
        }
    }
}