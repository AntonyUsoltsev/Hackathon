using Contract;

namespace HrManager.Service;

public interface ITeamBuilder
{
    public void SaveJuniorWishlist(WishlistMessage dto);
    public void SaveTeamLeadWishlist(WishlistMessage dto);
}