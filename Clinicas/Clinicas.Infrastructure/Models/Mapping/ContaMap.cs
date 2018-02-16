using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class ContaMap : EntityTypeConfiguration<Conta>
    {
        public ContaMap()
        {
            // Primary Key
            this.HasKey(t => t.IdConta);

            // Properties
            this.Property(t => t.NmConta);
            this.Property(t => t.Situacao);
            this.Property(t => t.Saldo);

            this.HasRequired(t => t.Clinica).WithMany().HasForeignKey(x=>x.IdClinica);
        }
    }
}