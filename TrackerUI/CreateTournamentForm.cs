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
    public partial class CreateTournamentForm : Form, IPrizeRequester, ITeamRequester
    {
        List<TeamModel> AvaliableTeams = GlobalConfig.Connection.GetTeam_All();
        List<TeamModel> SelectedTeams = new List<TeamModel>();
        List<PriceModel> Selectedprizes = new List<PriceModel>();

        public CreateTournamentForm()
        {
            InitializeComponent();
            InitilizeTeamsList();
        }

        private void InitilizeTeamsList()
        {
            SelectTeamDropDown.DataSource = null;
            SelectTeamDropDown.DataSource = AvaliableTeams;
            SelectTeamDropDown.DisplayMember = "TeamName";

            TournamentTeamslistBox.DataSource = null;
            TournamentTeamslistBox.DataSource = SelectedTeams;
            TournamentTeamslistBox.DisplayMember = "TeamName";

            PrizeslistBox.DataSource = null;
            PrizeslistBox.DataSource = Selectedprizes;
            PrizeslistBox.DisplayMember = "PlaceName";
        }

        private void AddTeambutton_Click(object sender, EventArgs e)
        {
            TeamModel t = (TeamModel)SelectTeamDropDown.SelectedItem;

            if (t != null)
            {
                AvaliableTeams.Remove(t);
                SelectedTeams.Add(t);
                InitilizeTeamsList();
            }
        }

        private void CreatePrizebutton_Click(object sender, EventArgs e)
        {
            // Call Create prize form
            CreatePriceForm frm = new CreatePriceForm(this);
            frm.Show();
        }

        public void PrizeComplete(PriceModel model) // Get back from the form a price model
        {
            // Take price model and put it to list of selected prizes
            Selectedprizes.Add(model);
            InitilizeTeamsList();
        }

        private void CreateNewTeamlinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CreateTeamForm frm = new CreateTeamForm(this);
            frm.Show();
        }

        public void TeamComplete(TeamModel model)
        {
            SelectedTeams.Add(model);
            InitilizeTeamsList();
        }

        private void RemovePlayerbutton_Click(object sender, EventArgs e)
        {
            TeamModel t = (TeamModel)TournamentTeamslistBox.SelectedItem;

            if (t != null)
            {
                SelectedTeams.Remove(t);
                AvaliableTeams.Add(t);
                InitilizeTeamsList();
            }
        }

        private void RemovePrizebutton_Click(object sender, EventArgs e)
        {
            PriceModel p = (PriceModel)PrizeslistBox.SelectedItem;

            if (p != null)
            {
                Selectedprizes.Remove(p);
                InitilizeTeamsList();
            }
        }

        private void CreateTournamentbutton_Click(object sender, EventArgs e)
        {
            //Validate Data
            decimal fee = 0;
            bool ISFeesOK = decimal.TryParse(EntryFeeValue.Text, out fee);
            if (!ISFeesOK)
            {
                MessageBox.Show("You need to enter valid entry fee", "Invalid fee", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //Create Tournament model
            TournamentModel tm = new TournamentModel();
            tm.TournamentName = TournamentNameValue.Text;
            tm.EntryFee = fee;
            tm.Prices = Selectedprizes;
            tm.EnteredTeams = SelectedTeams;

            // Create Matchups
            TournamentLogic.CreateRounds(tm);
            
            // Create Tournament entry
            // Create all of the prize entries
            // Create all team entries
            GlobalConfig.Connection.CreateTournaments(tm);

        }
    }
}
