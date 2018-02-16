using Clinicas.Domain.Model;
using System.Data.Entity.ModelConfiguration;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class ClinicaMap : EntityTypeConfiguration<Clinica>
    {
        public ClinicaMap()
        {
            // Primary Key
            this.HasKey(t => t.IdClinica);
            this.Property(t => t.Nome);
            this.Property(t => t.Logo);
            this.Property(t => t.Situacao);
            this.Property(t => t.DataInclusao);



        }
    }
}