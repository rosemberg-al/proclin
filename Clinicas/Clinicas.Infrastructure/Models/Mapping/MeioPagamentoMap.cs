using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class MeioPagamentoMap : EntityTypeConfiguration<MeioPagamento>
    {
        public MeioPagamentoMap()
        {
            // Primary Key
            this.HasKey(t => t.IdMeioPagamento);

            // Properties
            this.Property(t => t.NmMeioPagamento)
                .HasMaxLength(60);
        }
    }
}