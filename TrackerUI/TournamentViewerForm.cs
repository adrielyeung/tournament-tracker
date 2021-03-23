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

namespace TrackerUI
{
    public partial class TournamentViewerForm : Form
    {
        public event EventHandler<DateTime> OnTournamentViewerClosed;
        private TournamentModel tournament;
        List<int> rounds = new List<int>();
        List<MatchupModel> selectedMatchups = new List<MatchupModel>();
        public TournamentViewerForm(TournamentModel tournamentModel)
        {
            InitializeComponent();

            // Store the tournament into an attribute, which can be accessed by other objects on the form
            if (tournamentModel != null)
            {
                tournament = tournamentModel;
                tournament.OnTournamentComplete += Tournament_OnTournamentComplete;

                LoadFormData();
                LoadRounds();
            }
        }

        private void Tournament_OnTournamentComplete(object sender, DateTime e)
        {
            this.Close();
        }

        private void LoadFormData()
        {
            tournamentName.Text = tournament.TournamentName;
        }

        private void LoadRounds()
        {
            // Re-initialise rounds list every time it is run, to avoid duplications if run multiple times
            rounds = new List<int>();
            rounds.Add(1);
            int currRound = 1;

            foreach (List<MatchupModel> matchups in tournament.Rounds)
            {
                // Moved to next round, set to current round and add to rounds list
                if (matchups.First().MatchupRound > currRound)
                {
                    currRound = matchups.First().MatchupRound;
                    rounds.Add(currRound);
                    
                }
            }
            WireUpRoundsLists(); 
        }

        private void WireUpRoundsLists()
        {
            roundDropDown.DataSource = null;
            roundDropDown.DataSource = rounds;
        }

        private void WireUpMatchupsLists() {
            matchupListBox.DataSource = null;
            matchupListBox.DataSource = selectedMatchups;
            matchupListBox.DisplayMember = "DisplayName";
        }

        private void LoadMatchups()
        {
            int round = (int)roundDropDown.SelectedItem;

            foreach (List<MatchupModel> matchups in tournament.Rounds)
            {
                // Find all matchups related to selected round
                if (matchups.First().MatchupRound == round)
                {
                    if (unplayedOnlyCheckBox.Checked)
                    {
                        // Get all matchups with no winner (unplayed)
                        selectedMatchups = matchups.Where(x => x.Winner == null).ToList();
                    }
                    else
                    {
                        // Get all matchups into selected matchup list
                        selectedMatchups = matchups;
                    }
                    
                }
            }
            WireUpMatchupsLists();
            DisplayMatchupInfo();
        }

        private void LoadMatchup()
        {
            MatchupModel m = (MatchupModel)matchupListBox.SelectedItem;
            
            // Default no scoring enabled, only if 2 matching entries with team competing are found, enable scoring
            scoreButton.Enabled = false;
            teamOneScoreTextBox.Enabled = false;
            teamTwoScoreTextBox.Enabled = false;

            if (m != null)
            {
                // Use variable i to check if it is the first team called or second
                for (int i = 0; i < m.Entries.Count; i++)
                {
                    if (i == 0)
                    {
                        if (m.Entries[0].TeamCompeting != null)
                        {
                            teamOneName.Text = m.Entries[0].TeamCompeting.TeamName;
                            teamOneScoreTextBox.Text = m.Entries[0].Score.ToString();
                            
                            // Default the other matchup entry is 'bye', and disable scoring
                            // If the other matchup entry is found (i = 1), then enable scoring
                            teamTwoName.Text = "<Bye>";
                            teamTwoScoreTextBox.Text = "";
                        }
                        else
                        {
                            teamOneName.Text = "Not yet set";
                            teamOneScoreTextBox.Text = "";
                        }
                    }
                    else if (i == 1)
                    {
                        if (m.Entries[1].TeamCompeting != null)
                        {
                            teamTwoName.Text = m.Entries[1].TeamCompeting.TeamName;
                            teamTwoScoreTextBox.Text = m.Entries[1].Score.ToString();
                            teamTwoScoreTextBox.Enabled = true;
                            teamOneScoreTextBox.Enabled = true;
                            scoreButton.Enabled = true;
                        }
                        else
                        {
                            teamTwoName.Text = "Not yet set";
                            teamTwoScoreTextBox.Text = "";
                        }
                    }
                }
            }
            else
            {
                teamOneName.Text = "Not yet set";
                teamOneScoreTextBox.Text = "";
                teamTwoName.Text = "Not yet set";
                teamTwoScoreTextBox.Text = "";
            }
        }

        private void DisplayMatchupInfo()
        {
            bool isVisible = (selectedMatchups.Count > 0);

            teamOneName.Visible = isVisible;
            teamOneScoreLabel.Visible = isVisible;
            teamOneScoreTextBox.Visible = isVisible;
            teamTwoName.Visible = isVisible;
            teamTwoScoreLabel.Visible = isVisible;
            teamTwoScoreTextBox.Visible = isVisible;
            versusLabel.Visible = isVisible;
            scoreButton.Visible = isVisible;
        }

        /// <summary>
        /// Updates whenever the round drop down value is changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void roundDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchups();
        }

        private void matchupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchup();
        }

        private void unplayedOnlyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            LoadMatchups();
        }

        private void scoreButton_Click(object sender, EventArgs e)
        {
            // Error handling - Validate user input data before error occurs
            string errorMsg = ValidateData();
            if (errorMsg != null && errorMsg.Length > 0)
            {
                MessageBox.Show($"Input Error: { errorMsg }");
                return;
            }

            MatchupModel m = (MatchupModel)matchupListBox.SelectedItem;

            // Use variable i to check if it is the first team called or second
            for (int i = 0; i < m.Entries.Count; i++)
            {
                if (i == 0)
                {
                    if (m.Entries[0].TeamCompeting != null)
                    {
                        teamOneName.Text = m.Entries[0].TeamCompeting.TeamName;
                        bool scoreValid = double.TryParse(teamOneScoreTextBox.Text, out double teamOneScore);
                        if (scoreValid)
                        {
                            m.Entries[0].Score = teamOneScore;
                        }
                        else
                        {
                            MessageBox.Show("Please enter a valid score for team 1.");
                            return;
                        }
                    }
                }
                else if (i == 1)
                {
                    teamTwoName.Text = m.Entries[1].TeamCompeting.TeamName;
                    bool scoreValid = double.TryParse(teamTwoScoreTextBox.Text, out double teamTwoScore);
                    if (scoreValid)
                    {
                        m.Entries[1].Score = teamTwoScore;
                    }
                    else
                    {
                        MessageBox.Show("Please enter a valid score for team 2.");
                        return;
                    }
                }
            }

            // Catch exception and put msg in MessageBox
            try
            {
                TournamentLogic.UpdateTournamentResults(tournament);
            } catch (NotSupportedException ex)
            {
                MessageBox.Show($"The application had the following error: { ex.Message }");
            }
            
            // Refresh list of matchups (unplayed / played) after scoring
            LoadMatchups();
        }

        private string ValidateData()
        {
            bool scoreOneValid = double.TryParse(teamOneScoreTextBox.Text, out double teamOneScore);
            bool scoreTwoValid = double.TryParse(teamTwoScoreTextBox.Text, out double teamTwoScore);

            if (!scoreOneValid)
            {
                return "Please enter a valid number for team 1 (upper team) score.";
            }

            if (!scoreTwoValid)
            {
                return "Please enter a valid number for team 2 (lower team) score.";
            }

            if (teamOneScore == 0 && teamTwoScore == 0)
            {
                return "Please enter a non-zero score for at least 1 team";
            }

            if (teamOneScore == teamTwoScore)
            {
                return "Ties are not handled in this application.";
            }

            return "";
        }

        private void TournamentViewerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            OnTournamentViewerClosed?.Invoke(this, DateTime.Now);
        }
    }
}
