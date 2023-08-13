using IQMania.Helper;
using IQMania.Models.Account;
using IQMania.Repository.AdminRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace IQMania.Controllers
{
    
    
    public class AdminController : Controller
    {
        private IAdminServices? _adminRepository;
        public AdminController(IAdminServices adminRepository)
        {
            _adminRepository = adminRepository;
        }
        public IActionResult Panel()
        {
            return View();
        }

        
        public JsonResult Count()
        {

            var userinfo = HttpContext.GetLoginDetails();
            var role = userinfo?.Role;

            Messages messages = new();
            if (userinfo != null && userinfo.Role?.Contains("AdminUser") == true)
            {

                messages = _adminRepository.GetAdminMessages();
                return Json(messages);
            }
            messages.QuizMessages = 0;
            return Json(messages);

        }
    }
}
