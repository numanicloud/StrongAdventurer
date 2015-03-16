using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Adventure;
using Rpg1.Model.Adventure.Messages;
using Rpg1.Model.Battle.AI;
using Rpg1.Model.Battle.Messages;
using Rpg1.Model.Entities;

namespace Rpg1.Model.Battle
{
	public enum BattleResult
	{
		Win, Lose
	}

	class BattleFlow
	{
		BattleContext context { get; set; }
		bool isLoaded { get; set; }

		public BattleFlow(Adventure.AdventureContext adventure, EnemiesAbility enemy)
		{
			context = new BattleContext()
			{
				Game = adventure.DataBase,
				Player = adventure.Player,
				Turn = 1,
				RoundCount = adventure.RoundCount,
				HaveNotLose = true,
			};

			var aiFactory = new EnemyAIFactory();
			context.Enemy = new EnemyBattler
			{
				Index = Def.EnemyIndex,
				EnemiesAbility = enemy,
				Hp = enemy.MaxHp,
				Ai = aiFactory.EnemyAIs[enemy.AiId](),
			};

			isLoaded = false;
		}

		public BattleFlow(AdventureContext adventure, PauseData pauseData)
		{
			context = new BattleContext()
			{
				Game = adventure.DataBase,
				Player = adventure.Player,
				Turn = pauseData.Turn,
				RoundCount = adventure.RoundCount,
				HaveNotLose = pauseData.HaveNotLose,
			};

			var aiFactory = new EnemyAIFactory();
			var enemy = context.Game.Enemies.First(x => x.Id == pauseData.EnemyId);
			context.Enemy = new EnemyBattler
			{
				Index = Def.EnemyIndex,
				EnemiesAbility = enemy,
				Hp = pauseData.EnemyHp,
				Ai = aiFactory.EnemyAIs[enemy.AiId](),
			};
			context.Enemy.Ai.LoadPauseData(pauseData.EnemyAi);

			isLoaded = true;
		}

		public IEnumerable<IMessage> GetFlow(PauseData pauseData, Action<BattleResult> callback)
		{
			context.Enemy.Hp = pauseData.EnemyHp;
			context.Enemy.Ai.LoadPauseData(pauseData.EnemyAi);
			context.Turn = pauseData.Turn;
			context.HaveNotLose = pauseData.HaveNotLose;
			yield return new ChangeContextMessage<BattleContext>(context);

			foreach(var item in GetFlowMain(callback))
			{
				yield return item;
			}
		}

		public IEnumerable<IMessage> GetFlow(Action<BattleResult> callback)
		{
			yield return new ChangeContextMessage<BattleContext>(context);
			yield return new StringMessage(context.Enemy.Ability.Name + "が あらわれた！");

			foreach(var item in GetFlowMain(callback))
			{
				yield return item;
			}
		}

		public IEnumerable<IMessage> GetFlowMain(Action<BattleResult> callback)
		{
			var skillFactory = new Skill.SkillBehaviorFactory();
			var playerContext = CreateContext(context.Player, context.Enemy);
			var enemyContext = CreateContext(context.Enemy, context.Player);

			while(true)
			{
				context.Player.Statuses.RemoveAll(x => !x.IsActive);
				context.Enemy.Statuses.RemoveAll(x => !x.IsActive);

				SkillKind playerTactics = SkillKind.Attack;
				SkillKind enemyTactics = SkillKind.Attack;

				foreach(var msg in context.Player.DetermineTactics(playerContext, x => playerTactics = x))
					yield return msg;

				foreach(var msg in context.Enemy.DetermineTactics(enemyContext, x => enemyTactics = x))
					yield return msg;

				var playersSkill = new { Skill = skillFactory.Behaviors[playerTactics], Context = playerContext };
				var enemiesSkill = new { Skill = skillFactory.Behaviors[enemyTactics], Context = enemyContext };

				context.Enemy.Ai.TakePlayersTacticsSample(playerTactics);

				if(RpgHelper.IsLose(playerTactics, enemyTactics)
					&& !(context.Enemy.Hp <= 4 && playerTactics == SkillKind.Absorb))
				{
					context.HaveNotLose = false;
				}

				var behaviors = new[] { playersSkill, enemiesSkill }.OrderByDescending(x => x.Skill.Priority);
				foreach(var item in behaviors)
				{
					foreach(var msg in item.Skill.Execute(item.Context))
					{
						yield return msg;
					}
					if(context.Player.Hp == 0)
					{
						foreach(var msg in OnPlayerDeath(callback))
						{
							yield return msg;
						}
						yield break;
					}
					else if(context.Enemy.Hp == 0)
					{
						foreach(var msg in OnEnemyDeath(callback))
						{
							yield return msg;
						}
						foreach(var msg in context.Player.Statuses.SelectMany(x => x.OnBattleEnd()))
						{
							yield return msg;
						}
						yield break;
					}
				}

				foreach(var item in context.Player.Statuses.SelectMany(x => x.OnTurnEnd()))
				{
					yield return item;
				}
				foreach(var item in context.Enemy.Statuses.SelectMany(x => x.OnTurnEnd()))
				{
					yield return item;
				}

				foreach(var item in context.Enemy.Ai.OnTurnEnd())
				{
					yield return item;
				}

				context.Turn++;
			}
		}

		private IEnumerable<IMessage> OnEnemyDeath(Action<BattleResult> callback)
		{
			yield return new DeathOfBattler(context.Enemy.Index);
			yield return new StringMessage(context.Enemy.Ability.Name + "はたおれた！");

			context.Game.SaveData.OvercomedEnemies.Add(context.Enemy.EnemiesAbility.Id);
			var ids = context.Game.Enemies.Select(x => x.Id);
			if(context.Game.SaveData.OvercomedEnemies.IsSupersetOf(ids))
			{
				foreach(var item in Events.TryOpenAchievement(context.Game, Def.AchievementId.AllEnemy))
				{
					yield return item;
				}
			}

			if(context.HaveNotLose)
			{
				var id = Def.EnemyToAchievementId[context.Enemy.EnemiesAbility.Id];
				foreach(var item in Events.TryOpenAchievement(context.Game, id))
				{
					yield return item;
				}
			}

			yield return new StringMessage("戦いに勝った！");

			callback(BattleResult.Win);
		}

		private IEnumerable<IMessage> OnPlayerDeath(Action<BattleResult> callback)
		{
			yield return new DeathOfBattler(context.Player.Index);
			yield return new StringMessage(context.Player.Ability.Name + "はたおれた！");

			context.Game.SaveData.GameOverCount++;
			if(context.Game.SaveData.GameOverCount == 1)
			{
				foreach(var msg in Events.TryOpenAchievement(context.Game, Def.AchievementId.GameOver1))
				{
					yield return msg;
				}
			}
			else if(context.Game.SaveData.GameOverCount == 5)
			{
				foreach(var msg in Events.TryOpenAchievement(context.Game, Def.AchievementId.GameOver5))
				{
					yield return msg;
				}
			}
			else if(context.Game.SaveData.GameOverCount == 20)
			{
				foreach(var msg in Events.TryOpenAchievement(context.Game, Def.AchievementId.GameOver20))
				{
					yield return msg;
				}
			}

			yield return new LoseMessage();

			callback(BattleResult.Lose);
		}

		private ActionContext CreateContext(Battler doer, Battler target)
		{
			return new ActionContext
			{
				BattleContext = context,
				Doer = doer,
				Target = target,
			};
		}
	}
}
