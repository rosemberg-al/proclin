using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class AtestadoMap : EntityTypeConfiguration<Atestado>
    {
        public AtestadoMap()
        {

            // Primary Key
            this.HasKey(t => t.IdAtestado);

            this.Property(p => p.IdFuncionario).HasColumnName("IdFuncionario");

            // Properties
            this.Property(t => t.Descricao)
                .IsOptional()
                .HasMaxLength(4000);
            // Properties
            this.Property(t => t.Data)
                .IsOptional();

            this.HasRequired(t => t.Paciente)
                .WithMany(t => t.Atestados)
                .HasForeignKey(d => d.IdPaciente);

            this.HasRequired(t => t.Funcionario)
               .WithMany()
               .HasForeignKey(d => d.IdFuncionario);
        }
    }
}