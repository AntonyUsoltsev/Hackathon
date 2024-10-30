using Hackathon.DataBase;
using Hackathon.Repository;
using Hackathon.Service;
using Hackathon.Service.Strategy;
using HackathonTests.Model;
using Microsoft.EntityFrameworkCore;
using Moq;
using Newtonsoft.Json;

namespace HackathonTests.Tests;

public class HackathonTests : IDisposable
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
    public void HackathonConductTest()
    {
        // Setup
        
        var hrDirector = new HrDirector(_repository);
        ITeamBuildingStrategy teamBuildingStrategy = new MarriageStrategy();
        var hrManager = new HrManager(teamBuildingStrategy);
        
        var hackathon = new Hackathon.Service.Hackathon(hrManager, hrDirector, new TeamRepository(_context));

        string jsonData = File.ReadAllText(@"..\..\..\src\TestResources\MarriageStrategyTeamsData.json");

        var testData = JsonConvert.DeserializeObject<TestData>(jsonData);

        // Run
        double satisfactionIndex = hackathon.Conduct(testData.TeamLeads, testData.Juniors, testData.TeamLeadsWishlists,
            testData.JuniorsWishlists, 0);

        // Verify
        Assert.That(satisfactionIndex, Is.EqualTo(testData.SatIndex).Within(0.001));
    }
    [TearDown]
    public void Dispose()
    {
        _context.Database.CloseConnection();
    }
}