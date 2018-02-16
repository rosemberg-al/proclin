using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class MaterialMap : EntityTypeConfiguration<Material>
    {
        public MaterialMap()
        {
            // Primary Key
            this.HasKey(t => t.IdMaterial);
            this.Property(t => t.Nome);
            this.Property(t => t.EstoqueMinimo);
            this.Property(t => t.Marca);
            this.Property(t => t.Modelo);
            this.Property(t => t.Observacao);
            this.Property(t => t.EstadoConservacao);
            this.Property(t => t.Identificador);
            this.Property(t => t.ValorVenda);
            this.Property(t => t.ValorCompra);
            this.Property(t => t.Situacao);
            this.Property(t => t.EstoqueAtual);

            this.HasRequired(t => t.UnidadeAtendimento)
                .WithMany()
                .HasForeignKey(d => d.IdUnidadeAtendimento);

            this.HasRequired(t => t.TipoMaterial)
            .WithMany()
            .HasForeignKey(d => d.IdTipoMaterial);

        }
    }
}