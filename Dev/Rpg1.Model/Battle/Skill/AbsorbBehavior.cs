using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Adventure.Messages;
using Rpg1.Model.Battle.Messages;
using Rpg1.Model.Battle.Status;

namespace Rpg1.Model.Battle.Skill
{
	public class AbsorbBehavior : SkillBehavior
	{
		private float damageRate { get;set; }
		private int heal { get; set; }
		private int power { get; set; }

		public AbsorbBehavior(int power, int heal, float damageRate)
		{
			Priority = 1;
			this.power = power;
			this.heal = heal;
			this.damageRate = damageRate;
		}

		public override IEnumerable<IMessage> Execute(ActionContext context)
		{
			yield return new StringMessage(context.Doer.Ability.Name + "は吸収の魔法をつかった！");

			yield return new PlayEffect(context.Target.Index, "Absorb.efk", "jya03.wav", 0.7f);

			context.Target.Hp -= power;
			yield return new Damage(context.Target, power);

			context.Doer.Hp += heal;
			yield return new HealMessage(context.Doer, heal);

			context.Doer.Statuses.Add(new AbsorbingStatus(context.Doer, damageRate));
		}
	}
}
