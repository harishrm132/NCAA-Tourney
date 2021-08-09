using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace NCAA_UI.ViewModels
{
    public class CreatePrizeFormViewModel : Screen
    {
        private int _placeNumber;
        private string _placeName;
        private decimal _prizeAmount;
        private double _prizePercentage;

        public string PlaceName
        {
            get { return _placeName; }
            set 
            { 
                _placeName = value;
                NotifyOfPropertyChange(() => PlaceName);
            }
        }

        public decimal PrizeAmount
        {
            get { return _prizeAmount; }
            set 
            { 
                _prizeAmount = value;
                NotifyOfPropertyChange(() => PrizeAmount);
            }
        }

        public double PrizePercentage
        {
            get { return _prizePercentage; }
            set 
            { 
                _prizePercentage = value;
                NotifyOfPropertyChange(() => PrizePercentage);
            }
        }

        public bool CanCreatePrize(int placeNumber, string placeName, decimal prizeAmount, double prizePercentage)
        {
            return ValidateForm(placeNumber, placeName, prizeAmount, prizePercentage);
        }

        public void CreatePrize(int placeNumber, string placeName, decimal prizeAmount, double prizePercentage)
        {
            PriceModel model = new PriceModel
            {
                PlaceNumber = placeNumber,
                PlaceName = placeName,
                PriceAmount = prizeAmount,
                PricePercentage = prizePercentage
            };

            //GlobalConfig.Connection.CreatePrize(model);

            //TODO - Close out forma and alert the calling form
        }

        private bool ValidateForm(int placeNumber, string placeName, decimal prizeAmount, double prizePercentage)
        {
            bool output = true;

            if (placeNumber < 1) { output = false; }

            if (placeName.Length == 0) { output = false; }
 
            if (prizeAmount <= 0 && (prizePercentage <= 0 || prizePercentage > 100)) { output = false; }

            return output;
        }

    }
}
