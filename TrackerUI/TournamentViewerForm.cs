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
        private TournamentModel tournament;
        BindingList<int> rounds = new BindingList<int>();
        BindingList<MatchupModel> SelectedMatchups = new BindingList<MatchupModel>();
        
        public TournamentViewerForm(TournamentModel tournamentModel)
        {
            InitializeComponent(); 
            tournament = tournamentModel;
            LoadFormData();
            LoadRounds();
            WireupLists();
        }

        private void LoadFormData()
        {
            TournamentName.Text = tournament.TournamentName;
        }

        private void WireupLists()
        {
            RoundDropDown.DataSource = rounds;
            MatchupListBox.DataSource = SelectedMatchups;
            MatchupListBox.DisplayMember = "DisplayName";
        }

        private void LoadRounds()
        {
            rounds.Clear();
            rounds.Add(1);
            int currRound = 1;

            foreach (List<MatchupModel> matchups in tournament.Rounds)
            {
                if (matchups.First().MatchupRound > currRound)
                {
                    currRound = matchups.First().MatchupRound;
                    rounds.Add(currRound);
                }
            }
            LoadMatchups(1);
        }
        
        private void RoundDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchups((int) RoundDropDown.SelectedItem);
        }

        private void LoadMatchups(int round)
        {
            foreach (List<MatchupModel> matchups in tournament.Rounds)
            {
                if (matchups.First().MatchupRound == round)
                {
                    SelectedMatchups.Clear();
                    foreach (MatchupModel m in matchups)
                    {
                        if (m.Winner == null || !UnplayedOnlycheckBox.Checked)
                        {
                            SelectedMatchups.Add(m);
                        }
                        
                    }
                }
            }

            if (SelectedMatchups.Count > 0)
            {
                LoadMatchup(SelectedMatchups.First());    
            }
            DisplayMatchupInfo();
        }

        private void DisplayMatchupInfo()
        {
            bool IsVisible = SelectedMatchups.Count > 0;
            TeamOneName.Visible = IsVisible;
            TeamOneScoreLabel.Visible = IsVisible;
            TeamOneScoreValue.Visible = IsVisible;
            
            TeamTwoName.Visible = IsVisible;
            TeamTwoScoreLabel.Visible = IsVisible;
            TeamTwoScoreValue.Visible = IsVisible;

            Vslabel.Visible = IsVisible;
            Scorebutton.Visible = IsVisible;
        }
        
        private void MatchupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
           LoadMatchup((MatchupModel) MatchupListBox.SelectedItem);
        }

        private void LoadMatchup(MatchupModel m)
        {
            for (int i = 0; i < m.Entries.Count; i++)
            {
                // TODO - Refactor this single method
                if (i == 0)
                {
                    if (m.Entries[0].TeamCompeting != null)
                    {
                        TeamOneName.Text = m.Entries[0].TeamCompeting.TeamName;
                        TeamOneScoreValue.Text = m.Entries[0].Score.ToString();
                        TeamTwoName.Text = "<bye>";
                        TeamTwoScoreValue.Text = "0"; 
                    }
                    else
                    {
                        TeamOneName.Text = "Not Yet Set";
                        TeamOneScoreValue.Text = "";
                    }
                }
                if (i == 1)
                {
                    if (m.Entries[1].TeamCompeting != null)
                    {
                        TeamTwoName.Text = m.Entries[1].TeamCompeting.TeamName;
                        TeamTwoScoreValue.Text = m.Entries[1].Score.ToString();
                    }
                    else
                    {
                        TeamTwoName.Text = "Not Yet Set";
                        TeamTwoScoreValue.Text = "";
                    }
                }
            }
        }
        
        private void UnplayedOnlycheckBox_CheckedChanged(object sender, EventArgs e)
        {
             LoadMatchups((int) RoundDropDown.SelectedItem);
        }
        
        private void Scorebutton_Click(object sender, EventArgs e)
        {
            if(ValidateScore())
            {  MessageBox.Show("Please Enter a valid Score"); return; }
            
            MatchupModel m = (MatchupModel) MatchupListBox.SelectedItem;
            double teamOnescore = 0;
            double teamTwoscore = 0;
            
            for (int i = 0; i < m.Entries.Count; i++)
            {
                // TODO - Refactor this single method
                if (i == 0)
                {
                    if (m.Entries[0].TeamCompeting != null)
                    {
                        teamOnescore = double.Parse(TeamOneScoreValue.Text);
                        m.Entries[0].Score = teamOnescore;
                    }
                }
                if (i == 1)
                {
                    if (m.Entries[1].TeamCompeting != null)
                    {
                        teamTwoscore = double.Parse(TeamTwoScoreValue.Text);
                        m.Entries[1].Score = teamTwoscore;
                    }
                }
            }

            if (teamOnescore > teamTwoscore)
            {
                //Team One Wins
                m.Winner = m.Entries[0].TeamCompeting;
            }
            else if(teamTwoscore > teamOnescore)
            {
                m.Winner = m.Entries[1].TeamCompeting;
            }
            else
            {
                MessageBox.Show("I dont handle tie games");
            }

            foreach (List<MatchupModel> round in tournament.Rounds)
            {
                foreach (MatchupModel rm in round)
                {
                    foreach (MatchupEntryModel me in rm.Entries)
                    {
                        if (me.ParentMatchup != null)
                        {
                            if (me.ParentMatchup.Id == m.Id)
                            { 
                                //For Updating Parent Id
                                me.TeamCompeting = m.Winner;
                                GlobalConfig.Connection.UpdateMatchup(rm);
                            }
                        }
                    }
                }
            }
            LoadMatchups((int) RoundDropDown.SelectedItem); 
            //Update Matchup to database
            GlobalConfig.Connection.UpdateMatchup(m);
        }

        private bool ValidateScore()
        {
            bool ScoreValid1 = double.TryParse(TeamOneScoreValue.Text, out double Scoreval1);
            bool ScoreValid2 = double.TryParse(TeamTwoScoreValue.Text, out double Scoreval2);
            if (ScoreValid1 || ScoreValid2)
            {
                return false;
            }

            return true;
        }
    }
}
