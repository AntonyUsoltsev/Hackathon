using Hackathon.DataBase;
using Hackathon.DataBase.Dto;
using Hackathon.Model;

namespace Hackathon.Repository;

public class WishlistRepository(HackathonContext context)
{
    public IEnumerable<Wishlist> GetWishlistsByHackathonId(int hackathonId)
    {
        List<WishlistDto> wishlistDtos = context.Wishlists
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
            var wishlistDto = new WishlistDto(wishlist.EmployeeId, wishlist.DesiredEmployees[i],
                wishlist.DesiredEmployees.Length - i, hackathonId);
            context.Wishlists.Add(wishlistDto);
            context.SaveChanges();
        }
    }
}