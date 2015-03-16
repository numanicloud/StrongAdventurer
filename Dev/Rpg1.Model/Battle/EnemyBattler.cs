using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Battle.AI;
using Rpg1.Model.Entities;

namespace Rpg1.Model.Battle
{
	public class EnemyBattler : Battler
	{
		public EnemiesAbility EnemiesAbility { get; set; }

		public override Ability Ability
		{
			get { return EnemiesAbility; }
		}

		internal EnemyAI Ai { get; set; }

		public override IEnumerable<IMessage> DetermineTactics(ActionContext context, Action<SkillKind> callback)
		{
			callback(Ai.GetNextAction(context));
			yield break;
		}
	}
}
