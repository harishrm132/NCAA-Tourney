using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class TeamModel
    {
        /// <summary>
        /// The unique Indentifier for prize
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Represents team name 
        /// </summary>
        public string TeamName { get; set; }

        /// <summary>
        /// Represents list of teams in tournament
        /// </summary>
        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>();

    }
}
