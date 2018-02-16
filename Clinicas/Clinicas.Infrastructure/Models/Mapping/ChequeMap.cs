using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class ChequeMap : EntityTypeConfiguration<Cheque>
    {
        public ChequeMap()
        {
            // Primary Key
            this.HasKey(t => t.IdCheque);

            // Properties
            this.Property(t => t.Emitente)
                .HasMaxLength(60);

            this.Property(t => t.Banco)
                .HasMaxLength(60);

            this.Property(t => t.Agencia)
                .HasMaxLength(20);

            this.Property(t => t.Conta)
                .HasMaxLength(20);

            this.Property(t => t.Situacao)
                .HasMaxLength(60);

            this.Property(t => t.Historico)
                .HasMaxLength(4000);

            this.Property(t => t.Valor);

            // Table & Column Mappings
            this.Property(t => t.IdCheque).HasColumnName("IdCheque");
            this.Property(t => t.DataInclusao).HasColumnName("DataInclusao");
            this.Property(t => t.Emitente).HasColumnName("Emitente");
            this.Property(t => t.Banco).HasColumnName("Banco");
            this.Property(t => t.Agencia).HasColumnName("Agencia");
            this.Property(t => t.Conta).HasColumnName("Conta");
            this.Property(t => t.BomPara).HasColumnName("BomPara");
            this.Property(t => t.Situacao).HasColumnName("Situacao");
            this.Property(t => t.IdFinanceiro).HasColumnName("IdFinanceiro");
            this.Property(t => t.IdPessoa).HasColumnName("IdPessoa");
            this.Property(t => t.Historico).HasColumnName("Historico");

            this.HasOptional(t => t.Pessoa)
              .WithMany()
              .HasForeignKey(d => d.IdPessoa);

            this.HasRequired(t => t.Clinica)
             .WithMany()
             .HasForeignKey(d => d.IdClinica);

            this.HasOptional(t => t.Financeiro)
              .WithMany()
              .HasForeignKey(d => d.IdFinanceiro);

        }
    }
}