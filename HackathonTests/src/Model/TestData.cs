using Hackathon.Model;

namespace HackathonTests.Model;

public class TestData
{
    public List<Employee> TeamLeads { get; set; }
    public List<Employee> Juniors { get; set; }
    public List<Wishlist> TeamLeadsWishlists { get; set; }
    public List<Wishlist> JuniorsWishlists { get; set; }
    public List<Team> Teams { get; set; }

    public double SatIndex { get; set; }
}