using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Entities;

namespace Rpg1.Model.Title
{
	public class ScoreContext
	{
		public int[] Scores { get; set; }
		public int? CurrentRank { get; set; }
	}
}
