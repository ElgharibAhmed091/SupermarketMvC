using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.ViewModel; // تأكد من استخدام الـ Models المناسبة لك

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _httpContextAccessor = httpContextAccessor;
    }

    // صفحة التسجيل
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // إعادة التوجيه إلى صفحة تسجيل الدخول بعد التسجيل
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

    // صفحة تسجيل الدخول
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    // تخزين الـ userId في الجلسة بعد تسجيل الدخول بنجاح
                    _httpContextAccessor.HttpContext.Session.SetString("UserId", user.Id);

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }

        return View(model);
    }

    // تسجيل الخروج
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        // إزالة الـ userId من الجلسة بعد تسجيل الخروج
        _httpContextAccessor.HttpContext.Session.Remove("UserId");

        return RedirectToAction("Index", "Home");


    }
    public IActionResult AccessDenied()
    {
        return View(); // عرض صفحة مخصصة للمستخدمين الذين ليس لديهم صلاحية للوصول
    }
}
