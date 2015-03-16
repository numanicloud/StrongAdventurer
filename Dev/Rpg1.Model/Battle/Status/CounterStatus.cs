using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Adventure.Messages;
using Rpg1.Model.Battle.Messages;

namespace Rpg1.Model.Battle.Status
{
	public class CounterStatus : Status
	{
		private float damageRate { get; set; }
		private int power { get; set; }
		private bool isSucceeded { get;set; }

		public CounterStatus(Battler owner, float damageRate, int power)
			: base(owner)
		{
			this.damageRate = damageRate;
			this.power = power;
			isSucceeded = false;
		}

		public override IEnumerable<IMessage> BeforeAttacked(ActionContext context)
		{
			yield return new PlayEffect(Owner.Index, "Shield.efk", "metal28.wav", 0.2f);
		}

		public override IEnumerable<IMessage> AfterAttacked(ActionContext context)
		{
			if(context.Target.Hp == 0)
			{
				yield break;
			}

			yield return new StringMessage(context.Target.Ability.Name + "のカウンターだ！");
			context.Doer.Hp -= power;
			yield return new Damage(context.Doer, power);
			isSucceeded = true;
		}

		public override IEnumerable<IMessage> OnTurnEnd()
		{
			if(!isSucceeded)
			{
				yield return new StringMessage(Owner.Ability.Name + "のカウンターは失敗におわった。");
			}
			IsActive = false;
			yield break;
		}

		public override int ModifyDamage(int source)
		{
			return (int)(source * damageRate);
		}
	}
}
