using Clinicas.Domain.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Clinicas.Infrastructure.Models.Mapping
{
    public class HospitalMap : EntityTypeConfiguration<Hospital>
    {
        public HospitalMap()
        {

            // Primary Key
            this.HasKey(t => t.IdHospital);

            // Properties
            this.Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(60);
           
        }
    }
}