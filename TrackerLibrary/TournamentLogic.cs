using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerLibrary
{
    // All business logic should be placed here, called by the UI classes - the UI class should only deal with displaying text and
    // putting text into models
    public static class TournamentLogic
    {
        // Order our list randomly of teams
        // Check if it is big enough - if not, add in byes - To the closest power of 2 bigger than number of teams entered
        // Create first round of matchups
        // Create every round after that (divide by 2) etc.
        public static void CreateRounds(TournamentModel model)
        {
            List<TeamModel> randomizedTeams = RandomizeTeamOrder(model.EnteredTeams);
            int rounds = FindNumberOfRounds(randomizedTeams.Count);
            int byes = NumberOfByes(rounds, randomizedTeams.Count);

            model.Rounds.Add(CreateFirstRound(byes, randomizedTeams));
            CreateOtherRounds(model, rounds);
        }

        /// <summary>
        /// Updates the winner for any matchups with a score / 'bye' matchups for the whole tournament.
        /// </summary>
        /// <param name="model">The tournament to update.</param>
        public static void UpdateTournamentResults(TournamentModel model)
        {
            List<MatchupModel> toScore = new List<MatchupModel>();
            // Current round before updating matchup winners
            int startingRound = model.CheckCurrentRound();

            foreach (List<MatchupModel> round in model.Rounds)
            {
                foreach (MatchupModel rm in round)
                {
                    // If no winner set up yet, and
                    // At least 1 team has score != 0 (Tie games not handled, so won't have finished games and both teams' score = 0)
                    // Or a 'bye' matchup (Entries.Count = 1)
                    if (rm.Winner == null && (rm.Entries.Any(x => x.Score != 0) || rm.Entries.Count == 1))
                    {
                        toScore.Add(rm);
                    }
                }
            }

            MarkWinnerInMatchups(toScore);

            AdvanceWinners(toScore, model);

            toScore.ForEach(x => GlobalConfig.Connection.UpdateMatchup(x));

            int endingRound = model.CheckCurrentRound();

            // After update, has progressed to next round
            if (endingRound > startingRound)
            {
                // Alert users
                model.AlertUsersToNewRound();
            }
        }

        public static void AlertUsersToNewRound(this TournamentModel model)
        {
            int currentRoundNumber = model.CheckCurrentRound();

            // Find the list of matchups of the current round
            List<MatchupModel> currentRound = model.Rounds.Where(x => x.First().MatchupRound == currentRoundNumber).First();

            // Loop through all matchups in current round, search for all entries (teams competing),
            // then loop through all team members
            foreach (MatchupModel matchup in currentRound)
            {
                foreach (MatchupEntryModel me in matchup.Entries)
                {
                    foreach (PersonModel person in me.TeamCompeting.TeamMembers)
                    {
                        // Notify the opponents that they are matched up against the team (FirstOrDefault used to handle if only 1 matchup ('bye') cases)
                        AlertPersonToNewRound(model, person, me.TeamCompeting.TeamName, matchup.Entries.Where(x => x.TeamCompeting != me.TeamCompeting).FirstOrDefault());
                    }
                }
            }
        }

        private static void AlertPersonToNewRound(TournamentModel model, PersonModel person, string teamName, MatchupEntryModel opponent)
        {
            // Validate email address - simply not send if not valid, no need error msg
            if (person.EmailAddress.Length == 0 || !person.EmailAddress.Contains("@") || !person.EmailAddress.Contains("."))
            {
                return;
            }

            List<string> to = new List<string>();
            string subject;
            // Less memory intensive way to concatenate strings
            StringBuilder body = new StringBuilder();

            // Can add in other info, e.g. score info, status of last round
            if (opponent != null)
            {
                subject = $"In { model.TournamentName }, you have a new matchup with { opponent.TeamCompeting.TeamName }";

                // It seems that the Envrionment.NewLine character does not work in HTML emails in both Outlook and GMail
                // So, insert <br> tag instead
                body.Append("<h1>You have a new matchup</h1>");
                body.Append("<strong>Competitor: </strong>");
                body.Append(opponent.TeamCompeting.TeamName);
                body.Append("<br><br>");
                body.Append("Have a great time!<br>");
                body.Append("~Tournament Tracker<br><br>");
                body.Append("---------------------------<br>");
                body.Append("<i>This email is system-generated from an unmonitored address. Please do not reply to this email.</i>");
            }
            else
            {
                subject = $"In { model.TournamentName }, you have progressed to next round (bye week)";
                
                body.Append("Enjoy your round off!<br>");
                body.Append("~Tournament Tracker<br><br>");
                body.Append("---------------------------<br>");
                body.Append("<i>This email is system-generated from an unmonitored address. Please do not reply to this email.</i>");
            }

            to.Add(person.EmailAddress);
            
            EmailLogic.SendEmail(to, subject, body.ToString());
        }

        private static void MarkWinnerInMatchups(List<MatchupModel> models)
        {
            // higherWins - from config, if value = 0 then lower score is winner, otherwise higher score wins
            string higherWins = GlobalConfig.AppKeyLookup("higherWins");
            
            foreach (MatchupModel m in models)
            {
                // 'Bye' case handling
                if (m.Entries.Count == 1)
                {
                    m.Winner = m.Entries[0].TeamCompeting;
                    continue;
                }

                if (higherWins == "0")
                {
                    // Lower score wins
                    if (m.Entries[0].Score < m.Entries[1].Score)
                    {
                        m.Winner = m.Entries[0].TeamCompeting;
                    }
                    else if (m.Entries[0].Score > m.Entries[1].Score)
                    {
                        m.Winner = m.Entries[1].TeamCompeting;
                    }
                    else
                    {
                        throw new NotSupportedException("Tie games are not handled in this application.");
                    }
                }
                else
                {
                    // Higher score wins
                    if (m.Entries[0].Score > m.Entries[1].Score)
                    {
                        m.Winner = m.Entries[0].TeamCompeting;
                    }
                    else if (m.Entries[0].Score < m.Entries[1].Score)
                    {
                        m.Winner = m.Entries[1].TeamCompeting;
                    }
                    else
                    {
                        throw new NotSupportedException("Tie games are not handled in this application.");
                    }
                }
            }
        }

        private static int CheckCurrentRound(this TournamentModel model)
        {
            int output = 1;

            foreach (List<MatchupModel> round in model.Rounds)
            {
                // All matchups in the round has a winner
                if (round.All(x => x.Winner != null))
                {
                    output += 1;
                }
                else
                {
                    return output;
                }
            }

            // Cannot find a round with no winner, tournament is complete
            CompleteTournament(model);
            // Return last round value, not advanced to a non-existent round
            return output - 1;
        }

        // Not as extension method, since it is only called in 1 place, make it not so easy to call
        private static void CompleteTournament(TournamentModel model)
        {
            GlobalConfig.Connection.CompleteTournament(model);

            // Last round, first matchup (only 1 matchup)
            MatchupModel lastMatchup = model.Rounds.Last().First();
            TeamModel winner = lastMatchup.Winner;
            TeamModel runnerUp = lastMatchup.Entries.Where(x => x.TeamCompeting != winner).First().TeamCompeting;

            // In this application, only handle winner and runner up prizes (first 2 places)
            // It is more advanced to identify the third place in this system (could use the scores to compare)
            decimal winnerPrize = 0;
            decimal runnerUpPrize = 0;

            // Retrieve prize and fee info to deliver prizes
            if (model.Prizes.Count > 0)
            {
                decimal totalIncome = model.EnteredTeams.Count * model.EntryFee;

                PrizeModel firstPlacePrize = model.Prizes.Where(x => x.PlaceNumber == 1).FirstOrDefault();
                if (firstPlacePrize != null)
                {
                    winnerPrize = firstPlacePrize.CalculatePrizePayout(totalIncome);
                }

                PrizeModel secondPlacePrize = model.Prizes.Where(x => x.PlaceNumber == 2).FirstOrDefault();
                if (secondPlacePrize != null)
                {
                    runnerUpPrize = secondPlacePrize.CalculatePrizePayout(totalIncome);
                }
            }

            // Generate list of all tournament participant emails
            List<string> allParticipantEmails = new List<string>();

            foreach (TeamModel t in model.EnteredTeams)
            {
                foreach (PersonModel p in t.TeamMembers)
                {
                    if (p.EmailAddress.Length > 0)
                    {
                        allParticipantEmails.Add(p.EmailAddress);
                    }
                }
            }

            // Send email to all teams
            AlertUsersToCompletion(model, winner, winnerPrize, runnerUp, runnerUpPrize, allParticipantEmails);
        }

        private static void AlertUsersToCompletion(TournamentModel model, TeamModel winner, decimal winnerPrize, TeamModel runnerUp, decimal runnerUpPrize, List<string> emailList) {
            string subject = $"In { model.TournamentName }, { winner.TeamName } has won!";
            // Less memory intensive way to concatenate strings
            StringBuilder body = new StringBuilder();

            body.Append("<h1>We have a WINNER!</h1>");
            body.Append("<p>Congratuations to our winner on a great tournament.</p>");

            if (winnerPrize > 0)
            {
                body.Append($"<p>{ winner.TeamName } will receive ${ winnerPrize }</p>");
            }

            if (runnerUpPrize > 0)
            {
                body.Append($"<p>{ runnerUp.TeamName } will receive ${ runnerUpPrize }</p>");
            }
            body.Append("~Tournament Tracker<br><br>");
            body.Append("---------------------------<br>");
            body.Append("<i>This email is system-generated from an unmonitored address. Please do not reply to this email.</i>");

            EmailLogic.SendEmail(new List<string>(), emailList, subject, body.ToString());

            // Complete Tournament
            model.CompleteTournament();
        }

        private static decimal CalculatePrizePayout(this PrizeModel prize, decimal totalIncome)
        {
            if (prize.PrizeAmount > 0)
            {
                return prize.PrizeAmount;
            }
            else
            {
                // More accurate than using * operator
                return Decimal.Multiply(totalIncome, Convert.ToDecimal(prize.PrizePercentage / 100));
            }
        }

        private static void AdvanceWinners(List<MatchupModel> models, TournamentModel tournament)
        {
            // For each matchup, loop to find its child matchup (entry) and put the winner into that
            foreach (MatchupModel m in models)
            {
                // Write the winner to next round MatchupEntries with corresponding ParentMatchupId of this finished matchup (m.Id)
                foreach (List<MatchupModel> round in tournament.Rounds)
                {
                    foreach (MatchupModel rm in round)
                    {
                        foreach (MatchupEntryModel me in rm.Entries)
                        {
                            if (me.ParentMatchup != null)
                            {
                                if (me.ParentMatchup.Id == m.Id)
                                {
                                    me.TeamCompeting = m.Winner;
                                    GlobalConfig.Connection.UpdateMatchup(rm);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static List<MatchupModel> CreateFirstRound(int byes, List<TeamModel> teams)
        {
            List<MatchupModel> output = new List<MatchupModel>();
            MatchupModel curr = new MatchupModel();

            foreach (TeamModel team in teams)
            {
                // Add teams competing
                curr.Entries.Add(new MatchupEntryModel { TeamCompeting = team });

                // If there are 'byes', assign bye to this team, so only 1 team in this matchup
                // Or no byes, but have 2 teams already
                // Set matchup round and move to new MatchupModel
                if (byes > 0 || curr.Entries.Count > 1)
                {
                    // Must be first round
                    curr.MatchupRound = 1;
                    output.Add(curr);
                    curr = new MatchupModel();

                    if (byes > 0)
                    {
                        byes -= 1;
                    }
                }
            }
            return output;
        }

        // No byes in other rounds
        private static void CreateOtherRounds(TournamentModel model, int rounds)
        {
            int round = 2;
            // Take matchup data from previous round
            List<MatchupModel> previousRound = model.Rounds[0];
            List<MatchupModel> currRound = new List<MatchupModel>();
            MatchupModel currMatchup = new MatchupModel();

            while (round <= rounds)
            {
                foreach (MatchupModel match in previousRound)
                {
                    // For each match in previous round, it will generate a new matchup entry (half of a match) in this round
                    currMatchup.Entries.Add(new MatchupEntryModel { ParentMatchup = match });

                    // If matchup have 2 entries, move to new MatchupModel
                    // > 1 to handle error cases where Count goes beyond 3 -> still stop adding to current MatchupModel -- this can throw an error instead
                    if (currMatchup.Entries.Count > 1)
                    {
                        currMatchup.MatchupRound = round;
                        currRound.Add(currMatchup);
                        currMatchup = new MatchupModel();
                    }
                }
                // Add current round to all rounds
                model.Rounds.Add(currRound);

                // Set up for next round - current round becomes previous, new list for next round
                previousRound = currRound;
                currRound = new List<MatchupModel>();
                round += 1;
            }
        }

        private static int NumberOfByes(int rounds, int numberOfTeams)
        {
            // Can use Math.Pow(2, rounds) to get totalTeams, but need to parse double into int
            int totalTeams = 1;

            for (int i = 1; i <= rounds; i++)
            {
                totalTeams *= 2;
            }

            return totalTeams - numberOfTeams;
        }

        private static int FindNumberOfRounds(int teamCount)
        {
            // For every 2 teams, 1 round is needed
            int output = 1;
            int val = 2;

            while (val < teamCount)
            {
                output += 1;
                val *= 2;
            }
            return output;
        }

        private static List<TeamModel> RandomizeTeamOrder(List<TeamModel> teams)
        {
            // Generate a GUID for each team object, and order ascending of it
            return teams.OrderBy(x => Guid.NewGuid()).ToList();
        }
    }
}
