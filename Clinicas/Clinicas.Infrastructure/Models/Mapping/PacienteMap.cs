using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class PacienteMap : EntityTypeConfiguration<Paciente>
    {
        public PacienteMap()
        {

            // Primary Key
            this.HasKey(t => t.IdPaciente);
            this.HasRequired(x => x.Pessoa).WithOptional();

            // Properties
            this.Property(t => t.Situacao)
                .IsOptional();
            // Properties
            this.Property(t => t.CartaoSus)
                .IsOptional()
                .HasMaxLength(20);
            // Properties
            this.Property(t => t.Foto)
                .IsOptional();

           /*  this.HasRequired(t => t.DadosNascimento)
              .WithMany()
              .HasForeignKey(d => d.IdDadosNascimento); */

        }
    }
}