using IQMania.Helper;
using IQMania.Models.Account;
using IQMania.Models.Quiz;
using IQMania.Repository.AdminRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IQMania.Controllers
{
    
    
    public class AdminController : Controller
    {
        private readonly IAdminServices _adminRepository;
        public AdminController(IAdminServices adminRepository)
        {
            _adminRepository = adminRepository;
        }

        [Permission("AdminUser")]
        public IActionResult Panel()
        {
            return View();
        }

        [Permission("AdminUser")]
        public JsonResult Count()
        {

            var userinfo = HttpContext.GetLoginDetails();
            var role = userinfo?.Role;

            Messages messages = new();
            if (userinfo != null && role?.Contains("AdminUser") == true)
            {

                messages = _adminRepository.GetAdminMessages();
                return Json(messages);
            }
            messages.QuizMessages = 0;
            return Json(messages);

        }

        [Permission("AdminUser")]
        public IActionResult Useraddedquestions()
        {
            Getaddquiz getaddquiz = new();
            getaddquiz = _adminRepository.Getuseraddedquestions();
            if(getaddquiz != null)
            {
                    ViewBag.message = getaddquiz.ResponseDescription;
                return View(getaddquiz);
                
            }
            return View();
        }

        [Permission("AdminUser")]
        public IActionResult Userremovedquestions()
        {
            Getaddquiz getaddquiz = new();
            return View(getaddquiz);
        }

        [Permission("AdminUser")]
        public IActionResult Addquestions()
        {
            Getaddquiz getaddquiz = new();
            return View(getaddquiz);
        }

    }
}
