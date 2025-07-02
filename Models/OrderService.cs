using WebApplication1.Data;

namespace WebApplication1.Models
{
    public class OrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly ShoppingCartService _cartService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderService(ApplicationDbContext context, ShoppingCartService cartService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _cartService = cartService;
            _httpContextAccessor = httpContextAccessor;
        }

        public void CreateOrder(string userId)
        {
            // في حال كان معرّف المستخدم فارغًا
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("User not logged in");
            }

            var cartItems = _cartService.GetCart();  // جلب محتويات العربة

            // بدء المعاملة لضمان معالجة العمليات في حالة حدوث خطأ
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var order = new Order
                    {
                        UserIdentifier = userId,
                        OrderItems = cartItems.Select(item => new orderitem
                        {
                            ProductId = item.Product.Id,
                            Quantity = item.Quantity,
                            Price = item.Product.Price
                        }).ToList()
                    };

                    _context.Orders.Add(order);  // إضافة الطلب إلى قاعدة البيانات
                    _context.SaveChanges();  // حفظ التغييرات

                    _cartService.ClearCart();  // مسح العربة بعد إتمام الطلب

                    transaction.Commit();  // تأكيد المعاملة
                }
                catch (Exception)
                {
                    transaction.Rollback();  // التراجع في حال حدوث خطأ
                    throw;
                }
            }
        }
    }
}
