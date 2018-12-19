using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SanFu.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("CoresDomain")]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
    }
}