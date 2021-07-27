using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace kn.web.core.Controllers
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        /// <summary>  
        /// This will Authorize User  
        /// </summary>  
        /// <returns></returns>  
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            if (filterContext != null)
            {
                ClaimsIdentity claimsIdentity = filterContext.HttpContext.User.Identity as ClaimsIdentity;
                Claim isAdmin = claimsIdentity.Claims.Where(c => c.Type == "IsAdmin").FirstOrDefault();
                if (isAdmin != null && isAdmin.Value == "1") {
                }
                else
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Unauthorized";
                    filterContext.Result = new JsonResult("Unauthorized")
                    {
                        Value = new
                        {
                            Status = "Unauthorized",
                            Message = "No privileges"
                        },
                    };
                }
            }
        }

        public bool IsValidToken(string authToken)
        {
            //validate Token here  
            return true;
        }
    }
}
