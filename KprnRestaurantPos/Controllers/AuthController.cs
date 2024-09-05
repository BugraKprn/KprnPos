using Entity.Layer.Concrete;
using KprnRestaurantPos.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KprnRestaurantPos.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<Employee> _signInManager;
        private readonly UserManager<Employee> _userManager;

        public AuthController(
            UserManager<Employee> userManager,
            SignInManager<Employee> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kullanıcıyı giriş yapmaya çalış
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    // Giriş başarılı olduğunda, kullanıcının hesap durumunu kontrol et
                    var user = await _userManager.FindByNameAsync(model.Username);
                    if (user != null && user.Status == false)
                    {

                        // Kullanıcı pasif ise, oturumu kapat ve giriş sayfasına yönlendir
                        await _signInManager.SignOutAsync();
                        return RedirectToAction("Login");
                    }

                    // Kullanıcı pasif değilse, belirtilen yola yönlendir
                    return Redirect("/Pos/Dashboard");
                }
                if (result.IsLockedOut)
                {
                    // Kullanıcı hesabı kilitli ise, giriş yapmaya çalışma ve kilitleme mesajı göster
                    ModelState.AddModelError(string.Empty, "Hesabınız kilitlendi, lütfen bir süre sonra tekrar deneyin.");
                    return View(model);
                }
                else
                {
                    // Giriş başarısız ise, hata mesajı göster
                    ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı veya şifre.");
                    return View(model);
                }
            }

            // Model geçerli değilse, giriş sayfasını tekrar göster
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Auth");
        }
    }
}
