using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Net;
using System.Text;
using System.Security.Principal;
using System.Threading;
using AngularJSDemo7.Models;

namespace AngularJSDemo7.Filters
{
    public class CustomAuthorizeAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var authHeader = actionContext.Request.Headers.Authorization;

            if (authHeader != null)
            {
                if (authHeader.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) &&
                    !String.IsNullOrWhiteSpace(authHeader.Parameter))
                {
                    var rawCredentials = authHeader.Parameter;
                    var encoding = Encoding.GetEncoding("iso-8859-1");
                    var Credentials = encoding.GetString(Convert.FromBase64String(rawCredentials));
                    var split = Credentials.Split(':');
                    var username = split[0];
                    var password = split[1];

                    UserContext db = new UserContext();
                    var user = db.UserDetails.FirstOrDefault(s => s.Email == username && s.Password == password);

                    if (user != null)
                    {
                        var principal = new GenericPrincipal(new GenericIdentity(username), null);
                        Thread.CurrentPrincipal = principal;
                        if (HttpContext.Current != null)
                        {
                            HttpContext.Current.User = principal;
                        }
                    }
                }
            }
            else {
                HandleUnauthorized(actionContext);
            }
        }

        void HandleUnauthorized(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate", "Basic Scheme='Unauthorized'");
        }
    }
}