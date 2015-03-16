using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Entities;
using NacHelpers;

namespace Rpg1.Model.Battle.AI
{
	class CheckingAI : EnemyAI
	{
		private Func<SkillKind, SkillKind, SkillKind> getNextSkill { get; set; }
		private SkillKind? previousRivalsSkill { get; set; }
		private SkillKind? previousSkill { get; set; }

		public CheckingAI(Func<SkillKind, SkillKind, SkillKind> getNextSkill)
		{
			this.getNextSkill = getNextSkill;
		}

		public override SkillKind GetNextAction(ActionContext context)
		{
			SkillKind next;

			if(!previousSkill.HasValue)
			{
				next = TypeHelper.GetValues<SkillKind>().GetRandom();
			}
			else
			{
				next = getNextSkill(previousSkill.Value, previousRivalsSkill.Value);
			}

			previousSkill = next;
			return next;
		}

		public override void TakePlayersTacticsSample(SkillKind kind)
		{
			previousRivalsSkill = kind;
		}

		public override IAIPauseData GetPauseData()
		{
			return new CheckingAIPauseData()
			{
				PreviousSkill = previousSkill,
				PreviousRivalsSkill = previousRivalsSkill,
			};
		}

		public override void LoadPauseData(IAIPauseData data)
		{
			var d = data as CheckingAIPauseData;
			if(d == null)
			{
				throw new ArgumentException("中断データのAIの種類が間違っています。", "data");
			}
			else
			{
				previousSkill = d.PreviousSkill;
				previousRivalsSkill = d.PreviousRivalsSkill;
			}
		}
	}
}
