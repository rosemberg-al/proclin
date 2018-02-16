using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class FinanceiroMap : EntityTypeConfiguration<Financeiro>
    {
        public FinanceiroMap()
        {
            // Primary Key
            this.HasKey(t => t.IdFinanceiro);

            // Table & Column Mappings
            this.Property(t => t.IdFinanceiro).HasColumnName("IdFinanceiro");
            this.Property(t => t.TotalDesconto).HasColumnName("TotalDesconto");
            this.Property(t => t.Total).HasColumnName("Total");
            this.Property(t => t.TotalAcerto).HasColumnName("TotalAcerto");
            this.Property(t => t.IdPessoa).HasColumnName("IdPessoa");
            this.Property(t => t.Tipo).HasColumnName("Tipo");

            this.HasRequired(t => t.Pessoa)
                .WithMany(t => t.Financeiroes)
                .HasForeignKey(d => d.IdPessoa);

            this.HasRequired(t => t.Clinica)
               .WithMany()
               .HasForeignKey(d => d.IdClinica);

            this.HasRequired(t => t.UnidadeAtendimento)
               .WithMany()
               .HasForeignKey(d => d.IdUnidadeAtendimento);


        }
    }
}