﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using NBrowserID;
using Nancy.Authentication.Forms;
using Raven.Client;
using Newtonsoft.Json;
using System.Text;
using Rpsls.Models;

namespace Rpsls.Modules
{
	public class AuthModule : RavenModule
	{
		public AuthModule() : base("auths")
		{
			Post["/login"] = parameters =>
			{
				var authentication = new BrowserIDAuthentication();
				var verificationResult = authentication.Verify(Request.Form.assertion);
				if (verificationResult.IsVerified)
				{
					string email = verificationResult.Email;
					User user = null;

					user = RavenSession.Query<User>()
								.FirstOrDefault(x => x.Email == email);


					if (user == null)
					{
						user = new User
						{
							Email = email,
							UserName = email.Substring(0, email.IndexOf("@")),
							Guid = Guid.NewGuid()
						};

						RavenSession.Store(user);
					}

					//FormsAuthentication.UserLoggedInResponse(user.Guid, DateTime.Now.AddDays(7));
					var jsonResponseString = JsonConvert.SerializeObject(new { email = email });
					var jsonBytes = Encoding.UTF8.GetBytes(jsonResponseString);

					var response = this.LoginWithoutRedirect(user.Guid, DateTime.Now.AddDays(7));
					response.ContentType = "application/json";
					response.Contents = s => s.Write(jsonBytes, 0, jsonBytes.Length);

					return response;
				}

				return new Response
				{
					ContentType = "application/json",
					Contents = null
				};
			};

			Post["/logout"] = parameters =>
			{
				var jsonResponseString = JsonConvert.SerializeObject("/");
				var jsonBytes = Encoding.UTF8.GetBytes(jsonResponseString);

				var response = this.LogoutWithoutRedirect();

				response.ContentType = "application/json";
				response.Contents = s => s.Write(jsonBytes, 0, jsonBytes.Length);

				return response;
			};
		}
	}
}