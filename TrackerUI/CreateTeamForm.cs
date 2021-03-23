using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreateTeamForm : Form
    {
        ITeamRequester callingForm;
        private List<PersonModel> availableTeamMembers = GlobalConfig.Connection.GetPerson_All();
        private List<PersonModel> selectedTeamMembers = new List<PersonModel>();
        
        /// <summary>
        /// Creates a Team Form for inputting a team.
        /// </summary>
        /// <param name="caller">The parent object (must implement IPrizeRequester) that this prize form is binded to.</param>
        public CreateTeamForm(ITeamRequester caller)
        {
            InitializeComponent();
            ClearMemberForm();
            callingForm = caller;
            // CreateSampleData();
            WireUpLists();
        }

        private void CreateSampleData()
        {
            availableTeamMembers.Add(new PersonModel { FirstName = "Adriel", LastName = "Yeung" });
            selectedTeamMembers.Add(new PersonModel { FirstName = "Hello", LastName = "World" });
        }

        private void WireUpLists()
        {
            // Need to set to null first in order to refresh the list items
            selectTeamMemberDropDown.DataSource = null;
            selectTeamMemberDropDown.DataSource = availableTeamMembers;
            selectTeamMemberDropDown.DisplayMember = "FullName";

            teamMembersListBox.DataSource = null;
            teamMembersListBox.DataSource = selectedTeamMembers;
            teamMembersListBox.DisplayMember = "FullName";
        }

        private void createMemberButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                PersonModel p = new PersonModel();

                p.FirstName = firstNameTextBox.Text;
                p.LastName = lastNameTextBox.Text;
                p.EmailAddress = emailTextBox.Text;
                p.CellphoneNumber = cellPhoneTextBox.Text;

                GlobalConfig.Connection.CreatePerson(p);

                selectedTeamMembers.Add(p);

                WireUpLists();

                ClearMemberForm();
            }
            else
            {
                MessageBox.Show("You need to fill in all of the fields.");
            }
        }

        private bool ValidateForm()
        {
            if (firstNameTextBox.Text.Length == 0)
            {
                return false;
            }

            if (lastNameTextBox.Text.Length == 0)
            {
                return false;
            }

            if (emailTextBox.Text.Length == 0)
            {
                return false;
            }

            if (cellPhoneTextBox.Text.Length == 0)
            {
                return false;
            }
            return true;
        }

        private void ClearMemberForm()
        {
            firstNameTextBox.Text = "";
            lastNameTextBox.Text = "";
            emailTextBox.Text = "";
            cellPhoneTextBox.Text = "";
        }

        private void addMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel)selectTeamMemberDropDown.SelectedItem;

            if (p != null)
            {
                // Remove selected member from available member list, add to selected member list
                availableTeamMembers.Remove(p);
                selectedTeamMembers.Add(p);

                WireUpLists();
            }
        }

        private void removeSelectedMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel)teamMembersListBox.SelectedItem;

            if (p != null)
            {
                // Remove selected member from selected member list, add to available member list
                selectedTeamMembers.Remove(p);
                availableTeamMembers.Add(p);

                WireUpLists();
            }
        }

        private void createTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel t = new TeamModel();

            t.TeamName = teamNameTextBox.Text;
            t.TeamMembers = selectedTeamMembers;

            GlobalConfig.Connection.CreateTeam(t);

            // Sends back the created prize to the parent form.
            callingForm.TeamComplete(t);

            // Close the form once created team.
            this.Close();
        }
    }
}
