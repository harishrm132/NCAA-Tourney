using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
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

            MarkWinnerinMatchups(toScore);
            AdvanceWinners(toScore, model);

            //Update Matchup to database
            toScore.ForEach(x => GlobalConfig.Connection.UpdateMatchup(x));
            
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