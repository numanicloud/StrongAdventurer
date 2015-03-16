using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg1.Model.Battle.Skill
{
	public abstract class SkillBehavior
	{
		public int Priority { get; set; }

		public abstract IEnumerable<IMessage> Execute(ActionContext context);
	}
}
