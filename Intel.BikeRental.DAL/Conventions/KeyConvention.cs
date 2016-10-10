using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.BikeRental.DAL.Conventions
{
    public class KeyConvention : Convention
    {
        public KeyConvention()
        {
            this.Properties()
                .Where(x => x.Name.EndsWith("Key"))
                .Configure(x => x.IsKey());
        }
    }
}
