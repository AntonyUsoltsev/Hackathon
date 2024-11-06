using HrManager.Model;

namespace HrManager.Service;

public interface ITeamBuilder
{
    public void SaveJuniorWishlist(Wishlist wishlist);
    public void SaveTeamLeadWishlist(Wishlist wishlist);
}