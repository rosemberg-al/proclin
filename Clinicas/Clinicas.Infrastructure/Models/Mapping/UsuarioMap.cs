using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class UsuarioMap : EntityTypeConfiguration<Usuario>
    {
        public UsuarioMap()
        {
            // Primary Key
            this.HasKey(t => t.IdUsuario);

            this.Property(t => t.Nome)
                .IsRequired();

            this.Property(t => t.Login)
                .IsRequired();

            this.Property(t => t.Email)
                .IsRequired();

            this.Property(t => t.Senha)
                .IsRequired();

            this.Property(t => t.PrimeiroAcesso)
               .IsRequired();

            this.Property(t => t.Tipo)
               .IsRequired();

            this.Property(t => t.Situacao)
                .IsRequired();

            this.HasOptional(t => t.Funcionario)
               .WithMany()
               .HasForeignKey(d => d.IdFuncionario);

            // Relationships
            this.HasRequired(t => t.GrupoUsuario)
                .WithMany(t => t.Usuarios)
                .HasForeignKey(d => d.IdGrupoUsuario);

            // Relationships
            this.HasRequired(t => t.UnidadeAtendimento)
                .WithMany()
                .HasForeignKey(d => d.IdUnidadeAtendimento);
        }
    }
}