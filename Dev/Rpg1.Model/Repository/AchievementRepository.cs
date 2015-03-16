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
	static class AchievementRepository
	{
		private static readonly string AchievementDataPath = "Achievement.csv";

		public static IList<Achievement> Load()
		{
			using (var file = Def.GetDataStream(AchievementDataPath))
			{
				var csv = new CsvReader(file);
				return csv.GetRecords<Achievement>().ToList();
			}
		}

		public static void Dump()
		{
			using (var file = Def.GetDataStream(AchievementDataPath))
			{
				using (var outFile = new StreamWriter("Achievement.csv"))
				{
					outFile.WriteLine(file.ReadToEnd());
				}
			}
		}
	}
}
