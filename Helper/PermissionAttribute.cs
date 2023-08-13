using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace IQMania.Helper
{
    public class PermissionAttribute : TypeFilterAttribute
    {
        public PermissionAttribute(params string[] ids) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { ids };
        }

        private class ClaimRequirementFilter : IAuthorizationFilter
        {
            private readonly string[] _ids;
            public ClaimRequirementFilter(string[] ids)
            {
                _ids = ids;
            }
            public void OnAuthorization(AuthorizationFilterContext filterContext)
            {
                var IsAuthenticated = filterContext.HttpContext.User.Identity?.IsAuthenticated;
                var claimsIdentity = filterContext.HttpContext.User.Identity as ClaimsIdentity;
                bool isAuthenticated = (bool)IsAuthenticated;
                if (isAuthenticated)
                {
                    var role = claimsIdentity.FindFirst(c => c.Type == "Role")?.Value;
                    bool hasRole = false;
                    foreach(var id in _ids)
                    {
                        if (role == id) {
                            hasRole = true;
                        }
                    }
                   
                    if (!hasRole) {
                        filterContext.HttpContext.Response.StatusCode = 401;
                        filterContext.HttpContext.Response.Redirect("/Quiz/Index");
                    }
                    
                }
                else
                {
                    var request = filterContext.HttpContext.Request;
                    var returnUrl = request.Path;
                    var queryString = request.QueryString.Value;
                
                if (!string.IsNullOrEmpty(queryString))
                {
                    returnUrl += queryString;
                }

                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        action = "Index",
                        controller = "Account",
                        returnUrl = returnUrl.Value
                    }));
                }
            }

        }
    }
}
    
