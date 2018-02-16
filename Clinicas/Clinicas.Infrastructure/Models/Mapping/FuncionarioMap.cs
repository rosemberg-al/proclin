using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class FuncionarioMap : EntityTypeConfiguration<Funcionario>
    {
        public FuncionarioMap()
        {
            // Properties
            this.HasKey(t => t.IdFuncionario);

            this.HasRequired(x => x.Pessoa).WithRequiredDependent();

            this.HasMany(s => s.Especialidades)
              .WithMany(s => s.Funcionarios).Map(s =>
              {
                  s.MapLeftKey("IdFuncionario");
                  s.MapRightKey("IdEspecialidade");
                  s.ToTable("EspecialidadeFuncionario");
              });
        }
    }
}