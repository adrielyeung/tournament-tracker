using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackerLibrary.Models
{
    public class TournamentModel
    {
        /// <summary>
        /// Event triggers when tournament is complete
        /// </summary>
        public event EventHandler<DateTime> OnTournamentComplete;
        /// <summary>
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name of tournament.
        /// </summary>
        public string TournamentName { get; set; }
        /// <summary>
        /// Entry fee (in dollars).
        /// </summary>
        public decimal EntryFee { get; set; }
        /// <summary>
        /// List of teams which entered the tournament.
        /// </summary>
        public List<TeamModel> EnteredTeams { get; set; } = new List<TeamModel>();
        /// <summary>
        ///  List of prizes.
        /// </summary>
        public List<PrizeModel> Prizes { get; set; } = new List<PrizeModel>();
        /// <summary>
        /// List of the matchups in all rounds.
        /// Each entry in this list is a list of matchups in the particular round.
        /// </summary>
        public List<List<MatchupModel>> Rounds { get; set; } = new List<List<MatchupModel>>();
        /// <summary>
        /// Whether the tournament is active (1) or completed (0).
        /// Only used by the TextConnector. (In SQL this is handled by the stored procedure.)
        /// </summary>
        public int Active { get; set; }

        /// <summary>
        /// Triggers OnTournamentComplete event.
        /// </summary>
        public void CompleteTournament()
        {
            // If OnTournamentComplete event is available (has listeners), pass back the date time of finish (event arg)
            OnTournamentComplete?.Invoke(this, DateTime.Now);
        }
    }
}
