using Hackathon.DataBase;
using Hackathon.Repository;
using Hackathon.Service;
using HackathonTests.Model;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;

namespace HackathonTests.Tests;

public class HrDirectorTests: IDisposable
{
    private HackathonContext _context;
    private SatisfactionRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<HackathonContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        _context = new HackathonContext(options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();

        _repository = new SatisfactionRepository(_context);
    }

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
        HrDirector hrDirector = new HrDirector(_repository);
        // Run
        double satisfactionIndex =
            hrDirector.CalculateHarmony(testData.Teams, testData.TeamLeadsWishlists, testData.JuniorsWishlists, 0);

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
        
        HrDirector hrDirector = new HrDirector(_repository);
        // Run
        double satisfactionIndex =
            hrDirector.CalculateHarmony(testData.Teams, testData.TeamLeadsWishlists, testData.JuniorsWishlists, 0);

        // Verify
        Assert.That(satisfactionIndex, Is.LessThan(testData.Teams.Count));
        Assert.That(satisfactionIndex, Is.EqualTo(testData.SatIndex).Within(0.001));
    }
    
    [TearDown]
    public void Dispose()
    {
        _context.Database.CloseConnection();
    }
}