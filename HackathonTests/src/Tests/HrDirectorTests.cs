using Hackathon.Service;
using HackathonTests.Model;
using Newtonsoft.Json;

namespace HackathonTests.Tests;

public class HrDirectorTests
{
    [Test]
    public void HarmonicMeanCountTest()
    {
        List<int> numbers1 = [4, 4, 4, 4, 4, 4];
        Assert.That(HarmonicMeanCount.CountHarmonicMean(numbers1), Is.EqualTo(4));

        List<int> numbers2 = [1, 2, 3, 4];
        Assert.That(HarmonicMeanCount.CountHarmonicMean(numbers2), Is.EqualTo(1.92d).Within(0.0001));
    }


    [Test]
    public void MarriageStrategySatisfactionCountTest()
    {
        // Setup
        string jsonData = File.ReadAllText(@"..\..\..\src\TestResources\MarriageStrategyTeamsData.json");
        var testData = JsonConvert.DeserializeObject<TestData>(jsonData);

        // Run
        double satisfactionIndex =
            HarmonicMeanCount.CountSatisfaction(testData.Teams, testData.TeamLeadsWishlists, testData.JuniorsWishlists);

        // Verify
        Assert.That(satisfactionIndex, Is.LessThan(testData.Teams.Count));
        Assert.That(satisfactionIndex, Is.EqualTo(testData.SatIndex).Within(0.001));
    }


    [Test]
    public void GreedyStrategySatisfactionCountTest()
    {
        // Setup
        string jsonData = File.ReadAllText(@"..\..\..\src\TestResources\GreedyStrategyTeamsData.json");
        var testData = JsonConvert.DeserializeObject<TestData>(jsonData);

        // Run
        double satisfactionIndex =
            HarmonicMeanCount.CountSatisfaction(testData.Teams, testData.TeamLeadsWishlists, testData.JuniorsWishlists);

        // Verify
        Assert.That(satisfactionIndex, Is.LessThan(testData.Teams.Count));
        Assert.That(satisfactionIndex, Is.EqualTo(testData.SatIndex).Within(0.001));
    }
}