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
    public partial class CreateTournamentForm : Form, IPrizeRequester, ITeamRequester
    {
        public event EventHandler<DateTime> OnTournamentCreated;
        public event EventHandler<DateTime> OnTournamentViewerClosed;
        List<TeamModel> availableTeams = GlobalConfig.Connection.GetTeam_All();
        List<TeamModel> selectedTeams = new List<TeamModel>();
        List<PrizeModel> selectedPrizes = new List<PrizeModel>();

        public CreateTournamentForm()
        {
            InitializeComponent();
            WireUpLists();
        }

        private void WireUpLists()
        {
            selectTeamDropDown.DataSource = null;
            selectTeamDropDown.DataSource = availableTeams;
            selectTeamDropDown.DisplayMember = "TeamName";

            tournamentTeamsListBox.DataSource = null;
            tournamentTeamsListBox.DataSource = selectedTeams;
            tournamentTeamsListBox.DisplayMember = "TeamName";

            prizesListBox.DataSource = null;
            prizesListBox.DataSource = selectedPrizes;
            prizesListBox.DisplayMember = "PlaceName";
        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel t = (TeamModel)selectTeamDropDown.SelectedItem;

            if (t != null)
            {
                // Remove selected team from available team list, add to selected team list
                availableTeams.Remove(t);
                selectedTeams.Add(t);

                WireUpLists();
            }
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            // Call the CreatePrizeForm (isolated from this form - should not know its existence)
            // This is achieved by implementing the IPrizeRequester, which every form creating a prize will bind to the CreatePrize form using this interface.
            CreatePrizeForm frm = new CreatePrizeForm(this);
            frm.Show();
        }

        public void PrizeComplete(PrizeModel model)
        {
            // Get back from the form a PrizeModel
            // Take PrizeModel and put into List of selected prizes
            selectedPrizes.Add(model);
            WireUpLists();
        }

        public void TeamComplete(TeamModel model)
        {
            selectedTeams.Add(model);
            WireUpLists();
        }

        private void createNewTeamLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CreateTeamForm frm = new CreateTeamForm(this);
            frm.Show();
        }

        private void removeSelectedTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel t = (TeamModel)tournamentTeamsListBox.SelectedItem;

            if (t != null)
            {
                // Remove selected member from selected member list, add to available member list
                selectedTeams.Remove(t);
                availableTeams.Add(t);

                WireUpLists();
            }
        }

        private void removeSelectedPrizeButton_Click(object sender, EventArgs e)
        {
            PrizeModel p = (PrizeModel)prizesListBox.SelectedItem;

            if (p != null)
            {
                // Remove selected member from selected member list, add to available member list
                selectedPrizes.Remove(p);

                WireUpLists();
            }
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            // Validate tournament name
            if (tournamentNameTextBox.TextLength == 0)
            {
                MessageBox.Show("You need to enter a Tournament Name.",
                    "No Tournament Name",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                // Stop creating tournament process
                return;
            }

            // Validate if entry fee is decimal
            // Try catch the parsing process, do not want the application to crash due to user wrong input
            bool feeAcceptable = decimal.TryParse(entryValueTextBox.Text, out decimal fee);

            if (!feeAcceptable)
            {
                MessageBox.Show("You need to enter a valid entry fee.",
                    "Invalid fee",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                // Stop creating tournament process
                return;
            }

            // Validate if less than 2 teams entered
            if (tournamentTeamsListBox.Items.Count <= 1)
            {
                MessageBox.Show("You need to enter at least 2 teams.",
                    "Invalid number of teams",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                // Stop creating tournament process
                return;
            }

            // Create Tournament model
            TournamentModel tm = new TournamentModel();

            tm.TournamentName = tournamentNameTextBox.Text;
            tm.EntryFee = fee;

            tm.Prizes = selectedPrizes;
            tm.EnteredTeams = selectedTeams;

            // Use a static class to perform the logic, do not store data in the class -> static
            TournamentLogic.CreateRounds(tm);

            // Create Tournament entry
            // Create all prizes entries
            // Create all teams entries
            GlobalConfig.Connection.CreateTournament(tm);

            // Alert all players to the tournament matchups
            tm.AlertUsersToNewRound();

            TournamentViewerForm tournamentViewerForm = new TournamentViewerForm(tm);
            tournamentViewerForm.OnTournamentViewerClosed += TournamentViewerForm_OnTournamentViewerClosed;
            tournamentViewerForm.Show();
            this.Close();
        }

        private void TournamentViewerForm_OnTournamentViewerClosed(object sender, DateTime e)
        {
            OnTournamentViewerClosed?.Invoke(this, DateTime.Now);
        }

        private void CreateTournamentForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnTournamentCreated?.Invoke(this, DateTime.Now);
        }
    }
}
