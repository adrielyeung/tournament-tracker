using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.DataAccess;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreatePrizeForm : Form
    {
        IPrizeRequester callingForm;

        /// <summary>
        /// Creates a Prize Form for inputting a prize.
        /// </summary>
        /// <param name="caller">The parent object (must implement IPrizeRequester) that this prize form is binded to.</param>
        public CreatePrizeForm(IPrizeRequester caller)
        {
            InitializeComponent();
            clearForm();
            callingForm = caller;
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            string formError = ValidateForm();
            if ("".Equals(formError))
            {
                PrizeModel model = new PrizeModel(
                    placeNameTextBox.Text,
                    placeNumberTextBox.Text,
                    prizeAmountTextBox.Text,
                    prizePercentageTextBox.Text);

                GlobalConfig.Connection.CreatePrize(model);

                // Sends back the created prize to the parent form.
                callingForm.PrizeComplete(model);

                // Close the form once created prize.
                this.Close();
            }
            else
            {
                MessageBox.Show(formError);
            }
        }

        private string ValidateForm()
        {
            bool placeNumberValidNumber = int.TryParse(placeNumberTextBox.Text, out int placeNumber);

            if (!placeNumberValidNumber)
            {
                return "The place number must be an integer value.";
            }

            if (placeNumber < 1)
            {
                return "The place number must not be smaller than 1.";
            }

            if (placeNameTextBox.Text.Length == 0)
            {
                return "The place name must not be empty.";
            }

            bool prizeAmountValid = decimal.TryParse(prizeAmountTextBox.Text, out decimal prizeAmount);
            bool prizePercentageValid = double.TryParse(prizePercentageTextBox.Text, out double prizePercentage);

            if (!prizeAmountValid)
            {
                return "The prize amount must be a number.";
            }

            if (!prizePercentageValid)
            {
                return "The prize percentage must be a number.";
            }

            if (prizeAmount <= 0 && prizePercentage <= 0)
            {
                return "Either prize amount or prize percentage must be greater than zero.";
            }

            if (prizeAmount > 0 && prizePercentage > 0)
            {
                return "Either prize amount or prize percentage must be zero.";
            }

            if (prizePercentage < 0 || prizePercentage > 100)
            {
                return "The prize percentage must fall between 0 and 100";
            }
            return "";
        }

        private void clearForm()
        {
            placeNameTextBox.Text = "";
            placeNumberTextBox.Text = "";
            prizeAmountTextBox.Text = "0";
            prizePercentageTextBox.Text = "0";
        }
    }
}
