using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg1.Model.Battle.Status
{
	public abstract class Status
	{
		public Battler Owner { get; set; }
		public bool IsActive { get; set; }

		public Status(Battler owner)
		{
			Owner = owner;
			IsActive = true;
		}

		public virtual IEnumerable<IMessage> BeforeAttacked(ActionContext context)
		{
			yield break;
		}

		public virtual IEnumerable<IMessage> AfterAttacked(ActionContext context)
		{
			yield break;
		}

		public virtual IEnumerable<IMessage> OnTurnEnd()
		{
			yield break;
		}

		public virtual IEnumerable<IMessage> OnBattleEnd()
		{
			IsActive = false;
			yield break;
		}

		public virtual int ModifyDamage(int source)
		{
			return source;
		}
	}
}
