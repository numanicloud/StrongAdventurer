using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Rpg1.Model.Entities;

namespace Rpg1.Model.Repository
{
	public class EnemyRepository
	{
		private static readonly string EnemyDataPath = "Enemy.csv";

		public static IList<EnemiesAbility> LoadEnemies()
		{
			using (var file = Def.GetDataStream(EnemyDataPath))
			{
				var csv = new CsvReader(file);
				return csv.GetRecords<EnemiesAbility>().ToList();
			}
		}

		public static void Dump()
		{
			using (var file = Def.GetDataStream(EnemyDataPath))
			{
				using (var outFile = new StreamWriter("Enemy.csv"))
				{
					outFile.WriteLine(file.ReadToEnd());
				}
			}
		}
	}
}
