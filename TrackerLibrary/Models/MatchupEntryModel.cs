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
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Unique identifier for the team competing. Used to convert to TeamModel TeamCompeting.
        /// </summary>
        public int TeamCompetingId { get; set; }
        /// <summary>
        /// Represents one team in the matchup.
        /// </summary>
        public TeamModel TeamCompeting { get; set; }
        /// <summary>
        /// Represents the score for this particular team.
        /// </summary>
        public double Score { get; set; }
        /// <summary>
        /// Unique identifier for parent matchup. Used to convert to MatchupModel ParentMatchup.
        /// </summary>
        public int ParentMatchupId { get; set; }
        /// <summary>
        /// Represents the matchup that this team came
        /// from as a winner.
        /// </summary>
        public MatchupModel ParentMatchup { get; set; }
    }
}
