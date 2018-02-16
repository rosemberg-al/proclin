using Clinicas.Domain.Model;
using Clinicas.Domain.Tiss;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class GuiaMap : EntityTypeConfiguration<Guia>
    {
        public GuiaMap()
        {

            // Primary Key
            this.HasKey(t => t.IdGuia);

            this.Property(p => p.IdBeneficiario);

            // Properties
            this.Property(t => t.CabecalhoGuia.RegistroANS).HasColumnName("RegistroANS");
            this.Property(t => t.CabecalhoGuia.DataEmissaoGuia).HasColumnName("DataEmissaoGuia");
            this.Property(t => t.CabecalhoGuia.NumeroGuia).HasColumnName("NumeroGuia");

            this.Property(t => t.DadosBeneficiario.Nome).HasColumnName("Nome");
            this.Property(t => t.DadosBeneficiario.NumeroCartaoSus).HasColumnName("NumeroCartaoSus");
            this.Property(t => t.DadosBeneficiario.NumeroCarteira).HasColumnName("NumeroCarteira");
            this.Property(t => t.DadosBeneficiario.Plano).HasColumnName("Plano");
            this.Property(t => t.DadosBeneficiario.ValidadeCarteira).HasColumnName("ValidadeCarteira");                     

            this.Property(t => t.DadosContratado.CodigoNaOperadora).HasColumnName("CodigoNaOperadora");
            this.Property(t => t.DadosContratado.TipoPessoa).HasColumnName("TipoPessoa");
            this.Property(t => t.DadosContratado.NomeContratado).HasColumnName("NomeContratado");
            this.Property(t => t.DadosContratado.CodigoCnes).HasColumnName("CodigoCnes");
            this.Property(t => t.DadosContratado.TipoLogradouro).HasColumnName("TipoLogradouro");
            this.Property(t => t.DadosContratado.Logradouro).HasColumnName("Logradouro");
            this.Property(t => t.DadosContratado.Numero).HasColumnName("Numero");
            this.Property(t => t.DadosContratado.Complemento).HasColumnName("Complemento");
            this.Property(t => t.DadosContratado.Municipio).HasColumnName("Municipio");
            this.Property(t => t.DadosContratado.UFContratado).HasColumnName("UFContratado");
            this.Property(t => t.DadosContratado.CodigoIBGEMunicipio).HasColumnName("CodigoIBGEMunicipio");
            this.Property(t => t.DadosContratado.UFConselho).HasColumnName("UFConselho");
            this.Property(t => t.DadosContratado.CEP).HasColumnName("CEP");
            this.Property(t => t.DadosContratado.NomeProfissionalExecutante).HasColumnName("NomeProfissionalExecutante");
            this.Property(t => t.DadosContratado.ConselhoProfissional).HasColumnName("ConselhoProfissional");
            this.Property(t => t.DadosContratado.NumeroConselho).HasColumnName("NumeroConselho");
            this.Property(t => t.DadosContratado.NumeroConselho).HasColumnName("NumeroConselho");
            this.Property(t => t.DadosContratado.CodigoCBOs).HasColumnName("CodigoCBOs");


            this.Property(t => t.HipoteseDiagnostica.TipoDoenca).HasColumnName("TipoDoenca");
            this.Property(t => t.HipoteseDiagnostica.TempoDoenca).HasColumnName("TempoDoenca");
            this.Property(t => t.HipoteseDiagnostica.IndicacaoAcidente).HasColumnName("IndicacaoAcidente");
            this.Property(t => t.HipoteseDiagnostica.CID10).HasColumnName("CID10");
            this.Property(t => t.HipoteseDiagnostica.CID102).HasColumnName("CID102");
            this.Property(t => t.HipoteseDiagnostica.CID103).HasColumnName("CID103");
            this.Property(t => t.HipoteseDiagnostica.CID104).HasColumnName("CID104");


            this.Property(t => t.DadosAtendimento.DataAtendimento).HasColumnName("DataAtendimento");
            this.Property(t => t.DadosAtendimento.CodigoTabela).HasColumnName("CodigoTabela");
            this.Property(t => t.DadosAtendimento.CodigoProcedimento).HasColumnName("CodigoProcedimento");
            this.Property(t => t.DadosAtendimento.TipoConsulta).HasColumnName("TipoConsulta");
            this.Property(t => t.DadosAtendimento.TipoSaida).HasColumnName("TipoSaida");
            this.Property(t => t.DadosAtendimento.Observacao).HasColumnName("Observacao");
            this.Property(t => t.DadosAtendimento.DataAssinaturaBeneficiario).HasColumnName("DataAssinaturaBeneficiario");

            this.Property(t => t.DadosAtendimento.DataAssinaturaMedico).HasColumnName("DataAssinaturaMedico");

            this.Property(t => t.TipoGuia).HasColumnName("TipoGuia");

            this.HasRequired(t => t.Clinica)
            .WithMany()
            .HasForeignKey(d => d.IdClinica);

            this.HasRequired(t => t.Convenio)
            .WithMany()
            .HasForeignKey(d => d.IdConvenio);

            this.HasOptional(t => t.Lote)
               .WithMany(t => t.Guias)
               .HasForeignKey(d => d.IdLote);
        }
    }
}