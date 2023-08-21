using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using IQMania.Models.Account;
using IQMania.Repository;
using System.Web;
using IQMania.Controllers;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json;
using IQMania.Helper;
using IQMania.Models.Quiz;
using Microsoft.AspNetCore.Authorization;

namespace IQMania.Controllers
{
    public class AccountController : Controller
    {
        public const string SessionKeyName = "Name";
        public const string SessionKeyEmail = "Email";
        public const string SessionKeyPhone = "PhoneNumber";
        public const string SessionKeyUser = "UID";
        public const string Role = "Role";

        private readonly ILogger<AccountController> _logger;
        private readonly IAccountServices _accountrepository;
        public AccountController(IAccountServices accountrepository, ILogger<AccountController> logger)
        {
            _logger = logger;
            _accountrepository = accountrepository;
            _logger.LogInformation("Account Controller Called");
        }

        [HttpGet]
        public IActionResult Signup()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Signup(Signup signup)
        {
            if (ModelState.IsValid)
            {
                ResponseResult response = _accountrepository.Signup(signup);
                return View(nameof(Login));
            }
            return View(signup);
        }
        [HttpPost]
        public IActionResult Login(Login logindata)
        {
            

            //HttpContext.Session.Clear();
            if (ModelState.IsValid)
            {
                

                Account account =  _accountrepository.Login(logindata);
                if (account.ResponseCode == 404)
                {

                    ViewBag.Message = account.ResponseDescription;
                    ViewBag.Pass = "Forgot Password?";
                    return View();
                }
                else
                {
                    var claims = new List<Claim>
                {
                    new Claim("Name", account.Name),
                    new Claim("UserId", account.UId),
                    new Claim("Email", account.Email),
                    new Claim("Phone", account.Phonenumber),
                    new Claim("Role", account.Role)
                        
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var login =  HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                  new ClaimsPrincipal(claimsIdentity),
                                  new AuthenticationProperties
                                  {
                                      IsPersistent = true,
                                      ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                                  });

                    string ReturnUrl = TempData["ReturnUrl"] as string;

                    if (!string.IsNullOrEmpty(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    _logger.LogInformation("Login page browsed by: {0}, ", account.Name);
                    return RedirectToAction("Index", "Quiz");

                }

            }
            ViewBag.Pass = "Forgot Password?";
            return View(logindata);
        }

        [HttpGet]
        public IActionResult Login(string ReturnUrl)
        {
            TempData["ReturnUrl"] = ReturnUrl;
            return View();
        }
        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SessionKeyName);
            _ = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Quiz");
        }


        
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        
        public IActionResult ForgotPassword(Signup signup)
        {
            if(ModelState.IsValid)
            {
                ResponseResult response = new();
                response =_accountrepository.ChangePassword(signup);
                ViewBag.message = response.ResponseDescription;
                ViewBag.code = response.ResponseCode;
                
                return View();
            }
            return View();
        }
    }
}
