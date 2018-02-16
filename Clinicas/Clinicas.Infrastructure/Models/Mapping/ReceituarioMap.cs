using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class ReceituarioMap : EntityTypeConfiguration<Receituario>
    {
        public ReceituarioMap()
        {

            // Primary Key
            this.HasKey(t => t.IdReceituario);

            this.Property(p => p.IdFuncionario).HasColumnName("IdFuncionario");

            // Properties
            this.Property(t => t.Descricao)
                .IsOptional()
                .HasMaxLength(4000);
            // Properties
            this.Property(t => t.Data)
                .IsOptional();

            this.HasRequired(t => t.Paciente)
                .WithMany(t => t.Receituarios)
                .HasForeignKey(d => d.IdPaciente);

            this.HasRequired(t => t.Funcionario)
               .WithMany()
               .HasForeignKey(d => d.IdFuncionario);
        }
    }
}