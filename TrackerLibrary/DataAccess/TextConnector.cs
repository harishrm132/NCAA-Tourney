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
        public void CreatePerson(PersonModel model)
        {
            List<PersonModel> persons = GlobalConfig.PersonFile.FullFilePath().LoadFile().ConverttoPersonModel();

            //Find the Max ID 
            int currentId = 1;
            if (persons.Count > 0) { currentId = persons.OrderByDescending(x => x.Id).First().Id + 1; }
            model.Id = currentId;

            //Add new record new ID - max + 1
            persons.Add(model);

            //Convert prizes to list<string> and Save list of string to text file
            persons.SavetoPersonFile();
        }

        public void CreatePrize(PriceModel model)
        {
            //Load the text file and Convert text to list<prize model>
            List<PriceModel> prizes = GlobalConfig.PrizesFile.FullFilePath().LoadFile().ConverttoPrizeModel();

            //Find the Max ID 
            int currentId = 1;
            if(prizes.Count > 0) { currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1; }
            model.Id = currentId;
            
            //Add new record new ID - max + 1
            prizes.Add(model);

            //Convert prizes to list<string> and Save list of string to text file
            prizes.SavetoPrizeFile();
        }

        public void CreateTeam(TeamModel model)
        {
            List<TeamModel> Teams = GlobalConfig.TeamFile.FullFilePath().LoadFile().ConverttoTeamModel();

            //Find the Max ID 
            int currentId = 1;
            if (Teams.Count > 0) { currentId = Teams.OrderByDescending(x => x.Id).First().Id + 1; }
            model.Id = currentId;

            //Add new record new ID - max + 1
            Teams.Add(model);

            //Convert prizes to list<string> and Save list of string to text file
            Teams.SavetoTeamsFile();
        }

        public void CreateTournaments(TournamentModel model)
        {
            List<TournamentModel> tournaments = GlobalConfig.TournamentFile.FullFilePath().LoadFile().
                ConverttoTournamentModel();
 
            //Find the Max ID 
            int currentId = 1;
            if (tournaments.Count > 0) { currentId = tournaments.OrderByDescending(x => x.Id).First().Id + 1; }
            model.Id = currentId;

            model.SaveRoundstoFile();
            
            //Add new record new ID - max + 1
            tournaments.Add(model);

            //Convert prizes to list<string> and Save list of string to text file
            tournaments.SavetoTournamentsFile();

            TournamentLogic.UpdateTournamentResults(model);
        }

        public void UpdateMatchup(MatchupModel model)
        {
            model.UpdateMatchuptoFile();
        }

        public void CompleteTournaments(TournamentModel model)
        {
            List<TournamentModel> tournaments = GlobalConfig.TournamentFile.FullFilePath().LoadFile().
                ConverttoTournamentModel();
            
            //Find and Remove model that matches
            tournaments.Remove(model);

            //Convert prizes to list<string> and Save list of string to text file
            tournaments.SavetoTournamentsFile();

            TournamentLogic.UpdateTournamentResults(model);
        }

        public List<PersonModel> GetPerson_All()
        {
            return GlobalConfig.PersonFile.FullFilePath().LoadFile().ConverttoPersonModel();
        }

        public List<TeamModel> GetTeam_All()
        {
            return GlobalConfig.TeamFile.FullFilePath().LoadFile().ConverttoTeamModel();
        }

        public List<TournamentModel> GetTournaments_All()
        {
            return GlobalConfig.TournamentFile.FullFilePath().LoadFile().
                ConverttoTournamentModel();
        }
    }
}
