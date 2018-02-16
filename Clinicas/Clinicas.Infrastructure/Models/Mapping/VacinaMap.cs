using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class VacinaMap : EntityTypeConfiguration<Vacina>
    {
        public VacinaMap()
        {

            // Primary Key
            this.HasKey(t => t.IdVacina);

            // Properties
            this.Property(t => t.Descricao)
                .IsRequired()
                .HasMaxLength(100);
            // Properties
            this.Property(t => t.Situacao)
                .IsRequired();

            this.Property(t => t.Idade)
             .IsRequired();
            
            this.Property(t => t.Doses)
             .IsRequired();


        }
    }
}