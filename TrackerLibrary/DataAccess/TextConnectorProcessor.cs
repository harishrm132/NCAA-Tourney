using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess.TextHelpers
{
    public static class TextConnectorProcessor
    {
        public static string FullFilePath(this string filename)
        {
             return $"{ConfigurationManager.AppSettings.Get("filePath")}\\{filename}";
        }

        public static List<string> LoadFile(this string filepath)
        {
            if (!System.IO.File.Exists(filepath))
            {
                return new List<string>();
            }
            return File.ReadAllLines(filepath).ToList();
        }

        public static List<PriceModel> ConverttoPrizeModel(this List<string> lines)
        {
            List<PriceModel> output = new List<PriceModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                PriceModel p = new PriceModel();
                p.Id = int.Parse(cols[0]); 
                p.PlaceNumber = int.Parse(cols[1]); 
                p.PlaceName = cols[2]; 
                p.PriceAmount = decimal.Parse(cols[3]); 
                p.PricePercentage = double.Parse(cols[4]);
                
                output.Add(p);
            }
            return output;
        }

        public static List<PersonModel> ConverttoPersonModel(this List<string> lines)
        {
            List<PersonModel> output = new List<PersonModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                PersonModel p = new PersonModel();
                p.Id = int.Parse(cols[0]);
                p.FirstName = cols[1];
                p.LastName = cols[2];
                p.EmailAdress = cols[3];
                p.CellphoneNumber = cols[4];

                output.Add(p);
            }

            return output;
        }

        public static List<TeamModel> ConverttoTeamModel(this List<string> lines, string personfilename)
        {
            //id, team name, list of ids seperated by pipes
            List<TeamModel> output = new List<TeamModel>();
            List<PersonModel> persons = personfilename.FullFilePath().LoadFile().ConverttoPersonModel();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                TeamModel t = new TeamModel();
                t.Id = int.Parse(cols[0]);
                t.TeamName = cols[1];

                string[] personIds = cols[2].Split('|');

                foreach (string id in personIds)
                {
                    t.TeamMembers.Add(persons.Where(x => x.Id == int.Parse(id)).First());
                }

                output.Add(t);
            }

            return output;
        }

        public static List<TournamentModel> ConverttoTournamentModel(this List<string> lines, string teamfilename, string personsfilename, string pricesfilename)
        {
            //id, Tournament Name, entry fee, (id|id|id - Entered Teams), (id|id - Prices), (Rounds - (id^id^id)|(id^id)|() )
            List<TournamentModel> output = new List<TournamentModel>();
            List<TeamModel> teams = teamfilename.FullFilePath().LoadFile().ConverttoTeamModel(personsfilename);
            List<PriceModel> prices = pricesfilename.FullFilePath().LoadFile().ConverttoPrizeModel();
            List<MatchupModel> matchups = GlobalConfig.MatchupFile.FullFilePath().LoadFile().ConverttoMatchupModel();
            
            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                TournamentModel t = new TournamentModel();
                t.Id = int.Parse(cols[0]);
                t.TournamentName = cols[1];
                t.EntryFee = decimal.Parse(cols[2]);

                string[] teamIds = cols[3].Split('|');
                foreach (string id in teamIds)
                {
                    t.EnteredTeams.Add(teams.Where(x => x.Id == int.Parse(id)).First());
                }

                if(cols[4].Length > 0)
                {
                    string[] pricesIds = cols[4].Split('|');
                    foreach (string id in pricesIds)
                    {
                        t.Prices.Add(prices.Where(x => x.Id == int.Parse(id)).First());
                    }
                }
                
                // capture round information
                string[] rounds = cols[5].Split('|');
                foreach (string round in rounds)
                {
                    string[] msText = round.Split('^');
                    List<MatchupModel> ms = new List<MatchupModel>();
                    foreach (string matchupmodelTextId in msText)
                    {
                        ms.Add(matchups.Where(x => x.Id == int.Parse(matchupmodelTextId)).First());
                    }
                    t.Rounds.Add(ms);
                }
                
                output.Add(t);
            }

            return output;

        }

        public static void SavetoPrizeFile(this List<PriceModel> models, string filename)
        {
            List<string> lines = new List<string>();

            foreach (PriceModel p in models)
            {
                lines.Add($"{p.Id},{p.PlaceNumber},{p.PlaceName},{p.PriceAmount},{p.PricePercentage}");
            }

            File.WriteAllLines(filename.FullFilePath(), lines);
        }

        public static void SavetoPersonFile(this List<PersonModel> models, string filename)
        {
            List<string> lines = new List<string>();

            foreach (PersonModel p in models)
            {
                lines.Add($"{p.Id},{p.FirstName},{p.LastName},{p.EmailAdress},{p.CellphoneNumber}");
            }

            File.WriteAllLines(filename.FullFilePath(), lines);
        }

        public static void SavetoTeamsFile(this List<TeamModel> models, string filename)
        {
            List<string> lines = new List<string>();

            foreach (TeamModel t in models)
            {
                lines.Add($"{t.Id},{t.TeamName},{ConvertPeopleListtosting(t.TeamMembers)}");
            }

            File.WriteAllLines(filename.FullFilePath(), lines);
        }

        public static void  SaveRoundstoFile(this TournamentModel model, string MatchupFile, string MatchupEntryFile)
        {
            //Loop through each round
            //Loop through each matchup
            //Get the id for the new matchup and save the record
            //Loop through each entry, get the id and save it

            foreach (List<MatchupModel> round in model.Rounds)
            {
                foreach (MatchupModel matchup in round)
                {
                    //Load all the matchups from file
                    //Get the top id and add one 
                    //Store the id
                    //Save the matchup record
                    matchup.SaveMatchuptoFile(MatchupFile, MatchupEntryFile);

                }
            }
        }

        public static List<MatchupEntryModel> ConverttoMatchupEntryModel(this List<string> lines)
        {
            List<MatchupEntryModel> output = new List<MatchupEntryModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                MatchupEntryModel me = new MatchupEntryModel();
                me.Id = int.Parse(cols[0]);
                if(cols[1].Length  == 0)
                {
                    me.TeamCompeting = null;
                }
                else
                {
                    me.TeamCompeting = LookUpTeambyId(int.Parse(cols[1]));
                }
                me.Score = Double.Parse(cols[2]);

                int parentId = 0;
                if (int.TryParse(cols[3], out parentId))
                    me.ParentMatchup = LookUpMatchbyId(int.Parse(cols[3]));
                else
                    me.ParentMatchup = null;

                output.Add(me);
            }

            return output;
        }
        
        public static List<MatchupEntryModel> ConvertStringtoMatchupEntryModel(string input)
        {
            string[] ids = input.Split('|');
            List<MatchupEntryModel> output = new List<MatchupEntryModel>();
            List<string> entries = GlobalConfig.MatchupEntryFile.FullFilePath().LoadFile();
            List<string> matchingEntries = new List<string>();

            foreach (string id in ids)
            {
                foreach (string entry in entries)
                {
                    string[] cols = entry.Split(',');

                    if(cols[0] == id)
                    {
                        matchingEntries.Add(entry);
                    }
                }
            }
            output = matchingEntries.ConverttoMatchupEntryModel();
            return output;
        }

        private static TeamModel LookUpTeambyId(int id)
        {
            List<string> teams = GlobalConfig.TeamFile.FullFilePath().LoadFile();

            foreach (string team in teams)
            {
                string[] cols = team.Split(',');
                if(cols[0] == id.ToString())
                {
                    List<string> matchingTeams = new List<string>();    
                    matchingTeams.Add(team);
                    return matchingTeams.ConverttoTeamModel(GlobalConfig.PersonFile).First();
                }
            }
            return null; 
        }
        
        private static MatchupModel LookUpMatchbyId(int id)
        {
            List<string> matchups = GlobalConfig.MatchupFile.FullFilePath().LoadFile();

            foreach (string matchup in matchups)
            {
                string[] cols = matchup.Split(',');
                if (cols[0] == id.ToString())
                {
                    List<string> matchingMatchups = new List<string>();
                    matchingMatchups.Add(matchup);
                    return matchingMatchups.ConverttoMatchupModel().First();
                }
            }
            return null;
        }
        
        public static List<MatchupModel> ConverttoMatchupModel(this List<string> lines)
        {
            //id=0, entries = 1(pipe delemited by id | ), winner = 2, matchupround = 3
            List<MatchupModel> output = new List<MatchupModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                MatchupModel p = new MatchupModel();
                p.Id = int.Parse(cols[0]); 
                p.Entries = ConvertStringtoMatchupEntryModel(cols[1]);

                if(cols[2].Length == 0)
                {
                    p.Winner = null;
                }
                else
                {
                    p.Winner = LookUpTeambyId(int.Parse(cols[2]));  
                }
                
                p.MatchupRound = int.Parse(cols[3]);
                output.Add(p);
            }
            return output;
        }
        
        public static void SaveMatchuptoFile(this MatchupModel model, string MatchupFile, string MatchupEntryFile)
        {
            List<MatchupModel> matchups = GlobalConfig.MatchupFile.FullFilePath().LoadFile().
                ConverttoMatchupModel();
            //Find the Max ID 
            int currentId = 1;
            if (matchups.Count > 0) { currentId = matchups.OrderByDescending(x => x.Id).First().Id + 1; }
            model.Id = currentId;
            matchups.Add(model);

            ////save to file
            List<string> lines = new List<string>();
            //foreach (MatchupModel m in matchups)
            //{
            //    string winner = "";
            //    if (m.Winner != null)
            //    {
            //        winner = m.Winner.Id.ToString();
            //    }
            //    lines.Add($@"{m.Id},{""},{winner},{m.MatchupRound}");
            //}
            //File.WriteAllLines(GlobalConfig.MatchupFile.FullFilePath(), lines);


            foreach (MatchupEntryModel entry in model.Entries)
            {
                entry.SaveEntrytoFile(MatchupEntryFile);
            }
            
            //save to file
            lines = new List<string>();
            foreach (MatchupModel m in matchups)
            {
                string winner = "";
                if (m.Winner != null)
                {
                    winner = m.Winner.Id.ToString();
                }
                lines.Add($@"{m.Id},{ConvertMatchupEntryListtostring(m.Entries)},{winner},{m.MatchupRound}");
            }
            File.WriteAllLines(GlobalConfig.MatchupFile.FullFilePath(), lines);
        }
        
        public static void SaveEntrytoFile(this MatchupEntryModel model, string MatchupEntryFile)
        {
            List<MatchupEntryModel> entries = GlobalConfig.MatchupEntryFile.FullFilePath().LoadFile().
                ConverttoMatchupEntryModel();
            //Find the Max ID 
            int currentId = 1;
            if (entries.Count > 0) { currentId = entries.OrderByDescending(x => x.Id).First().Id + 1; }
            model.Id = currentId;
            entries.Add(model);
            
            //save to file
            List<string> lines = new List<string>();
            foreach (MatchupEntryModel e in entries)
            {
                string parent = "";
                if (e.ParentMatchup != null)
                {
                    parent = e.ParentMatchup.Id.ToString();
                }
                string teamCompeteing = "";
                if(e.TeamCompeting != null)
                {
                    teamCompeteing = e.TeamCompeting.Id.ToString();
                }
                lines.Add($@"{e.Id},{teamCompeteing},{e.Score},{parent}");
            }
            File.WriteAllLines(GlobalConfig.MatchupEntryFile.FullFilePath(), lines);
        }
         
        public static void SavetoTournamentsFile(this List<TournamentModel> models, string filename)
        {
            List<string> lines = new List<string>();

            foreach (TournamentModel t in models)
            {
                lines.Add($"{t.Id},{t.TournamentName},{t.EntryFee},{ConvertTeamsListtostring(t.EnteredTeams)},{ConvertPriceListtostring(t.Prices)},{ConvertRoundListtostring(t.Rounds)}");
            }

            File.WriteAllLines(filename.FullFilePath(), lines);
        }

        private static string ConvertPeopleListtosting(List<PersonModel> person)
        {
            string output = "";
            if (person.Count == 0) return output;

            foreach (PersonModel p in person)
            {
                output += $"{p.Id}|";
            }

            output = output.Substring(0, output.Length - 1);
            return output;
        }

        private static string ConvertTeamsListtostring(List<TeamModel> teams)
        {
            string output = "";
            if (teams.Count == 0) return output;

            foreach (TeamModel t in teams)
            {
                output += $"{t.Id}|";
            }

            output = output.Substring(0, output.Length - 1);
            return output;
        }
        
        private static string ConvertPriceListtostring(List<PriceModel> Prices)
        {
            string output = "";
            if (Prices.Count == 0) return output;

            foreach (PriceModel p in Prices)
            {
                output += $"{p.Id}|";
            }

            output = output.Substring(0, output.Length - 1);
            return output;
        }
        
        private static string ConvertRoundListtostring(List<List<MatchupModel>> rounds)
        {
            string output = "";
            if (rounds.Count == 0) return output;

            foreach (List<MatchupModel> r in rounds)
            {
                output += $"{ConvertMatchupListtostring(r)}|";
            }

            output = output.Substring(0, output.Length - 1);
            return output;
        }

        private static string ConvertMatchupListtostring(List<MatchupModel> matchups)
        {
            string output = "";
            if (matchups.Count == 0) return output;

            foreach (MatchupModel m in matchups)
            {
                output += $"{m.Id}^";
            }

            output = output.Substring(0, output.Length - 1);
            return output;
        }
        
        private static string ConvertMatchupEntryListtostring(List<MatchupEntryModel> entry)
        {
            string output = "";
            if (entry.Count == 0) return output;

            foreach (MatchupEntryModel e in entry)
            {
                output += $"{e.Id}|";
            }

            output = output.Substring(0, output.Length - 1);
            return output;
        }
    }
}
