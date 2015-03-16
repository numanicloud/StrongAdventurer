using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Entities;
using NacHelpers;
using Rpg1.Model.Battle;
using Rpg1.Model.Adventure.Messages;
using Rpg1.Model.Battle.Messages;
using Rpg1.Model.Repository;
using Rpg1.Model.Messages;

namespace Rpg1.Model.Adventure
{
	class AdventureFlow
	{
		static readonly string AdventureBgmPath = "Adventure.ogg";
		static readonly string BossBgmPath = "Boss.ogg";

		bool isBossBgm = false;
		PauseData pause;

		public AdventureContext Context { get; set; }

		public AdventureFlow(DataBase game)
		{
			Context = new AdventureContext
			{
				DataBase = game,
				Player = new PlayerBattler()
				{
					PlayersAbility = game.Player,
					Hp = game.Player.MaxHp,
					Index = Def.PlayerIndex,
				},
				RoundCount = 1,
				DayOrNight = DayOrNight.Morning,
			};
		}

		public AdventureFlow(DataBase db, PauseData pause)
		{
			Context = new AdventureContext
			{
				DataBase = db,
				Player = new PlayerBattler()
				{
					PlayersAbility = db.Player,
					Hp = pause.PlayerHp,
					Index = Def.PlayerIndex,
				},
				RoundCount = pause.RoundCount,
				DayOrNight = RoundToDayOrNight(pause.RoundCount),
			};
			this.pause = pause;
		}

		public IEnumerable<IMessage> GetFlow()
		{
			yield return new PlayBgmMessage(AdventureBgmPath);
			yield return new StringMessage("運命の森へやってきた。");
			//yield return new GetAchievementMessage(Context.Game.Achievements[2]);

			foreach(var item in GetFlowMain())
			{
				yield return item;
			}
		}

		public IEnumerable<IMessage> GetFlow(PauseData pauseData)
		{
			if(pauseData.EnemyId == Def.BossId)
			{
				yield return new PlayBgmMessage(BossBgmPath);
				isBossBgm = true;
			}
			else
			{
				yield return new PlayBgmMessage(AdventureBgmPath);
			}

			var enemy = Context.DataBase.Enemies.First(x => x.Id == pauseData.EnemyId);

			var battleFlow = new BattleFlow(Context, enemy);
			BattleResult result = BattleResult.Win;
			foreach(var item in battleFlow.GetFlow(pauseData, x => result = x))
			{
				yield return item;
			}
			if(result == BattleResult.Lose)
			{
				foreach(var item in OnLose()) yield return item;
			}
			else if(result == BattleResult.Win)
			{
				foreach(var item in OnWin(enemy)) yield return item;
			}

			if(Context.RoundCount % 7 == 0)
			{
				foreach(var item in EventOnLake()) yield return item;
			}

			Context.RoundCount++;

			foreach(var item in GetFlowMain())
			{
				yield return item;
			}
		}

		private IEnumerable<IMessage> GetFlowMain()
		{
			while(true)
			{
				if(isBossBgm)
				{
					yield return new PlayBgmMessage(AdventureBgmPath);
					isBossBgm = false;
				}

				ScoreRepository.Save(Context.DataBase.SaveData, Def.DoEncrypt);

				EnemiesAbility enemy;
				if(Context.RoundCount == Def.BossRound)
				{
					enemy = Context.DataBase.Enemies.First(x => x.Id == Def.BossId);
					yield return new PlayBgmMessage(BossBgmPath);
					isBossBgm = true;
				}
				else
				{
					enemy = Context.DataBase.Enemies
						.Where(x => x.Start <= Context.RoundCount)
						.Where(x => x.End == -1 || x.End >= Context.RoundCount)
						.GetRandom();
				}

				//enemy = Context.Game.Enemies.First(x => x.Id == 21);

				var battleFlow = new BattleFlow(Context, enemy);
				BattleResult result = BattleResult.Win;
				foreach(var item in battleFlow.GetFlow(x => result = x))
					yield return item;

				if(result == BattleResult.Lose)
				{
					foreach(var item in OnLose()) yield return item;
				}
				else if(result == BattleResult.Win)
				{
					foreach(var item in OnWin(enemy)) yield return item;
				}

				if(Context.RoundCount % 7 == 0)
				{
					foreach(var item in EventOnLake()) yield return item;
				}

				Context.RoundCount++;
			}
		}

		private IEnumerable<IMessage> OnWin(EnemiesAbility enemy)
		{
			if(enemy.Id == Def.BossId)
			{
				Context.DataBase.SaveData.NightKingCount++;
				if(Context.DataBase.SaveData.NightKingCount == 1)
				{
					foreach(var item in Events.TryOpenAchievement(Context.DataBase, Def.AchievementId.NightKing1))
					{
						yield return item;
					}
				}
				else if(Context.DataBase.SaveData.NightKingCount == 5)
				{
					foreach(var item in Events.TryOpenAchievement(Context.DataBase, Def.AchievementId.NightKing5))
					{
						yield return item;
					}
				}
			}

			if(Context.RoundCount == 20)
			{
				Context.DataBase.SaveData.Round20Count++;
				foreach(var item in Events.TryOpenAchievement(Context.DataBase, Def.AchievementId.Round20))
				{
					yield return item;
				}

				if(Context.DataBase.SaveData.Round20Count == 10)
				{
					foreach(var item in Events.TryOpenAchievement(Context.DataBase, Def.AchievementId.Round20_10))
					{
						yield return item;
					}
				}
			}
			else if(Context.RoundCount == 30)
			{
				foreach(var item in Events.TryOpenAchievement(Context.DataBase, Def.AchievementId.Round30))
				{
					yield return item;
				}
			}
			else if(Context.RoundCount == 40)
			{
				foreach(var item in Events.TryOpenAchievement(Context.DataBase, Def.AchievementId.Round40))
				{
					yield return item;
				}
			}
			else if(Context.RoundCount == 70)
			{
				foreach(var item in Events.TryOpenAchievement(Context.DataBase, Def.AchievementId.Round70))
				{
					yield return item;
				}
			}

			var d = RoundToDayOrNight(Context.RoundCount + 1);
			if(Context.DayOrNight != d)
			{
				yield return new ChangeBackgroundMessage(GetBGPath(d));
				Context.DayOrNight = d;
			}
		}

		private IEnumerable<IMessage> OnLose()
		{
			var save = Context.DataBase.SaveData;
			var score = Context.RoundCount - 1;
			var rank = save.Scores.Count(x => x > score) + 1;

			save.Scores = save.Scores
				.Append(score)
				.OrderByDescending(x => x)
				.Take(Def.ScoreCount)
				.ToArray();

			ScoreRepository.Save(save, Def.DoEncrypt);

			var title = new Title.TitleFlow();
			var context = new Title.ScoreContext { Scores = save.Scores, CurrentRank = rank };
			yield return new ChangeFlowMessage<Title.ScoreContext>(title.GetScoreSceneFlow(), context);
		}

		private IEnumerable<IMessage> EventOnLake()
		{
			int Heal = Math.Min(Context.RoundCount, 35);

			yield return new ChangeContextMessage<AdventureContext>(Context);
			yield return new ChangeBackgroundMessage("Lake.png");
			yield return new StringMessage("命の泉を見つけた！");
			yield return new SetCharactor("mon_195r.png");
			yield return new StringMessage("泉の精があらわれた。");

			Context.DataBase.SaveData.FairyCount += 1;
			if(Context.DataBase.SaveData.FairyCount == 1)
			{
				foreach(var item in Events.TryOpenAchievement(Context.DataBase, Def.AchievementId.Fairy1))
					yield return item;
			}
			else if(Context.DataBase.SaveData.FairyCount == 30)
			{
				foreach(var item in Events.TryOpenAchievement(Context.DataBase, Def.AchievementId.Fairy30))
					yield return item;
			}

			yield return new StringMessage("「大変お疲れのようですね。癒して差し上げましょう…」");

			Context.Player.Hp += Heal;
			yield return new HealMessage(Context.Player, Heal);

			yield return new SetCharactor(null);
			yield return new StringMessage("命の泉を後にした…");
			yield return new ChangeBackgroundMessage(GetBGPath(Context.DayOrNight));
		}

		private static string GetBGPath(DayOrNight dayOrNight)
		{
			switch(dayOrNight)
			{
			case DayOrNight.Morning: return "Back1.png";
			case DayOrNight.Noon: return "Back2.png";
			case DayOrNight.Evening: return "Back3.png";
			case DayOrNight.Night: return "Back4.png";
			default: throw new Exception();
			}
		}
		private static DayOrNight RoundToDayOrNight(int round)
		{
			round %= 50;
			if(round == 0)
			{
				return DayOrNight.Night;
			}
			else if(round <= 9)
			{
				return DayOrNight.Morning;
			}
			else if(round <= 27)
			{
				return DayOrNight.Noon;
			}
			else if(round <= 36)
			{
				return DayOrNight.Evening;
			}
			else
			{
				return DayOrNight.Night;
			}
		}
	}
}
