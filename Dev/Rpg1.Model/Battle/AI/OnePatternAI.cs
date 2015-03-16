using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Entities;

namespace Rpg1.Model.Battle.AI
{
	class OnePatternAI : EnemyAI
	{
		public SkillKind SkillKind { get; set; }

		public OnePatternAI(SkillKind skillKind)
		{
			SkillKind = skillKind;
		}

		public override SkillKind GetNextAction(ActionContext context)
		{
			return SkillKind;
		}

		public override void LoadPauseData(IAIPauseData data)
		{
			return;
		}

		public override IAIPauseData GetPauseData()
		{
			return null;
		}
	}
}
