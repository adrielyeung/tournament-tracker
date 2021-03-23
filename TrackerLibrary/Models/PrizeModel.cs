using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class PrizeModel
    {
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Number of the place (1, 2, 3 etc.)
        /// </summary>
        public int PlaceNumber { get; set; }
        /// <summary>
        /// Name of the place (e.g. champion, runner-up etc.)
        /// </summary>
        public string PlaceName { get; set; }
        /// <summary>
        /// Amount of the prize (in dollars).
        /// </summary>
        public decimal PrizeAmount { get; set; }
        /// <summary>
        /// Percentage of the prize out of total prize cash.
        /// </summary>
        public double PrizePercentage { get; set; }

        public PrizeModel()
        {

        }

        public PrizeModel(string placeName, string placeNumber, string prizeAmount, string prizePercentage)
        {
            PlaceName = placeName;
            int placeNumberValue = 0;
            int.TryParse(placeNumber, out placeNumberValue);

            // Set PlaceNumber to zero if invalid parse
            PlaceNumber = placeNumberValue;

            decimal prizeAmountValue = 0;
            decimal.TryParse(prizeAmount, out prizeAmountValue);

            PrizeAmount = prizeAmountValue;

            double prizePercentageValue = 0;
            double.TryParse(prizePercentage, out prizePercentageValue);

            PrizePercentage = prizePercentageValue;
        }
    }
}
