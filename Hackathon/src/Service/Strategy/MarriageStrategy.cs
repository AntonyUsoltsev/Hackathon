using System.Collections;
using Hackathon.Model;
using Hackathon.Strategy;

namespace Hackathon
{
    public class MarriageStrategy : ITeamBuildingStrategy
    {
        enum Answer
        {
            No = 0,
            Maybe = 1,
            Yes = 2
        }

        public IEnumerable<Team> BuildTeams(
            IEnumerable<Employee> teamLeads,
            IEnumerable<Employee> juniors,
            IEnumerable<Wishlist> teamLeadsWishlists,
            IEnumerable<Wishlist> juniorsWishlists)
        {
            Dictionary<(int teamLeadId, int juniorId), Answer> offers = new Dictionary<(int, int), Answer>();

            Dictionary<int, IEnumerator> teamLeadsChoosesEnumerators = new Dictionary<int, IEnumerator>();

            foreach (var tlWishlist in teamLeadsWishlists)
            {
                teamLeadsChoosesEnumerators.Add(tlWishlist.EmployeeId,
                    tlWishlist.DesiredEmployees.GetEnumerator());
            }

            while (teamLeadsChoosesEnumerators.Count > 0)
            {
                foreach (var tlWishlist in teamLeadsWishlists)
                {
                    int teamLeadId = tlWishlist.EmployeeId;

                    if (!teamLeadsChoosesEnumerators.ContainsKey(teamLeadId))
                        continue;

                    if (teamLeadsChoosesEnumerators[teamLeadId].MoveNext())
                    {
                        int juniorId = (int)teamLeadsChoosesEnumerators[teamLeadId].Current;

                        if (offers.ContainsKey((teamLeadId, juniorId)) && offers[(teamLeadId, juniorId)] != Answer.No)
                            continue;

                        int juniorNewPriority = Array.IndexOf(
                            juniorsWishlists.First(it => it.EmployeeId == juniorId).DesiredEmployees,
                            teamLeadId
                        );

                        var juniorCurrentOffer = offers
                            .Where(o => o.Key.juniorId == juniorId && o.Value != Answer.No)
                            .FirstOrDefault();
                        int juniorCurrentPriority = -1;
                        int currentTeamLead = -1;

                        if (juniorCurrentOffer.Key != default)
                        {
                            currentTeamLead = juniorCurrentOffer.Key.teamLeadId;
                            juniorCurrentPriority = Array.IndexOf(
                                juniorsWishlists.First(it => it.EmployeeId == juniorId).DesiredEmployees,
                                currentTeamLead
                            );
                        }

                        if (juniorNewPriority == 0)
                        {
                            offers[(teamLeadId, juniorId)] = Answer.Yes;
                            if (currentTeamLead != -1)
                            {
                                offers[(currentTeamLead, juniorId)] = Answer.No;
                            }
                        }
                        else if (juniorCurrentPriority == -1 || juniorNewPriority < juniorCurrentPriority)
                        {
                            offers[(teamLeadId, juniorId)] = Answer.Maybe;

                            if (currentTeamLead != -1)
                            {
                                offers[(currentTeamLead, juniorId)] = Answer.No;
                            }
                        }

                        break;
                    }

                    if (!teamLeadsChoosesEnumerators[teamLeadId].MoveNext())
                    {
                        teamLeadsChoosesEnumerators.Remove(teamLeadId);
                    }
                }
            }

            var teams = BuildTeamsFromOffers(teamLeads, offers);

            return teams;
        }

        private List<Team> BuildTeamsFromOffers(IEnumerable<Employee> teamLeads, Dictionary<(int teamLeadId, int juniorId), Answer> offers)
        {
            foreach (var offer in offers.ToList())
            {
                if (offer.Value == Answer.Maybe)
                {
                    offers[offer.Key] = Answer.Yes;
                }
            }

            var teams = new List<Team>();

            foreach (var offer in offers)
            {
                if (offer.Value == Answer.Yes)
                {
                    var teamLead = teamLeads.First(tl => tl.Id == offer.Key.teamLeadId);
                    var junior = teamLeads.First(jn => jn.Id == offer.Key.juniorId);

                    teams.Add(new Team(teamLead, junior));
                }
            }

            return teams;
        }
    }
}