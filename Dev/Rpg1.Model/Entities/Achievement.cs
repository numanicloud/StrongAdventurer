using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg1.Model.Entities
{
	public class Achievement
	{
		public int Id { get; set; }
		public StatsColor Color { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public string PraiseText { get; set; }

		public override string ToString()
		{
			return Title;
		}
	}
}
