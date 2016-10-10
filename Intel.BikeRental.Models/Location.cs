using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.BikeRental.Models
{
    public class Location : Base
    {
        public Location()
        {
        }

        public Location(double lat, double lon)
        {
            this.Latitude = lat;
            this.Longitude = lon;
        }

        public double Longitude { get; set; }

        public double Latitude { get; set; }
    }
}
