using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe.Checkout;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PaymentController : Controller
    {
        private readonly StripeSettings _stripeSettings;
        private readonly ShoppingCartService _shoppingCartService;

        public PaymentController(IConfiguration configuration, ShoppingCartService shoppingCartService)
        {
            _stripeSettings = configuration.GetSection("Stripe").Get<StripeSettings>();
            _shoppingCartService = shoppingCartService;
        }
        public async Task<IActionResult> Index()
        {
           return View();
        }
        public IActionResult Checkout()
        {
            // تمرير PublicKey إلى View
            ViewBag.StripePublicKey = _stripeSettings.PublicKey;
            return View();
        }

        [HttpPost]
        public IActionResult CreateCheckoutSession()
        {
            var domain = $"{Request.Scheme}://{Request.Host}";

            var cartItems = _shoppingCartService.GetCart(); // استرجاع جميع العناصر من السلة
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
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = itemName // اسم المنتج أو الخضروات
                            },
                            UnitAmount = (long)(itemPrice * 100), // تحويل السعر إلى سنتات
                        },
                        Quantity = item.Quantity // الكمية
                    });
                }
            }

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = domain + "/Payment/Success",
                CancelUrl = domain + "/Payment/Cancel",
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Json(new { id = session.Id });
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Cancel()
        {
            return View();
        }
    }
}
