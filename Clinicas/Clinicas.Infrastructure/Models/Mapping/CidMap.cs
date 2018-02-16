using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class CidMap : EntityTypeConfiguration<Cid>
    {
        public const string TableName = "Cid";
        public CidMap()
        {
            ToTable(TableName);
            // Primary Key
            this.HasKey(t => t.Codigo);

            this.Property(p => p.Codigo).IsRequired().HasColumnName("CdCid");
            this.Property(p => p.Descricao).IsRequired().HasMaxLength(100).HasColumnName("NmCid");
            this.Property(p => p.Agravo).IsRequired().HasColumnName("tp_agravo");
            this.Property(p => p.SexoOcorrencia).IsRequired().HasColumnName("tp_sexo");
            this.Property(p => p.Estadio).IsRequired().HasColumnName("tp_estadio");
            this.Property(p => p.CamposIrradiados).IsRequired().HasMaxLength(4).HasColumnName("vl_campos_irradiados");
        }
    }
}