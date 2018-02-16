using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class ConsultorioMap : EntityTypeConfiguration<Consultorio>
    {
        public ConsultorioMap()
        {
            // Primary Key
            this.HasKey(t => t.IdConsultorio);

            this.Property(t => t.NmConsultorio)
                .HasMaxLength(100);

            this.HasRequired(t => t.UnidadeAtendimento)
               .WithMany()
               .HasForeignKey(d => d.IdUnidadeAtendimento);


            this.HasRequired(t => t.Clinica)
                .WithMany()
                .HasForeignKey(d => d.IdClinica);

        }
    }
}