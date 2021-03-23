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
        /// Unique identifier.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// A list of team members.
        /// </summary>
        public List<PersonModel> TeamMembers { get; set; } = new List<PersonModel>();
        /// <summary>
        /// Name of team.
        /// </summary>
        public string TeamName { get; set; }
    }
}
