using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly ShoppingCartService _shoppingCartService;
        private readonly StripeSettings _stripeSettings;

        public ShoppingCartController(ShoppingCartService shoppingCartService, IOptions<StripeSettings> stripeSettings)
        {
            _shoppingCartService = shoppingCartService;
            _stripeSettings = stripeSettings.Value;
        }

        // عرض محتويات سلة التسوق والمجموع الكلي
        public IActionResult Index()
        {
            var cart = _shoppingCartService.GetCart();
            var total = _shoppingCartService.GetTotal();
            ViewBag.Total = total;
            ViewData["StripePublicKey"] = _stripeSettings.PublicKey;
            return View(cart);
        }

        // إضافة منتج إلى السلة
        public IActionResult AddToCart(int productId)
        {
            _shoppingCartService.AddToCart(productId);
            return RedirectToAction("Index");
        }

        // إزالة منتج من السلة
        public IActionResult RemoveFromCart(int productId)
        {
            _shoppingCartService.RemoveFromCart(productId);
            return RedirectToAction("Index");
        }

        // إزالة خضار من السلة
        public IActionResult RemoveFromCartt(int vegetablesId)
        {
            _shoppingCartService.RemoveFromCartt(vegetablesId);
            return RedirectToAction("Index");
        }

        // إنشاء جلسة Checkout في Stripe
        [HttpPost]
        public IActionResult CreateCheckoutSession()
        {
            var domain = $"{Request.Scheme}://{Request.Host}";

            // استرجاع العناصر من السلة
            var cartItems = _shoppingCartService.GetCart();
            var lineItems = new List<SessionLineItemOptions>();

            foreach (var item in cartItems)
            {
                string itemName = item.Product?.Name ?? item.VEGETABLES?.Name;
                decimal itemPrice = item.Product?.Price ?? item.VEGETABLES?.Price ?? 0;

                if (item.Product != null || item.VEGETABLES != null)
                {
                    lineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",  // العملة
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = itemName  // اسم المنتج أو الخضروات
                            },
                            UnitAmount = (long)(itemPrice * 100), // تحويل السعر إلى سنتات
                        },
                        Quantity = item.Quantity  // الكمية
                    });
                }
            }

            // إعدادات الجلسة في Stripe
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = lineItems,
                Mode = "payment", // وضع الدفع
                SuccessUrl = domain + "/Payment/Success",  // رابط النجاح
                CancelUrl = domain + "/Payment/Cancel",    // رابط الإلغاء
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Json(new { id = session.Id });  // إرسال sessionId إلى الـ JavaScript
        }

        // زيادة كمية منتج
        public IActionResult IncreaseQuantity(int id, string type)
        {
            if (type == "product")
            {
                _shoppingCartService.IncreaseQuantity(id, isProduct: true);
            }
            else if (type == "vegetables")
            {
                _shoppingCartService.IncreaseQuantity(id, isProduct: false);
            }

            return RedirectToAction("Index");
        }

        // تقليل كمية منتج
        public IActionResult DecreaseQuantity(int id, string type)
        {
            if (type == "product")
            {
                _shoppingCartService.DecreaseQuantity(id, isProduct: true);
            }
            else if (type == "vegetables")
            {
                _shoppingCartService.DecreaseQuantity(id, isProduct: false);
            }

            return RedirectToAction("Index");
        }

        // عرض صفحة نجاح الدفع
        public IActionResult Success()
        {
            ViewBag.Message = "تم الدفع بنجاح!";
            return View();
        }

        // عرض صفحة فشل الدفع
        public IActionResult Cancel()
        {
            ViewBag.Message = "تم إلغاء الدفع.";
            return View();
        }

        // صفحة الدفع
        public IActionResult Checkout()
        {
            return View();
        }
    }
}
