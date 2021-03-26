using Autofac.Extras.Moq;
using Moq;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary;
using TrackerLibrary.DataAccess;
using TrackerLibrary.Models;
using Xunit;

namespace TrackerLibrary.Tests
{
    public class TournamentLogicTest
    {
        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        public void CreateRounds_ShouldAddRoundsToTournamentModel(int numberOfTeams)
        {
            TournamentModel testTournamentModel = GetTestTournament(numberOfTeams);

            TournamentLogic.CreateRounds(testTournamentModel);

            int expectedNumberOfRounds = (int)Math.Ceiling(Decimal.Divide(numberOfTeams, 2));

            Assert.Equal(expectedNumberOfRounds, testTournamentModel.Rounds.Count);
        }

        [Fact]
        public void UpdateTournamentResults_ShouldMarkAndAdvanceWinners()
        {
            // GetLoose - tests passes whenever tested method is called
            // GetStrict - test passes when only the tested method is called
            // AutoMock creates a framework for creating mock objects
            using (AutoMock mock = AutoMock.GetLoose())
            {
                // Subsitute for interface, then mock the interface
                GlobalConfig.Connection = Substitute.For<IDataConnector>();
                GlobalConfig.Connection = mock.Mock<IDataConnector>().Object;

                EmailLogic.emailSender = Substitute.For<IEmailSender>();
                EmailLogic.emailSender = mock.Mock<IEmailSender>().Object;

                TournamentModel testTournamentModel = GetTestTournament(4);

                TournamentLogic.CreateRounds(testTournamentModel);
                testTournamentModel.Rounds[0][0].Entries[0].Score = 1;
                testTournamentModel.Rounds[0][1].Entries[0].Score = 1;
                TournamentLogic.UpdateTournamentResults(testTournamentModel);

                // Winners are recorded
                Assert.NotNull(testTournamentModel.Rounds[0][0].Winner);
                Assert.NotNull(testTournamentModel.Rounds[0][1].Winner);

                // Winners advanced to next round
                Assert.NotNull(testTournamentModel.Rounds[1][0].Entries[0].TeamCompeting);
                Assert.NotNull(testTournamentModel.Rounds[1][0].Entries[1].TeamCompeting);
            }
        }

        [Fact]
        public void AlertUsersToNewRound_HaveEmailAddress_ShouldAlertUsers()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                // Subsitute for interface, then mock the interface
                GlobalConfig.Connection = Substitute.For<IDataConnector>();
                GlobalConfig.Connection = mock.Mock<IDataConnector>().Object;

                EmailLogic.emailSender = Substitute.For<IEmailSender>();
                EmailLogic.emailSender = mock.Mock<IEmailSender>().Object;

                TournamentModel testTournamentModel = GetTestTournament(4);

                TournamentLogic.CreateRounds(testTournamentModel);
                TournamentLogic.AlertUsersToNewRound(testTournamentModel);

                // Email is sent to each team
                mock.Mock<IEmailSender>().Verify(x => x.SendEmail("test@person0.com", It.IsAny<string>(), It.IsAny<string>(), null), Times.Once);
                mock.Mock<IEmailSender>().Verify(x => x.SendEmail("test@person1.com", It.IsAny<string>(), It.IsAny<string>(), null), Times.Once);
                mock.Mock<IEmailSender>().Verify(x => x.SendEmail("test@person2.com", It.IsAny<string>(), It.IsAny<string>(), null), Times.Once);
                mock.Mock<IEmailSender>().Verify(x => x.SendEmail("test@person3.com", It.IsAny<string>(), It.IsAny<string>(), null), Times.Once);
            }
        }

        [Fact]
        public void AlertUsersToNewRound_InvallidEmailAddress_ShouldNotAlertUsers()
        {
            using (AutoMock mock = AutoMock.GetLoose())
            {
                // Subsitute for interface, then mock the interface
                GlobalConfig.Connection = Substitute.For<IDataConnector>();
                GlobalConfig.Connection = mock.Mock<IDataConnector>().Object;

                EmailLogic.emailSender = Substitute.For<IEmailSender>();
                EmailLogic.emailSender = mock.Mock<IEmailSender>().Object;

                TournamentModel testTournamentModel = GetTestTournament(3);
                testTournamentModel.EnteredTeams[0].TeamMembers[0].EmailAddress = "";
                testTournamentModel.EnteredTeams[1].TeamMembers[0].EmailAddress = "test@person";
                testTournamentModel.EnteredTeams[2].TeamMembers[0].EmailAddress = "testperson.com";

                TournamentLogic.CreateRounds(testTournamentModel);
                TournamentLogic.AlertUsersToNewRound(testTournamentModel);

                // Email is not sent
                mock.Mock<IEmailSender>().Verify(x => x.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SmtpClient>()), Times.Never);
            }
        }

        private TournamentModel GetTestTournament(int numberOfTeams)
        {
            return new TournamentModel
            {
                Id = 1,
                TournamentName = "Test",
                EntryFee = 0,
                EnteredTeams = GetTestTeams(numberOfTeams),
                Active = 1
            };
        }

        private List<TeamModel> GetTestTeams(int numberOfTeams)
        {
            List<TeamModel> output = new List<TeamModel>();

            for (int i = 0; i < numberOfTeams; i++)
            {
                output.Add(GetTestTeam(i));
            }
            return output;
        }

        private TeamModel GetTestTeam(int id)
        {
            return new TeamModel
            {
                Id = id,
                TeamName = "Team " + id,
                TeamMembers = new List<PersonModel> {
                        GetTestPerson("Test", "Person" + id, "test@person" + id + ".com", "12333214" + id)
                    }
            };
        }

        private PersonModel GetTestPerson(string firstName, string lastName, string email, string cellPhone)
        {
            return new PersonModel
            {
                Id = 1,
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = email,
                CellphoneNumber = cellPhone
            };
        }

        private List<PrizeModel> GetTestPrize()
        {
            return new List<PrizeModel>
            {
                new PrizeModel
                {
                    Id = 1
                }
            };
        }
    }
}
