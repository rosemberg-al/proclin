using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class UnidadeAtendimentoMap : EntityTypeConfiguration<UnidadeAtendimento>
    {
       public UnidadeAtendimentoMap()
        {
            this.HasKey(t => t.IdUnidadeAtendimento);

            this.Property(p => p.Nome).IsRequired();

            this.HasRequired(t => t.Clinica)
              .WithMany(t => t.Unidades)
              .HasForeignKey(d => d.IdClinica);

            this.Property(t => t.Telefone1)
             .IsRequired()
             .HasMaxLength(30);

            this.Property(t => t.Situacao)
             .IsRequired();

            this.Property(t => t.CpfCnpj)
           .IsOptional();

            this.Property(t => t.Telefone2)
                .IsOptional()
                .HasMaxLength(30);

            this.Property(t => t.Fax)
               .IsOptional()
               .HasMaxLength(30);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(150);

            this.Property(t => t.Cep)
            .IsOptional();

            this.Property(t => t.Bairro)
                .IsOptional();

            this.Property(t => t.Logradouro)
                .IsOptional();

            this.Property(t => t.Numero)
                .IsOptional();

            this.Property(t => t.Complemento)
                .IsOptional();


            this.HasMany(s => s.Especialidades)
              .WithMany(s => s.UnidadeAtendimentos).Map(s =>
              {
                  s.MapLeftKey("IdEspecialidade");
                  s.MapRightKey("IdUnidadeAtendimento");
                  s.ToTable("EspecialidadeUnidadeAtendimento");
              });

        }
    }
}
