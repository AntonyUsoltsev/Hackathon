using Hackathon.Model;
using Hackathon.Service;

namespace HackathonTests.Tests;

public class WishlistTests
{
    [Test]
    public void GenerateWishlistsTest()
    {
        // Setup
        List<Employee> teamLeads =
            CsvReader.ReadCsv(@"..\..\..\..\Hackathon\Resources\Teamleads20.csv");
        List<Employee> juniors =
            CsvReader.ReadCsv(@"..\..\..\..\Hackathon\Resources\Juniors20.csv");
        List<Wishlist> teamLeadsWishlists = WishlistService.BuildWishlist(teamLeads, juniors);
        List<Wishlist> juniorsWishlists = WishlistService.BuildWishlist(juniors, teamLeads);

        Employee junior1 = new Employee(5, "Ильин Тимофей");
        Employee junior2 = new Employee(10, "Галкина Есения");

        Employee teamlead1 = new Employee(5, "Кузнецов Александр");
        Employee teamlead2 = new Employee(10, "Астафьев Андрей");

        // Verify
        Assert.That(teamLeadsWishlists, Has.Count.EqualTo(juniorsWishlists.Count));

        Assert.That(teamLeadsWishlists[0].DesiredEmployees, Does.Contain(junior1.Id));
        Assert.That(teamLeadsWishlists[1].DesiredEmployees, Does.Contain(junior2.Id));

        Assert.That(juniorsWishlists[0].DesiredEmployees, Does.Contain(teamlead1.Id));
        Assert.That(juniorsWishlists[1].DesiredEmployees, Does.Contain(teamlead2.Id));
    }
}