using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Entities;

namespace Rpg1.Model.Battle.AI
{
	[DataContract]
	abstract class EnemyAI
	{
		public abstract SkillKind GetNextAction(ActionContext context);
		public virtual void TakePlayersTacticsSample(SkillKind kind)
		{
			return;
		}
		public virtual IEnumerable<IMessage> OnTurnEnd()
		{
			yield break;
		}

		public abstract IAIPauseData GetPauseData();

		public abstract void LoadPauseData(IAIPauseData data);
	}
}
