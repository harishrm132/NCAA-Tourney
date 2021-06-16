using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess;

namespace TrackerUI
{
    public partial class CreatePriceForm : Form
    {
        IPrizeRequester CallingForm;

        public CreatePriceForm(IPrizeRequester caller)
        {
            InitializeComponent();
            CallingForm = caller;
        }

        private void CreatePrizebutton_Click(object sender, EventArgs e)
        {
            if (ValidateForm()) 
            {
                PriceModel model = new PriceModel(
                    PlaceNameValue.Text, PlaceNoValue.Text, PrizeAmountValue.Text, PrizePercentageValue.Text);

                GlobalConfig.Connection.CreatePrize(model);

                CallingForm.PrizeComplete(model);

                this.Close();
                //PlaceNameValue.Text = "";
                //PlaceNoValue.Text = "";
                //PrizeAmountValue.Text = "0";
                //PrizePercentageValue.Text = "0";
            }
            else { MessageBox.Show("Forms has invalid information", "Input Validation"); }
        }

        private bool ValidateForm()
        {
            bool output = true;

            int placeNumber = 0;
            bool IsplaceNumberValid = int.TryParse(PlaceNoValue.Text, out placeNumber);
            if (!IsplaceNumberValid || placeNumber < 1) { output = false; }

            if(PlaceNameValue.Text.Length == 0) { output =  false; } 

            decimal prizeAmount = 0;
            double prizePercentage = 0;
            bool IsprizeAmountValid = decimal.TryParse(PrizeAmountValue.Text, out prizeAmount);
            bool IsprizePercentageValid = double.TryParse(PrizeAmountValue.Text, out prizePercentage);
            if (IsprizeAmountValid == false || IsprizePercentageValid == false) { output = false; }
            if(prizeAmount <= 0 && (prizePercentage <= 0 || prizePercentage > 100)) { output = false; }

            return output;
        }
    }
}
