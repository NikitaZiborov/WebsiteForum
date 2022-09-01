using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebsiteForum.DataBase;
using WebsiteForum.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebsiteForum.Controllers
{
    public class AccountController : Controller
    {
        private DataBaseContext db;
        public static User usercurrent = new Models.User();
        public AccountController(DataBaseContext context)
        {
            db = context;
        }
        public IActionResult Thanks()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View(db.Questions.ToList());
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = db.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
                if (user != null)
                {
                    usercurrent = user;
                    return RedirectToAction("Index", "Account");
                }
            }

            ClaimsIdentity identity = null;
            bool isAuthenticate = false;
            if (model.Email == "admin123@ukr.net" && model.Password == "admin123")
            {
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name , model.Email),
                    new Claim(ClaimTypes.Role , "Admin")
                }, CookieAuthenticationDefaults.AuthenticationScheme);
                isAuthenticate = true;
            }
            if (isAuthenticate)
            {
                var principal = new ClaimsPrincipal(identity);
                var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index", "Account");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Email == model.Email && u.NickName == model.NickName);
                if (user == null)
                {
                    db.Users.Add(new User { Name = model.Name, SurName = model.SurName, NickName = model.NickName, BirthDate = model.BirthDate, Email = model.Email, Password = model.Password, ConfirmationPassword = model.ConfirmationPassword });
                    await db.SaveChangesAsync();

                    await Authenticate(model.Email);

                    return RedirectToAction("Thanks", "Account");
                }
            }
            return View(model);
        }
        public async Task Authenticate(string userName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType , userName),
            };

            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
