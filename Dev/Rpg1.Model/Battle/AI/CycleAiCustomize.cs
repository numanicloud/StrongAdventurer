using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Entities;
using NacHelpers;
using System.Runtime.Serialization;

namespace Rpg1.Model.Battle.AI
{
	[DataContract]
	class CycleAiCustomize : AiCustomize
	{
		[DataMember]
		private int turn { get; set; }
		[DataMember]
		public int cycle { get; set; }
		[DataMember]
		IEnumerable<SkillKind> skills { get; set; }

		public CycleAiCustomize(int turn, int cycle, params SkillKind[] kind)
		{
			this.turn = turn;
			this.cycle = cycle;
			this.skills = kind;

			if(turn > cycle)
			{
				throw new ArgumentException("turnはcycle以下である必要があります。");
			}

			Priority = 0;
		}

		public override bool ConditionIsSatisfied(ActionContext context)
		{
			return (context.BattleContext.Turn - turn) % cycle == 0;
		}

		public override SkillKind Invoke()
		{
			return skills.GetRandom();
		}
	}
}
