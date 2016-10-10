using Intel.BikeRental.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.BikeRental.DAL.Configurations
{
    public class BikeConfiguration : EntityTypeConfiguration<Bike>
    {
        public BikeConfiguration()
        {
            this.Property(x => x.Number)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

            this.Property(x => x.Color)
                .HasMaxLength(20)
                .IsUnicode(false);
        }
    }
}
