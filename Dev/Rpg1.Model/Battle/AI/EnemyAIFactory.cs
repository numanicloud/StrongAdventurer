using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Entities;
using NacHelpers;

namespace Rpg1.Model.Battle.AI
{
	class EnemyAIFactory
	{
		public IDictionary<string, Func<EnemyAI>> EnemyAIs { get; set; }

		public EnemyAIFactory()
		{
			var emptyCustomize = Enumerable.Empty<AiCustomize>();

			EnemyAIs = new Dictionary<string, Func<EnemyAI>>()
			{
				{"LivingMail", LivingMail},
				{"Liquid", Liquid},
				{"Absorber", () => new OnePatternAI(SkillKind.Absorb)},
				{"Mimicry", Mimicry},
				{"Attacker", () => new OnePatternAI(SkillKind.Attack)},
				{"Counterer", () => new OnePatternAI(SkillKind.Counter)},
				{"ChangeOnLose", ChangeOnLose},
				{"Repeat2", () => new RepeatAI(2)},
				{"LaughingRoot", LaugingRoot},
				{"Shief", Shief},
				{"Scorpio", Scorpio},
				{"Kobold", Kobold},
				{"NoCounter", () => new CustomAI(emptyCustomize, SkillKind.Attack, SkillKind.Absorb)},
				{"NoAbsorb", () => new CustomAI(emptyCustomize, SkillKind.Attack, SkillKind.Counter)},
				{"Golem", Golem},
				{"Rotation", Rotation},
				{"GotoWin", GotoWin},
				{"GotoLose", GotoLose},
				{"NoAttack", () => new CustomAI(emptyCustomize, SkillKind.Counter, SkillKind.Absorb)},
				{"Random", () => new CustomAI(emptyCustomize, SkillKind.Attack, SkillKind.Counter, SkillKind.Absorb)},
				{"Dragon", Dragon},
				{"Alraune", Alraune},
				{"NightKing", () => new NightKingAi()},
			};
		}

		private EnemyAI Alraune()
		{
			return new CustomAI(
				new[] {
					new CycleAiCustomize(1, 3, SkillKind.Attack, SkillKind.Counter),
					new CycleAiCustomize(2, 3, SkillKind.Counter, SkillKind.Absorb),
					new CycleAiCustomize(3, 3, SkillKind.Absorb, SkillKind.Attack),
				}, SkillKind.Attack, SkillKind.Counter, SkillKind.Absorb);
		}

		private EnemyAI Dragon()
		{
			return new CheckingAI((me, rival) =>
			{
				if(RpgHelper.IsLose(rival, me))
				{
					return me.RotateEnum(1);
				}
				else
				{
					return me.RotateEnum(2);
				}
			});
		}

		private EnemyAI GotoLose()
		{
			// 負ける手をだす
			return new CheckingAI((me, rival) => rival.RotateEnum(2));
		}

		private EnemyAI GotoWin()
		{
			// 勝てる手をだす
			return new CheckingAI((me, rival) => rival.RotateEnum(1));
		}

		private EnemyAI Rotation()
		{
			return new CheckingAI((me, rival) => (SkillKind)(((int)me + 1)  % 3));
		}

		private EnemyAI Golem()
		{
			return new CustomAI(
				new[] {
					new CycleAiCustomize(2, 4, SkillKind.Attack),
					new CycleAiCustomize(3, 4, SkillKind.Attack),
				}, SkillKind.Attack, SkillKind.Counter, SkillKind.Absorb);
		}

		private EnemyAI Kobold()
		{
			return new CustomAI(
				new[] {
					new CycleAiCustomize(1, 3, SkillKind.Counter, SkillKind.Absorb),
					new CycleAiCustomize(2, 3, SkillKind.Attack),
					new CycleAiCustomize(3, 3, SkillKind.Counter),
				}, SkillKind.Attack, SkillKind.Counter, SkillKind.Absorb);
		}

		private EnemyAI Scorpio()
		{
			return new CustomAI(
				new[] {
					new CycleAiCustomize(1, 3, SkillKind.Attack, SkillKind.Counter),
					new CycleAiCustomize(2, 3, SkillKind.Attack, SkillKind.Absorb),
					new CycleAiCustomize(3, 3, SkillKind.Attack),
				}, SkillKind.Attack, SkillKind.Counter, SkillKind.Absorb);
		}

		private EnemyAI Shief()
		{
			return new CustomAI(
				new[] {
					new CycleAiCustomize(1, 3, SkillKind.Attack),
					new CycleAiCustomize(2, 3, SkillKind.Counter),
				}, SkillKind.Attack, SkillKind.Counter, SkillKind.Absorb);
		}

		private EnemyAI LaugingRoot()
		{
			return new CustomAI(
				new[] {
					new CycleAiCustomize(1, 3, SkillKind.Absorb),
					new CycleAiCustomize(3, 3, SkillKind.Absorb),
				}, SkillKind.Attack, SkillKind.Counter, SkillKind.Absorb);
		}

		private EnemyAI Mimicry()
		{
			return new CheckingAI((me, rival) => rival);
		}

		private EnemyAI ChangeOnLose()
		{
			return new CheckingAI((me, rival) =>
			{
				if(RpgHelper.IsLose(me, rival))
				{
					return RpgHelper.GetSkillExcept(me).GetRandom();
				}
				else
				{
					return me;
				}
			});
		}

		private EnemyAI Liquid()
		{
			return new CustomAI(
				new[] {
					new CycleAiCustomize(2, 2, SkillKind.Absorb),
				},
				SkillKind.Attack,
				SkillKind.Counter,
				SkillKind.Absorb);
		}

		private EnemyAI LivingMail()
		{
			return new CustomAI(
				new[] {
					new CycleAiCustomize(2, 3, SkillKind.Attack, SkillKind.Counter),
					new CycleAiCustomize(3, 3, SkillKind.Counter)
				},
				SkillKind.Attack,
				SkillKind.Counter,
				SkillKind.Absorb);
		}
	}
}
