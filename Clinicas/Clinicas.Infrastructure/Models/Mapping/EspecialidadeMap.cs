using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class EspecialidadeMap : EntityTypeConfiguration<Especialidade>
    {
       // public const string TableName = "especialidade";

        public EspecialidadeMap()
        {
            // ToTable(TableName);
            // Primary Key
            this.HasKey(t => t.IdEspecialidade);

            // Properties
            this.Property(t => t.NmEspecialidade)
                .IsOptional()
                .HasMaxLength(60);

            this.Property(t => t.Situacao)
               .IsRequired();

            this.HasMany(s => s.Procedimentos)
              .WithMany(s => s.Especialidades).Map(s =>
              {
                  s.MapLeftKey("IdProcedimento");
                  s.MapRightKey("IdEspecialidade");
                  s.ToTable("EspecialidadeProcedimento");
              });

            this.HasMany(s => s.Funcionarios)
              .WithMany(s=>s.Especialidades).Map(s =>
              {
                  s.MapLeftKey("IdEspecialidade");
                  s.MapRightKey("IdFuncionario");
                  s.ToTable("EspecialidadeFuncionario");
              });
        }
    }
}