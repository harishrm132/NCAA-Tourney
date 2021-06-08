using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class PriceModel
    {
        /// <summary>
        /// The unique Indentifier for prize
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents place in the tournament 
        /// </summary>
        public int PlaceNumber { get; set; }

        /// <summary>
        /// Represents place name of place in tournament
        /// </summary>
        public string PlaceName { get; set; }

        /// <summary>
        /// Represents Prizeamount for the place
        /// </summary>
        public decimal PriceAmount { get; set; }

        /// <summary>
        /// Represents percentage of amount for the place
        /// </summary>
        public double PricePercentage { get; set; }

        public PriceModel()
        {

        }

        public PriceModel(string placename, string placeno, string prizeamount, string prizepercentage)
        {
            PlaceName = placename;
            PlaceNumber = int.Parse(placeno);
            PriceAmount = decimal.Parse(prizeamount);
            PricePercentage = double.Parse(prizepercentage);
        }
    }
}
