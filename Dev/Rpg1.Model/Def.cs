using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rpg1.Model
{
	public static class Def
	{
		public enum AchievementId
		{
			GameOver1, GameOver5, GameOver20,
			Fairy1, Fairy30,
			Round20, Round20_10, Round30, Round40,
			NightKing1, NightKing5,
			Round70,
			AllEnemy,
		}

		public static readonly string DataPassword = "hogehoge";
		public static readonly string InitialSavePath = "Resources/Data/InitialSave.csv";
		public static readonly string PauseDataPath = "pause.dat";
		public static readonly string ScoreDataPath = "score.dat";
		public static readonly int PlayerIndex = 0;
		public static readonly int EnemyIndex = 1;
		public static readonly int ScoreCount = 5;
		public static readonly int BossId = 22;
		public static readonly int BossRound = 50;
		public static readonly Dictionary<int, int> EnemyToAchievementId;
#if DEBUG
		public static readonly bool DoEncrypt = false;
#else
		public static readonly bool DoEncrypt = true;
#endif

		public static Func<string, TextReader> GetDataStream { get; set; }

		static Def()
		{
			EnemyToAchievementId = new Dictionary<int, int>()
			{
				{ 4, 13 },
				{ 0, 14 },
				{ 2, 15 },
				{ 1, 16 },
				{ 3, 17 },
				{ 7, 18 },
				{ 8, 19 },
				{ 9, 20 },
				{ 6, 21 },
				{ 10, 22 },
				{ 11, 23 },
				{ 13, 24 },
				{ 14, 25 },
				{ 15, 26 },
				{ 5, 27 },
				{ 18, 28 },
				{ 19, 29 },
				{ 20, 30 },
				{ 21, 31 },
				{ 22, 32 },
			};
		}
	}
}
