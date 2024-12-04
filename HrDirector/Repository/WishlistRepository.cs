using Contract;
using HrDirector.DataBase;
using HrDirector.DataBase.Dto;
using HrDirector.Model;

namespace HrDirector.Repository;

public class WishlistRepository(HackathonContext context)
{
    public IEnumerable<Wishlist> GetWishlistsByHackathonId(int hackathonId)
    {
        List<WishlistEntity> wishlistDtos = context.Wishlists
            .Where(w => w.HackathonId == hackathonId)
            .ToList();

        List<Wishlist> wishlists = wishlistDtos
            .GroupBy(w => w.EmployeeId)
            .Select(g => new Wishlist(g.Key, g.Select(w => w.ChosenEmployeeId).ToArray()))
            .ToList();

        return wishlists;
    }

    public void SaveWishlist(Wishlist wishlist, int hackathonId)
    {
        for (int i = 0; i < wishlist.DesiredEmployees.Length; i++)
        {
            var wishlistDto = new WishlistEntity(wishlist.EmployeeId, wishlist.DesiredEmployees[i],
                wishlist.DesiredEmployees.Length - i, hackathonId);
            context.Wishlists.Add(wishlistDto);
            context.SaveChanges();
        }
    }
}