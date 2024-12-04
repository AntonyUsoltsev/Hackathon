namespace Contract;

public record WishlistMessage(
    Wishlist Wishlist,
    Role Role,
    int hackathonId
);