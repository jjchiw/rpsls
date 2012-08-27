using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Security;
using Raven.Client;
using Rpsls.Models;
using Rpsls.Helpers;
using System.Web.Security;

namespace Rpsls.Modules
{
	public class UserModule : RavenModule
	{
		public UserModule() : base("user")
		{
			this.RequiresAuthentication();

			Post["/"] = parameter =>
			{
				var user = (Context.CurrentUser as User);

                user.UserName = Request.Form.Username;

				RavenSession.Store(user);

				Context.CurrentUser = user;

				return Response.AsRedirect("/User/");
			};

            Post["/token"] = parameter =>
            {
                var user = (Context.CurrentUser as User);

				user.Token = Membership.GeneratePassword(6, 3);//new Random().Next(1, 10).ToString();

				RavenSession.Store(user);
                    
                return Response.AsJson(new {Token = user.Token}, HttpStatusCode.OK );
            };

			Get["/"] = parameter =>
			{
				var m = Context.Model("Edit User");
                m.Token = (Context.CurrentUser as User).Token;
				return View["User/Edit", m];
			};

			Get[".json"] = parameter =>
			{
				var m = Context.Model("Edit User");
				return Response.AsJson(new { m });
			};
		}
	}
}