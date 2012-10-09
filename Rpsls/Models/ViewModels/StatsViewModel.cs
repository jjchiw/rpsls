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

		public StatsViewModel()
		{
			RockWinRate = 0d;
			RockTieRate = 0d;
			RockLoseRate = 0d;

			PaperWinRate = 0d;
			PaperTieRate = 0d;
			PaperLoseRate = 0d;

			ScissorsWinRate = 0d;
			ScissorsTieRate = 0d;
			ScissorsLoseRate = 0d;

			LizardWinRate = 0d;
			LizardTieRate = 0d;
			LizardLoseRate = 0d;

			SpockWinRate = 0d;
			SpockTieRate = 0d;
			SpockLoseRate = 0d;
		}

		public StatsViewModel(dynamic expando) : this()
		{
			RockLoseCount = expando.RockLoseCount;
			RockTieCount = expando.RockTieCount;
			RockWinCount = expando.RockWinCount;

			PaperLoseCount = expando.PaperLoseCount;
			PaperTieCount = expando.PaperTieCount;
			PaperWinCount = expando.PaperWinCount;

			ScissorsLoseCount = expando.ScissorsLoseCount;
			ScissorsTieCount = expando.ScissorsTieCount;
			ScissorsWinCount = expando.ScissorsWinCount;

			LizardLoseCount = expando.LizardLoseCount;
			LizardTieCount = expando.LizardTieCount;
			LizardWinCount = expando.LizardWinCount;

			SpockLoseCount = expando.SpockLoseCount;
			SpockTieCount = expando.SpockTieCount;
			SpockWinCount = expando.SpockWinCount;

			TotalLoseCount = expando.TotalLoseCount;
			TotalTieCount = expando.TotalTieCount;
			TotalWinCount = expando.TotalWinCount;

			SetTotals();
		}

		private void SetTotals()
		{
			RockTotalCount = RockWinCount + RockLoseCount + RockTieCount;

			if (RockTotalCount > 0)
			{
				RockWinRate = Math.Round((((double)RockWinCount / (double)RockTotalCount) * 100d), 2);
				RockLoseRate = Math.Round((((double)RockLoseCount / (double)RockTotalCount) * 100d), 2);
				RockTieRate = Math.Round((((double)RockTieCount / (double)RockTotalCount) * 100d), 2);
			}


			PaperTotalCount = PaperWinCount + PaperLoseCount + PaperTieCount;
			if (PaperTotalCount > 0)
			{
				PaperWinRate = Math.Round((((double)PaperWinCount / (double)PaperTotalCount) * 100d), 2);
				PaperLoseRate = Math.Round((((double)PaperLoseCount / (double)PaperTotalCount) * 100d), 2);
				PaperTieRate = Math.Round((((double)PaperTieCount / (double)PaperTotalCount) * 100d), 2);
			}

			ScissorsTotalCount = ScissorsWinCount + ScissorsLoseCount + ScissorsTieCount;

			if (ScissorsTotalCount > 0)
			{
				ScissorsWinRate = Math.Round((((double)ScissorsWinCount / (double)ScissorsTotalCount) * 100d), 2);
				ScissorsLoseRate = Math.Round((((double)ScissorsLoseCount / (double)ScissorsTotalCount) * 100d), 2);
				ScissorsTieRate = Math.Round((((double)ScissorsTieCount / (double)ScissorsTotalCount) * 100d), 2);
			}

			LizardTotalCount = LizardWinCount + LizardLoseCount + LizardTieCount;

			if (LizardTotalCount > 0)
			{
				LizardWinRate = Math.Round((((double)LizardWinCount / (double)LizardTotalCount) * 100d), 2);
				LizardLoseRate = Math.Round((((double)LizardLoseCount / (double)LizardTotalCount) * 100d), 2);
				LizardTieRate = Math.Round((((double)LizardTieCount / (double)LizardTotalCount) * 100d), 2);
			}

			SpockTotalCount = SpockWinCount + SpockLoseCount + SpockTieCount;

			if (SpockTotalCount > 0)
			{
				SpockWinRate = Math.Round((((double)SpockWinCount / (double)SpockTotalCount) * 100d), 2);
				SpockLoseRate = Math.Round((((double)SpockLoseCount / (double)SpockTotalCount) * 100d), 2);
				SpockTieRate = Math.Round((((double)SpockTieCount / (double)SpockTotalCount) * 100d), 2);
			}

			TotalTotalCount = TotalWinCount + TotalLoseCount + TotalTieCount;
			if (TotalTotalCount > 0)
			{
				TotalWinRate = Math.Round((((double)TotalWinCount / (double)TotalTotalCount) * 100d), 2);
				TotalLoseRate = Math.Round((((double)TotalLoseCount / (double)TotalTotalCount) * 100d), 2);
				TotalTieRate = Math.Round((((double)TotalTieCount / (double)TotalTotalCount) * 100d), 2);
			}
		}
	}
}