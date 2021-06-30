using System;
using System.Collections.Generic;
using System.Linq;
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