using Hackathon.Model;

namespace Hackathon.Service.Strategy
{
    public class MarriageStrategy : ITeamBuildingStrategy
    {
        public IEnumerable<Team> BuildTeams(
            IEnumerable<Employee> teamLeads,
            IEnumerable<Employee> juniors,
            IEnumerable<Wishlist> teamLeadsWishlists,
            IEnumerable<Wishlist> juniorsWishlists)
        {
            var currentMatches = new Dictionary<int, int?>();

            var proposals = WishListToDictionary(teamLeads, teamLeadsWishlists);

            var freeTeamLeads = new Queue<int>(teamLeads.Select(t => t.Id));

            while (freeTeamLeads.Count > 0)
            {
                var teamLeadId = freeTeamLeads.Dequeue();
                var preferences = proposals[teamLeadId];

                foreach (var juniorId in preferences)
                {
                    if (!currentMatches.ContainsValue(juniorId))
                    {
                        // Временно принимаем предложение
                        currentMatches[teamLeadId] = juniorId;
                        break;
                    }
                    else
                    {
                        var currentTeamLeadId = currentMatches.FirstOrDefault(x => x.Value == juniorId).Key;

                        var juniorWishlist = juniorsWishlists.First(w => w.EmployeeId == juniorId).DesiredEmployees;

                        if (Array.IndexOf(juniorWishlist, teamLeadId) <
                            Array.IndexOf(juniorWishlist, currentTeamLeadId))
                        {
                            currentMatches[currentTeamLeadId] = null; // Отказываем текущему Team Lead
                            currentMatches[teamLeadId] = juniorId; // Принять новое предложение
                            freeTeamLeads.Enqueue(currentTeamLeadId); // Возвращаем текущего Team Lead в очередь
                            break;
                        }
                    }
                }
            }

            var teams = BuildTeamsFromOffers(teamLeads, currentMatches);
            return teams;
        }

        private Dictionary<int, List<int>> WishListToDictionary(IEnumerable<Employee> teamLeads,
            IEnumerable<Wishlist> teamLeadsWishlists)
        {
            var proposals = teamLeads.ToDictionary(t => t.Id, t => new List<int>());
            foreach (var wishlist in teamLeadsWishlists)
            {
                proposals[wishlist.EmployeeId].AddRange(wishlist.DesiredEmployees);
            }

            return proposals;
        }

        private List<Team> BuildTeamsFromOffers(IEnumerable<Employee> teamLeads, Dictionary<int, int?> currentMatches)
        {
            var teams = new List<Team>();
            foreach (var match in currentMatches)
            {
                if (match.Value.HasValue)
                {
                    var teamLead = teamLeads.First(tl => tl.Id == match.Key);
                    var junior = teamLeads.First(jn => jn.Id == match.Value);
                    teams.Add(new Team(teamLead, junior));
                }
            }

            return teams;
        }
    }
}