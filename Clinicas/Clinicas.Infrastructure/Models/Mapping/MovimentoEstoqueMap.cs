using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class MovimentoEstoqueMap : EntityTypeConfiguration<MovimentoEstoque>
    {
        public MovimentoEstoqueMap()
        {
            // Primary Key
            this.HasKey(t => t.IdMovimentoEstoque);

            this.Property(t => t.Data);
            this.Property(t => t.Tipo);
            this.Property(t => t.Quantidade);
            this.Property(t => t.Situacao);

            this.HasRequired(t => t.UnidadeAtendimento)
                .WithMany()
                .HasForeignKey(d => d.IdUnidadeAtendimento);

            this.HasRequired(t => t.Material)
            .WithMany(t=>t.MovimentoEstoque)
            .HasForeignKey(d => d.IdMaterial);
        }
    }
}