using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class MedicamentoMap : EntityTypeConfiguration<Medicamento>
    {
        public MedicamentoMap()
        {
            // Primary Key
            this.HasKey(t => t.IdMedicamento);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(60);

            this.Property(t => t.Fabricante);
            this.Property(t => t.RegistroMS);
            this.Property(t => t.PrincipioAtivo);
            this.Property(t => t.Posologia);
            this.Property(t => t.Indicacao);
            this.Property(t => t.ContraIndicacao);


        }
    }
}