using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace kn.web.core.Controllers
{
    [Authorize]
    public abstract class BaseController : ControllerBase
    {

        public BaseController() : base()
        {
        }

 
    }
}
