using Newtonsoft.Json;
using WebApplication1.Data;

namespace WebApplication1.Models
{
    public class ShoppingCartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        public ShoppingCartService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        private List<cartitem> GetCartItems()
        {
            var cart = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<List<cartitem>>("Cart") ?? new List<cartitem>();
            return cart;
        }

        private void SaveCartItems(List<cartitem> cart)
        {
            _httpContextAccessor.HttpContext.Session.SetObjectAsJson("Cart", cart);
        }

        public void AddToCart(int productId)
        {
            var cart = GetCartItems();
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);

            if (product != null)
            {
                var existingItem = cart.FirstOrDefault(c => c.Product?.Id == productId);

                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
                else
                {
                    cart.Add(new cartitem { Product = product, Quantity = 1 });
                }
            }

            SaveCartItems(cart);
        }

        public void AddToCartt(int vegetablesId)
        {
            var cart = GetCartItems();
            var vegetables = _context.vegetables.FirstOrDefault(v => v.Id == vegetablesId);

            if (vegetables != null)
            {
                var existingItem = cart.FirstOrDefault(c => c.VEGETABLES?.Id == vegetablesId);

                if (existingItem != null)
                {
                    existingItem.Quantity++;
                }
                else
                {
                    cart.Add(new cartitem { VEGETABLES = vegetables, Quantity = 1 });
                }
            }

            SaveCartItems(cart);
        }

        public List<cartitem> GetCart()
        {
            return GetCartItems();
        }

        public void RemoveFromCart(int productId)
        {
            var cart = GetCartItems();
            var item = cart.FirstOrDefault(c => c.Product?.Id == productId || c.VEGETABLES?.Id == productId);

            if (item != null)
            {
                cart.Remove(item); 
            }

            SaveCartItems(cart);
        }
        public void RemoveFromCartt(int vegetablesId)
        {
            var cart = GetCartItems();
            var item = cart.FirstOrDefault(c => c.VEGETABLES?.Id == vegetablesId);

            if (item != null)
            {
                cart.Remove(item);
            }

            SaveCartItems(cart);
        }
        public void IncreaseQuantity(int id, bool isProduct)
        {
            var cart = GetCartItems();
            var item = cart.FirstOrDefault(c =>
                isProduct ? c.Product?.Id == id : c.VEGETABLES?.Id == id);

            if (item != null)
            {
                item.Quantity++;
                SaveCartItems(cart);
            }
        }

        public void DecreaseQuantity(int id, bool isProduct)
        {
            var cart = GetCartItems();
            var item = cart.FirstOrDefault(c =>
                isProduct ? c.Product?.Id == id : c.VEGETABLES?.Id == id);

            if (item != null && item.Quantity > 1)
            {
                item.Quantity--;
                SaveCartItems(cart);
            }
        }

        public decimal GetTotal()
        {
            var cartItems = GetCartItems();
            return cartItems.Sum(item =>
            {
                var price = (item.Product?.Price ?? item.VEGETABLES?.Price) ?? 0;
                return price * item.Quantity;
            });
        }

        internal void ClearCart()
        {
            SaveCartItems(new List<cartitem>());
        }
    }

    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
