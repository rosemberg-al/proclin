using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class RegistroVacinaMap : EntityTypeConfiguration<RegistroVacina>
    {
        public RegistroVacinaMap()
        {

            // Primary Key
            this.HasKey(t => t.IdRegistroVacina);

            // Properties
            this.Property(t => t.Lote)
                .IsRequired()
                .HasMaxLength(60);
            // Properties
            this.Property(t => t.Data)
                .IsRequired();

            this.Property(t => t.IdPaciente)
              .IsRequired();

            // Properties
            this.Property(t => t.Dose)
                .IsRequired();

            // Properties
            this.Property(t => t.Hora)
                .IsRequired();

            this.HasRequired(t => t.Paciente)
              .WithMany(t => t.RegistroVacinas)
              .HasForeignKey(d => d.IdPaciente);

            this.HasRequired(t => t.Vacina)
               .WithMany()
               .HasForeignKey(d => d.IdVacina);
        }
    }
}