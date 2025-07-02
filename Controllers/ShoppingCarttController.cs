using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using WebApplication1.Models;
 
namespace WebApplication1.Controllers
{
	public class ShoppingCarttController : Controller
	{
		private readonly ShoppingCartService _shoppingCartService;
		private readonly StripeSettings _stripeSettings;

		// Constructor to inject ShoppingCartService and Stripe settings
		public ShoppingCarttController(ShoppingCartService shoppingCartService, IOptions<StripeSettings> stripeSettings)
		{
			_shoppingCartService = shoppingCartService;
			_stripeSettings = stripeSettings.Value;
		}

		// Action to view the cart and total price

		public IActionResult Index()
		{
			var cart = _shoppingCartService.GetCart();
			var total = _shoppingCartService.GetTotal();
			ViewBag.Total = total;
			ViewData["StripePublicKey"] = _stripeSettings.PublicKey;
			return View(cart);
		}


		// Action to add a product to the cart
		public IActionResult AddToCartt(int vegetablesId)
		{
			_shoppingCartService.AddToCartt(vegetablesId);
			return RedirectToAction("Index");  // أو إلى صفحة العربة
		}


		// Action to remove a product from the cart
		public IActionResult RemoveFromCartt(int vegetablesId)
		{
			_shoppingCartService.RemoveFromCart(vegetablesId);
			return RedirectToAction("Index");
		}
		  
		// Action to create a Stripe Checkout Session
		[HttpPost]
		public IActionResult CreateCheckoutSession()
		{
			var cartItems = _shoppingCartService.GetCart();
			var options = new SessionCreateOptions
			{
				PaymentMethodTypes = new List<string> { "card" },
				LineItems = new List<SessionLineItemOptions>()
			};

			foreach (var item in cartItems)
			{
				options.LineItems.Add(new SessionLineItemOptions
				{
					PriceData = new SessionLineItemPriceDataOptions
					{
						Currency = "usd",
						ProductData = new SessionLineItemPriceDataProductDataOptions
						{
							Name = item.VEGETABLES.Name
						},
						UnitAmount = (long)(item.VEGETABLES.Price * 100), // تحويل السعر إلى سنتات
					},
					Quantity = item.Quantity
				});
			}

			options.Mode = "payment";
			options.SuccessUrl = Url.Action("Success", "ShoppingCart", null, Request.Scheme);
			options.CancelUrl = Url.Action("Cancel", "ShoppingCart", null, Request.Scheme);

			var service = new SessionService();
			Session session = service.Create(options);

			return Json(new { id = session.Id });
		}

		public IActionResult Success()
		{
			ViewBag.Message = "Payment successful!";
			return View();
		}

		public IActionResult Cancel()
		{
			ViewBag.Message = "Payment was cancelled.";
			return View();
		}
		public IActionResult Checkout()
		{
			return View();
		}

	}
}
