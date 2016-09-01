using AngularJSDemo7.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AngularJSDemo7.Controllers
{
    public class EmployeeController : ApiController
    {
        [HttpGet]
        [CustomAuthorize]
        public HttpResponseMessage Profile()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
