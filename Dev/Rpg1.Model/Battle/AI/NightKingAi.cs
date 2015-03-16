using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Adventure.Messages;
using Rpg1.Model.Entities;

namespace Rpg1.Model.Battle.AI
{
	enum NightKingTactics
	{
		GotoWin, Next, Absorb, Mimic
	}

	[DataContract]
	class NightKingAIPauseData : IAIPauseData
	{
		[DataMember]
		public NightKingTactics Tactics { get; set; }
		[DataMember]
		public SkillKind? PreviousMySkill { get; set; }
		[DataMember]
		public SkillKind? PreviousRivalsSkill { get; set; }
	}

	class NightKingAi : EnemyAI
	{
		private NightKingTactics tactics = NightKingTactics.Absorb;
		private SkillKind? previousMySkill;
		private SkillKind? previousRivalsSkill;

		public override SkillKind GetNextAction(ActionContext context)
		{
			SkillKind result;
			switch(tactics)
			{
			case NightKingTactics.GotoWin:
				result = previousRivalsSkill.Value.RotateEnum(1);
				break;

			case NightKingTactics.Next:
				result = previousMySkill.Value.RotateEnum(1);
				break;

			case NightKingTactics.Absorb:
				result = SkillKind.Absorb;
				break;

			case NightKingTactics.Mimic:
				result = previousRivalsSkill.Value;
				break;

			default:
				result = SkillKind.Attack;
				break;
			}

			previousMySkill = result;
			return result;
		}

		public override IAIPauseData GetPauseData()
		{
			return new NightKingAIPauseData()
			{
				Tactics = tactics,
				PreviousMySkill = previousMySkill,
				PreviousRivalsSkill = previousRivalsSkill,
			};
		}

		public override void LoadPauseData(IAIPauseData data)
		{
			var obj = data as NightKingAIPauseData;
			if(obj == null)
			{
				throw new ArgumentException("中断データのAIの種類が間違っています。", "data");
			}

			tactics = obj.Tactics;
			previousMySkill = obj.PreviousMySkill;
			previousRivalsSkill = obj.PreviousRivalsSkill;
		}

		public override void TakePlayersTacticsSample(SkillKind kind)
		{
			previousRivalsSkill = kind;
		}

		public override IEnumerable<IMessage> OnTurnEnd()
		{
			var random = new Random();
			tactics = (NightKingTactics)random.Next(0, 4);

			switch(tactics)
			{
			case NightKingTactics.GotoWin:
				yield return new StringMessage("夜の王はニヤリと笑った。");
				break;

			case NightKingTactics.Next:
				yield return new StringMessage("夜の王の目が怪しく輝く…");
				break;

			case NightKingTactics.Absorb:
				yield return new StringMessage("夜の王は沈黙している。");
				break;

			case NightKingTactics.Mimic:
				yield return new StringMessage("夜の王は唸り声を上げた！");
				break;

			default:
				break;
			}
		}
	}
}
