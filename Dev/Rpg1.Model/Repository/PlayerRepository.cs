using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rpg1.Model.Entities;
using CsvHelper;
using System.IO;

namespace Rpg1.Model.Repository
{
	public class PlayerRepository
	{
		private static readonly string PlayerDataPath = "Player.csv";

		public static Ability LoadPlayer()
		{
			using (var file = Def.GetDataStream(PlayerDataPath))
			{
				var csv = new CsvReader(file);
				csv.Read();
				return csv.GetRecord<Ability>();
			}
		}

		public static void Dump()
		{
			using (var file = Def.GetDataStream(PlayerDataPath))
			{
				using (var outFile = new StreamWriter("Player.csv"))
				{
					outFile.WriteLine(file.ReadToEnd());
				}
			}
		}
	}
}
