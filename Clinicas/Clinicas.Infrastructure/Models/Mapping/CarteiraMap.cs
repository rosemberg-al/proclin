using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class CarteiraMap : EntityTypeConfiguration<Carteira>
    {
        public CarteiraMap()
        {

            // Primary Key
            this.HasKey(t => t.IdCarteira);

            // Properties
            this.Property(t => t.NumeroCarteira)
                .IsRequired()
                .HasMaxLength(30);
            // Properties
            this.Property(t => t.ValidadeCarteira)
                .IsRequired();
            // Properties
            this.Property(t => t.Plano)
                .IsRequired()
                .HasMaxLength(150);

            this.HasRequired(t => t.Paciente)
               .WithMany(t => t.Carteiras)
               .HasForeignKey(d => d.IdPaciente);

            this.HasRequired(t => t.Convenio)
              .WithMany()
              .HasForeignKey(d => d.IdConvenio);
        }
    }
}