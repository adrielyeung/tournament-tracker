using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess.TextHelpers;

namespace TrackerLibrary.DataAccess
{
    public class TextConnector : IDataConnector
    {
        /// <summary>
        /// Saves a new prize to the database
        /// </summary>
        /// <param name="model">The prize information.</param>
        /// <returns>The prize information, including the unique identifier.</returns>
        public void CreatePrize(PrizeModel model)
        {
            // Load the text file
            // Convert text to List<PrizeModel>
            List<PrizeModel> prizes = GlobalConfig.PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

            // Default Id is 1 when no records
            int currentId = 1;
            
            if (prizes.Count > 0)
            {
                // Order by descending Id of all prizes, get the first prize (max Id), then add 1 for current Id
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }
            model.Id = currentId;
            currentId += 1;

            // Add the new record with the new ID (max + 1) -- no auto-increment ID, need to manually find it
            prizes.Add(model);

            // Convert the prizes to List<String>
            // Save the List<String> to text file
            prizes.SaveToPrizeFile();
        }

        public void CreatePerson(PersonModel model)
        {
            List<PersonModel> people = GetPerson_All();

            int currentId = 1;

            if (people.Count > 0)
            {
                currentId = people.OrderByDescending(x => x.Id).First().Id + 1;
            }
            model.Id = currentId;
            currentId += 1;

            people.Add(model);

            people.SaveToPeopleFile();
        }

        public void CreateTeam(TeamModel model)
        {
            List<TeamModel> teams = GlobalConfig.TeamsFile.FullFilePath().LoadFile().ConvertToTeamModels();
            List<PersonModel> people = GetPerson_All();

            int currentId = 1;

            if (teams.Count > 0)
            {
                currentId = teams.OrderByDescending(x => x.Id).First().Id + 1;
            }
            model.Id = currentId;
            currentId += 1;

            teams.Add(model);

            teams.SaveToTeamsFile();
        }

        public void CreateTournament(TournamentModel model)
        {
            List<TournamentModel> tournaments = GlobalConfig.TournamentFile.FullFilePath().LoadFile().ConvertToTournamentModels();

            int currentId = 1;

            if (tournaments.Count > 0)
            {
                currentId = tournaments.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;
            model.Active = 1;

            // Save the ids of all matchups to the model before saving to text file
            model.SaveRoundsToFile();

            tournaments.Add(model);
            tournaments.SaveToTournamentsFile();

            // Automatically move all 'bye' entries to next round
            TournamentLogic.UpdateTournamentResults(model);
        }

        public List<PersonModel> GetPerson_All()
        {
            return GlobalConfig.PeopleFile.FullFilePath().LoadFile().ConvertToPersonModels();
        }

        public List<TeamModel> GetTeam_All()
        {
            return GlobalConfig.TeamsFile.FullFilePath().LoadFile().ConvertToTeamModels();
        }

        public List<PrizeModel> GetPrize_All()
        {
            return GlobalConfig.PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();
        }

        public List<MatchupModel> GetMatchup_All()
        {
            return GlobalConfig.MatchupFile.FullFilePath().LoadFile().ConvertToMatchupModels();
        }

        public List<TournamentModel> GetTournaments_All()
        {
            return GlobalConfig.TournamentFile.FullFilePath().LoadFile().ConvertToTournamentModels();
        }

        public void UpdateMatchup(MatchupModel model)
        {
            model.UpdateMatchupToFile();
        }

        public void CompleteTournament(TournamentModel model)
        {
            List<TournamentModel> tournaments = GlobalConfig.TournamentFile.FullFilePath().LoadFile().ConvertToTournamentModels();

            // Set tournament active bit to 0, then save it back
            tournaments.Remove(model);
            model.Active = 0;
            tournaments.Add(model);
            tournaments.SaveToTournamentsFile();
        }
    }
}
