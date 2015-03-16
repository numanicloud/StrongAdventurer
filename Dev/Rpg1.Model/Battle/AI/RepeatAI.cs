using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Entities;
using NacHelpers;

namespace Rpg1.Model.Battle.AI
{
	class RepeatAI : EnemyAI
	{
		private SkillKind? prev { get; set; }
		private int cycle { get; set; }

		public RepeatAI(int cycle)
		{
			this.cycle = cycle;
		}

		public override SkillKind GetNextAction(ActionContext context)
		{
			SkillKind next;

			if(!prev.HasValue || context.BattleContext.Turn % cycle == 1)
			{
				var baseList = prev.HasValue ? RpgHelper.GetSkillExcept(prev.Value) : TypeHelper.GetValues<SkillKind>();
				next = baseList.GetRandom();
			}
			else
			{
				next = prev.Value;
			}

			prev = next;
			return next;
		}

		public override IAIPauseData GetPauseData()
		{
			return new RepeatAIPauseData { PreviousSkill = prev };
		}

		public override void LoadPauseData(IAIPauseData data)
		{
			var d = data as RepeatAIPauseData;
			if(d == null)
			{
				throw new ArgumentException("中断データのAIの種類が間違っています。", "data");
			}
			else
			{
				prev = d.PreviousSkill;
			}
		}
	}
}
