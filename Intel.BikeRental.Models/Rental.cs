using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intel.BikeRental.Models
{
    public class Rental
    {
        public int RentalId { get; set; }

        // Needed for lazy loading
        public virtual User User { get; set; }
        
        public Vehicle Vehicle { get; set; }

        public Station StationFrom { get; set; }

        public Station StationTo { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime? DateTo { get; set; }

        public decimal Cost { get; set; }

    }
}
