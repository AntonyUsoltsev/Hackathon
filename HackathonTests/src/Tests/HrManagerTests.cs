using Hackathon.Model;
using Hackathon.Service;
using Hackathon.Service.Strategy;
using HackathonTests.Model;
using Moq;
using Newtonsoft.Json;

namespace HackathonTests.Tests;

public class HrManagerTests
{
    [Test]
    public void HrManagerMarriageStrategyTest()
    {
        // Setup
        ITeamBuildingStrategy testStrategy = new MarriageStrategy();
        var hrManagerMock = new Mock<HrManager>(testStrategy)
        {
            CallBase = true
        };

        string jsonData = File.ReadAllText(@"..\..\..\src\TestResources\MarriageStrategyTeamsData.json");
        var data = JsonConvert.DeserializeObject<TestData>(jsonData);

        // Run
        IEnumerable<Team> actualTeams =
            hrManagerMock.Object.FormTeams(data.TeamLeads, data.Juniors, data.TeamLeadsWishlists, data.JuniorsWishlists);

        // Verify
        Assert.That(data.Teams, Has.Count.EqualTo(actualTeams.Count()));
        Assert.That(actualTeams, Is.EquivalentTo(data.Teams));
        hrManagerMock.Verify(x => x.FormTeams(It.IsAny<IEnumerable<Employee>>(),
                It.IsAny<IEnumerable<Employee>>(),
                It.IsAny<IEnumerable<Wishlist>>(),
                It.IsAny<IEnumerable<Wishlist>>()),
            Times.Once);
    }

    [Test]
    public void HrManagerGreedyStrategyTest()
    {
        // Setup
        ITeamBuildingStrategy testStrategy = new GreedyStrategy();
        var hrManagerMock = new Mock<HrManager>(testStrategy)
        {
            CallBase = true
        };

        string jsonData = File.ReadAllText(@"..\..\..\src\TestResources\GreedyStrategyTeamsData.json");
        var data = JsonConvert.DeserializeObject<TestData>(jsonData);
        
        // Run
        IEnumerable<Team> actualTeams =
            hrManagerMock.Object.FormTeams(data.TeamLeads, data.Juniors, data.TeamLeadsWishlists,
                data.JuniorsWishlists);

        // Verify
        Assert.That(data.Teams, Has.Count.EqualTo(actualTeams.Count()));
        Assert.That(actualTeams, Is.EquivalentTo(data.Teams));
        hrManagerMock.Verify(x => x.FormTeams(It.IsAny<IEnumerable<Employee>>(),
                It.IsAny<IEnumerable<Employee>>(),
                It.IsAny<IEnumerable<Wishlist>>(),
                It.IsAny<IEnumerable<Wishlist>>()),
            Times.Once);
    }
}