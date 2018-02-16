using Clinicas.Domain.Model;
using System.Data.Entity.ModelConfiguration;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class ReceituarioMedicamentoMap : EntityTypeConfiguration<ReceituarioMedicamento>
    {
        public ReceituarioMedicamentoMap()
        {
            // Primary Key
            this.HasKey(t => new { t.IdMedicamento, t.IdReceituario });

            // Properties
            this.Property(t => t.Nome)
                .IsRequired();

            this.Property(t => t.Quantidade)
                .IsRequired();

            this.Property(t => t.Posologia)
                .IsRequired();
        }
    }
}