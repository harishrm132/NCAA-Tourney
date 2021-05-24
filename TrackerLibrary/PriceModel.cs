using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class PriceModel
    {
        public int PlaceNumber { get; set; }
        public string PlaceName { get; set; }
        public decimal PriceAmount { get; set; }
        public double PricePercentage { get; set; }
    }
}
