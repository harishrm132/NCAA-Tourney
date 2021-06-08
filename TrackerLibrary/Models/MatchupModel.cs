using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class MatchupModel
    {
        /// <summary>
        /// Contains set of team involved match 
        /// </summary>
        public List<MatchupEntryModel> Entries { get; set; } = new List<MatchupEntryModel>();

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
