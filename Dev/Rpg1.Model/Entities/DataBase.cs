using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Battle.AI;
using Rpg1.Model.Repository;

namespace Rpg1.Model.Entities
{
	public class DataBase
	{
		public Ability Player { get; private set; }
		public IList<EnemiesAbility> Enemies { get; private set; }
		internal EnemyAIFactory EnemyAis { get; private set; }
		public IList<Achievement> Achievements { get; private set; }
		public Score SaveData { get; private set; }

		public static DataBase CreateDB()
		{
			var game = new DataBase();
			game.Player = PlayerRepository.LoadPlayer();
			game.Enemies = EnemyRepository.LoadEnemies();
			game.Achievements = AchievementRepository.Load();
			game.EnemyAis = new EnemyAIFactory();
			game.SaveData = ScoreRepository.Load(Def.DoEncrypt);
			return game;
		}
	}
}
