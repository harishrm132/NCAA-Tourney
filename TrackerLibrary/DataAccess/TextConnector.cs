using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess.TextHelpers;

namespace TrackerLibrary.DataAccess
{
    public class TextConnector : IDataConnection
    {
        private const string PrizesFile = "PrizeModels.csv";
        private const string PersonFile = "PersonModels.csv";
        private const string TeamFile = "TeamModels.csv";
        private const string TournamentFile = "TournamentModels.csv";
        private const string MatchupFile = "MatchupModels.csv";
        private const string MatchupEntryFile = "MatchupEntryModels.csv";

        public PersonModel CreatePerson(PersonModel model)
        {
            List<PersonModel> persons = PersonFile.FullFilePath().LoadFile().ConverttoPersonModel();

            //Find the Max ID 
            int currentId = 1;
            if (persons.Count > 0) { currentId = persons.OrderByDescending(x => x.Id).First().Id + 1; }
            model.Id = currentId;

            //Add new record new ID - max + 1
            persons.Add(model);

            //Convert prizes to list<string> and Save list of string to text file
            persons.SavetoPersonFile(PersonFile);

            return model;
        }

        public PriceModel CreatePrize(PriceModel model)
        {
            //Load the text file and Convert text to list<prize model>
            List<PriceModel> prizes = PrizesFile.FullFilePath().LoadFile().ConverttoPrizeModel();

            //Find the Max ID 
            int currentId = 1;
            if(prizes.Count > 0) { currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1; }
            model.Id = currentId;
            
            //Add new record new ID - max + 1
            prizes.Add(model);

            //Convert prizes to list<string> and Save list of string to text file
            prizes.SavetoPrizeFile(PrizesFile);

            return model;
        }

        public TeamModel CreateTeam(TeamModel model)
        {
            List<TeamModel> Teams = TeamFile.FullFilePath().LoadFile().ConverttoTeamModel(PersonFile);

            //Find the Max ID 
            int currentId = 1;
            if (Teams.Count > 0) { currentId = Teams.OrderByDescending(x => x.Id).First().Id + 1; }
            model.Id = currentId;

            //Add new record new ID - max + 1
            Teams.Add(model);

            //Convert prizes to list<string> and Save list of string to text file
            Teams.SavetoTeamsFile(TeamFile);

            return model;
        }

        public void CreateTournaments(TournamentModel model)
        {
            List<TournamentModel> tournaments = TournamentFile.FullFilePath().LoadFile().
                ConverttoTournamentModel(TeamFile, PersonFile, PrizesFile);
 
            //Find the Max ID 
            int currentId = 1;
            if (tournaments.Count > 0) { currentId = tournaments.OrderByDescending(x => x.Id).First().Id + 1; }
            model.Id = currentId;

            model.SaveRoundstoFile(MatchupFile, MatchupEntryFile);
            
            //Add new record new ID - max + 1
            tournaments.Add(model);

            //Convert prizes to list<string> and Save list of string to text file
            tournaments.SavetoTournamentsFile(TournamentFile);

            //return model;
        }

        public void UpdateMatchup(MatchupModel model)
        {
            model.UpdateMatchuptoFile();
        }

        public List<PersonModel> GetPerson_All()
        {
            return PersonFile.FullFilePath().LoadFile().ConverttoPersonModel();
        }

        public List<TeamModel> GetTeam_All()
        {
            return TeamFile.FullFilePath().LoadFile().ConverttoTeamModel(PersonFile);
        }

        public List<TournamentModel> GetTournaments_All()
        {
            return TournamentFile.FullFilePath().LoadFile().
                ConverttoTournamentModel(TeamFile, PersonFile, PrizesFile);
        }
    }
}
