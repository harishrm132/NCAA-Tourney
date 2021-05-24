using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary
{
    public class MatchupEntryModel
    {
        /// <summary>
        /// Represent One Team in the matchup.
        /// </summary>
        public TeamModel TeamCompeteting { get; set; }

        /// <summary>
        /// Reprensent Score for particular match.
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// Represent the matchup that this team came up as winner.
        /// </summary>
        public MatchupModel ParentMatchup { get; set; }
    }
}
