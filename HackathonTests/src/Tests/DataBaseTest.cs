using Hackathon.DataBase;
using Hackathon.Repository;
using Microsoft.EntityFrameworkCore;

namespace HackathonTests.Tests;

[TestFixture]
public class DataBaseTests : IDisposable
{
    private HackathonContext _context;
    private HackathonRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<HackathonContext>()
            .UseSqlite("DataSource=:memory:")
            .Options;

        _context = new HackathonContext(options);
        _context.Database.OpenConnection();
        _context.Database.EnsureCreated();

        _repository = new HackathonRepository(_context);
    }

    [Test]
    public void CreateEmptyHackathonTest()
    {
        // Run
        var hackathon = _repository.CreateEmptyHackathon();

        // Verify
        var hackathonInDb = _context.Hackathons.SingleOrDefault(h => h.Id == hackathon.Id);
        Assert.That(hackathonInDb, Is.Not.Null);
        Assert.That(hackathon.Id, Is.EqualTo(hackathonInDb.Id));
    }

    [Test]
    public void SaveNewHackathonTest()
    {
        // Setup
        double expectedResultHarmony = 85.5;

        // Run
        var hackathon = _repository.SaveNewHackathon(expectedResultHarmony);

        // Verify
        var hackathonInDb = _context.Hackathons.SingleOrDefault(h => h.Id == hackathon.Id);
        Assert.That(hackathonInDb, Is.Not.Null);
        Assert.That(hackathonInDb.ResultHarmony, Is.EqualTo(expectedResultHarmony));
    }

    [Test]
    public void UpdateHackathonTest()
    {
        // Setup
        double expectedResultHarmony = 14.5;

        // Run
        var hackathon = _repository.CreateEmptyHackathon();
        hackathon = _repository.UpdateHackathonResult(hackathon.Id, expectedResultHarmony);

        // Verify
        var hackathonInDb = _context.Hackathons.SingleOrDefault(h => h.Id == hackathon.Id);
        Assert.That(hackathonInDb, Is.Not.Null);
        Assert.That(hackathonInDb.ResultHarmony, Is.EqualTo(expectedResultHarmony));
    }

    [Test]
    public void GetHackathonByIdTest()
    {
        // Setup
        double resultHarmony = 75.0;
        var savedHackathon = _repository.SaveNewHackathon(resultHarmony);

        // Run
        var hackathonInDb = _repository.GetHackathonById(savedHackathon.Id);

        // Verify
        Assert.That(hackathonInDb, Is.Not.Null);
        Assert.That(hackathonInDb.ResultHarmony, Is.EqualTo(resultHarmony));
    }

    [Test]
    public void AverageResultHarmony_CalculatesCorrectAverage()
    {
        // Setup
        _repository.SaveNewHackathon(80.0);
        _repository.SaveNewHackathon(90.0);
        _repository.SaveNewHackathon(70.0);

        // Run
        double averageHarmony = _repository.AverageResultHarmony();

        // Verify
        Assert.That(averageHarmony, Is.EqualTo(80.0).Within(0.01));
    }

    [TearDown]
    public void Dispose()
    {
        _context.Database.CloseConnection();
    }
}