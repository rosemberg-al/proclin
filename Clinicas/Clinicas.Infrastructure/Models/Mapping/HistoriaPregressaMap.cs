using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class HistoriaPregressaMap : EntityTypeConfiguration<HistoriaPregressa>
    {
        public HistoriaPregressaMap()
        {

            this.HasKey(t => t.IdHistoriaPregressa);

            this.Property(t => t.Descricao)
                .IsRequired()
                .HasMaxLength(4000);

            this.Property(t => t.Data)
                .IsRequired();

            this.Property(t => t.Situacao)
                .IsRequired();

            this.HasRequired(t => t.Paciente)
               .WithMany(t => t.HistoriasPregressa)
               .HasForeignKey(d => d.IdPaciente);

            this.HasRequired(t => t.ProfissionalSaude)
             .WithMany()
             .HasForeignKey(t => t.IdFuncionario);
        }
    }
}