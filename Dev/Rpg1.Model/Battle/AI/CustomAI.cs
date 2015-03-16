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
	class CustomAI : EnemyAI
	{
		IEnumerable<AiCustomize> customizes { get; set; }
		IEnumerable<SkillKind> randomSkills { get; set; }

		/// <summary>
		/// カスタムAIを初期化します。
		/// </summary>
		/// <param name="customizes">カスタマイズのリスト。</param>
		/// <param name="randomSkill">いずれのカスタマイズも条件が満たされないとき、ここからランダムにスキルが選ばれます。</param>
		public CustomAI(IEnumerable<AiCustomize> customizes, params SkillKind[] randomSkills)
		{
			this.customizes = customizes;
			this.randomSkills = randomSkills;
		}

		public override SkillKind GetNextAction(ActionContext context)
		{
			var availableCustomizes = customizes.Where(x => x.ConditionIsSatisfied(context))
				.OrderByDescending(x => x.Priority);

			if(availableCustomizes.Any())
			{
				return availableCustomizes.First().Invoke();
			}
			else
			{
				return randomSkills.GetRandom();
			}
		}

		public override IAIPauseData GetPauseData()
		{
			return null;
		}

		public override void LoadPauseData(IAIPauseData data)
		{
			return;
		}
	}
}
