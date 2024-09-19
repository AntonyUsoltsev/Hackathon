using Hackathon.Model;

namespace Hackathon.Service
{
    public class WishlistService
    {
        private static Random _random = new Random();

        public static List<Wishlist> BuildWishlist(List<Employee> selectors, List<Employee> chooseables)
        {
            List<Wishlist> wishlists = new List<Wishlist>();
            IEnumerable<int> chooseableIds = from с in chooseables select с.Id;
            foreach (var selector in selectors)
            {
                List<int> shuffledNumbers = chooseableIds.OrderBy(x => _random.Next()).ToList();
                Wishlist wishlist = new Wishlist(selector.Id, shuffledNumbers.ToArray());
                wishlists.Add(wishlist);
            }

            return wishlists;
        }
    }
}