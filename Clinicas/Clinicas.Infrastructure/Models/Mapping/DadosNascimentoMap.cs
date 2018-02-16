using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class DadosNascimentoMap : EntityTypeConfiguration<DadosNascimento>
    {
        public DadosNascimentoMap()
        {

            // Primary Key
           this.HasKey(t => t.IdDadosNascimento);

            // Properties
            this.Property(t => t.AleitamentoMaternoPrimeiraHoraVida)
                .IsOptional();
            // Properties
            this.Property(t => t.AlturaNascimento)
                .IsOptional();
            // Properties
            this.Property(t => t.AssistiuRecemNascidoRN)
                .IsOptional();
            // Properties
            this.Property(t => t.Condutas)
                .IsOptional();
            // Properties
            this.Property(t => t.DataAlta)
                .IsOptional();
            // Properties
            this.Property(t => t.DataNascimento)
                .IsOptional();
            // Properties
            this.Property(t => t.DataTestePezinho)
                .IsOptional();

            // Properties
            this.Property(t => t.HoraNascimento)
                .IsOptional();
            // Properties
            this.Property(t => t.IdadeGestacional)
                .IsOptional();
            // Properties
            this.Property(t => t.PerimetroCraniano)
                .IsOptional();
            // Properties
            this.Property(t => t.PerimetroAbdominal)
                .IsOptional();

            // Properties
            this.Property(t => t.PesoAlta)
                .IsOptional();
            // Properties
            this.Property(t => t.PesoNascimento)
                .IsOptional();

            this.HasRequired(t => t.Hospital)
              .WithMany()
              .HasForeignKey(d => d.IdHospital);

        }
    }
}