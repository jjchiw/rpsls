using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpsls.Models.Helpers
{
	public class BadgeDenormalized<T> where T : IBadgeDenormalized
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public int Total { get; set; }

		public static implicit operator BadgeDenormalized<T>(T doc)
		{
			return new BadgeDenormalized<T>
			{
				Id = doc.Id,
				Name = doc.Name,
			};
		}
	}
}