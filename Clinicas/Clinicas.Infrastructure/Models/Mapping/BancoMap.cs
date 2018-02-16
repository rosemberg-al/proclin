using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class BancoMap : EntityTypeConfiguration<Banco>
    {
        public BancoMap()
        {
            // Primary Key
            this.HasKey(t => t.IdBanco);
            this.Property(t => t.NomeBanco).HasColumnName("NmBanco");
            this.Property(t => t.Codigo);
        }
    }
}