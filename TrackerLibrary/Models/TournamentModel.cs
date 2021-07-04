using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class TournamentModel
    {
        public event EventHandler<DateTime> OnTournamentComplete;
        
        /// <summary>
        /// The unique Indentifier for prize
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents the name of tournament
        /// </summary>
        public string TournamentName { get; set; }

        /// <summary>
        /// Represents the entry fee for tournament
        /// </summary>
        public decimal EntryFee { get; set; }

        /// <summary>
        /// Represents the list of teams enterd for the tournement
        /// </summary>
        public List<TeamModel> EnteredTeams { get; set; } = new List<TeamModel>();

        /// <summary>
        /// Represents the price for the place hoders
        /// </summary>
        public List<PriceModel> Prices { get; set; } = new List<PriceModel>();

        /// <summary>
        /// Represents list of rounds in which the tournament is countucted
        /// </summary>
        public List<List<MatchupModel>> Rounds { get; set; } = new List<List<MatchupModel>>();

        public void CompleteTournament()
        {
            // If its available do it
            OnTournamentComplete?.Invoke(this, DateTime.Now);
        }
    }
}
