using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NacHelpers;
using Rpg1.Model.Entities;

namespace Rpg1.Model.Battle
{
	public abstract class Battler : NotificationObject
	{
		private int hp;

		public int Index { get; set; }

		public abstract Ability Ability { get; }

		public int Hp
		{
			get { return hp; }
			set
			{
				hp = Helper.Clamp(value, 0, Ability.MaxHp);
				Raise();
			}
		}

		public List<Status.Status> Statuses { get; set; }

		public Battler()
		{
			Statuses = new List<Status.Status>();
		}

		public abstract IEnumerable<IMessage> DetermineTactics(ActionContext context, Action<SkillKind> callback);
	}
}
