using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Entities;

namespace Rpg1.Model.Battle.Skill
{
	public class SkillBehaviorFactory
	{
		public IDictionary<SkillKind, SkillBehavior> Behaviors { get; private set; }

		public SkillBehaviorFactory()
		{
			Behaviors = new Dictionary<SkillKind, SkillBehavior>
			{
				{SkillKind.Attack, new AttackBehavior(6)},
				{SkillKind.Counter, new CounterBehavior(0.5f, 9)},
				{SkillKind.Absorb, new AbsorbBehavior(4, 5, 2)},
			};
		}
	}
}
