using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class OcupacaoMap : EntityTypeConfiguration<Ocupacao>
    {
        public OcupacaoMap()
        {
            // Primary Key
            this.HasKey(t => t.IdOcupacao);

            this.Property(p => p.IdOcupacao).IsRequired();
            this.Property(p => p.NmOcupacao).IsRequired().HasMaxLength(60);
            this.Property(p => p.Codigo).IsOptional().HasMaxLength(30);
        }
    }
}