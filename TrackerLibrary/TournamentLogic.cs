using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerLibrary
{
    public static class TournamentLogic
    {
        public static void CreateRounds(TournamentModel model)
        {
            // Order our list randomly of teams
            // Check if it is big enough - if not, add in byes 2n Teams Find
            List<TeamModel> randomizeTeams = RandomizeTeamOrder(model.EnteredTeams);
            int rounds = FindNoofRounds(randomizeTeams.Count);
            int byes = FindNoofByes(rounds, randomizeTeams.Count);
            
            // Create First round matchups
            model.Rounds.Add(CreateFirstRound(byes, randomizeTeams));
            // Create every round after that - 8/4/2/1 matchup
            CreateOtherRounds(model, rounds);
        }

        public static void UpdateTournamentResults(TournamentModel model)
        {
            int startingRound = model.CheckCurrentRound();
            List<MatchupModel> toScore = new List<MatchupModel>();
            foreach (List<MatchupModel> round in model.Rounds)
            {
                foreach (MatchupModel rm in round)
                {
                    //For Bye Round matchup
                    if (rm.Winner == null && rm.Entries.Any(x => x.Score != 0) || rm.Entries.Count == 1)
                    {
                        toScore.Add(rm);
                    }
                }
            }
            //Checking winners
            MarkWinnerinMatchups(toScore);
            //Update winners to next round
            AdvanceWinners(toScore, model);
            //Update Matchup to database
            toScore.ForEach(x => GlobalConfig.Connection.UpdateMatchup(x));
            
            int endingRound = model.CheckCurrentRound();
            if (endingRound > startingRound)
            {
                model.AlertUsersonRounds();
            }
            
        }

        public static void AlertUsersonRounds(this TournamentModel model)
        {
            int currentRoundNo = model.CheckCurrentRound();
            List<MatchupModel> currentRound =
                model.Rounds.Where(x => x.First().MatchupRound == currentRoundNo).First();
            foreach (MatchupModel matchup in currentRound)
            {
                foreach (MatchupEntryModel me in matchup.Entries)
                {
                    foreach (PersonModel p in me.TeamCompeting.TeamMembers)
                    {
                        AlertPersontoRounds(p, me.TeamCompeting.TeamName, matchup.Entries.Where(x => x.TeamCompeting != me.TeamCompeting).FirstOrDefault());
                    }
                }        
            }
        }

        private static void AlertPersontoRounds(PersonModel p, string teamName, MatchupEntryModel competitor)
        {
            if (p.EmailAddress.Length == 0) { return; }
            string to = "";
            string subject = "";
            StringBuilder body = new StringBuilder();
            
            if (competitor != null)
            {
                subject = $"You have a new matchup with {competitor.TeamCompeting.TeamName}";
                body.AppendLine("<h1>You have a new matchup</h1>");
                body.AppendLine("<strong>Competitor: </strong>");
                body.AppendLine(competitor.TeamCompeting.TeamName);
                body.AppendLine();
                body.AppendLine();
                body.AppendLine("Have a great match!");
                body.AppendLine("~ Tournament Tracker");
            }
            else
            {
                subject = "You have a bye week this round";
                body.AppendLine("Enjoy your round off!");
                body.AppendLine("~ Tournament Tracker");
            }
            
            to = p.EmailAddress;
            
            EmailLogic.SendEmail(to, subject, body.ToString());
        }


        private static int CheckCurrentRound(this TournamentModel model)
        {
            int output = 1;

            foreach (List<MatchupModel> round in model.Rounds)
            {
                if (round.All(x => x.Winner != null)) { output += 1; }
                else { return output; }
            }

            //Tournament is complete
            CompleteTournament(model);
            return output - 1;
        }

        private static void CompleteTournament(TournamentModel model)
        {
            //Push data using connector
            GlobalConfig.Connection.CompleteTournaments(model);
            TeamModel Winners = model.Rounds.Last().First().Winner;
            TeamModel RunnerUp = model.Rounds.Last().First().Entries.Where(x => x.TeamCompeting != Winners).First().TeamCompeting;
            decimal winnerPrize = 0;
            decimal runnerupPrize = 0;
            
            if (model.Prices.Count > 0)
            {
                decimal totalIncome = model.EnteredTeams.Count * model.EntryFee;
                PriceModel price1st = model.Prices.Where(x => x.PlaceNumber == 1).FirstOrDefault();
                PriceModel price2nd = model.Prices.Where(x => x.PlaceNumber == 2).FirstOrDefault();
                if (price1st != null) { winnerPrize = price1st.CalculatePricePayout(totalIncome); }
                if (price2nd != null) { runnerupPrize = price2nd.CalculatePricePayout(totalIncome); }
            }
            
            //Send Email to all tournament
            string subject = $"In {model.TournamentName}, {Winners.TeamName} has won";
            StringBuilder body = new StringBuilder();
            body.AppendLine("<h1>You have a new WINNER!</h1>");
            body.AppendLine("<p>Congrats to winner on a great tournamnets.</p>");
            body.AppendLine("<br/>");
            if (winnerPrize > 0)
            {
                body.AppendLine($"<p>{Winners.TeamName} will receive ${winnerPrize}.</p>");
            }
            if (runnerupPrize > 0)
            {
                body.AppendLine($"<p>{RunnerUp.TeamName} will receive ${runnerupPrize}.</p>");
            }
            body.AppendLine("<p>Thanks for great tournament</p>");
            body.AppendLine("~ Tournament Tracker");

            List<string> bcc = new List<string>();
            foreach (TeamModel t in model.EnteredTeams)
            {
                foreach (PersonModel p in t.TeamMembers)
                {
                    if (p.EmailAddress.Length > 0)
                    {
                        bcc.Add(p.EmailAddress);
                    }
                }
            }
            EmailLogic.SendEmail(new List<string>(), bcc, subject, body.ToString());
            
            //Complete
            model.CompleteTournament();
        }

        private static decimal CalculatePricePayout(this PriceModel p, decimal income)
        {
            decimal output = 0;
            if (p.PriceAmount > 0) { output = p.PriceAmount; }
            else { output = decimal.Multiply(income, Convert.ToDecimal(p.PricePercentage / 100)); }
            return output;
        }
        private static void MarkWinnerinMatchups(List<MatchupModel> models)
        {
            string greaterWins = ConfigurationManager.AppSettings.Get("greaterWins");
            foreach (MatchupModel m in models)
            {
                // For Bye week entry
                if (m.Entries.Count == 1)
                {
                    m.Winner = m.Entries[0].TeamCompeting; continue; 
                }
                //greater wins - 1 or lesser wins - 0
                if (greaterWins == "0")
                {
                    if (m.Entries[0].Score < m.Entries[1].Score)
                    {
                        m.Winner = m.Entries[0].TeamCompeting;
                    }
                    else if(m.Entries[1].Score < m.Entries[0].Score)
                    {
                        m.Winner = m.Entries[1].TeamCompeting;
                    }
                    else
                    {
                        throw new Exception("We dont allow ties in application");
                    }
                }
                else
                {
                    if (m.Entries[0].Score > m.Entries[1].Score)
                    {
                        m.Winner = m.Entries[0].TeamCompeting;
                    }
                    else if(m.Entries[1].Score > m.Entries[0].Score)
                    {
                        m.Winner = m.Entries[1].TeamCompeting;
                    }
                    else
                    {
                        throw new Exception("We dont allow ties in application");
                    }
                }
            }
        }

        private static void AdvanceWinners(List<MatchupModel> models, TournamentModel tournaments)
        {
            foreach (MatchupModel m in models)
            {
                foreach (List<MatchupModel> round in tournaments.Rounds)
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
            }
            
        }
        
        private static void CreateOtherRounds(TournamentModel model, int rounds)
        {
            int cRound = 2;
            // First round matchups
            List<MatchupModel> previousRound = model.Rounds[0];
            List<MatchupModel> CurrRound = new List<MatchupModel>();
            MatchupModel currMatchup = new MatchupModel();

            while (cRound <= rounds)
            {
                foreach (MatchupModel match in previousRound)
                {
                    currMatchup.Entries.Add(new MatchupEntryModel{ParentMatchup = match} );

                    if (currMatchup.Entries.Count > 1)
                    {
                        currMatchup.MatchupRound = cRound;
                        CurrRound.Add(currMatchup);
                        currMatchup = new MatchupModel();
                    }
                }
                model.Rounds.Add(CurrRound);
                previousRound = CurrRound;
                CurrRound = new List<MatchupModel>();
                cRound++;
            }
        }
        
        private static List<MatchupModel> CreateFirstRound(int byes, List<TeamModel> teams)
        {
            List<MatchupModel> output = new List<MatchupModel>();
            MatchupModel curr = new MatchupModel();
            
            foreach (TeamModel team in teams)
            {
                curr.Entries.Add(new  MatchupEntryModel{ TeamCompeting = team } );

                if (byes > 0 || curr.Entries.Count > 1)
                {
                    curr.MatchupRound = 1;
                    output.Add(curr);
                    curr = new MatchupModel();
                    if (byes>0) byes--;
                }
            }
            
            return  output;
        }

        private static int FindNoofByes(int rounds, int teamcount)
        {
            int output = 0;
            int totalTeams = 1;

            for (int i = 1; i <= rounds; i++)
            {
                totalTeams *= 2;
            }

            output = totalTeams - teamcount;
            return output;
        }
        
        private static int FindNoofRounds(int teamcount)
        {
            int output = 1;
            int val = 2;

            while (val < teamcount)
            {
                output++;
                val *= 2;
            }

            return output;
        }
        
        private static List<TeamModel> RandomizeTeamOrder(List<TeamModel> teams)
        {
            return teams.OrderBy(x => Guid.NewGuid()).ToList();
        }
        
    }
}