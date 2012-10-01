using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rpsls.Models.ViewModels
{
	public class StatsViewModel
	{
		public int RockWinCount { get; set; }

		public int ScissorsWinCount { get; set; }

		public int PaperWinCount { get; set; }

		public int LizardWinCount { get; set; }

		public int SpockWinCount { get; set; }

		public int RockLoseCount { get; set; }

		public int ScissorsLoseCount { get; set; }

		public int PaperLoseCount { get; set; }

		public int LizardLoseCount { get; set; }

		public int SpockLoseCount { get; set; }

		public int RockTieCount { get; set; }

		public int ScissorsTieCount { get; set; }

		public int PaperTieCount { get; set; }

		public int LizardTieCount { get; set; }

		public int SpockTieCount { get; set; }

		public int TotalWinCount { get; set; }

		public int TotalLoseCount { get; set; }

		public int TotalTieCount { get; set; }



		public double RockWinRate { get; set; }

		public double RockLoseRate { get; set; }

		public double RockTieRate { get; set; }

		public double PaperWinRate { get; set; }

		public double PaperLoseRate { get; set; }

		public double PaperTieRate { get; set; }

		public double ScissorsWinRate { get; set; }

		public double ScissorsLoseRate { get; set; }

		public double ScissorsTieRate { get; set; }

		public double LizardWinRate { get; set; }

		public double LizardLoseRate { get; set; }

		public double LizardTieRate { get; set; }

		public double SpockWinRate { get; set; }

		public double SpockLoseRate { get; set; }

		public double SpockTieRate { get; set; }

		public double TotalWinRate { get; set; }

		public double TotalLoseRate { get; set; }

		public double TotalTieRate { get; set; }

		public int RockTotalCount { get; set; }

		public int TotalTotalCount { get; set; }

		public int SpockTotalCount { get; set; }

		public int LizardTotalCount { get; set; }

		public int ScissorsTotalCount { get; set; }

		public int PaperTotalCount { get; set; }
	}
}