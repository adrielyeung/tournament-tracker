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
    public partial class TournamentDashboardForm : Form
    {
        List<TournamentModel> tournaments;
        public TournamentDashboardForm()
        {
            InitializeComponent();

            WireUpLists();
        }

        private void WireUpLists()
        {
            loadTournamentDropDown.DataSource = null;
            tournaments = GlobalConfig.Connection.GetTournaments_All();
            loadTournamentDropDown.DataSource = tournaments;
            loadTournamentDropDown.DisplayMember = "TournamentName";
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            CreateTournamentForm createTournamentForm = new CreateTournamentForm();
            createTournamentForm.OnTournamentCreated += CreateTournamentForm_OnTournamentCreated;
            createTournamentForm.OnTournamentViewerClosed += TournamentViewerForm_OnTournamentViewerClosed;
            createTournamentForm.Show();
        }

        private void CreateTournamentForm_OnTournamentCreated(object sender, DateTime e)
        {
            WireUpLists();
        }

        private void loadTournamentButton_Click(object sender, EventArgs e)
        {
            TournamentModel tm = (TournamentModel) loadTournamentDropDown.SelectedItem;
            if (tm != null)
            {
                TournamentViewerForm tournamentViewerForm = new TournamentViewerForm(tm);
                tournamentViewerForm.OnTournamentViewerClosed += TournamentViewerForm_OnTournamentViewerClosed;
                tournamentViewerForm.Show();
            }
        }

        private void TournamentViewerForm_OnTournamentViewerClosed(object sender, DateTime e)
        {
            WireUpLists();
        }
    }
}
