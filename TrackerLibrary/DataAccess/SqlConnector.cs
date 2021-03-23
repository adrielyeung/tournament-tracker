using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public class SqlConnector : IDataConnector
    {
        private const string db = "Tournaments";

        /// <summary>
        /// Saves a new prize to the database
        /// </summary>
        /// <param name="model">The prize information.</param>
        /// <returns>The prize information, including the unique identifier.</returns>
        public void CreatePrize(PrizeModel model)
        {
            // Connection is destroyed beyond the curly braces of the "using" statement
            // Saves memory with too many connections
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@PlaceNumber", model.PlaceNumber);
                p.Add("@PlaceName", model.PlaceName);
                p.Add("@PrizeAmount", model.PrizeAmount);
                p.Add("@PrizePercentage", model.PrizePercentage);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                // connection.Execute SQL will NOT return any values
                connection.Execute("dbo.spPrizes_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@id");
            }
        }

        public void CreatePerson(PersonModel model)
        {
            // Connection is destroyed beyond the curly braces of the "using" statement
            // Saves memory with too many connections
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@FirstName", model.FirstName);
                p.Add("@LastName", model.LastName);
                p.Add("@EmailAddress", model.EmailAddress);
                p.Add("@CellphoneNumber", model.CellphoneNumber);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                // connection.Execute SQL will NOT return any values
                connection.Execute("dbo.spPeople_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@id");
            }
        }

        public void CreateTeam(TeamModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();

                p.Add("@TeamName", model.TeamName);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spTeams_Insert", p, commandType: CommandType.StoredProcedure);
                model.Id = p.Get<int>("@id");

                foreach (PersonModel tm in model.TeamMembers)
                {
                    p = new DynamicParameters();
                    p.Add("@TeamId", model.Id);
                    p.Add("@PersonId", tm.Id);
                    p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("dbo.spTeamMembers_Insert", p, commandType: CommandType.StoredProcedure);
                }
            }
        }

        public void CreateTournament(TournamentModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                SaveTournament(model, connection);

                SaveTournamentPrizes(model, connection);

                SaveTournamentEntries(model, connection);

                SaveTournamentRounds(model, connection);

                // Automatically move all 'bye' entries to next round
                TournamentLogic.UpdateTournamentResults(model);
            }
        }

        public List<PersonModel> GetPerson_All()
        {
            List<PersonModel> output;

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<PersonModel>("dbo.spPeople_GetAll").ToList();
            }
            return output;
        }

        public List<TeamModel> GetTeam_All()
        {
            List<TeamModel> output;

            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<TeamModel>("dbo.spTeam_GetAll").ToList();

                foreach (TeamModel team in output)
                {
                    var p = new DynamicParameters();
                    p.Add("@TeamId", team.Id);

                    team.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMembers_GetByTeam", p, commandType: CommandType.StoredProcedure).ToList();
                }
            }
            return output;
        }

        public List<TournamentModel> GetTournaments_All()
        {
            List<TournamentModel> output;
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<TournamentModel>("dbo.spTournaments_GetAll").ToList();

                foreach (TournamentModel t in output)
                {
                    var p = new DynamicParameters();
                    p.Add("@TournamentId", t.Id);
                    // Populate prizes
                    t.Prizes = connection.Query<PrizeModel>("dbo.spPrizes_GetByTournament", p, commandType: CommandType.StoredProcedure).ToList();

                    // Populate teams
                    t.EnteredTeams = connection.Query<TeamModel>("dbo.spTeam_GetByTournament", p, commandType: CommandType.StoredProcedure).ToList();
                    // Populate team members
                    foreach (TeamModel team in t.EnteredTeams)
                    {
                        var pt = new DynamicParameters();
                        pt.Add("@TeamId", team.Id);

                        team.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMembers_GetByTeam", pt, commandType: CommandType.StoredProcedure).ToList();
                    }

                    // Populate rounds
                    // Get all matchups of this tournament, order by ascending matchup round
                    List<MatchupModel> matchups = connection.Query<MatchupModel>("dbo.spMatchups_GetByTournament", p, commandType: CommandType.StoredProcedure).ToList();

                    foreach (MatchupModel m in matchups)
                    {
                        var pm = new DynamicParameters();
                        pm.Add("@MatchupId", m.Id);

                        m.Entries = connection.Query<MatchupEntryModel>("dbo.spMatchupEntries_GetByMatchup", pm, commandType: CommandType.StoredProcedure).ToList();

                        // Populate each entry (2 models)
                        // Populate each matchup (1 model)
                        List<TeamModel> allTeams = GetTeam_All();

                        if (m.WinnerId > 0)
                        {
                            m.Winner = allTeams.Where(x => x.Id == m.WinnerId).First();
                        }

                        foreach (MatchupEntryModel me in m.Entries)
                        {
                            // TeamCompetingId = 0 if no TeamCompetingId (default)
                            // Convert TeamCompetingId into TeamCompeting by searching through all teams' records
                            if (me.TeamCompetingId > 0)
                            {
                                me.TeamCompeting = allTeams.Where(x => x.Id == me.TeamCompetingId).First();
                            }

                            if (me.ParentMatchupId > 0)
                            {
                                me.ParentMatchup = matchups.Where(x => x.Id == me.ParentMatchupId).First();
                            }
                        }
                    }

                    // Each row display information of each round
                    List<MatchupModel> currRow = new List<MatchupModel>();
                    int currRound = 1;

                    foreach (MatchupModel m in matchups)
                    {
                        // Moved to next round
                        if (m.MatchupRound > currRound)
                        {
                            t.Rounds.Add(currRow);
                            currRow = new List<MatchupModel>();
                            currRound += 1;
                        }

                        currRow.Add(m);
                    }
                    t.Rounds.Add(currRow);
                }
            }
            return output;
            throw new NotImplementedException();
        }

        private void SaveTournament(TournamentModel model, IDbConnection connection)
        {
            var p = new DynamicParameters();

            p.Add("@TournamentName", model.TournamentName);
            p.Add("@EntryFee", model.EntryFee);
            p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

            connection.Execute("dbo.spTournaments_Insert", p, commandType: CommandType.StoredProcedure);
            model.Id = p.Get<int>("@id");
        }

        private void SaveTournamentPrizes(TournamentModel model, IDbConnection connection)
        {
            var p = new DynamicParameters();

            foreach (PrizeModel pz in model.Prizes)
            {
                p = new DynamicParameters();
                p.Add("@TournamentId", model.Id);
                p.Add("@PrizeId", pz.Id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spTournamentPrizes_Insert", p, commandType: CommandType.StoredProcedure);
            }
        }

        private void SaveTournamentEntries(TournamentModel model, IDbConnection connection)
        {
            var p = new DynamicParameters();
            foreach (TeamModel tm in model.EnteredTeams)
            {
                p = new DynamicParameters();
                p.Add("@TournamentId", model.Id);
                p.Add("@TeamId", tm.Id);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spTournamentEntries_Insert", p, commandType: CommandType.StoredProcedure);
            }
        }

        private void SaveTournamentRounds(TournamentModel model, IDbConnection connection)
        {
            // Loop through the rounds, List<MatchupModel>
            // Loop through the matchups
            // Save the matchup
            // Loop through the entries and save them
            foreach (List<MatchupModel> round in model.Rounds)
            {
                foreach (MatchupModel matchup in round)
                {
                    var p = new DynamicParameters();
                    p.Add("@TournamentId", model.Id);
                    p.Add("@MatchupRound", matchup.MatchupRound);
                    p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("dbo.spMatchups_Insert", p, commandType: CommandType.StoredProcedure);

                    matchup.Id = p.Get<int>("@id");

                    foreach (MatchupEntryModel entry in matchup.Entries)
                    {
                        p = new DynamicParameters();
                        p.Add("@MatchupId", matchup.Id);
                        if (entry.ParentMatchup == null)
                        {
                            p.Add("@ParentMatchupId", null);
                        }
                        else 
                        { 
                            p.Add("@ParentMatchupId", entry.ParentMatchup.Id);
                        }
                        
                        if (entry.TeamCompeting == null)
                        {
                            p.Add("@TeamCompetingId", null);
                        }
                        else
                        {
                            p.Add("@TeamCompetingId", entry.TeamCompeting.Id);
                        }
                        
                        p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                        connection.Execute("dbo.spMatchupEntries_Insert", p, commandType: CommandType.StoredProcedure);
                    }
                }
            }
        }

        public void UpdateMatchup(MatchupModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                // For updating next round matchup entries, we do not have a winner yet, so bypass updating matchups in this case
                if (model.Winner != null)
                {
                    var p = new DynamicParameters();
                    p.Add("@Id", model.Id);
                    p.Add("@WinnerId", model.Winner.Id);

                    connection.Execute("dbo.spMatchups_Update", p, commandType: CommandType.StoredProcedure);
                }

                foreach (MatchupEntryModel me in model.Entries)
                {
                    // For updating one of the next round matchup entries, the other team competing may be null, so bypass updating that
                    if (me.TeamCompeting != null)
                    {
                        var pe = new DynamicParameters();
                        pe.Add("@Id", me.Id);
                        pe.Add("@TeamCompetingId", me.TeamCompeting.Id);
                        pe.Add("@Score", me.Score);
                        connection.Execute("dbo.spMatchupEntries_Update", pe, commandType: CommandType.StoredProcedure);
                    }
                }
            }
        }

        public void CompleteTournament(TournamentModel model)
        {
            using (IDbConnection connection = new System.Data.SqlClient.SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@Id", model.Id);

                connection.Execute("dbo.spTournaments_Complete", p, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
