using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class RequisicaoExamesMap : EntityTypeConfiguration<RequisicaoExames>
    {
        public RequisicaoExamesMap()
        {

            // Primary Key
            this.HasKey(t => t.IdRequisicao);

            // Properties
            this.Property(t => t.Classe)
                .IsOptional()
                .HasMaxLength(100);

            // Properties
            this.Property(t => t.Data)
                .IsOptional();

            // Properties
            this.Property(t => t.Tipo)
                .IsOptional();

            // Properties
            this.Property(t => t.NaturezaExame)
                .IsOptional();

            // Properties
            this.Property(t => t.Material)
                .IsOptional();

            // Properties
            this.Property(t => t.DiagnosticoClinico)
                .IsOptional();

            this.HasRequired(t => t.Paciente)
                .WithMany(t => t.RequisicaoExames)
                .HasForeignKey(d => d.IdPaciente);

            this.HasRequired(t => t.Medico)
               .WithMany()
               .HasForeignKey(d => d.IdMedico);
        }
    }
}