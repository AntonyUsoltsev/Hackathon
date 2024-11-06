namespace HrManager.Model;

public record AllDataDto(
    List<Wishlist> teamLeadsWishlists,
    List<Wishlist> juniorsWishlists,
    IEnumerable<Team> formedTeams);