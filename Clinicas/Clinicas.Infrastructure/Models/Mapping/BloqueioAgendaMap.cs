using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class BloqueioAgendaMap : EntityTypeConfiguration<BloqueioAgenda>
    {
        public BloqueioAgendaMap()
        {
            // Primary Key
            this.HasKey(t => t.IdBloqueio);

            // Properties
            this.Property(t => t.DataFim);
            this.Property(t => t.DataInicio);
            this.Property(t => t.Motivo);

            this.HasRequired(t => t.Funcionario).WithMany().HasForeignKey(x => x.IdFuncionario);
            this.HasRequired(t => t.Clinica).WithMany().HasForeignKey(x => x.IdClinica);
        }
    }
}