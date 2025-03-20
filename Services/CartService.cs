using electro_shop_backend.Data;
using electro_shop_backend.Exceptions;
using electro_shop_backend.Models.DTOs.Cart;
using electro_shop_backend.Models.Entities;
using electro_shop_backend.Models.Mappers;
using electro_shop_backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace electro_shop_backend.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CartDto>> GetAllCartASyncs()
        {
            return await _context.Carts
                .AsNoTracking()
                .Include(cartitem => cartitem.CartItems)
                .Include(username => username.User)
                .Select(cart => cart.ToCartDto())
                .ToListAsync();
        }

        public async Task<CartItemDto> AddToCartAsync(string userId, int productId, int quantity)
        {
            DateTime utcVN = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "SE Asia Standard Time");

            var cart = await _context.Carts
                .Include(cartitem => cartitem.CartItems)
                .FirstOrDefaultAsync(cart => cart.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    TimeStamp = utcVN
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var cartItem = cart.CartItems.FirstOrDefault(cartitem => cartitem.ProductId == productId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = productId,
                    Quantity = quantity
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            await _context.SaveChangesAsync();
            return cartItem.ToCartItemDto(); 
        }

        public async Task<List<UserCartDto>> GetCartByUserIdAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(cart => cart.CartItems)
                .ThenInclude(cartitem => cartitem.Product)
                .FirstOrDefaultAsync(cart => cart.UserId == userId);

            if (cart == null)
            {
                throw new NotFoundException("Cart not found");
            }

            var availableProducts = cart.CartItems.Where(cartitem => cartitem.Product != null && cartitem.Product.Stock >0).ToList();

            return availableProducts.Select(cartitem => cartitem.ToUserCartDto()).ToList();
        }

        public async Task<List<CartItemDto>> GetCartByUserIdForAdminAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(cartitem => cartitem.CartItems)
                .FirstOrDefaultAsync(cart => cart.UserId == userId);

            if (cart == null)
            {
                throw new NotFoundException("Cart not found");
            }
            return cart.CartItems.Select(cartitem => cartitem.ToCartItemDto()).ToList();
        }

        public async Task<List<CartItemDto>> GetCartByCartIdAsync(int cartId)
        {
            var cart = await _context.Carts
                .Include(cartitem => cartitem.CartItems)
                .FirstOrDefaultAsync(cart => cart.CartId == cartId);

            if (cart == null)
            {
                throw new NotFoundException("Cart not found");
            }
            return cart.CartItems.Select(cartitem => cartitem.ToCartItemDto()).ToList();
        }

        public async Task<CartItemDto> UpdateCartItemQuantityAsync(string userid, int cartItemId, int quantity)
        {
            var cart = await _context.Carts
                .Include(cartitem => cartitem.CartItems)
                .FirstOrDefaultAsync(cart => cart.UserId == userid);

            if (cart == null) {
                throw new NotFoundException("Cart not found");
            }

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(cartitem => cartitem.CartItemId == cartItemId);

            if (cartItem == null)
            {
                throw new NotFoundException("Cart item not found");
            }

            cartItem.Quantity = quantity;
            await _context.SaveChangesAsync();
            return cartItem.ToCartItemDto();
        }

        public async Task<CartItemDto> UpdateCartItemQuantityForAdminAsync(string userid, int cartItemId, int quantity)
        {
            var cart = await _context.Carts
                .Include(cartitem => cartitem.CartItems)
                .FirstOrDefaultAsync(cart => cart.UserId == userid);

            if (cart == null)
            {
                throw new NotFoundException("Cart not found");
            }

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(cartitem => cartitem.CartItemId == cartItemId);

            if (cartItem == null)
            {
                throw new NotFoundException("Cart item not found");
            }

            cartItem.Quantity = quantity;
            await _context.SaveChangesAsync();
            return cartItem.ToCartItemDto();
        }

        public async Task<bool> DeleteCartAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(cartitem => cartitem.CartItems)
                .FirstOrDefaultAsync(cart => cart.UserId == userId);

            if (cart == null)
            {
                throw new NotFoundException("Cart not found");
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCartItemAsync(string userId, int cartItemId)
        {
            var cart = await _context.Carts
                .Include(cartitem => cartitem.CartItems)
                .FirstOrDefaultAsync(cart => cart.UserId == userId);

            if (cart == null)
            {
                throw new NotFoundException("Cart not found");
            }

            var cartItem = cart.CartItems.FirstOrDefault(cartitem => cartitem.CartItemId == cartItemId);

            if (cartItem == null)
            {
                throw new NotFoundException("Cart item not found");
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCartAdminAsync(string userId)
        {
            var cart = await _context.Carts
                .Include(cartitem => cartitem.CartItems)
                .FirstOrDefaultAsync(cart => cart.UserId == userId);

            if (cart == null)
            {
                throw new NotFoundException("Cart not found");
            }

            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCartItemAdminAsync(string userId, int cartItemId)
        {
            var cart = await _context.Carts
                .Include(cartitem => cartitem.CartItems)
                .FirstOrDefaultAsync(cart => cart.UserId == userId);

            if (cart == null)
            {
                throw new NotFoundException("Cart not found");
            }

            var cartItem = cart.CartItems.FirstOrDefault(cartitem => cartitem.CartItemId == cartItemId);

            if (cartItem == null)
            {
                throw new NotFoundException("Cart item not found");
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
