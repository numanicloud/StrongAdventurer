using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Entities;

namespace Rpg1.Model.Title
{
	public class AchievementStatus
	{
		public Achievement Achievement { get; private set; }
		public bool IsGotten { get; private set; }

		public AchievementStatus(Achievement achievement, bool isGotten)
		{
			this.Achievement = achievement;
			this.IsGotten = isGotten;
		}
	}

	public class AchievementContext
	{
		public IEnumerable<AchievementStatus> Status { get; private set; }

		public AchievementContext(IEnumerable<Achievement> list, IEnumerable<int> gotten)
		{
			Status = list.Select(x => new AchievementStatus(x, gotten.Contains(x.Id)));
		}
	}
}
