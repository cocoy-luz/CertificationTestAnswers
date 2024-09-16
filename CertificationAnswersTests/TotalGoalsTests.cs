//using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NUnit.Framework;

namespace CertificationAnswers.Tests
{
    [TestFixture()]
    public class TotalGoalsTests
    {
        private Mock<HttpClient> httpClientMock;
        private TotalGoals totalGoals;

        [SetUp]
        public void SetUp()
        {
            httpClientMock = new Mock<HttpClient>();
            totalGoals = new TotalGoals(httpClientMock.Object);
        }

        [Test()]
        public async Task GetGoalsForTeam_ReturnsCorrectTotalGoals_WhenMultiplePages()
        {
            // Arrange
            string team = "TeamA";
            int year = 2021;
            string teamPosition = "team1";
            string jsonResponsePage1 = "{\"total_pages\": 2, \"per_page\": 10, \"data\": [{\"team1goals\": 2}, {\"team1goals\": 1}]}";
            string jsonResponsePage2 = "{\"total_pages\": 2, \"per_page\": 10, \"data\": [{\"team1goals\": 3}]}";

            httpClientMock.Setup(client => client.GetStringAsync(It.IsAny<string>()))
                .ReturnsAsync(jsonResponsePage1)
                .Callback(() => httpClientMock.Setup(client => client.GetStringAsync(It.IsAny<string>()))
                    .ReturnsAsync(jsonResponsePage2));

            // Act
            int result = await totalGoals.GetGoalsForTeam(team, year, teamPosition);

            // Assert
            Assert.That(6, Is.EqualTo(result)); // 2 + 1 + 3 = 6
        }

        [Test]
        public async Task GetGoalsForTeam_ReturnsZero_WhenNoGoals()
        {
            // Arrange
            string team = "TeamB";
            int year = 2021;
            string teamPosition = "team1";
            string jsonResponse = "{\"total_pages\": 1, \"per_page\": 10, \"data\": [{\"team1goals\": 0}]}";

            httpClientMock.Setup(client => client.GetStringAsync(It.IsAny<string>()))
                .ReturnsAsync(jsonResponse);

            // Act
            int result = await totalGoals.GetGoalsForTeam(team, year, teamPosition);

            // Assert
            Assert.That(0, Is.EqualTo(result));
        }

        [Test]
        public async Task GetGoalsForTeam_ReturnsZero_WhenNoMatches()
        {
            // Arrange
            string team = "TeamD";
            int year = 2021;
            string teamPosition = "team1";
            string jsonResponse = "{\"total_pages\": 1, \"per_page\": 10, \"data\": []}";

            httpClientMock.Setup(client => client.GetStringAsync(It.IsAny<string>()))
                .ReturnsAsync(jsonResponse);

            // Act
            int result = await totalGoals.GetGoalsForTeam(team, year, teamPosition);

            // Assert
            Assert.That(0, Is.EqualTo(result));
        }

        [Test]
        public async Task GetGoalsForTeam_ReturnsCorrectGoals_WhenSinglePage()
        {
            // Arrange
            string team = "TeamC";
            int year = 2021;
            string teamPosition = "team1";
            string jsonResponse = "{\"total_pages\": 1, \"per_page\": 10, \"data\": [{\"team1goals\": 1}, {\"team1goals\": 2}]}";

            httpClientMock.Setup(client => client.GetStringAsync(It.IsAny<string>()))
                .ReturnsAsync(jsonResponse);

            // Act
            int result = await totalGoals.GetGoalsForTeam(team, year, teamPosition);

            // Assert
            Assert.That(3, Is.EqualTo(result)); // 1 + 2 = 3
        }

    }
}