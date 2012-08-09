using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SignalR.Hubs;

namespace Rpsls.Hubs
{
	public class FreeForAll
	{
		private readonly static Lazy<FreeForAll> _instance = new Lazy<FreeForAll>(() => new FreeForAll());
		public static List<Client> Clients = new List<Client>();

		public Hub Hub { get; set; }

		public static FreeForAll Instance
		{
			get
			{
				return _instance.Value;
			}
		}

		public void SpreadMessage(string message)
		{
			BroadCastMessage(message);
		}

		private void BroadCastMessage(string message)
		{

			var clients = this.Hub.Clients;

			clients.newMessage(message);
			clients.isAlive();
		}

		public void GetClients()
		{
			System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
			string sJSON = oSerializer.Serialize(Clients);

			var clients = this.Hub.Clients;
			clients.userList(sJSON);
		}
	}
}