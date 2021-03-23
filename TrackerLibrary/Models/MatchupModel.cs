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
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// A list of entries to this particular matchup (an entry is one team or one person).
        /// </summary>
        public List<MatchupEntryModel> Entries { get; set; } = new List<MatchupEntryModel>();
        /// <summary>
        /// TeamId of the winning team, read from database. Used to convert to TeamModel Winner.
        /// </summary>
        public int WinnerId { get; set; }
        /// <summary>
        /// Winner of this matchup.
        /// </summary>
        public TeamModel Winner { get; set; }
        /// <summary>
        /// Round of the matchup.
        /// </summary>
        public int MatchupRound { get; set; }

        public string DisplayName
        {
            get
            {
                string output = "";
                foreach (MatchupEntryModel me in Entries)
                {
                    // Team competing = null if previous matchup not finished yet
                    if (me.TeamCompeting != null)
                    {
                        // For the first team, add the team name only, for next teams, add vs. before team name
                        if (output.Length == 0)
                        {
                            output = me.TeamCompeting.TeamName;
                        }
                        else
                        {
                            output += $" vs. { me.TeamCompeting.TeamName }";
                        }
                    }
                    else
                    {
                        output = "Matchup not yet determined";
                        break;
                    }
                }
                return output;
            }
        }
    }
}
