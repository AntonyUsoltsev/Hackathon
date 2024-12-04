using Contract;
using EmployeeService.Model;

namespace EmployeeService.Util;

public class WishListBuilder
{
    private static Random _random = new Random();

    public static Wishlist BuildWishlist(Employee selector, List<Employee> chooseables)
    {
        IEnumerable<int> chooseableIds = from с in chooseables select с.Id;
        List<int> shuffledNumbers = chooseableIds.OrderBy(x => _random.Next()).ToList();
        return new Wishlist(selector.Id, shuffledNumbers.ToArray());
    }
}