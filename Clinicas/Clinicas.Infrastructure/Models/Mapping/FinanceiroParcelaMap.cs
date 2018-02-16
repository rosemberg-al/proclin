using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class FinanceiroParcelaMap : EntityTypeConfiguration<FinanceiroParcela>
    {
        public FinanceiroParcelaMap()
        {
            // Primary Key
            this.HasKey(t => t.IdParcela);

            this.Property(t => t.IdFinanceiro);

            this.Property(t => t.Situacao)
                .HasMaxLength(30);

            this.Property(t => t.Observacao)
                .HasMaxLength(255);

            this.Property(t => t.NossoNumero)
                .HasMaxLength(200);

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

            this.HasOptional(t => t.UsuarioExclusao)
                .WithMany()
                .HasForeignKey(d => d.IdUsuarioExclusao);

            this.HasOptional(t => t.UsuarioBaixa)
              .WithMany()
              .HasForeignKey(d => d.IdUsuarioBaixa);

            this.HasRequired(t => t.Financeiro)
             .WithMany(t => t.Parcelas)
             .HasForeignKey(d => d.IdFinanceiro);

            this.HasRequired(t => t.PlanoConta)
                .WithMany(t => t.Financeiroes)
                .HasForeignKey(d => d.IdPlanoConta);

            this.HasOptional(t => t.MeioPagamento)
                .WithMany(t => t.FinanceiroParcelas)
                .HasForeignKey(d => d.IdMeioPagamento);

            this.HasRequired(t => t.Conta)
                .WithMany(t => t.Financeiroes)
                .HasForeignKey(d => d.IdConta);

            this.HasOptional(t => t.Remessa)
            .WithMany(t => t.Parcelas)
          .HasForeignKey(d => d.IdRemessa);


        }
    }
}