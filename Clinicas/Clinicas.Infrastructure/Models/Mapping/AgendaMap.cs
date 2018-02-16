using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class AgendaMap : EntityTypeConfiguration<Agenda>
    {
        public AgendaMap()
        {
            // Primary Key
            this.HasKey(t => t.IdAgenda);
            
            // Properties
            this.HasOptional(t => t.Paciente)
            .WithMany()
            .HasForeignKey(d => d.IdPaciente);


            this.Property(t => t.DataConvocacaoPaciente)
           .IsOptional();

            this.HasOptional(t => t.TipoAtendimento)
            .WithMany()
            .HasForeignKey(d => d.IdTipoAtendimento);

            this.Property(t => t.Data)
                .IsRequired();

            this.Property(t => t.Hora)
                .IsRequired();

            this.Property(t => t.Situacao)
                .IsRequired();

            this.Property(t => t.Avulsa)
               .IsOptional();

            this.Property(t => t.SalaEspera)
             .IsOptional();

            this.Property(t => t.ConvocarPaciente)
                .IsOptional();


            this.Property(t => t.Observacao);
            
            this.HasOptional(t => t.Procedimento)
                .WithMany()
                .HasForeignKey(d => d.IdProcedimento);
            
            this.HasOptional(t => t.Especialidade)
               .WithMany()
               .HasForeignKey(d => d.IdEspecialidade);

            this.HasOptional(t => t.Convenio)
                 .WithMany()
                 .HasForeignKey(d => d.IdConvenio);

            this.HasRequired(t => t.ProfissionalSaude)
             .WithMany()
             .HasForeignKey(d => d.IdFuncionario);

            this.HasRequired(t => t.UnidadeAtendimento)
           .WithMany()
           .HasForeignKey(d => d.IdUnidadeAtendimento);

            this.Property(t => t.Valor).IsOptional();
            this.Property(t => t.ValorProfissional).IsOptional();

            this.Property(t => t.Tipo);
            this.Property(t => t.Solicitante);

            this.HasRequired(t => t.Clinica)
            .WithMany()
            .HasForeignKey(d => d.IdClinica);
            
            this.HasRequired(t => t.UsuarioInclusao)
              .WithMany()
              .HasForeignKey(d => d.IdUsuarioInclusao);

            this.Property(t => t.DataInclusao).IsRequired();

            this.HasOptional(t => t.UsuarioMarcado)
             .WithMany()
             .HasForeignKey(d => d.IdUsuarioMarcado);
            this.Property(t => t.DataMarcado).IsOptional();

            this.HasOptional(t => t.UsuarioRealizado)
             .WithMany()
             .HasForeignKey(d => d.IdUsuarioRealizado);
            this.Property(t => t.DataRealizado).IsOptional();

            this.HasOptional(t => t.UsuarioCancelado)
             .WithMany()
             .HasForeignKey(d => d.IdUsuarioCancelado);
            this.Property(t => t.DataCancelado).IsOptional();



        }
    }
}