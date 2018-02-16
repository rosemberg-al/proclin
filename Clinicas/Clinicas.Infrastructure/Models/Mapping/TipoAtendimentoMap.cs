using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class TipoAtendimentoMap : EntityTypeConfiguration<TipoAtendimento>
    {
        public TipoAtendimentoMap()
        {
            this.HasKey(t => t.IdTipoAtendimento);

            this.Property(t => t.NmTipoAtendimento)
                .IsRequired();

            this.Property(t => t.Descricao)
                .IsOptional();

        }
    }
}