using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class MatchupModel
    {
        /// <summary>
        /// The unique Indentifier for prize
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Contains set of team involved match 
        /// </summary>
        public List<MatchupEntryModel> Entries { get; set; } = new List<MatchupEntryModel>();
        
        /// <summary>
        /// The ID from Database to lookup/identify the winner
        /// </summary>
        public int WinnerId { get; set; }
        
        /// <summary>
        /// Represents the winner of Match
        /// </summary>
        public TeamModel Winner { get; set; }

        /// <summary>
        /// Represents the round of the matchup in tournament
        /// </summary>
        public int MatchupRound { get; set; }
    }
}
