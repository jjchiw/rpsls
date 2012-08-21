using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Rpsls.Models;
using Rpsls.Components;
using System.Dynamic;
using Rpsls.Helpers;

namespace Rpsls.Modules
{
	public class RootModule : RavenModule
	{
		public RootModule()
		{
			Get["/"] = Root;
		}

		private Response Root(dynamic o)
		{
			var m = Context.Model("Rpsls");
			return View["GameSignalR", m];
		}
	}
}