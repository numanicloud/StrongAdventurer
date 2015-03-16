using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Adventure.Messages;
using Rpg1.Model.Battle.Messages;

namespace Rpg1.Model.Battle.Skill
{
	public class AttackBehavior : SkillBehavior
	{
		private int power { get;set; }

		public AttackBehavior(int power)
		{
			this.power = power;
		}

		public override IEnumerable<IMessage> Execute(ActionContext context)
		{
			yield return new StringMessage(context.Doer.Ability.Name + "の攻撃！");

			yield return new PlayEffect(context.Target.Index, "Slash.efk", "sen_ka_katana_kiru01.wav", 0.3f);
			foreach(var item in context.Target.Statuses.SelectMany(x => x.BeforeAttacked(context)))
			{
				yield return item;
			}

			var damage = context.Target.Statuses.Aggregate(power, (a, s) => s.ModifyDamage(a));
			context.Target.Hp -= damage;
			yield return new Damage(context.Target, damage);

			foreach(var item in context.Target.Statuses.SelectMany(x => x.AfterAttacked(context)))
			{
				yield return item;
			}
		}
	}
}
