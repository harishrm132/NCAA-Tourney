using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public interface IDataConnection
    {
        void CreatePrize(PriceModel model);
        void CreatePerson(PersonModel model);
        void CreateTeam(TeamModel model); 
        void CreateTournaments(TournamentModel model);
        void UpdateMatchup(MatchupModel model);
        void CompleteTournaments(TournamentModel model);
        
        List<PersonModel> GetPerson_All();
        List<TeamModel> GetTeam_All();
        List<TournamentModel> GetTournaments_All();
    }
}
