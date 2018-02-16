using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class ConvenioMap : EntityTypeConfiguration<Convenio>
    {  
        public ConvenioMap()
        {
            // Primary Key
            this.HasKey(t => t.IdConvenio);
            this.HasRequired(x => x.Pessoa).WithOptional();

            // Properties
            this.Property(t => t.RegistroAns)
                .IsOptional();

            this.Property(t => t.LogoGuia)
                .IsOptional();
        }
    }
}