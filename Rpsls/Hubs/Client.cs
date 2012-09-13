using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using Rpsls.Models;

namespace Rpsls.Hubs
{
	public class Client
	{

		public string Id { get; set; }
		public string Name { get; set; }
		public bool Waiting { get; set; }
		public string UserId { get; set; }

		[ScriptIgnore]
		public string LastMove { get; set; }

		[ScriptIgnore]
		public DateTime? LastMoveResponse { get; set; }

		[ScriptIgnore]
		public GestureType Gesture
		{
			get
			{
				return (GestureType) Enum.Parse(typeof(GestureType), LastMove);
			}
			
		}

		internal void Reset()
		{
			Waiting = false;
			LastMoveResponse = null;
			LastMove = "";
		}
	}
}