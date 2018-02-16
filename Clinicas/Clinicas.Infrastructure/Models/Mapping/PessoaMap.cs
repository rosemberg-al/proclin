using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class PessoaMap : EntityTypeConfiguration<Pessoa>
    {
        public PessoaMap()
        {
            // Primary Key
            this.HasKey(t => t.IdPessoa);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired();

            this.Property(t => t.Sexo)
            .IsOptional();

            this.Property(t => t.Conjuge)
               .IsOptional();

            this.Property(t => t.RazaoSocial)
                .IsOptional()
                .HasMaxLength(255);

            this.Property(t => t.Tipo)
                .IsRequired()
                .HasMaxLength(65532);

            this.Property(t => t.CpfCnpj)
                .IsOptional()
                .HasMaxLength(20);

            this.Property(t => t.RG)
                .IsOptional()
                .HasMaxLength(20);

            this.Property(t => t.IE)
                .IsOptional()
                .HasMaxLength(20);

            this.Property(t => t.Situacao)
                .IsOptional()
                .HasMaxLength(65532);

            this.Property(t => t.Profissao)
                .IsOptional()
                .HasMaxLength(60);

            this.Property(t => t.Mae)
                .IsOptional()
                .HasMaxLength(60);

            this.Property(t => t.Pai)
                .IsOptional()
                .HasMaxLength(60);

            this.Property(t => t.Telefone1)
                .IsOptional()
                .HasMaxLength(30);

            this.Property(t => t.Telefone2)
                .IsOptional()
                .HasMaxLength(30);

            this.Property(t => t.Email)
                .IsOptional()
                .HasMaxLength(100);

            this.Property(t => t.Site)
                .IsOptional()
                .HasMaxLength(80);

            this.Property(t => t.Observacoes)
                .IsOptional();

            this.Property(t => t.DataInclusao)
                .IsRequired();

            this.Property(t => t.DataAlteracao)
                .IsOptional();

            this.Property(t => t.DataExclusao)
                .IsOptional();

            this.HasRequired(t => t.UsuarioInclusao)
                 .WithMany()
                 .HasForeignKey(d => d.IdUsuarioInclusao);

            this.HasOptional(t => t.UsuarioAlteracao)
                .WithMany()
                .HasForeignKey(d => d.IdUsuarioAlteracao);

            this.HasRequired(t => t.UsuarioExclusao)
                .WithMany()
                .HasForeignKey(d => d.IdUsuarioExclusao);

            // Properties
            this.Property(t => t.TipoEndereco)
                .IsOptional();

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

            this.Property(t => t.Referencia)
                .IsOptional();



            this.HasRequired(t => t.Clinica)
               .WithMany()
               .HasForeignKey(d => d.IdClinica);
        }
    }
}