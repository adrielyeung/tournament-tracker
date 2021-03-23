using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess.TextHelpers
{
    public static class TextConnectorProcessor
    {
        private static readonly TextConnector textUtils = new TextConnector();

        /// <summary>
        /// Load the full path of the text file, appending to the folder path as read from App.config.
        /// </summary>
        /// <param name="fileName">Name of file.</param>
        /// <returns>Full file path of file.</returns>
        public static string FullFilePath(this string fileName) // this keyword turns the method into an extension method, could use as <string>.FullFilePath
        {
            // C:\Users\Adriel\Documents\Visual Studio 2019\source\repos\TournamentTracker\data
            return $"{ GlobalConfig.AppKeyLookup("filePath") }\\{fileName}";
        }

        /// <summary>
        /// Loads the text file.
        /// </summary>
        /// <param name="file">FULL file path of file.</param>
        /// <returns>All lines of file, turned into a list.</returns>
        public static List<string> LoadFile(this string file)
        {
            if (!File.Exists(file))
            {
                return new List<string>();
            }
            return File.ReadAllLines(file).ToList();
        }

        /// <summary>
        /// Convert text file to List<PrizeModel>.
        /// </summary>
        /// <param name="lines">A List of strings read from text file.</param>
        /// <returns>List of PrizeModel, with each item from 1 line in the input list.</returns>
        public static List<PrizeModel> ConvertToPrizeModels(this List<string> lines)
        {
            // File structure:
            // Id, place number, place name, prize amount, prize percentage
            List<PrizeModel> output = new List<PrizeModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                PrizeModel p = new PrizeModel();
                p.Id = int.Parse(cols[0]);
                p.PlaceNumber = int.Parse(cols[1]);
                p.PlaceName = cols[2];
                p.PrizeAmount = decimal.Parse(cols[3]);
                p.PrizePercentage = double.Parse(cols[4]);
                output.Add(p);
            }
            return output;
        }

        public static List<PersonModel> ConvertToPersonModels(this List<string> lines)
        {
            // File structure:
            // Id, first name, last name, email address, cellphone number
            List<PersonModel> output = new List<PersonModel>();

            foreach (string line in lines)
            {
                // Be careful of data having comma, it will be recognised as a different column, use complex separator if necessary
                string[] cols = line.Split(',');

                PersonModel p = new PersonModel();
                p.Id = int.Parse(cols[0]);
                p.FirstName = cols[1];
                p.LastName = cols[2];
                p.EmailAddress = cols[3];
                p.CellphoneNumber = cols[4];
                output.Add(p);
            }
            return output;
        }

        public static List<TeamModel> ConvertToTeamModels(this List<string> lines)
        {
            // File structure:
            // Id,team name,list of team members' people ids separated by the pipe (|)
            // 1,Team A,1|3|5
            List<TeamModel> output = new List<TeamModel>();
            List<PersonModel> people = textUtils.GetPerson_All();

            foreach (string line in lines)
            {
                // Be careful of data having comma, it will be recognised as a different column, use complex separator if necessary
                string[] cols = line.Split(',');

                TeamModel t = new TeamModel();
                t.Id = int.Parse(cols[0]);
                t.TeamName = cols[1];
                
                // List of team members' Id in People table
                string[] personIds = cols[2].Split('|');

                foreach (string id in personIds)
                {
                    // Search in people list, where Id equals the personId in the team list, return the first item (1-1 relationship, should contain 1 item in list)
                    t.TeamMembers.Add(people.Where(x => x.Id == int.Parse(id)).First());
                }

                output.Add(t);
            }
            return output;
        }

        public static List<TournamentModel> ConvertToTournamentModels(this List<string> lines)
        {
            // File structure:
            // Id,TournamentName,EntryFee,(TeamId1|TeamId2),(PrizeId1|PrizeId2),(MatchupId1^MatchupId2|MatchupId3^MatchupId4),Active
            // 0        1           2             3                  4           |------Round1-------|5|------Round2-------|    6
            List<TournamentModel> output = new List<TournamentModel>();
            List<TeamModel> teams = textUtils.GetTeam_All();
            List<PrizeModel> prizes = textUtils.GetPrize_All();
            List<MatchupModel> matchups = textUtils.GetMatchup_All();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                // Only return active tournaments
                if ("1".Equals(cols[6]))
                {
                    TournamentModel tm = new TournamentModel();
                    tm.Id = int.Parse(cols[0]);
                    tm.TournamentName = cols[1];
                    tm.EntryFee = decimal.Parse(cols[2]);

                    if (cols[3].Length > 0)
                    {
                        string[] teamIds = cols[3].Split('|');

                        foreach (string id in teamIds)
                        {
                            // Search in entered teams list, where Id equals the teamId in the team list, return the first item (1-1 relationship, should contain 1 item in list)
                            tm.EnteredTeams.Add(teams.Where(x => x.Id == int.Parse(id)).First());
                        }
                    }

                    if (cols[4].Length > 0)
                    {
                        string[] prizeIds = cols[4].Split('|');

                        foreach (string id in prizeIds)
                        {
                            tm.Prizes.Add(prizes.Where(x => x.Id == int.Parse(id)).First());
                        }
                    }

                    // Capture Rounds information
                    if (cols[5].Length > 0)
                    {
                        string[] rounds = cols[5].Split('|');

                        foreach (string round in rounds)
                        {
                            if (round.Length > 0)
                            {
                                string[] msText = round.Split('^');
                                List<MatchupModel> ms = new List<MatchupModel>();

                                foreach (string matchupModelTextId in msText)
                                {
                                    ms.Add(matchups.Where(x => x.Id == int.Parse(matchupModelTextId)).First());
                                }
                                tm.Rounds.Add(ms);
                            }
                        }
                    }

                    output.Add(tm);
                }
            }
            return output;
        }


        public static List<MatchupModel> ConvertToMatchupModels(this List<string> lines)
        {
            // File structure:
            // Id=0, entries=1(pipe delimited), winnerId=2, matchupRound=3
            List<MatchupModel> output = new List<MatchupModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                MatchupModel m = new MatchupModel();
                m.Id = int.Parse(cols[0]);
                m.Entries = ConvertToMatchupEntryModels(cols[1]);
                if (int.TryParse(cols[2], out int winnerId))
                {
                    m.Winner = LookupTeamById(int.Parse(cols[2]));
                }
                else
                {
                    m.Winner = null;
                }
                
                m.MatchupRound = int.Parse(cols[3]);
                output.Add(m);
            }
            return output;
        }

        /// <summary>
        /// Converts a '|'-delimited string (in the Matchup file) to a list of matchup entries.
        /// </summary>
        /// <param name="input">'|'-delimited string of MatchupEntryIds.</param>
        /// <returns></returns>
        private static List<MatchupEntryModel> ConvertToMatchupEntryModels(string input)
        {
            string[] matchupFileEntryIds = input.Split('|');
            List<MatchupEntryModel> output = new List<MatchupEntryModel>();
            List<string> entries = GlobalConfig.MatchupEntryFile.FullFilePath().LoadFile();
            List<string> matchingEntries = new List<string>();

            // matchupFileEntryIds contain the matchup entries in the matchup file.
            // entry.Split(',') -> entryFileCols[0] contain the matchup entries in the matchup entry file.
            // We only select the matching MatchupEntry.
            foreach (string id in matchupFileEntryIds)
            {
                foreach (string entry in entries)
                {
                    string[] entryFileCols = entry.Split(',');

                    if (entryFileCols[0] == id)
                    {
                        matchingEntries.Add(entry);
                    }
                }
                // Only convert the MatchupEntry which match
                output = matchingEntries.ConvertToMatchupEntryModels();
            }
            return output;
        }

        /// <summary>
        /// Converts all matchup entries in the MatchupEntry file into MatchupEntryModels.
        /// </summary>
        /// <param name="lines">All lines in the MatchupEntry file.</param>
        /// <returns></returns>
        private static List<MatchupEntryModel> ConvertToMatchupEntryModels(this List<string> lines)
        {
            // File structure:
            // Id, TeamCompetingId, Score, ParentMatchupId
            List<MatchupEntryModel> output = new List<MatchupEntryModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                MatchupEntryModel me = new MatchupEntryModel();
                me.Id = int.Parse(cols[0]);
                if (cols[1].Length == 0)
                {
                    me.TeamCompeting = null;
                } else
                {
                    me.TeamCompeting = LookupTeamById(int.Parse(cols[1]));
                }
                
                me.Score = double.Parse(cols[2]);
                // During first round, no parent matchup
                if (int.TryParse(cols[3], out int parentId))
                {
                    me.ParentMatchup = LookupMatchupById(int.Parse(cols[3]));
                }
                else
                {
                    me.ParentMatchup = null;
                }
                
                output.Add(me);
            }
            return output;
        }

        public static void SaveToPrizeFile(this List<PrizeModel> models)
        {
            List<string> lines = new List<string>();

            foreach (PrizeModel p in models)
            {
                lines.Add($"{ p.Id },{ p.PlaceNumber },{ p.PlaceName },{ p.PrizeAmount },{ p.PrizePercentage }");
            }

            File.WriteAllLines(GlobalConfig.PrizesFile.FullFilePath(), lines);
        }

        public static void SaveToPeopleFile(this List<PersonModel> models)
        {
            List<string> lines = new List<string>();

            foreach (PersonModel p in models)
            {
                lines.Add($"{ p.Id },{ p.FirstName },{ p.LastName },{ p.EmailAddress },{ p.CellphoneNumber }");
            }

            File.WriteAllLines(GlobalConfig.PeopleFile.FullFilePath(), lines);
        }

        public static void SaveToTeamsFile(this List<TeamModel> models)
        {
            List<string> lines = new List<string>();

            foreach (TeamModel t in models)
            {
                lines.Add($"{ t.Id },{ t.TeamName },{ ConvertPeopleListToString(t.TeamMembers) }");
            }

            File.WriteAllLines(GlobalConfig.TeamsFile.FullFilePath(), lines);
        }

        public static void SaveToTournamentsFile(this List<TournamentModel> models)
        {
            List<string> lines = new List<string>();

            foreach (TournamentModel tm in models)
            {
                lines.Add($"{ tm.Id }," +
                    $"{ tm.TournamentName }," +
                    $"{ tm.EntryFee }," +
                    $"{ ConvertTeamsListToString(tm.EnteredTeams) }," +
                    $"{ ConvertPrizesListToString(tm.Prizes) }," +
                    $"{ ConvertRoundsListToString(tm.Rounds) }," +
                    $"{ tm.Active }");
            }

            File.WriteAllLines(GlobalConfig.TournamentFile.FullFilePath(), lines);
        }

        public static void SaveRoundsToFile(this TournamentModel model)
        {
            // Loop through each round
            // Loop through each matchup
            // Get id for new matchup then save record
            // Loop through each entry, get id, save it

            foreach (List<MatchupModel> round in model.Rounds)
            {
                foreach (MatchupModel matchup in round)
                {
                    // Load all matchups from file
                    // Get the top id and add 1
                    // Store the id
                    // Save the matchup record
                    matchup.SaveMatchupToFile();
                }
            }
        }

        public static void SaveMatchupToFile(this MatchupModel matchup)
        {
            List<MatchupModel> matchups = textUtils.GetMatchup_All();

            // Generate id for each matchup
            int currentId = 1;

            if (matchups.Count > 0)
            {
                currentId = matchups.OrderByDescending(x => x.Id).First().Id + 1;
            }

            matchup.Id = currentId;

            // First save the matchups with their id, so that the MatchupEntries can use those id as ParentMatchupId
            matchups.Add(matchup);

            // Save each entry to file
            foreach (MatchupEntryModel entry in matchup.Entries)
            {
                entry.SaveEntryToFile();
            }

            // Save matchup - after each entry saved, so that all the MatchupEntryIds are generated
            List<string> lines = new List<string>();

            foreach (MatchupModel m in matchups)
            {
                string winnerId = "";
                if (m.Winner != null)
                {
                    winnerId = m.Winner.Id.ToString();
                }
                lines.Add($"{ m.Id },{ ConvertMatchupEntriesListToString(m.Entries) },{ winnerId },{ m.MatchupRound }");
            }

            File.WriteAllLines(GlobalConfig.MatchupFile.FullFilePath(), lines);
        }

        public static void SaveEntryToFile(this MatchupEntryModel entry)
        {
            List<MatchupEntryModel> entries = GlobalConfig.MatchupEntryFile.FullFilePath().LoadFile().ConvertToMatchupEntryModels();

            int currentId = 1;

            if (entries.Count > 0)
            {
                currentId = entries.OrderByDescending(x => x.Id).First().Id + 1;
            }

            entry.Id = currentId;
            entries.Add(entry);

            // Save entries to file
            List<string> lines = new List<string>();

            foreach (MatchupEntryModel e in entries)
            {
                string parentMatchupId = "";
                if (e.ParentMatchup != null)
                {
                    parentMatchupId = e.ParentMatchup.Id.ToString();
                }
                string teamCompetingId = "";
                if (e.TeamCompeting != null)
                {
                    teamCompetingId = e.TeamCompeting.Id.ToString();
                }
                lines.Add($"{ e.Id },{ teamCompetingId },{ e.Score },{ parentMatchupId }");
            }

            File.WriteAllLines(GlobalConfig.MatchupEntryFile.FullFilePath(), lines);
        }

        public static void UpdateMatchupToFile(this MatchupModel matchup)
        {
            List<MatchupModel> matchups = textUtils.GetMatchup_All();
            MatchupModel oldMatchup = new MatchupModel();

            // Remove the matchup to update first, then add the matchup back with updated info
            foreach (MatchupModel m in matchups)
            {
                if (m.Id == matchup.Id)
                {
                    oldMatchup = m;
                }
            }
            matchups.Remove(oldMatchup);
            matchups.Add(matchup);

            // Save each entry to file
            foreach (MatchupEntryModel entry in matchup.Entries)
            {
                entry.UpdateEntryToFile();
            }

            // Save matchup - after each entry saved, so that all the MatchupEntryIds are generated
            List<string> lines = new List<string>();

            foreach (MatchupModel m in matchups)
            {
                string winnerId = "";
                if (m.Winner != null)
                {
                    winnerId = m.Winner.Id.ToString();
                }
                lines.Add($"{ m.Id },{ ConvertMatchupEntriesListToString(m.Entries) },{ winnerId },{ m.MatchupRound }");
            }

            File.WriteAllLines(GlobalConfig.MatchupFile.FullFilePath(), lines);
        }

        public static void UpdateEntryToFile(this MatchupEntryModel entry)
        {
            List<MatchupEntryModel> entries = GlobalConfig.MatchupEntryFile.FullFilePath().LoadFile().ConvertToMatchupEntryModels();
            MatchupEntryModel oldEntry = new MatchupEntryModel();

            // Remove the matchup to update first, then add the matchup back with updated info
            foreach (MatchupEntryModel m in entries)
            {
                if (m.Id == entry.Id)
                {
                    oldEntry = m;
                }
            }
            entries.Remove(oldEntry);
            entries.Add(entry);

            // Save entries to file
            List<string> lines = new List<string>();

            foreach (MatchupEntryModel e in entries)
            {
                string parentMatchupId = "";
                if (e.ParentMatchup != null)
                {
                    parentMatchupId = e.ParentMatchup.Id.ToString();
                }
                string teamCompetingId = "";
                if (e.TeamCompeting != null)
                {
                    teamCompetingId = e.TeamCompeting.Id.ToString();
                }
                lines.Add($"{ e.Id },{ teamCompetingId },{ e.Score },{ parentMatchupId }");
            }

            File.WriteAllLines(GlobalConfig.MatchupEntryFile.FullFilePath(), lines);
        }

        private static string ConvertPeopleListToString(List<PersonModel> people)
        {
            string output = "";

            if (people.Count == 0)
            {
                return output;
            }

            // Append people Id of team member and pipe character (e.g. 1|3|)
            foreach (PersonModel p in people) {
                output += $"{ p.Id }|";
            }
            // Remove final pipe character (e.g. 1|3| -> 1|3)
            output = output.Substring(0, output.Length - 1);
            return output;
        }

        private static string ConvertTeamsListToString(List<TeamModel> teams)
        {
            string output = "";

            if (teams.Count == 0)
            {
                return output;
            }

            foreach (TeamModel tm in teams)
            {
                output += $"{ tm.Id }|";
            }
            // Remove final pipe character (e.g. 1|3| -> 1|3)
            output = output.Substring(0, output.Length - 1);
            return output;
        }

        private static string ConvertPrizesListToString(List<PrizeModel> prizes)
        {
            string output = "";

            if (prizes.Count == 0)
            {
                return output;
            }

            foreach (PrizeModel pz in prizes)
            {
                output += $"{ pz.Id }|";
            }
            // Remove final pipe character (e.g. 1|3| -> 1|3)
            output = output.Substring(0, output.Length - 1);
            return output;
        }

        private static string ConvertMatchupEntriesListToString(List<MatchupEntryModel> entries)
        {
            string output = "";

            if (entries.Count == 0)
            {
                return output;
            }

            foreach (MatchupEntryModel e in entries)
            {
                output += $"{ e.Id }|";
            }
            // Remove final pipe character (e.g. 1|3| -> 1|3)
            output = output.Substring(0, output.Length - 1);
            return output;
        }

        private static string ConvertRoundsListToString(List<List<MatchupModel>> rounds)
        {
            string output = "";

            if (rounds.Count == 0)
            {
                return output;
            }

            // Append people Id of team member and pipe character (e.g. 1|3|)
            foreach (List<MatchupModel> rd in rounds)
            {
                output += $"{ ConvertMatchupListToString(rd) }|";
            }
            // Remove final pipe character (e.g. 1|3| -> 1|3)
            output = output.Substring(0, output.Length - 1);
            return output;
        }
        
        private static string ConvertMatchupListToString(List<MatchupModel> matchups)
        {
            string output = "";

            if (matchups.Count == 0)
            {
                return output;
            }

            // Append people Id of team member and caret character (e.g. 1^3^)
            foreach (MatchupModel mu in matchups)
            {
                output += $"{ mu.Id }^";
            }
            // Remove final caret character (e.g. 1^3^ -> 1^3)
            output = output.Substring(0, output.Length - 1);
            return output;
        }

        private static TeamModel LookupTeamById(int id)
        {
            List<string> teams = GlobalConfig.TeamsFile.FullFilePath().LoadFile();

            // Only convert the team with matching id in TeamsFile.
            foreach (string team in teams)
            {
                string[] cols = team.Split(',');
                if (cols[0] == id.ToString())
                {
                    List<string> matchingTeams = new List<string>();
                    matchingTeams.Add(team);
                    return matchingTeams.ConvertToTeamModels().First();
                }
            }
            // No matching team found in TeamsFile.
            return null;
        }

        private static MatchupModel LookupMatchupById(int id)
        {
            List<string> matchups = GlobalConfig.MatchupFile.FullFilePath().LoadFile();

            // Only convert the team with matching id in TeamsFile.
            foreach (string matchup in matchups)
            {
                string[] cols = matchup.Split(',');
                if (cols[0] == id.ToString())
                {
                    List<string> matchingMatchups = new List<string>();
                    matchingMatchups.Add(matchup);
                    return matchingMatchups.ConvertToMatchupModels().First();
                }
            }
            // No matching team found in TeamsFile.
            return null;
        }
    }
}
