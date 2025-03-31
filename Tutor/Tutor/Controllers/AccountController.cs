using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tutor.Data;
using Tutor.Models;

namespace Tutor.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;
        

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = _userManager.GetUserId(User);

            var courses = await _context.Applications
                .Where(a => a.UserId == userId && a.Status == "Approved")
                .Select(a => a.Course)
                .ToListAsync();

            var model = new ProfileViewModel
            {
                FullName = User.Identity.Name,
                Email = User.FindFirstValue(ClaimTypes.Email),
                Phone = User.FindFirstValue(ClaimTypes.MobilePhone),
                Courses = courses
            };

            return View(model);
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Email, FullName = model.FullName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
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
            if (!ModelState.IsValid)
                return View(model);

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Неверный email или пароль");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult UpdateProfile(UpdateProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Profile", model);

            var user = _userManager.GetUserAsync(User).Result;
            if (user == null)
                return NotFound();

            user.Email = model.Email;
            user.PhoneNumber = model.Phone;
            if (!string.IsNullOrEmpty(model.Password))
            {
                var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
                var result = _userManager.ResetPasswordAsync(user, token, model.Password).Result;
                if (!result.Succeeded)
                    return View("Profile", model);
            }

            _userManager.UpdateAsync(user).Wait();
            return RedirectToAction("Profile");
        }
    }

}
