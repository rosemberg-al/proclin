using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class MedicoMap : EntityTypeConfiguration<Medico>
    {
        public MedicoMap()
        {

            // Primary Key
            this.HasKey(t => t.IdMedico);

            // Properties
            this.Property(t => t.NomeMedico)
                .IsRequired()
                .HasMaxLength(150);
            this.Property(t => t.Crm)
               .IsRequired()
               .HasMaxLength(20);
            this.Property(t => t.DataCadastro)
               .IsRequired();
        }
    }

}