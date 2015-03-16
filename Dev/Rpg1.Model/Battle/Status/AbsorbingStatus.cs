using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Adventure.Messages;
using Rpg1.Model.Battle.Messages;

namespace Rpg1.Model.Battle.Status
{
	public class AbsorbingStatus : Status
	{
		private float damageRate { get;set; }

		public AbsorbingStatus(Battler owner, float damageRate)
			: base(owner)
		{
			this.damageRate = damageRate;
		}

		public override IEnumerable<IMessage> BeforeAttacked(ActionContext context)
		{
			yield return new StringMessage("吸収魔法のスキをつかれた！");
		}

		public override int ModifyDamage(int source)
		{
			return (int)(source * damageRate);
		}

		public override IEnumerable<IMessage> OnTurnEnd()
		{
			IsActive = false;
			yield break;
		}
	}
}
