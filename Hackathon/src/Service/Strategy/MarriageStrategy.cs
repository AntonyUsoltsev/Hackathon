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

            Dictionary<int, List<int>> wishListDictionary = WishListToDictionary(teamLeads, teamLeadsWishlists);

            var freeTeamLeads = new Queue<int>(teamLeads.Select(t => t.Id));

            while (freeTeamLeads.Count > 0)
            {
                int teamLeadId = freeTeamLeads.Dequeue();
                List<int> preferences = wishListDictionary[teamLeadId];

                foreach (int juniorId in preferences)
                {
                    if (!currentMatches.ContainsValue(juniorId))
                    {
                        // Временно принимаем предложение
                        currentMatches[teamLeadId] = juniorId;
                        break;
                    }
                    else
                    {
                        int currentTeamLeadId = currentMatches.FirstOrDefault(x => x.Value == juniorId).Key;

                        int[] juniorPreferences = juniorsWishlists.First(w => w.EmployeeId == juniorId).DesiredEmployees;

                        if (Array.IndexOf(juniorPreferences, teamLeadId) <
                            Array.IndexOf(juniorPreferences, currentTeamLeadId))
                        {
                            currentMatches[currentTeamLeadId] = null; // Отказываем текущему Team Lead
                            currentMatches[teamLeadId] = juniorId;    // Принимаем новое предложение
                            freeTeamLeads.Enqueue(currentTeamLeadId); // Возвращаем текущего Team Lead в очередь
                            break;
                        }
                    }
                }
            }

            List<Team> teams = BuildTeamsFromOffers(teamLeads, currentMatches);
            return teams;
        }

        private Dictionary<int, List<int>> WishListToDictionary(IEnumerable<Employee> teamLeads,
            IEnumerable<Wishlist> teamLeadsWishlists)
        {
            Dictionary<int, List<int>> dict = teamLeads.ToDictionary(t => t.Id, t => new List<int>());
            foreach (var wishlist in teamLeadsWishlists)
            {
                dict[wishlist.EmployeeId].AddRange(wishlist.DesiredEmployees);
            }

            return dict;
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