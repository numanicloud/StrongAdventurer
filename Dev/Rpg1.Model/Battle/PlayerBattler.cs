using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Battle.Messages;
using Rpg1.Model.Entities;
using Rpg1.Model.Messages;
using Rpg1.Model.Repository;

namespace Rpg1.Model.Battle
{
	public class PlayerBattler : Battler
	{
		public Ability PlayersAbility { get; set; }
		public override Ability Ability
		{
			get { return PlayersAbility; }
		}

		public override IEnumerable<IMessage> DetermineTactics(ActionContext context, Action<SkillKind> callback)
		{
			var skillInput = new ChooseSkill();
			yield return skillInput;

			if(skillInput.Response == BattleCommand.Quit)
			{
				var repo = new PauseRepository();
				repo.DoEncrypt = Def.DoEncrypt;
				repo.Save(new PauseData
				{
					RoundCount = context.BattleContext.RoundCount,
					Turn = context.BattleContext.Turn,
					PlayerHp = context.BattleContext.Player.Hp,
					EnemyId = context.BattleContext.Enemy.EnemiesAbility.Id,
					EnemyHp = context.BattleContext.Enemy.Hp,
					EnemyAi = context.BattleContext.Enemy.Ai.GetPauseData(),
					HaveNotLose = context.BattleContext.HaveNotLose,
				});

				var title = new Title.TitleFlow();
				yield return new ChangeFlowMessage<object>(title.GetFlow(), null);
			}
			else
			{
				callback((SkillKind)skillInput.Response);
			}
			yield break;
		}
	}
}
