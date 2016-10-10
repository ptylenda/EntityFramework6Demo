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
        }
    }
}
