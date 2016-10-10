using Intel.BikeRental.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.BikeRental.DAL.Configurations
{
    public class RentalConfiguration : EntityTypeConfiguration<Rental>
    {
        public RentalConfiguration()
        {
            this.Property(x => x.DateFrom)
                .HasColumnType("datetime2");

            this.Property(x => x.DateTo)
                .HasColumnType("datetime2");
        }
    }
}
