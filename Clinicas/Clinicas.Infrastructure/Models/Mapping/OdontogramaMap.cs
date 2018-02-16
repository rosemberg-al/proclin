using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class OdontogramaMap : EntityTypeConfiguration<Odontograma>
    {
        public OdontogramaMap()
        {
            // Primary Key
            this.HasKey(t => t.IdOdontograma);

            this.HasRequired(t => t.Paciente)
            .WithMany()
            .HasForeignKey(d => d.IdPaciente);

            this.HasRequired(t => t.Funcionario)
            .WithMany()
            .HasForeignKey(d => d.IdFuncionario);

            this.Property(t => t.DataInicio)
               .IsRequired();

            this.Property(t => t.DataTermino)
              .IsRequired();

            this.Property(t => t.Situacao)
                .IsRequired();

            this.Property(t => t.Descricao)
               .IsRequired();

            this.Property(t => t.Dente)
              .IsRequired();




        }
    }
}