using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Battle.Status;

namespace Rpg1.Model.Battle.Skill
{
	public class CounterBehavior : SkillBehavior
	{
		private int power { get; set; }
		private float rate { get; set; }

		public CounterBehavior(float rate, int power)
		{
			this.rate = rate;
			this.power = power;
			Priority = 1;
		}

		public override IEnumerable<IMessage> Execute(ActionContext context)
		{
			context.Doer.Statuses.Add(new CounterStatus(context.Doer, rate, power));
			yield break;
		}
	}
}
