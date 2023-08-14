using Microsoft.AspNetCore.Http;
using IQMania.Models.Account;
using System.Security.Claims;
using System.Xml.Linq;

namespace IQMania.Helper
{
    public static class LoginUserInfo
    {
        public static Account GetLoginDetails(this HttpContext context)
        {
            
            var model = new Account();

                model.UId = context.User.FindFirst("UserId")?.Value.ToString();
                model.Name = context.User.FindFirst("Name")?.Value.ToString();
                model.Email = context.User.FindFirst("Email")?.Value.ToString();
                model.Phonenumber = context.User.FindFirst("Phone")?.Value.ToString();
                model.Role = context.User.FindFirst("Role")?.Value.ToString() ?? string.Empty;
            
           
            return model;
        }
        //var userName = HttpContext.User.FindFirst("Name")?.Value;
        //ViewBag.UserName = userName;
    }
}
