using Intel.BikeRental.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.BikeRental.DAL.Configurations
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            //this.HasKey(x => x.UserId);

            this.Property(x => x.FirstName)
                .HasMaxLength(50);

            this.Property(x => x.LastName)
                .HasMaxLength(50);

            // Property ignoring example
            this.Ignore(x => x.IsLogged);
            
            // Checking concurrency for updated on this single field
            this.Property(x => x.PhoneNumber)
                .IsConcurrencyToken();

            this.MapToStoredProcedures();
        }
    }
}
