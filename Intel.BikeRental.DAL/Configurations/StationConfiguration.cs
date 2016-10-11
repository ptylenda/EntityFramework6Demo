using Intel.BikeRental.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.BikeRental.DAL.Configurations
{
    public class StationConfiguration : EntityTypeConfiguration<Station>
    {
        public StationConfiguration()
        {
            this.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Ignore(x => x.Parameters);

            this.Property(x => x.SerializedParameters)
                .HasColumnName("Parameters");

            // Versioning the whole row for concurrency checks
            this.Property(x => x.RowVersion)
                .IsRowVersion();
        }
    }
}
