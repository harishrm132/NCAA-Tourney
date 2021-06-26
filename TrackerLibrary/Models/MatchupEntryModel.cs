using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class MatchupEntryModel
    {
        /// <summary>
        /// The unique Indentifier for prize
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Represent One Team in the matchup.
        /// </summary>
        public TeamModel TeamCompeteting { get; set; }

        /// <summary>
        /// Reprensent Score for this particular team.
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// Represent the matchup that this team came up as winner.
        /// </summary>
        public MatchupModel ParentMatchup { get; set; }
    }
}
