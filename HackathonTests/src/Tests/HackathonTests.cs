using Hackathon.Service;
using Hackathon.Service.Strategy;
using HackathonTests.Model;
using Newtonsoft.Json;

namespace HackathonTests.Tests;

public class HackathonTests
{
    [Test]
    public void HackathonConductTest()
    {
        // Setup
        var hrDirector = new HrDirector();
        ITeamBuildingStrategy teamBuildingStrategy = new MarriageStrategy();
        var hrManager = new HrManager(teamBuildingStrategy);
        var hackathon = new Hackathon.Service.Hackathon(hrManager, hrDirector);

        string jsonData = File.ReadAllText(@"..\..\..\src\TestResources\MarriageStrategyTeamsData.json");

        var testData = JsonConvert.DeserializeObject<TestData>(jsonData);

        // Run
        double satisfactionIndex = hackathon.Conduct(testData.TeamLeads, testData.Juniors, testData.TeamLeadsWishlists,
            testData.JuniorsWishlists);

        // Verify
        Assert.That(satisfactionIndex, Is.EqualTo(testData.SatIndex).Within(0.001));
    }
}